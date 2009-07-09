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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ViewState
{
    public partial class ViewStateView : UserControl
    {
        private ViewStateViewer viewStateViewer;
        public ViewStateView(ViewStateViewer viewStateViewer)
        {
            this.viewStateViewer = viewStateViewer;
            InitializeComponent();
        }

        private void encodeButtonClicked(object sender, EventArgs e)
        {
            this.viewStateViewer.encodeButtonPressed(sender, e);
        }

        private void decodeButtonClicked(object sender, EventArgs e)
        {
            this.viewStateViewer.decodeButtonPressed(sender, e);
        }

        private void TextView_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void viewStateXMLEncodedTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
