using System.Xml;
using Geo.KptData.KptReaders.Kpt10;
using UnityEngine;

namespace Geo.Data
{
    public class Test : MonoBehaviour
    {
        [ContextMenu("Foo")]
        void Foo()
        {
            var txt = Resources.Load<TextAsset>("xml/file");
            
            var doc = new XmlDocument();
            doc.Load("doc.xml");
 
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);  
            nsmgr.AddNamespace("root", "urn://x-artefacts-rosreestr-ru/outgoing/kpt/10.0.1");  
            nsmgr.AddNamespace("ns3", "urn://x-artefacts-rosreestr-ru/commons/complex-types/entity-spatial/5.0.1");  
            
            //Log(FindParcelByCadastralNumber(doc.DocumentElement, nsmgr, "91:01:049001:2"));
            
            Kpt10Reader d = new Kpt10Reader(doc);

            //Debug.Log( d.FindParcelByCadastralNumber("91:01:049001:2").GetArea());
            var points =d.FindParcelByCadastralNumber("91:01:049001:2").GetContours()[0].GetPoints();

            foreach (var p in points)
            {
                Debug.Log(p);
            }
        } 
    }
}