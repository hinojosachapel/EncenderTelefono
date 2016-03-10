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
using System.Windows.Forms;
using Microsoft.WindowsMobile.Status;

#endregion

namespace RH.MobilePhone
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Enciende el teléfono (la radio del móvil)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEncender_Click(object sender, EventArgs e)
        {
            if (SystemState.PhoneRadioOff == false)
            {
                MessageBox.Show("El teléfono está encendido.");
            }
            else
            {
                MobileRadio.SetDeviceState(MobileRadio.RADIODEVTYPE.PHONE, MobileRadio.RADIODEVSTATE.ON);
            }
        }

        /// <summary>
        /// Apaga el teléfono (la radio del móvil)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (SystemState.PhoneRadioOff)
            {
                MessageBox.Show("El teléfono está apagado.");
            }
            else
            {
                MobileRadio.SetDeviceState(MobileRadio.RADIODEVTYPE.PHONE, MobileRadio.RADIODEVSTATE.OFF);
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateStatus();            
        }

        private void UpdateStatus()
        {
            if (Microsoft.WindowsMobile.Status.SystemState.PhoneRadioOff)
            {
                label1.Text = "Estado: Apagado";
            }
            else
            {
                label1.Text = "Estado: Encendido";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateStatus();
        }
    }
}