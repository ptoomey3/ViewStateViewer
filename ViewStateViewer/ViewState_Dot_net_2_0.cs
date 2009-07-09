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
using System.Web.UI;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;

namespace ViewState
{
    class ViewState_Dot_Net_2_0:ViewState
    {
        public ViewState_Dot_Net_2_0(String viewStateBase64) : base(viewStateBase64)
        {
         
        }

        public ViewState_Dot_Net_2_0(XmlDocument viewStateXMLDocument) : base(viewStateXMLDocument)
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
            String tempViewStateXML = getViewStateXMLFromBase64(m_viewStateBase64);
            byte[] viewStateDescerializedFromXML = System.Convert.FromBase64String(getViewStateBase64FromXMLTree(tempViewStateXML));

            if (originalViewStateDeserialized.Length != viewStateDescerializedFromXML.Length &&
                originalViewStateDeserialized.Length - viewStateDescerializedFromXML.Length == 20)
            {
                m_MACProtected = true;
                ArrayList tempList = new ArrayList(originalViewStateDeserialized);
                byte[] tempArray = new byte[20];  //the size of the MAC
                int MACIndex = originalViewStateDeserialized.Length - 20; // 20 is the size of the MAC 
                tempList.GetRange(MACIndex, 20).CopyTo(tempArray);
                String hexString = BitConverter.ToString(tempArray);
                m_MAC = hexString.Replace("-", "");
            }
            else
            {
                m_MACProtected = false;

            }
        }
        protected override String getViewStateBase64FromXMLTree(String viewStateXML)
         {

            LosFormatter formatter = new LosFormatter();
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            XmlDocument dom = new XmlDocument();
            StringReader reader = new StringReader(viewStateXML);
            dom.Load(reader);
            XmlElement xmlElement = (XmlElement) dom.DocumentElement.GetElementsByTagName("ViewStateDeserialized").Item(0);
            object viewStateObjectTree = null;
            if (xmlElement.Name == "ViewStateDeserialized")
            {
                viewStateObjectTree = buildObjectElement((XmlElement)xmlElement.ChildNodes.Item(0));
            }

            formatter.Serialize(writer, viewStateObjectTree);
            return sb.ToString();
        }

        protected override String getViewStateXMLFromBase64(String viewStateBase64)
        {
                
                LosFormatter formatter = new LosFormatter();
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
                dom.DocumentElement.AppendChild(element);
                buildXMLElement(dom, (XmlElement) element, formatter.Deserialize(this.viewStateBase64));
                StringBuilder sb = new StringBuilder();
                StringWriter writer = new StringWriter(sb);
                dom.Save(writer);
                return sb.ToString();
        }

        private void buildXMLElement(XmlDocument dom, XmlElement elem, object treeNode)
        {
            XmlElement element;
            Type type;
            if (treeNode == null)
            {
                element = dom.CreateElement("Null");
                elem.AppendChild(element);
                element.InnerText = true.ToString();
            }
            else
            {
                type = treeNode.GetType();
                
                if (type == typeof(Pair))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    buildXMLElement(dom, element, ((Pair)treeNode).First);
                    buildXMLElement(dom, element, ((Pair)treeNode).Second);
                } else if (type == typeof(Triplet))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    buildXMLElement(dom, element, ((Triplet)treeNode).First);
                    buildXMLElement(dom, element, ((Triplet)treeNode).Second);
                    buildXMLElement(dom, element, ((Triplet)treeNode).Third);
                }
                else if (type == typeof(ArrayList))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    foreach (object arrayListEntry in (ArrayList)treeNode)
                    {
                        buildXMLElement(dom, element, arrayListEntry);
                    }
                }
                else if (type == typeof(Array))
                {
                    element = dom.CreateElement("Array");
                    elem.AppendChild(element);
                    foreach (object arrayEntry in (Array)treeNode)
                    {
                        buildXMLElement(dom, element, arrayEntry);
                    }
                }
                else if (type == typeof(Hashtable))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    foreach (object hashTableEntry in (Hashtable)treeNode)
                    {
                        buildXMLElement(dom, element, hashTableEntry);
                    }
                }
                else if (type == typeof(HybridDictionary))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    foreach (object dictionaryEntry in (HybridDictionary)treeNode)
                    {
                        buildXMLElement(dom, element, dictionaryEntry);
                    }
                }
                else if (type == typeof(IDictionary))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    foreach (object dictionaryEntry in (IDictionary)treeNode)
                    {
                        buildXMLElement(dom, element, dictionaryEntry);
                    }

                }
                else if (type == typeof(DictionaryEntry))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    DictionaryEntry entry = (DictionaryEntry)treeNode;
                    buildXMLElement(dom, element, entry.Key);
                    DictionaryEntry entry2 = (DictionaryEntry)treeNode;
                    buildXMLElement(dom, element, entry2.Value);
                }
                else if (type == typeof(IndexedString))
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    element.InnerText = ((IndexedString)treeNode).Value;
                    elem.AppendChild(element);
                }
                else if (type == typeof(Color)) {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    elem.AppendChild(element);
                    Color color = (Color)treeNode;
                    int a = color.A;
                    buildXMLElement(dom, element, a);
                    int r = color.R;
                    buildXMLElement(dom, element, r);
                    int g = color.G;
                    buildXMLElement(dom, element, g);
                    int b = color.B;
                    buildXMLElement(dom, element, b);
                }
                else
                {
                    element = dom.CreateElement(treeNode.GetType().ToString());
                    element.InnerText = treeNode.ToString();
                    elem.AppendChild(element);
                }
            }
        }
    }

      
   
}
