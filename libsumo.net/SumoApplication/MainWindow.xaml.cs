﻿using System;
using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Diagnostics;
using System.Linq;

using LibSumo.Net;
using LibSumo.Net.Events;
using LibSumo.Net.Logger;
using LibSumo.Net.Hook;
using LibSumo.Net.Protocol;

using OpenCvSharp;
using OpenCvSharp.Extensions;

using System.Reflection;
using System.Drawing;
using SumoApplication.Video;
using System.Net;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections.Generic;
using BrandonPotter.XBox;
using System.Windows.Threading;

namespace SumoApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : System.Windows.Window, IDisposable
    {
        private SumoController controller;
        private SumoKeyboardPiloting piloting;
        private SumoInformations sumoInformations;

        XBoxControllerWatcher watcher = new BrandonPotter.XBox.XBoxControllerWatcher();
        XBoxController activeController = null;

        // Framerate calculation
        private double frameRate;
        private Stopwatch frameWatch;

        private Mat SplashImage;
        private System.Windows.Controls.Image TempImage;

        // Multiple Drones        
        //private const string prefixName = "";
        //private const string DNSString = "";
        private bool MultipleDroneMode = false;
        List<MiniDevice> ListOfDevices;        
        class MiniDevice
        {
            public string Name { get; set; }
            public Button Button { get; set; }
            public int Id { get; set; }            
        }


        public MainWindow()
        {

            InitializeComponent();

            watcher.ControllerConnected += (c)=>
            {
                if (activeController == null)
                {
                    activeController = c;
                }
            };
            watcher.ControllerDisconnected += (c) =>
            {
                if (activeController == c)
                {
                    activeController = null;
                }
            };
            var tim = new DispatcherTimer();
            tim.Tick += RunXboxControlelr;
            tim.Interval = TimeSpan.FromMilliseconds(10);
            tim.Start();

            sumoInformations = new SumoInformations();

            LOGGER.GetInstance.MessageAvailable += GetInstance_MessageAvailable;
            //LOGGER.GetInstance.MessageLevel = log4net.Core.Level.Debug;
            frameWatch = new Stopwatch();

            // Init Audio Theme Box
            cbxAudioTheme.ItemsSource = Enum.GetValues(typeof(SumoEnumGenerated.Theme_theme)).Cast<SumoEnumGenerated.Theme_theme>();

            //cbxWifiBand.SelectedIndex = 1;
            // Init Default UI
            panelJump.IsEnabled = false;
            panelSettings.IsEnabled = false;
            //imgMain.IsEnabled = false;
            SetSplashImage();

            InitDrone();

            CompositionTarget.Rendering += RunXboxControlelr;
        }


        double filtTur = 0;
        double filtSpeed = 0;

        private void RunXboxControlelr(object sender, EventArgs e)
        {
            if (activeController == null || controller == null || !controller.IsConnected)
            {
                piloting.Should_run = true;
                return;
            }
            piloting.Should_run = false;
            if (activeController.ButtonAPressed)
                controller.StartJump(SumoEnumGenerated.Jump_type.high);
            if (activeController.ButtonBPressed)
                controller.ChangePostures(LibSumo.Net.Protocol.SumoEnumGenerated.Posture_type.jumper);
            if (activeController.ButtonXPressed)
                controller.StartJump(SumoEnumGenerated.Jump_type._long);
            if (activeController.ButtonYPressed)
                controller.ChangePostures(LibSumo.Net.Protocol.SumoEnumGenerated.Posture_type.standing);


            if (activeController.ButtonUpPressed)
                controller.QuickTurn((float)Math.PI);
            if (activeController.ButtonDownPressed)
                controller.QuickTurn((float)Math.PI);
            if (activeController.ButtonLeftPressed)
                controller.QuickTurn(0.75f * (float)Math.PI);
            if (activeController.ButtonRightPressed)
                controller.QuickTurn(0.5f*(float)Math.PI);


             var speed = (-activeController.TriggerLeftPosition + activeController.TriggerRightPosition) /100;
             var turn = (activeController.ThumbLeftX -50) /50;

            turn = Math.Pow(turn, 3);
            turn *= 0.6;


            var c1Turn = 0.8;
            filtTur = filtTur * c1Turn + (1 - c1Turn) * turn;
            var c1speed = 0.8;
            filtSpeed = filtSpeed * c1speed + (1 - c1speed) * speed;


            speed = filtSpeed;
            turn = filtTur;

            speed *= 100;
            turn *= 100;

            //Limit
            if (speed > 100) speed = 100;
            if (speed < -100) speed = -100;

            //Limit
            if (turn > 100) turn = 100;
            if (turn < -100) turn = -100;

            controller.SendMove((sbyte)(speed), (sbyte)(turn));

        }

        private void SetSplashImage()
        {
            // Set SplashImage
            Assembly _assembly = Assembly.GetExecutingAssembly();
            SplashImage = BitmapConverter.ToMat(new Bitmap(_assembly.GetManifestResourceStream("SumoApplication.Images.SplashScreen.jpg")));

            // add HUD logo on Splash Image for testing
            Mat FinalImage = ImageManipulation.Decorate(SplashImage, sumoInformations);
            imgMain.Source = FinalImage.ToWriteableBitmap();
        }

        private void InitDrone()
        {

            if (controller==null)
                controller = new SumoController(out piloting);

            controller.ImageAvailable += Controller_ImageAvailable;
            controller.SumoEvents += Controller_SumoEvents;

            // Connect Piloting Events to control Sumo
            piloting.Disconnect += Piloting_Disconnect;
            piloting.Move += Piloting_Move;
            piloting.KeyboardKeysAvailable += Piloting_KeyboardKeysAvailable;

            // If you want process video in OpenCV separate window Set EnableOpenCV to true
            // In this case controller.ImageAvailable is not fired
            //controller.EnableOpenCV = true;
            
        }

        private void Btn_MultidronesConnectClick(object sender, RoutedEventArgs e)
        {
            // Get Btn Name
            string host = ((Button)sender).Content.ToString() + ".wifi.intranet.chuv";
            //Main_Ping_Should_run = false;
            IPHostEntry hostIP = Dns.GetHostEntry(host);           
            controller.Connect(hostIP.AddressList[0].ToString());
        }

        
        private void InitUI()
        {            
            panelJump.IsEnabled = sumoInformations.IsCapapableOf(SumoInformations.Capability.Jump);
            panelSettings.IsEnabled = true;
            imgMain.IsEnabled = true;

            cbxWifiBand.ItemsSource = Enum.GetValues(typeof(SumoEnumGenerated.WifiSelection_band)).Cast<SumoEnumGenerated.WifiSelection_band>(); 

            if(MultipleDroneMode)
            {
                btnConnect.Visibility = Visibility.Hidden;
                btnEnableMultiDrones.Visibility = Visibility.Hidden;
            }
            else
            {
                btnConnect.Visibility = Visibility.Visible;
                btnEnableMultiDrones.Visibility = Visibility.Visible;                
            }
        }


        private void InitSoundUI()
        {                                                
            // Init Sound Box
            string[] soundsFiles = Directory.EnumerateFiles(@".\Sound", "*.wav", SearchOption.TopDirectoryOnly).Select(x => Path.GetFileName(x)).ToArray();
            cbxSounds.ItemsSource = soundsFiles;                      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            controller.SetAudioDroneRX(true);
        }
               
        #region Controller CallBack
        private void Controller_SumoEvents(object sender, SumoEventArgs e)
        {
            if (e.SumoInformations == null) return;
            sumoInformations = e.SumoInformations;
            switch(e.TypeOfEvent)
            {
                case (SumoEnumCustom.TypeOfEvents.AlertEvent):
                    break;
                case (SumoEnumCustom.TypeOfEvents.BatteryLevelEvent):                    
                    lblBatteryLevel.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lblBatteryLevel.Content = sumoInformations.BatteryLevel + "%";                        
                    }));
                    break;
                case (SumoEnumCustom.TypeOfEvents.Connected):                    
                    InitUI();                    // Enable Btn
                    break;
                case (SumoEnumCustom.TypeOfEvents.Disconnected):
                    InitUI();
                    SetSplashImage();
                    break;
                case (SumoEnumCustom.TypeOfEvents.Discovered):

                    txtBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LOGGER.GetInstance.Info(String.Format("Sumo {1} is available {0} ", Environment.NewLine, sumoInformations.DeviceName));                        
                    }));

                    // Get btn reference for multipledrone And display it                
                    if (ListOfDevices != null && ListOfDevices.Count > 0)
                    {
                        try
                        {
                            MiniDevice d = ListOfDevices.Find(x => x.Name == sumoInformations.DeviceName);
                            Button btn = d.Button;
                            int suffixe = d.Id;
                            btn.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                btn.Content = Properties.Settings.Default.prefixName + suffixe.ToString();
                                btn.Visibility = Visibility.Visible;
                            }));
                        }
                        catch { }
                    }                                    
                    break;
                case (SumoEnumCustom.TypeOfEvents.PilotingEvent):
                    break;
                case (SumoEnumCustom.TypeOfEvents.PostureEvent):                   
                    lblPostureState.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lblPostureState.Content = "Sumo in: " + sumoInformations.Posture.ToString()+" position";
                    }));
                    break;
                case (SumoEnumCustom.TypeOfEvents.RSSI):
                    lblRssi.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lblRssi.Content = "Wifi Signal : "+ sumoInformations.Rssi.ToString() +" dbm";                        
                    }));
                    break;
                case (SumoEnumCustom.TypeOfEvents.LinkQuality):                    
                    lblQuality.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lblQuality.Content = "Link Quality: " +sumoInformations.LinkQuality.ToString() +"/6";
                        
                    }));
                    break;

                case (SumoEnumCustom.TypeOfEvents.VolumeChange):
                    slVolume.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        slVolume.Value = sumoInformations.Volume;
                    }));
                    break;

                case (SumoEnumCustom.TypeOfEvents.CapabilitiesChange):
                    chkBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        chkBox.IsChecked = sumoInformations.IsCapapableOf(SumoInformations.Capability.Box);                        
                        chkBoost.IsChecked = sumoInformations.IsCapapableOf(SumoInformations.Capability.Boost);
                        pnlAudio.IsEnabled = sumoInformations.IsCapapableOf(SumoInformations.Capability.Audio);
                        slLight.IsEnabled = sumoInformations.IsCapapableOf(SumoInformations.Capability.Light);
                        if (pnlAudio.IsEnabled)
                        {
                            InitSoundUI();
                            controller.InitAudio();
                        }
                    }));
                    break;

                case (SumoEnumCustom.TypeOfEvents.WifiChanged):
                    cbxWifiBand.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        cbxWifiBand.SelectedIndex = (int)sumoInformations.WifiBand;
                    }));
                    break;

                case (SumoEnumCustom.TypeOfEvents.AudioThemeChanged):
                    cbxAudioTheme.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        cbxAudioTheme.SelectedIndex = (int)sumoInformations.AudioTheme;
                    }));
                    break;

                case (SumoEnumCustom.TypeOfEvents.CSTMSupervisorStarted):
                    txtBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LOGGER.GetInstance.Info(String.Format("Multidrone Ping supervisor Started"));
                        
                    }));
                    break;
                case (SumoEnumCustom.TypeOfEvents.BoxError):
                    txtBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LOGGER.GetInstance.Info(String.Format("{0}", sumoInformations.LastErrorStr));
                        sumoInformations.LastErrorStr = "";
                    }));
                    
                    break;
                case (SumoEnumCustom.TypeOfEvents.BoxOpened):
                    txtBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LOGGER.GetInstance.Info(String.Format("Box opened"));                        
                    }));
                    break;
                case (SumoEnumCustom.TypeOfEvents.BoxClosed):
                    txtBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        LOGGER.GetInstance.Info(String.Format("Box closed"));
                    }));
                    break;
            }
          
        }

       

        private void Controller_ImageAvailable(object sender, ImageEventArgs e)
        {
            imgMain.Dispatcher.BeginInvoke((Action)(() =>
            {
                DisplayFPS();
                try
                {
                    var FinalImage = ImageManipulation.Decorate(e.RawImage, sumoInformations);
                    imgMain.Source = FinalImage.ToWriteableBitmap(PixelFormats.Bgr24);                    
                }
                catch
                {
                }
            }));
        }
        
        private void DisplayFPS()
        {
            if (frameWatch.IsRunning)
            {
                frameWatch.Stop();
                if (frameWatch.ElapsedMilliseconds > 0)
                {
                    double fps = (1000 / frameWatch.ElapsedMilliseconds);
                    if (fps < 20) frameRate = fps;
                    lblBatteryLevel.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        lblFramerate.Content = Math.Round(frameRate, 1) + " fps";
                    }));
                }
            }
            else frameWatch = Stopwatch.StartNew();
        }
      
