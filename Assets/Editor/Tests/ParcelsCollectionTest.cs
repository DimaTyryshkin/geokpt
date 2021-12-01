using System;
using System.Collections.Generic;
using Geo.KptData;
using NUnit.Framework;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class ParcelsCollectionTest
	{
		[Test]
		public void Filter()
		{
			List<IParcel>     parcels = GetParcels();
			ParcelsCollection c       = new ParcelsCollection(parcels);

			Test(c, "00 01", 1);
			Test(c, "00 02", 2);
			Test(c, "00:01", 1);
			Test(c, "00:02", 2);

			Test(c, "777 00", 1, 2, 3, 4, 5, 6, 7);
			Test(c, "777:00", 1, 2, 3, 4, 5, 6, 7);

			Test(c, "777 ", 1, 2, 3, 4, 5, 6, 7, 8, 9);
			Test(c, "777:", 1, 2, 3, 4, 5, 6, 7, 8, 9);
			Test(c, "777" , 1, 2, 3, 4, 5, 6, 7, 8, 9);

			Test(c, "ленина 4", 4);
			Test(c, "лен 4", 4);
			Test(c, "д 4", 4);
			
			Test(c, "прос кир 8", 8);
			Test(c, "кирова 9", 9);
			Test(c, "прос 9", 9);
			
			Test(c, "улица кир 8");
			Test(c, "кирова 3");
			Test(c, "ленинааа 4");
		}

		void Test(ParcelsCollection c, string filter, params int[] parcelsId)
		{
			var parcels = c.FilterParcels(filter);

			string msg = filter + Environment.NewLine;
			foreach (var parcel in parcels)
			{
				var p = (FakeParcel) parcel;
				msg += p.readableAddress + Environment.NewLine;
			}
			
			Debug.Log(msg); 
			
			Assert.AreEqual(parcels.Count, parcelsId.Length);

			for (int i = 0; i < parcels.Count; i++)
			{
				var fp = parcels[i] as FakeParcel;
				int id = parcelsId[i];
				Assert.AreEqual(fp.id, id);
			}
		}

		List<IParcel> GetParcels()
		{
			return new List<IParcel>()
			{
				new FakeParcel(1,"777:00:01","ул.Ленина д.1"),
				new FakeParcel(2,"777:00:02","ул.Ленина д.2"),
				new FakeParcel(3,"777:00:03","ул.Ленина д.3"),
				new FakeParcel(4,"777:00:04","ул.Ленина д.4"),
				new FakeParcel(5,"777:00:05","ул.Ленина д.5"),
				new FakeParcel(6,"777:00:06","ул.Ленина д.6"),
				new FakeParcel(7,"777:00:07","ул.Ленина д.7"),
				new FakeParcel(8,"777:01:11","проспект.кирова 8"),
				new FakeParcel(9,"777:01:12","проспект.кирова 9"),
			};
		}



		class FakeParcel : IParcel
		{
			public int id;
			
			public string cadastralNumber;
			public string readableAddress;

			public FakeParcel(int id, string cadastralNumber, string readableAddress)
			{
				this.id = id;
				this.cadastralNumber = cadastralNumber;
				this.readableAddress = readableAddress;
			}

			public string GetCadastralNumber() => cadastralNumber;

			public string GetArea() => "0";

			public string GetReadableAddress() => readableAddress;

			public List<IContour> GetContours() => null;
		}
	}
	
	
}