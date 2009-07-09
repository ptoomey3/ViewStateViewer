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
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Specialized;
using System.Drawing;
using System.Web.UI;
using System.Web;
using System.Text.RegularExpressions;

namespace ViewState
{
    abstract class ViewState
    {
        protected internal int m_version = 0;           // ex. 1 (for ASP.Net 1.1)
        protected internal bool m_encrypted = false;
        protected internal bool m_MACProtected = false;
        protected internal String m_versionString = "";  // ex. ASP.Net 2.0
        protected internal String m_viewStateBase64 = ""; // stores the  serialized after decoding the original viewstate and then reenncoding it (all MACs and such will be removed)
        protected internal String m_viewStateXML = "";
        protected internal String m_MAC = "None";


        public ViewState(String viewStateBase64)
        {
            m_viewStateBase64 = viewStateBase64;
            computeVersionInfo();
            computeMACInfo();
            m_viewStateXML = getViewStateXMLFromBase64(m_viewStateBase64);

        }

        public ViewState(XmlDocument viewStateXMLDocument)
        {
            m_version = Int32.Parse(viewStateXMLDocument.GetElementsByTagName("Version").Item(0).InnerText);
            m_versionString = viewStateXMLDocument.GetElementsByTagName("VersionString").Item(0).InnerText;
            m_MAC = viewStateXMLDocument.GetElementsByTagName("MAC").Item(0).InnerText;
            if (m_MAC == "None")
            {
                m_MACProtected = false;
            }
            else
            {
                m_MACProtected = true;
            }
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            viewStateXMLDocument.Save(writer);
            m_viewStateXML = sb.ToString();
            m_viewStateBase64 = getViewStateBase64FromXMLTree(m_viewStateXML);
            

        }

        protected abstract void computeMACInfo();
        protected abstract String getViewStateXMLFromBase64(String viewStateBase64);
        protected abstract String   getViewStateBase64FromXMLTree(String viewStateXML);

        protected static object buildObjectElement(XmlElement xmlElement)
        {