#endregion

#region Piloting CallBack
        private void Piloting_KeyboardKeysAvailable(object sender, KeyboardEventArgs e)
        {            
            HookUtils.VirtualKeyStates key = e.CurrentKey;
            bool IsPressed = e.IsPressed;

            switch((int)key)
            {
                // Postures
                case ((int)HookUtils.VirtualKeyStates.VK_F1): // Normal
                    controller.ChangePostures(LibSumo.Net.Protocol.SumoEnumGenerated.Posture_type.jumper);
                    break;
                case ((int)HookUtils.VirtualKeyStates.VK_F2): // Upside-Down
                    controller.ChangePostures(LibSumo.Net.Protocol.SumoEnumGenerated.Posture_type.kicker);
                    break;
                case (int)(HookUtils.VirtualKeyStates.VK_F3): // Auto-Balance
                    controller.ChangePostures(LibSumo.Net.Protocol.SumoEnumGenerated.Posture_type.standing);
                    break;

                // Quick Turn
                case ((int)HookUtils.VirtualKeyStates.VK_SPACE): // Quick turn of 180°                  
                case (0x57): // Letter w
                    controller.QuickTurn(ToRadians(180)); //Quick turn right
                    break;
                case (0x53): // Letter s
                    controller.QuickTurn(ToRadians(-180));//Quick turn left
                    break;
                case (0x41): // Letter a 
                    controller.QuickTurn(ToRadians(-90)); //Quick half turn left
                    break;
                case (0x44): // Letter d  
                    controller.QuickTurn(ToRadians(90)); //Quick half turn right
                    break;

                //Accessory
                case (0x59): // Letter y  
                    controller.OpenBox(); 
                    break;
                case (0x58): // Letter x  
                    controller.CloseBox(); 
                    break;

                // Animations
                case (0x31): // nuber 1
                    controller.StartAnimation(LibSumo.Net.Protocol.SumoEnumGenerated.SimpleAnimation_id.tap);
                    break;
                case (0x32): // nuber 2
                    controller.StartAnimation(LibSumo.Net.Protocol.SumoEnumGenerated.SimpleAnimation_id.ondulation);
                    break;
                case (0x33): // nuber 3
                    controller.StartAnimation(LibSumo.Net.Protocol.SumoEnumGenerated.SimpleAnimation_id.slowshake);
                    break;
                case (0x34): //Number 4                
                    controller.StartBoost();
                    break;
                
                case (0x51): // letter q
                    controller.Headlight_off();
                    break;
                case (0x45): // letter e
                    controller.Headlight_on();
                    break;

            }

           
        }

        private void Piloting_Move(object sender, MoveEventArgs e)
        {
            // Move the Sumo
            controller.SendMove(e.Speed, e.Turn);

            // Display value on UI
            //HSlider.Dispatcher.BeginInvoke((Action)(() => HSlider.Value = e.Turn));
            //VSlider.Dispatcher.BeginInvoke((Action)(() => VSlider.Value = e.Speed));
            //Vlbl.Dispatcher.BeginInvoke((Action)(() => Vlbl.Content = e.Speed));
            //Hlbl.Dispatcher.BeginInvoke((Action)(() => Hlbl.Content = e.Turn));
        }

        private void Piloting_Disconnect(object sender, EventArgs e)
        {
            controller.Disconnect();
            panelJump.IsEnabled = false;
            panelSettings.IsEnabled = false;
            imgMain.IsEnabled = false;
        }     
