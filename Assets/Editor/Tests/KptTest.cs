using Geo.KptData.KptReaders;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class KptTest
	{
		[Test]
		public void TrimFileName()
		{
			string name = "91_04_032001_2021-01-18_kpt11";
			Assert.AreEqual(name, KptFile.TrimEndKptExtensions(name + ".xml.zip"));
			Assert.AreEqual(name, KptFile.TrimEndKptExtensions(name + ".xml.sig.zip"));
			Assert.AreEqual(name, KptFile.TrimEndKptExtensions(name + ".xml"));
			Assert.AreEqual(name, KptFile.TrimEndKptExtensions(name + ".zip"));
		}
	}
}