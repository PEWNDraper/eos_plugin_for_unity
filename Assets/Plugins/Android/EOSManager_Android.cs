/*
* Copyright (c) 2021 PlayEveryWare
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Epic.OnlineServices.Platform;
using Epic.OnlineServices;
using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Logging;
using System.Runtime.InteropServices;
using UnityEngine.Assertions;
using System;

using jint = System.Int32;
using jsize = System.Int32;
using JavaVM = System.IntPtr;

#if UNITY_ANDROID && !UNITY_EDITOR
namespace PlayEveryWare.EpicOnlineServices
{
    //-------------------------------------------------------------------------
    // Android specific Unity Parts.
    public partial class EOSManager
    {
        static EOSAndroidInitializeOptions androidInitOptions;
        static IntPtr androidInitializeOptionsAllocH;

        [DllImport("UnityHelpers_Android")]
        private static extern JavaVM UnityHelpers_GetJavaVM();

        //-------------------------------------------------------------------------
        static string GetTempDir()
        {
            return Application.temporaryCachePath;
        }

        //-------------------------------------------------------------------------
        static private void ConfigureAndroidActivity()
        {
            Debug.Log("EOSAndroid: Getting activity context...");
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");

            if(context != null)
            {
                Debug.Log("EOSAndroid: activity context found!");
                AndroidJavaClass pluginClass = new AndroidJavaClass("com.epicgames.mobile.eossdk.EOSSDK");

                Debug.Log("EOSAndroid: call EOS SDK init.");
                pluginClass.CallStatic("init", context);
            }
            else
            {
                Debug.LogError("EOSAndroid: activity context is null!");
            }
        }

        //-------------------------------------------------------------------------
        // This does some work to configure the Android side of things before doing the
        // 'normal' EOS init things.
        // TODO: Configure the internal and external directory
        static private void ConfigureSystemInitOptions(ref InitializeOptions initializeOptions)
        {
            ConfigureAndroidActivity();

            // It should be safe to assume there is only one JVM, because
            // android assumes there is only one
            JavaVM javavm = UnityHelpers_GetJavaVM();

            Assert.IsTrue(javavm != IntPtr.Zero, "Fetched JavaVM is Null!");
            androidInitOptions = new EOSAndroidInitializeOptions
            {
                ApiVersion = 1,
                           VM = javavm,
                           OptionalInternalDirectory = IntPtr.Zero,
                           OptionalExternalDirectory = IntPtr.Zero
            };

            androidInitializeOptionsAllocH = Marshal.AllocHGlobal(Marshal.SizeOf<EOSAndroidInitializeOptions>());
            Marshal.StructureToPtr(androidInitOptions, androidInitializeOptionsAllocH, false);
            initializeOptions.SystemInitializeOptions = androidInitializeOptionsAllocH; 
        }

        //-------------------------------------------------------------------------
        static private Options CreateSystemPlatformOption()
        {
            return new Options();
        }

        //-------------------------------------------------------------------------
        static private void ConfigureSystemPlatformCreateOptions(ref Options createOptions)
        {
        }
    }
}
#endif