#endregion

#region UI
        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            btnEnableMultiDrones.Visibility = Visibility.Hidden;
            controller.Connect();
        }
        private void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            controller.Disconnect();
            SetSplashImage();
        }

        private void BtnSaveLog_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText("sumo.log", txtBox.Text);
        }

        private void BtnLongJump_Click(object sender, RoutedEventArgs e)
        {            
            controller.StartJump(LibSumo.Net.Protocol.SumoEnumGenerated.Jump_type._long);
        }
        private void BtnHighJump_Click(object sender, RoutedEventArgs e)
        {
            controller.StartJump(LibSumo.Net.Protocol.SumoEnumGenerated.Jump_type.high);
        }
        private void BtnKick_Click(object sender, RoutedEventArgs e)
        {
            controller.JumpLoad();
        }
        private void BtnCancelJump_Click(object sender, RoutedEventArgs e)
        {
            controller.CancelJump();
        }
        private void BtnSTOP_Click(object sender, RoutedEventArgs e)
        {
            controller.STOP();
        }
        private void CbxAudioTheme_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {            
            controller.SetAudioTheme((SumoEnumGenerated.Theme_theme)cbxAudioTheme.SelectedItem);
        }
        private void SlVolume_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            controller.ChangeVolume((byte)slVolume.Value);
        }
        private void GetInstance_MessageAvailable(object sender, MessageEventArgs e)
        {
            txtBox.Dispatcher.BeginInvoke((Action)(() =>
            {
                txtBox.AppendText(DateTime.Now.ToString() + " " + e.Level + " : " + e.Message + Environment.NewLine);
                txtBox.ScrollToEnd();
            }));
        }


       


        private void BtnAudioRecord_Click(object sender, RoutedEventArgs e)
        {
            // Send selected Wave to Drone
            // Not working for the moment
            //MessageBox.Show("Not Working for the moment"); return;            
            string item = cbxSounds.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(item))
            {                
                controller.StreamAudioToDrone(item);
            }
        }

        private void SlLight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Send Light level to Drone
            controller.Headlight_Value(e.NewValue);
        }

        private void BtnAudioStreamOn_Click(object sender, RoutedEventArgs e)
        {
            controller.SetAudioDroneTX(true);
        }

        private void BtnAudioStreamOff_Click(object sender, RoutedEventArgs e)
        {
            controller.SetAudioDroneTX(false);
        }


