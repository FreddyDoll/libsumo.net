﻿using LibSumo.Net.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibSumo.Net.Protocol
{
    internal  class SumoCommandsGenerated
    {
        // Autogenerated below this line:
        // If you find error, dont correct-it here
        // See ARSdk3To.Net ConvertTools

        /// <summary>
        /// Settings_AllSettings 
        /// Ask for all settings.
        /// **Please note that you should not send this command if you are using the
        /// libARController API as this library is handling the connection process for you.** 
        /// The product will trigger all settings events (such as [CameraSettings](#0-15-0), or product specific settings as the [MaxAltitude](#1-6-0) for the Bebop).
        /// Then, it will trigger [AllSettingsEnd](#0-3-0). 
        /// </summary>
        public static byte[] Settings_AllSettings_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 2, 0);
        }

        /// <summary>
        /// Settings_Reset 
        /// Reset all settings. 
        /// It will trigger [ResetChanged](#0-3-1).
        /// Then, the product will trigger all settings events (such as [CameraSettings](#0-15-0), or product specific settings as the [MaxAltitude](#1-6-0) for the Bebop) with factory values. 
        /// </summary>
        public static byte[] Settings_Reset_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 2, 1);
        }

        /// <summary>
        /// Settings_ProductName 
        /// Set the product name.
        /// It also sets the name of the SSID for Wifi products and advertisement name for BLE products (changed after a reboot of the product). 
        /// Name is changed.
        /// Then, it will trigger [NameChanged](#0-3-2). 
        /// args : string 
        /// </summary>
        public static byte[] Settings_ProductName_cmd(String name)
        {
            return StructConverter.Pack("<BBHs", 0, 2, 2, name);
        }

        /// <summary>
        /// Settings_Country 
        /// Set the country for Wifi products.
        /// This can modify Wifi band and/or channel.
        /// **Please note that you might be disconnected from the product after changing the country as it changes Wifi parameters.** 
        /// The country is set.
        /// Then, it will trigger [CountryChanged](#0-3-6). 
        /// args : string 
        /// </summary>
        public static byte[] Settings_Country_cmd(String code)
        {
            return StructConverter.Pack("<BBHs", 0, 2, 3, code);
        }

        /// <summary>
        /// Settings_AutoCountry 
        /// Enable auto-country.
        /// If auto-country is set, the drone will guess its Wifi country by itself by checking other Wifi country around it.
        /// **Please note that you might be disconnected from the product after changing the country as it changes Wifi parameters.** 
        /// The auto-country of the product is changed.
        /// Then, it will trigger [AutoCountryChanged](#0-3-7) and [CountryChanged](#0-3-6). 
        /// args : u8 
        /// </summary>
        public static byte[] Settings_AutoCountry_cmd(byte automatic)
        {
            return StructConverter.Pack("<BBHB", 0, 2, 4, automatic);
        }

        /// <summary>
        /// Common_AllStates 
        /// Ask for all states.
        /// **Please note that you should not send this command if you are using the
        /// libARController API as this library is handling the connection process for you.** 
        /// The product will trigger all states events (such as [FlyingState](#1-4-1) for the Bebop).
        /// Then, it will trigger [AllStatesEnd](#0-5-0). 
        /// </summary>
        public static byte[] Common_AllStates_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 4, 0);
        }

        /// <summary>
        /// Common_CurrentDate 
        /// Set the date.
        /// This date is taken by the drone as its own date.
        /// So medias and other files will be dated from this date
        /// **Please note that you should not send this command if you are using the
        /// libARController API as this library is handling the connection process for you.** 
        /// The date of the product is set.
        /// Then, it will trigger [DateChanged](#0-5-4). 
        /// args : string 
        /// </summary>
        public static byte[] Common_CurrentDate_cmd(String date)
        {
            return StructConverter.Pack("<BBHs", 0, 4, 1, date);
        }

        /// <summary>
        /// Common_CurrentTime 
        /// Set the time.
        /// This time is taken by the drone as its own time.
        /// So medias and other files will be dated from this time
        /// **Please note that you should not send this command if you are using the
        /// libARController API as this library is handling the connection process for you.** 
        /// The time of the product is set.
        /// Then, it will trigger [TimeChanged](#0-5-5). 
        /// args : string 
        /// </summary>
        public static byte[] Common_CurrentTime_cmd(String time)
        {
            return StructConverter.Pack("<BBHs", 0, 4, 2, time);
        }

        /// <summary>
        /// Common_Reboot 
        /// Reboot the product.
        /// The product will accept this command only if is not flying. 
        /// The product will reboot if it can. 
        /// </summary>
        public static byte[] Common_Reboot_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 4, 3);
        }

        /// <summary>
        /// Controller_isPiloting 
        /// Inform about hud entering.
        /// Tell the drone that the controller enters/leaves the piloting hud.
        /// On a non-flying products it is used to know when a run begins. 
        /// If yes, the product will begin a new session (so it should send a new [runId](#0-30-0)).
        /// Also, on the JumpingSumos, if the video is in autorecord mode, it will start recording. 
        /// args : u8 
        /// </summary>
        public static byte[] Controller_isPiloting_cmd(byte piloting)
        {
            return StructConverter.Pack("<BBHB", 0, 8, 0, piloting);
        }

        /// <summary>
        /// WifiSettings_OutdoorSetting 
        /// Set wifi indoor/outdoor mode.
        /// **Please note that you might be disconnected from the product after changing the indoor/outdoor setting as it changes Wifi parameters.** 
        /// The product change its indoor/outdoor wifi settings.
        /// Then, it will trigger [WifiOutdoorMode](#0-10-0). 
        /// args : u8 
        /// </summary>
        public static byte[] WifiSettings_OutdoorSetting_cmd(byte outdoor)
        {
            return StructConverter.Pack("<BBHB", 0, 9, 0, outdoor);
        }

        /// <summary>
        /// Mavlink_Start 
        /// Start a FlightPlan based on a mavlink file existing on the drone.
        /// Requirements are:
        /// * Product is calibrated
        /// * Product should be in outdoor mode
        /// * Product has fixed its GPS
        /// 
        /// If the FlightPlan has been started, event [FlightPlanPlayingStateChanged](#0-12-0) is triggered with param state set to *playing*.
        /// Otherwise, event [FlightPlanPlayingStateChanged](#0-12-0) is triggered with param state set to stopped and event [MavlinkPlayErrorStateChanged](#0-12-1) is triggered with an explanation of the error. 
        /// args : string enum 
        /// </summary>
        public static byte[] Mavlink_Start_cmd(String filepath, SumoEnumGenerated.Start_type type)
        {
            return StructConverter.Pack("<BBHsI", 0, 11, 0, filepath, (UInt16)type);
        }

        /// <summary>
        /// Mavlink_Pause 
        /// Pause a FlightPlan that was playing.
        /// To unpause a FlightPlan, see [StartFlightPlan](#0-11-0)
        /// 
        /// The currently playing FlightPlan will be paused. Then, event [FlightPlanPlayingStateChanged](#0-12-0) is triggered with param state set to the current state of the FlightPlan (should be *paused* if everything went well). 
        /// </summary>
        public static byte[] Mavlink_Pause_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 11, 1);
        }

        /// <summary>
        /// Mavlink_Stop 
        /// Stop a FlightPlan that was playing.
        /// 
        /// The currently playing FlightPlan will be stopped. Then, event [FlightPlanPlayingStateChanged](#0-12-0) is triggered with param state set to the current state of the FlightPlan (should be *stopped* if everything went well). 
        /// </summary>
        public static byte[] Mavlink_Stop_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 11, 2);
        }

        /// <summary>
        /// FlightPlanSettings_ReturnHomeOnDisconnect 
        /// Set ReturnHome behavior during FlightPlan
        /// When set, drone will return home, after return home delay, if a disconnection occurs during execution of FlightPlan 
        /// The return home mode is enabled or disabled.
        /// Then, event [ReturnHomeOnDisconnectionChanged](#0-33-0) is triggered. 
        /// args : u8 
        /// </summary>
        public static byte[] FlightPlanSettings_ReturnHomeOnDisconnect_cmd(byte value)
        {
            return StructConverter.Pack("<BBHB", 0, 32, 0, value);
        }

        /// <summary>
        /// Calibration_MagnetoCalibration 
        /// Start or abort magnetometer calibration process.
        /// 
        /// The magnetometer calibration process is started or aborted. Then, event [MagnetoCalibrationStartedChanged](#0-14-3) is triggered.
        /// If started, event [MagnetoCalibrationStateChanged](#0-14-3) is triggered with the current calibration state: a list of all axis and their calibration states.
        /// It will also trigger [MagnetoCalibrationAxisToCalibrateChanged](#0-14-2), that will inform the controller about the current axis to calibrate. 
        /// args : u8 
        /// </summary>
        public static byte[] Calibration_MagnetoCalibration_cmd(byte calibrate)
        {
            return StructConverter.Pack("<BBHB", 0, 13, 0, calibrate);
        }

        /// <summary>
        /// Calibration_PitotCalibration 
        /// Start or abort Pitot tube calibration process.
        /// 
        /// The pitot calibration process is started or aborted. Then, event [PitotCalibrationStateChanged](#0-14-4) is triggered with the current calibration state. 
        /// args : u8 
        /// </summary>
        public static byte[] Calibration_PitotCalibration_cmd(byte calibrate)
        {
            return StructConverter.Pack("<BBHB", 0, 13, 1, calibrate);
        }

        /// <summary>
        /// GPS_ControllerPositionForRun 
        /// Set the position of a run.
        /// This will let the product know the controller location for the flight/run. The location is typically used to geotag medias.
        /// Only used on products that have no gps.
        /// Watch out, this command is not used by BLE products. 
        /// The position is set. 
        /// args : double double 
        /// </summary>
        public static byte[] GPS_ControllerPositionForRun_cmd(double latitude, double longitude)
        {
            return StructConverter.Pack("<BBHdd", 0, 16, 0, latitude, longitude);
        }

        /// <summary>
        /// Audio_ControllerReadyForStreaming 
        /// Set audio stream direction. 
        /// The audio stream direction is set.
        /// Then, event [AudioStreamDirection](#0-21-0) is triggered. 
        /// args : u8 
        /// </summary>
        public static byte[] Audio_ControllerReadyForStreaming_cmd(byte ready)
        {
            return StructConverter.Pack("<BBHB", 0, 20, 0, ready);
        }

        /// <summary>
        /// Headlights_intensity 
        /// Set lighting LEDs intensity. 
        /// The intensity of the LEDs is changed.
        /// Then, event [LedIntensity](#0-23-0) is triggered. 
        /// args : u8 u8 
        /// </summary>
        public static byte[] Headlights_intensity_cmd(byte left, byte right)
        {
            return StructConverter.Pack("<BBHBB", 0, 22, 0, left, right);
        }

        /// <summary>
        /// Animations_StartAnimation 
        /// Start a paramaterless animation.
        /// List of available animations can be retrieved from [AnimationsStateList](#0-25-0). 
        /// If possible, the product starts the requested animation. Then, event [AnimationsStateList](#0-25-0) is triggered. 
        /// args : enum 
        /// </summary>
        public static byte[] Animations_StartAnimation_cmd(SumoEnumGenerated.StartAnimation_anim anim)
        {
            return StructConverter.Pack("<BBHI", 0, 24, 0, (UInt16)anim);
        }

        /// <summary>
        /// Animations_StopAnimation 
        /// Stop a paramaterless animation.
        /// List of running animations can be retrieved from [AnimationsStateList](#0-25-0). 
        /// If the requested animation was running, it will be stopped.
        /// Then, event [AnimationsStateList](#0-25-0) is triggered. 
        /// args : enum 
        /// </summary>
        public static byte[] Animations_StopAnimation_cmd(SumoEnumGenerated.StopAnimation_anim anim)
        {
            return StructConverter.Pack("<BBHI", 0, 24, 1, (UInt16)anim);
        }

        /// <summary>
        /// Animations_StopAllAnimations 
        /// Stop all running paramaterless animations.
        /// List of running animations can be retrieved from [AnimationsStateList](#0-25-0). 
        /// All running animations are stopped.
        /// Then, event [AnimationsStateList](#0-25-0) is triggered. 
        /// </summary>
        public static byte[] Animations_StopAllAnimations_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 24, 2);
        }

        /// <summary>
        /// Accessory_Config 
        /// Declare an accessory.
        /// You can choose the accessory between all accessible for this product.
        /// You can get this list through event [SupportedAccessories](#0-27-0).
        /// You can only set the accessory when the modification is enabled.
        /// You can know if it possible with the event [AccessoryDeclarationAvailability](#0-27-2). 
        /// The product knows which accessory it is wearing.
        /// Then, event [AccessoryConfigChanged](#0-27-1) is triggered. 
        /// args : enum 
        /// </summary>
        public static byte[] Accessory_Config_cmd(SumoEnumGenerated.Config_accessory accessory)
        {
            return StructConverter.Pack("<BBHI", 0, 26, 0, (UInt16)accessory);
        }

        /// <summary>
        /// Factory_Reset 
        /// This command will request a factory reset from the prodcut. *The factory reset procedure implies an automatic reboot*, which will be done immediately after receiving this command. 
        /// The product will reboot, all settings will be reset to their default values. All data on the product will also be erased. 
        /// </summary>
        public static byte[] Factory_Reset_cmd()
        {
            return StructConverter.Pack("<BBH", 0, 31, 0);
        }

        /// <summary>
        /// Piloting_PCMD 
        /// 
        /// Ask the JS speed and turn ratio.
        ///  
        ///  
        /// args : u8 i8 i8 
        /// </summary>
        public static byte[] Piloting_PCMD_cmd(byte flag, sbyte speed, sbyte turn)
        {
            return StructConverter.Pack("<BBHBbb", 3, 0, 0, flag, speed, turn);
        }

        /// <summary>
        /// Piloting_Posture 
        /// 
        /// Request a posture
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] Piloting_Posture_cmd(SumoEnumGenerated.Posture_type type)
        {
            return StructConverter.Pack("<BBHI", 3, 0, 1, (UInt16)type);
        }

        /// <summary>
        /// Piloting_addCapOffset 
        /// 
        /// Add the specified offset to the current cap.
        ///  
        ///  
        /// args : float 
        /// </summary>
        public static byte[] Piloting_addCapOffset_cmd(float offset)
        {
            return StructConverter.Pack("<BBHd", 3, 0, 2, offset);
        }

        /// <summary>
        /// Animations_JumpStop 
        /// 
        /// Stop jump, emergency jump stop, stop jump motor and stay there.

        ///  
        /// </summary>
        public static byte[] Animations_JumpStop_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 2, 0);
        }

        /// <summary>
        /// Animations_JumpCancel 
        /// 
        /// Cancel jump and come back to previous state (if possible).

        ///  
        /// </summary>
        public static byte[] Animations_JumpCancel_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 2, 1);
        }

        /// <summary>
        /// Animations_JumpLoad 
        /// 
        /// Request jump loading

        ///  
        /// </summary>
        public static byte[] Animations_JumpLoad_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 2, 2);
        }

        /// <summary>
        /// Animations_Jump 
        /// 
        /// Request a jump
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] Animations_Jump_cmd(SumoEnumGenerated.Jump_type type)
        {
            return StructConverter.Pack("<BBHI", 3, 2, 3, (UInt16)type);
        }

        /// <summary>
        /// Animations_SimpleAnimation 
        /// 
        /// Play a parameterless animation.
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] Animations_SimpleAnimation_cmd(SumoEnumGenerated.SimpleAnimation_id id)
        {
            return StructConverter.Pack("<BBHI", 3, 2, 4, (UInt16)id);
        }

        /// <summary>
        /// MediaRecord_PictureV2 
        /// 
        /// Take picture

        ///  
        /// </summary>
        public static byte[] MediaRecord_PictureV2_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 6, 2);
        }

        /// <summary>
        /// MediaRecord_VideoV2 
        /// 
        /// Video record
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] MediaRecord_VideoV2_cmd(SumoEnumGenerated.VideoV2_record record)
        {
            return StructConverter.Pack("<BBHI", 3, 6, 3, (UInt16)record);
        }

        /// <summary>
        /// NetworkSettings_WifiSelection 
        /// 
        /// Auto-select channel of choosen band
        ///  
        ///  
        /// args : enum enum u8 
        /// </summary>
        public static byte[] NetworkSettings_WifiSelection_cmd(SumoEnumGenerated.WifiSelection_type type, SumoEnumGenerated.WifiSelection_band band, byte channel)
        {
            return StructConverter.Pack("<BBHIIB", 3, 8, 0, (UInt16)type, (UInt16)band, channel);
        }

        /// <summary>
        /// Network_WifiScan 
        /// 
        /// Launches wifi network scan
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] Network_WifiScan_cmd(SumoEnumGenerated.WifiScan_band band)
        {
            return StructConverter.Pack("<BBHI", 3, 10, 0, (UInt16)band);
        }

        /// <summary>
        /// Network_WifiAuthChannel 
        /// 
        /// Controller inquire the list of authorized wifi channels.

        ///  
        /// </summary>
        public static byte[] Network_WifiAuthChannel_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 10, 1);
        }

        /// <summary>
        /// AudioSettings_MasterVolume 
        /// 
        /// Master volume control.
        ///  
        ///  
        /// args : u8 
        /// </summary>
        public static byte[] AudioSettings_MasterVolume_cmd(byte volume)
        {
            return StructConverter.Pack("<BBHB", 3, 12, 0, volume);
        }

        /// <summary>
        /// AudioSettings_Theme 
        /// 
        /// Audio Theme.
        ///  
        ///  
        /// args : enum 
        /// </summary>
        public static byte[] AudioSettings_Theme_cmd(SumoEnumGenerated.Theme_theme theme)
        {
            return StructConverter.Pack("<BBHI", 3, 12, 1, (UInt16)theme);
        }

        /// <summary>
        /// RoadPlan_AllScriptsMetadata 
        /// 
        /// Command to ask device all metadata scripts.

        ///  
        /// </summary>
        public static byte[] RoadPlan_AllScriptsMetadata_cmd()
        {
            return StructConverter.Pack("<BBH", 3, 14, 0);
        }

        /// <summary>
        /// RoadPlan_ScriptUploaded 
        /// 
        /// Notify device that a new file has been uploaded.
        ///  
        ///  
        /// args : string string 
        /// </summary>
        public static byte[] RoadPlan_ScriptUploaded_cmd(String uuid, String md5Hash)
        {
            return StructConverter.Pack("<BBHss", 3, 14, 1, uuid, md5Hash);
        }

        /// <summary>
        /// RoadPlan_ScriptDelete 
        /// 
        /// Ask the device to delete a script.
        ///  
        ///  
        /// args : string 
        /// </summary>
        public static byte[] RoadPlan_ScriptDelete_cmd(String uuid)
        {
            return StructConverter.Pack("<BBHs", 3, 14, 2, uuid);
        }

        /// <summary>
        /// RoadPlan_PlayScript 
        /// 
        /// Ask the device to play a script.
        ///  
        ///  
        /// args : string 
        /// </summary>
        public static byte[] RoadPlan_PlayScript_cmd(String uuid)
        {
            return StructConverter.Pack("<BBHs", 3, 14, 3, uuid);
        }

        /// <summary>
        /// MediaStreaming_VideoEnable 
        /// 
        /// Enable/disable video streaming.
        ///  
        ///  
        /// args : u8 
        /// </summary>
        public static byte[] MediaStreaming_VideoEnable_cmd(byte enable)
        {
            return StructConverter.Pack("<BBHB", 3, 18, 0, enable);
        }

        /// <summary>
        /// VideoSettings_Autorecord 
        /// 
        /// Set video automatic recording state.
        ///  
        ///  
        /// args : u8 
        /// </summary>
        public static byte[] VideoSettings_Autorecord_cmd(byte enabled)
        {
            return StructConverter.Pack("<BBHB", 3, 21, 0, enabled);
        }



    }
}
