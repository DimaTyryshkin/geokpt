using System.Xml;
using Geo.KptData;
using Geo.KptData.KptReader.Kpt10Excerpt;

namespace Tests
{
	public class Kpt10Excerpt : KptVersionTest
	{
		protected override string KptVersionName              => "10Excerpt";
		protected override string KptFileName                 => "kv_e8760d0a-c596-4504-a7c6-60992fdab8eb.xml";
		protected override string KptTestDataFileName         => "kpt10Excerpt_testData.json";
		protected override string ReferenceParcelDataFileName => "70_16_0403001_52.txt";
		protected override string CadastralNumber             => "70:16:0403001:52";
        
		protected override IKpt GetReader(XmlDocument doc)
		{
			return new Kpt10ExcerptReader(doc);
		}
	}
}