/* 
   Copyright (c) 2009, Neohapsis, Inc.
   All rights reserved.

 Implementation by Patrick Toomey

 Redistribution and use in source and binary forms, with or without modification, 
 are permitted provided that the following conditions are met: 

  - Redistributions of source code must retain the above copyright notice, this list 
    of conditions and the following disclaimer. 
  - Redistributions in binary form must reproduce the above copyright notice, this 
    list of conditions and the following disclaimer in the documentation and/or 
    other materials provided with the distribution. 
  - Neither the name of Neohapsis nor the names of its contributors may be used to 
    endorse or promote products derived from this software without specific prior 
    written permission. 

 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
 DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR 
 ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES 
 (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON 
 ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
 SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using Fiddler;
using System.Web;
using System.Text.RegularExpressions;

[assembly: Fiddler.RequiredVersion("2.1.1.0")]

namespace ViewState
{
    public class ViewStateViewer : Inspector2, IRequestInspector2
    {

        ViewStateView myControl;
        ViewState viewState;
        private byte[] m_entityBody;
        private bool m_bDirty;
        private bool m_overrideDefaultBReadOnly = false;

        private bool m_bReadOnly;

        public bool bReadOnly
        {
            get
            {
                return m_bReadOnly;
            }
            set
            {
                m_bReadOnly = value;   // TODO: You probably also want to turn your visible control GRAY (false) or WHITE (true) here depending on the value being passed in.  
                // there is one situation where we want to not abide by the default fiddler behavior, so we have a bool to check for that one situation
                if (!m_overrideDefaultBReadOnly)
                {
                    myControl.viewStateEncodedTextBox.ReadOnly = value;
                    myControl.viewStateXMLEncodedTextBox.ReadOnly = value;
                    myControl.encodeButton.Enabled = !value;
                    myControl.decodeButton.Enabled = !value;
                }

            }
        }

        public String bodyString
        {
            get
            {
                return HttpUtility.UrlDecode(System.Text.Encoding.UTF8.GetString(m_entityBody));
            }
        }
        public void encodeButtonPressed(object sender, EventArgs e)
        {
            try
            {
                viewState = ViewState.newViewStateFromXMLString(myControl.viewStateXMLEncodedTextBox.Text);
                if (viewState.m_MACProtected == true)
                {
                    myControl.errorLabel.Text = "Warning: Reencoded ViewState does not contain MAC protection";
                }
                else
                {
                    myControl.errorLabel.Text = "";
                }
                myControl.viewStateEncodedTextBox.Text = viewState.viewStateBase64;
                myControl.MACValueLabel.Text = viewState.MAC;
                myControl.versionValueLabel.Text = viewState.versionString;
                String viewStateString = getViewStateString(bodyString);
                m_entityBody = System.Text.Encoding.UTF8.GetBytes(bodyString.Replace(viewStateString, HttpUtility.UrlEncode(myControl.viewStateEncodedTextBox.Text)));
                m_bDirty = true;
            }
            catch (Exception)
            {
                myControl.errorLabel.Text = "Error: Cannot encode malformed XML";
                myControl.MACValueLabel.Text = "Unknown";
                myControl.versionValueLabel.Text = "Unknown";

            }

        }

        public void decodeButtonPressed(object sender, EventArgs e)
        {

            try
            {
                myControl.errorLabel.Text = "";
                viewState = ViewState.newViewStateFromBase64String(myControl.viewStateEncodedTextBox.Text);
                String viewStateString = getViewStateString(bodyString);
                myControl.viewStateXMLEncodedTextBox.Text = viewState.viewStateXML;
                myControl.MACValueLabel.Text = viewState.MAC;
                myControl.versionValueLabel.Text = viewState.versionString;
                m_entityBody = System.Text.Encoding.UTF8.GetBytes(bodyString.Replace(viewStateString, HttpUtility.UrlEncode(myControl.viewStateEncodedTextBox.Text)));
                m_bDirty = true;
            }
            catch (Exception)
            {
                myControl.errorLabel.Text = "Error: Cannot decode malformed, encrypted, or unsupported ViewState";
                myControl.MACValueLabel.Text = "Unknown";
                myControl.versionValueLabel.Text = "Unknown";

            }

        }

        public void Clear()
        {
            m_entityBody = null;

            //clear out all the UI Elements
            myControl.errorLabel.Text = "";
            myControl.MACValueLabel.Text = "";
            myControl.versionValueLabel.Text = "";
            myControl.viewStateEncodedTextBox.Text = "No ViewState";
            myControl.viewStateXMLEncodedTextBox.Text = "No ViewState";
            myControl.encodeButton.Enabled = true;
            myControl.decodeButton.Enabled = true;
            m_overrideDefaultBReadOnly = false;
        }

        public ViewStateViewer()
        {

        }

        public HTTPRequestHeaders headers
        {
            get
            {
                return null;    // Return null if your control doesn't allow header editing.
            }
            set
            {
            }
        }

        public byte[] body
        {
            get
            {
                return m_entityBody;
            }
            set
            {
                // clear out all UI elements before we go mucking with them
                Clear();
                // Here's where the action is.  It's time to update the visible display of the text
                m_entityBody = value;

                // if the body doesn't contain viewstate we don't let the user make any edits to the body
                if (m_entityBody == null || getViewStateString(bodyString) == "")
                {
                    myControl.errorLabel.Text = "No ViewState";
                    myControl.viewStateEncodedTextBox.Text = "";
                    myControl.viewStateXMLEncodedTextBox.Text = "";
                    myControl.viewStateEncodedTextBox.ReadOnly = true;
                    myControl.viewStateXMLEncodedTextBox.ReadOnly = true;
                    myControl.encodeButton.Enabled = false;
                    myControl.decodeButton.Enabled = false;
                    m_overrideDefaultBReadOnly = true;
                    m_bDirty = true;
                    return;
                }

                try
                {
                    String viewStateString = getViewStateString(bodyString);
                    myControl.viewStateEncodedTextBox.Text = viewStateString;
                    viewState = ViewState.newViewStateFromBase64String(viewStateString);
                    myControl.viewStateXMLEncodedTextBox.Text = viewState.viewStateXML;
                    myControl.MACValueLabel.Text = viewState.MAC;
                    myControl.versionValueLabel.Text = viewState.versionString;
                    myControl.errorLabel.Text = "";
                }
                catch (Exception)
                {
                    myControl.viewStateXMLEncodedTextBox.Text = "";
                    myControl.errorLabel.Text = "Error: Cannot decode malformed, encrypted, or unsupported ViewState" ;
                    myControl.MACValueLabel.Text = "Unknown";
                    myControl.versionValueLabel.Text = "Unknown";
                }

                m_bDirty = true;   // Note: Be sure to have an OnTextChanged handler for the textbox which sets m_bDirty to true!
            }
        }

        private String getViewStateString(String htmlBody)
        {
            String viewStateString = "";
            int viewStateIndex = htmlBody.IndexOf("__VIEWSTATE=");
            //  String foobar = "";
            if (viewStateIndex >= 0)
            {
                viewStateIndex += "__VIEWSTATE=".Length;
                viewStateString = htmlBody.Substring(viewStateIndex, htmlBody.IndexOf("&", viewStateIndex) - viewStateIndex);
            }
            return viewStateString;
        }
        public bool bDirty
        {
            get
            {
                return m_bDirty;
            }
        }

        public override int GetOrder()
        {
            return 0;
        }

        public override void AddToTab(System.Windows.Forms.TabPage o)
        {
            myControl = new ViewStateView(this);   // Essentially the TextView class is simply a usercontrol containing a textbox.
            o.Text = "ViewState";
            o.Controls.Add(myControl);
            o.Controls[0].Dock = DockStyle.Fill;

        }

    }
}

