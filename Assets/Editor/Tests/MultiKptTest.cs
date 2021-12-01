using System.IO;
using Geo.KptData.KptReaders;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class MultiKptTest
	{
		string dataFolderTemplate = "GeoData/DataForTest/KptExamples/MultiKpt/kpt10+kpt11.zip";
		int    totalParcelsCount  = 1027;

		[Test]
		public void ParcelsCount()
		{
			KptFile kptFile = new KptFile(new FileInfo(dataFolderTemplate));
			kptFile.LoadAllParcels();
			Assert.AreEqual(totalParcelsCount, kptFile.Parcels.Count);
		}
	}
}