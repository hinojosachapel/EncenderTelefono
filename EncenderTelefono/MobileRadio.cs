// Cómo encender el teléfono en Windows Mobile con C#

#region License

/* Copyright (c) 2010 Rubén Hinojosa Chapel
 * 
 * Based on
 *    Toggle Mobile Radios
 *    Tim D Garrett
 *    Intermec Technologies
 *    http://community.intermec.com/t5/General-Development-Developer/Turn-radio-on-and-off-as-needed/m-p/138
 *   
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Rubén Hinojosa Chapel
 * http://www.hinojosachapel.com
 */

#endregion

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace RH.MobilePhone
{
    class MobileRadio
    {
        [DllImport("ossvcs.dll", EntryPoint = "#276", CharSet = CharSet.Unicode)]
        private static extern uint GetWirelessDevice(ref IntPtr pDevice, int pDevVal);

        [DllImport("ossvcs.dll", EntryPoint = "#273", CharSet = CharSet.Unicode)]
        private static extern uint ChangeRadioState(ref RDD pDevice, int dwState, int saveAction);

        [DllImport("ossvcs.dll", EntryPoint = "#280", CharSet = CharSet.Unicode)]
        private static extern uint FreeDeviceList(IntPtr pDevice);

        [StructLayout(LayoutKind.Auto)]
        public struct RADIODEVSTATE
        {
            public const int ON = 1;
            public const int OFF = 0;
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        public struct RADIODEVTYPE
        {
            public const int WIFI = 1;
            public const int PHONE = 2;
            public const int BLUETOOTH = 3;
        }

        [StructLayout(LayoutKind.Auto, CharSet = CharSet.Unicode)]
        struct SAVEACTION
        {
            public const int DONT_SAVE = 0;
            public const int PRE_SAVE = 1;
            public const int POST_SAVE = 2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct RDD
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszDeviceName;

            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszDisplayName;

            public uint dwState;
            public uint dwDesired;
            public int DeviceType;
            public IntPtr pNext;
        }

        public static bool SetDeviceState(int dwDevice, int dwState)
        {
            IntPtr pDevice = new IntPtr(0);
            RDD device;
            uint result;

            //Get the first wireless device
            result = GetWirelessDevice(ref pDevice, 0);
            if (result != 0)
                return false;

            //If the first device has been found
            if (pDevice != null)
            {
                //While we're still looking at wireless devices
                while (pDevice != IntPtr.Zero)
                {
                    //Marshall the pointer into a C# structure
                    device = (RDD)System.Runtime.InteropServices.Marshal.PtrToStructure(pDevice, typeof(RDD));

                    //If this device is the device we're looking for
                    if (device.DeviceType == dwDevice)
                    {
                        //Change the state of the radio
                        result = ChangeRadioState(ref device, dwState, SAVEACTION.PRE_SAVE);
                    }

                    //Set the pointer to the next device in the linked list
                    pDevice = device.pNext;
                }

                //Free the list of devices
                FreeDeviceList(pDevice);
            }

            //Turning off radios doesn't correctly report the status, so return true anyway.
            if (result == 0 || dwState == RADIODEVSTATE.OFF)
                return true;

            return false;
        }
    }
}
