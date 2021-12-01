using System;
using System.Xml;
using Geo.KptData; 
using Geo.KptData.KptReaders.Kpt11; 

namespace Tests
{
	public class Kpt11 : KptVersionTest
	{ 
		protected override string KptVersionName              => "11";
		protected override string KptFileName                 => "91_01_003008_2020-11-28_kpt11.xml";
		protected override string KptTestDataFileName         => "kpt11_testData.json";
		protected override string ReferenceParcelDataFileName => "91_01_003008_1026.txt";
		protected override string CadastralNumber             => "91:01:003008:1026";

		protected override IKpt GetReader(XmlDocument doc)
		{
			return new Kpt11Reader(doc);
		}
	}
}