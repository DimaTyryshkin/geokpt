using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo.ForStudy
{

    public class XmlReaderForStudy
    {
        private readonly string path;
        private readonly int cad;
        private readonly int area;
        private readonly XmlNode data;
        private readonly XmlDocument xml;
        private readonly XmlNamespaceManager ns;

        public XmlReaderForStudy(string path)
        {
            Assert.IsNotNull(path);
            this.path = path;
            this.xml = new XmlDocument();
            try
            {
                this.xml.PreserveWhitespace = true;
                this.xml.Load(this.path);
                this.ns = new XmlNamespaceManager(xml.NameTable);
                this.ns.AddNamespace("root", "http://www.fireresist.com/immolate/improved/1.0");
                this.data = this.xml.DocumentElement.SelectSingleNode("root:geo-data/root:geo", this.ns);
                if (this.data!=null)
                {
                    this.cad = Int32.Parse(this.data.Attributes.GetNamedItem("cad").InnerText);
                    this.area = Int32.Parse(this.data.Attributes.GetNamedItem("area").InnerText);
                }
            }
            catch (FileNotFoundException e)
            {
                Debug.Log(e.Message);
            }
            catch (XmlException e)
            {
                Debug.Log(e.Message);
            }
            catch (System.ArgumentException e)
            {
                Debug.Log(e.Message);
            }
        }

        public int Cadastral { get => this.cad; }
        public int Area { get => this.area; }
    }

}

