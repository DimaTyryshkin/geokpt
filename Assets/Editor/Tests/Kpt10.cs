using System.Xml;
using Geo.KptData;
using Geo.KptData.KptReaders.Kpt10;

namespace Tests
{
    public class Kpt10 : KptVersionTest
    {
        protected override string KptVersionName              => "10";
        protected override string KptFileName                 => "91_01_049001_2020-03-27_kpt10.xml";
        protected override string KptTestDataFileName         => "kpt10_testData.json";
        protected override string ReferenceParcelDataFileName => "91_01_049001_4.txt";
        protected override string CadastralNumber             => "91:01:049001:4";
        
        protected override IKpt GetReader(XmlDocument doc)
        {
            return new Kpt10Reader(doc);
        }
    }
}