            String type = xmlElement.Name;
            if (type == "System.Web.UI.Pair")
            {
                Pair pair = new Pair();
                pair.First = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(0));
                pair.Second = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(1));
                return pair;
            }
            else if (type == "System.Web.UI.Triplet")
            {
                Triplet triplet = new Triplet();
                triplet.First = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(0));
                triplet.Second = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(1));
                triplet.Third = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(2));
                return triplet;
            }
            else if (type == "System.Collections.ArrayList")
            {
                ArrayList arrayList = new ArrayList();
                foreach (XmlElement innerXmlElement in xmlElement)
                {
                    arrayList.Add(buildObjectElement(innerXmlElement));
                }
                return arrayList;
            }
            else if (type == "System.Array")
            {
                object[] array = new object[xmlElement.ChildNodes.Count];
                for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
                {
                    array[i] = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(i));
                }
                return array;
            }
            else if (type == "System.Collections.Hashtable")
            {
                Hashtable hashTable = new Hashtable();
                return hashTable; // this needs to be fixed...I don't know what the child elements are

            }
            else if (type == "System.Collections.Specialized.HybridDictionary")
            {
                HybridDictionary hybridDictionary = new HybridDictionary();
                foreach (XmlElement innerXmlElement in xmlElement)
                {
                    DictionaryEntry dictionaryEntry = (DictionaryEntry)buildObjectElement((XmlElement)innerXmlElement);
                    hybridDictionary.Add(dictionaryEntry.Key, dictionaryEntry.Value);
                }
                return hybridDictionary;
            }
            /*else if (type == "System.Collections.IDictionary")
            {
                IDictionary iDictionary = new IDictionary();
                return iDictionary; // hmmm...not quite sure if we will ever see an IDicionary itself since it is an interface
            }*/

            else if (type == "System.Collections.DictionaryEntry")
            {
                DictionaryEntry dictionaryEnry = new DictionaryEntry();
                dictionaryEnry.Key = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(0));
                dictionaryEnry.Value = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(1));
                return dictionaryEnry;
            }
            else if (type == "System.String")
            {
                return xmlElement.InnerText;
            }
            else if (type == "System.Web.UI.IndexedString")
            {
                return new IndexedString(xmlElement.InnerText);
            }
            else if (type == "System.Int32")
            {
                return Int32.Parse(xmlElement.InnerText);
            }
            else if (type == "System.Int16")
            {
                return Int16.Parse(xmlElement.InnerText);
            }
            else if (type == "System.Boolean")
            {
                if (xmlElement.InnerText == "True")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (type == "System.Drawing.Color")
            {
                int a = (Int32)buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(0));
                int r = (Int32)buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(1));
                int g = (Int32)buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(2));
                int b = (Int32)buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(3));
                Color color = Color.FromArgb(a, r, g, b);
                return color;
            }
            else if (type == "Null" && xmlElement.InnerText == "True")
            {
                return null;
            }
            else
            {
                //there are likely other objects we might see in practice, but I haven't seen how they are serialized...this section will have to be update as new types are identified.
                return null;
            }

        }

        public virtual String viewStateXML
        {
            get
            {
                return m_viewStateXML;
            }

        }
        public String viewStateBase64
        {
            get
            {
                return m_viewStateBase64;
            }
        }


        public String MAC
        {
            get
            {
                return m_MAC;
            }
        }

        public bool MACProected
        {
            get
            {
                return m_MACProtected;
            }
        }
        public int version
        {
            get
            {
                return m_version;
            }
        }

        public String versionString
        {
            get
            {
                return m_versionString;
            }
        }
        public bool encrypted
        {
            get
            {
                return m_encrypted;
            }
        }

        protected void computeVersionInfo()
        {
            if (m_viewStateBase64 == null)
            {
                return;
            }

            m_version = getViewStateVersionFromBase64String(m_viewStateBase64);
            switch (m_version)
            {
                case 2:
                    m_versionString = "ASP.Net 2.X";
                    m_encrypted = false;
                    break;
                case 1:
                    m_versionString = "ASP.Net 1.X";
                    m_encrypted = false;
                    break;
                default:
                    m_versionString = "Unknown";
                    m_encrypted = true; //this isn't guranteed...but we'll assume it is true for now
                    break;
            }

            return;
        }

        public static ViewState newViewStateFromBase64String(String viewStateString)
        {
            ViewState viewState = null;
            int viewStateVersion = getViewStateVersionFromBase64String(viewStateString);
            switch (viewStateVersion)
            {
                case 1:
                    viewState = new ViewState_Dot_Net_1_0(viewStateString);
                    break;
                case 2:
                    viewState = new ViewState_Dot_Net_2_0(viewStateString);
                    break;
                default:
                    viewState = null;
                    break;

            }
            return viewState;
        }


        public static ViewState newViewStateFromXMLString(String viewStateXML)
        {
            ViewState viewState = null;
            int viewStateVersion = 0;
            String versionPattern = @"<Version>(.+)</Version>";
            Match match = Regex.Match(viewStateXML, versionPattern, RegexOptions.Singleline);
            if (match.Success)
            {
                viewStateVersion = Int32.Parse(match.Groups[1].Value);
            }
            // if this is viewstate version 1 we need to patch a few things up
            if (viewStateVersion == 1)
            {
                String viewStatePattern = @"<ViewStateDeserialized>(.+)</ViewStateDeserialized>";
                match = Regex.Match(viewStateXML, viewStatePattern, RegexOptions.Singleline);
                if (match.Success)
                {
                    viewStateXML = viewStateXML.Replace(match.Groups[1].Value, HttpUtility.HtmlEncode(match.Groups[1].Value));
                }
                // horrible horrible hack to account for RichTextBox removing CRLF with a LF...anyone have a better idea???
                viewStateXML = viewStateXML.Replace("\n", "\r\n");


            }

            XmlDocument dom = new XmlDocument();
            StringReader reader = new StringReader(viewStateXML);
            dom.Load(reader);
            switch (viewStateVersion)
            {
                case 1:
                    viewState = new ViewState_Dot_Net_1_0(dom);
                    break;
                case 2:
                    viewState = new ViewState_Dot_Net_2_0(dom);
                    break;
                default:
                    viewState = null;
                    break;

            }
            return viewState;
        } 

        public static Int32 getViewStateVersionFromBase64String(String viewStateBase64)
        {
            byte[] viewStateBytes;
            try
            {
                viewStateBytes = System.Convert.FromBase64String(viewStateBase64);
            }
            catch (System.ArgumentNullException)
            {
                return 0;
            }
            if (viewStateBytes.Length >= 2 &&
                    viewStateBytes[0] == '\xff' &&
                    viewStateBytes[1] == '\x01')
            {
                return 2;
            }
            else if (viewStateBytes.Length >= 2 &&
                         viewStateBytes[0] == 't' &&
                         viewStateBytes[1] == '<')
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
