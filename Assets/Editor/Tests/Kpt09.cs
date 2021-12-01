using System.Xml;
using Geo.KptData;
using Geo.KptData.KptReaders.Kpt10;

namespace Tests
{
	public class Kpt09 : KptVersionTest
	{
		protected override string KptVersionName              => "09";
		protected override string KptFileName                 => "77_01_0003031_2015-03-26_kpt09.xml";
		protected override string KptTestDataFileName         => "kpt09_testData.json";
		protected override string ReferenceParcelDataFileName => "77_01_0003031_1.txt";
		protected override string CadastralNumber             => "77:01:0003031:1";
        
		protected override IKpt GetReader(XmlDocument doc)
		{
			return new Kpt09Reader(doc);
		}
	}
}