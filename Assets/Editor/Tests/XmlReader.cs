using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Geo.ForStudy;

namespace Tests
{
    public class XmlReader
    {
        [Test]
        public void XmlReaderPass()
        {
            XmlReaderForStudy xml = new XmlReaderForStudy("Assets/Scripts/ForStudy/DataForTest/document.xml");
            Assert.AreEqual(xml.Area,1000);
            Assert.AreEqual(xml.Cadastral,2048);
        }
    }
}
