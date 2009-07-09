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
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace ViewState
{
    class ViewState_Dot_Net_1_0 : ViewState
    {


        public ViewState_Dot_Net_1_0(String viewStateBase64)
            : base(viewStateBase64)
        {

        }

        public ViewState_Dot_Net_1_0(XmlDocument viewStateXMLDocument)
            : base(viewStateXMLDocument)
        {

        }

        protected override void computeMACInfo()
        {
            if (m_viewStateBase64 == "")
            {
                return;
            }

            // this whole set of steps is a little cludgly, but there doesn't seem to be a better way for now.  We detect MAC info by
            // getting comparing the size of the original viewstate base64 decoded and the original viewstate decoded and then reencoded.  If the two objects
            // don't match in size and the difference between before and after is 20 bytes we assume there is MAC protection.
            byte[] originalViewStateDeserialized = System.Convert.FromBase64String(m_viewStateBase64);
            int arrowCount = 0;
            int i = 0;
            for(i = 0; i < originalViewStateDeserialized.Length; i++)
            {
                if (originalViewStateDeserialized[i] == (byte)'<')
                    arrowCount++;
                else if (originalViewStateDeserialized[i] == (byte)'>')
                    arrowCount--;
                else
                    continue;
                // if we find the closing ">" then we assume we are either at the end of the viewstate or their is trailing MAC info
                if (arrowCount == 0) 
                {
                    i++; // we bump the counter up to skip past the final ">"
                    break;
                }
                
            }
            // if we have 20 bytes left we assume there is MAC protection 
            if (originalViewStateDeserialized.Length - i == 20)
            {
                m_MACProtected = true;
                ArrayList tempList = new ArrayList(originalViewStateDeserialized);
                byte[] tempArray = new byte[20];  //the size of the MAC
                int MACIndex = originalViewStateDeserialized.Length - 20; // 20 is the size of the MAC 
                tempList.GetRange(MACIndex, 20).CopyTo(tempArray);
                String hexString = BitConverter.ToString(tempArray);
                m_MAC = hexString.Replace("-", "");
            } else 
            {
                m_MACProtected = false;
                    
            }
         }

        protected override String getViewStateXMLFromBase64(String viewStateBase64)
        {
            XmlDocument dom = new XmlDocument();
            XmlElement element = null;
            dom.AppendChild(dom.CreateElement("ViewState"));
            element = dom.CreateElement("Version");
            element.InnerText = m_version.ToString();
            dom.DocumentElement.AppendChild(element);
            element = dom.CreateElement("VersionString");
            element.InnerText = m_versionString;
            dom.DocumentElement.AppendChild(element);
            element = dom.CreateElement("MAC");
            element.InnerText = m_MAC;
            dom.DocumentElement.AppendChild(element);
            element = dom.CreateElement("ViewStateDeserialized");
            element.InnerText = getViewStateStringWithoutMAC();
            dom.DocumentElement.AppendChild(element);
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            dom.Save(writer);
            return sb.ToString();
            
        }

        private String getViewStateStringWithoutMAC()
        {
            int offset = 0;
            if (m_MACProtected)
            {
                offset = 20;
            }
            ArrayList tempList = new ArrayList(System.Convert.FromBase64String(m_viewStateBase64));
            byte[] tempArray = new byte[tempList.Count - offset];  //the size of the MAC
            tempList.GetRange(0, tempList.Count - offset).CopyTo(tempArray);
            return System.Text.Encoding.UTF8.GetString(tempArray);
        }
        protected override String getViewStateBase64FromXMLTree(String viewStateXML)
        {

            XmlDocument dom = new XmlDocument();
            StringReader reader = new StringReader(viewStateXML);
            dom.Load(reader);
            XmlElement xmlElement = (XmlElement)dom.DocumentElement.GetElementsByTagName("ViewStateDeserialized").Item(0);
            //we don't really support viewstate version 1.X right now...so we simply return the base64 encode of the fake xml viewstate (it is simply bae64 decoded)
            //luckily this isn't such a big deal, as viewstatate 1.X is a textual format that lends itself to direction manipulatoin from the base64 decode
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(xmlElement.InnerText)); ;
        }

        public override String viewStateXML
        {
            get
            {
                return HttpUtility.HtmlDecode(m_viewStateXML);
            }
        }

    }
}