#endregion

        public float ToRadians(float degree)
        {
            return (float)((degree / 2 * Math.PI) / 180.0);
        }

        public void Dispose()
        {
            // Inform controller to quit cleanly
            if (controller != null)
            {
                controller.Disconnect();
                controller.Dispose();
            }
        }

        private void ChkBowl_Checked(object sender, RoutedEventArgs e)
        {                        
            controller.RemoveCapabilities(SumoInformations.Capability.Jump );
            InitUI();
        }

        private void ChkBowl_Unchecked(object sender, RoutedEventArgs e)
        {            
            InitUI();
        }

        private void cbxWifiBand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            controller.setWifiBand((SumoEnumGenerated.WifiSelection_band)cbxWifiBand.SelectedItem);
        }

        private void btnSimulate_Click(object sender, RoutedEventArgs e)
        {
            Simulate sim = new Simulate(ref imgMain, SplashImage, ref sumoInformations);
            sim.Show();
        }

        private void btnRecordVideo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEnableMultiDrones_Click(object sender, RoutedEventArgs e)
        {
            #region Multiple Drones
            
            MultipleDroneMode = true;
            // Get IP of devices if you have multiples drone on the network            
            ListOfDevices = new List<MiniDevice>();
            for (int i = 1; i < 8; i++)
            {
                Button btn = new Button() { Content = "" };
                btn.Visibility = Visibility.Hidden;
                btn.Click += Btn_MultidronesConnectClick;
                stckPanel1.Children.Add(btn);
                string hostname = Properties.Settings.Default.prefixName + i.ToString() + Properties.Settings.Default.DNSString;
                ListOfDevices.Add(new MiniDevice() { Name = hostname, Button = btn, Id = i });
            }

            List<string> listOfName = new List<string>();
            foreach (MiniDevice str in ListOfDevices) listOfName.Add(str.Name);

            // Create Controller
            controller = new SumoController(out piloting, listOfName, true);
            InitDrone();
            InitUI();
            #endregion
        }


        private void ImgMain_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {   // FulScreen
                FullScreen fls = new FullScreen();
                // Save image context
                this.TempImage = this.imgMain;
                // Set new image target
                this.imgMain = fls.imgMain;
                SetSplashImage();                
                this.Visibility = Visibility.Hidden;
                fls.ShowDialog();

                // restaure image context
                this.Visibility = Visibility.Visible;
                this.imgMain = this.TempImage;
            }
        }

        private void enableVideo_Click(object sender, RoutedEventArgs e)
        {
            controller.EnableVideo();
        }

        private void disableVideo_Click(object sender, RoutedEventArgs e)
        {
            controller.DisableVideo();
        }

        private void wifi_Click(object sender, RoutedEventArgs e)
        {
            controller.receiver.ToString();
            controller.SendWifi();
        }
    }     

}
