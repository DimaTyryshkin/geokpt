using System;
using System.Collections.Generic;
using Geo.KptData;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tests
{
	[Serializable]
	public class KptTestData
	{
		public int              parcelsCount;
		public ParcelTestData[] parcelTests;

		public void AssertIt(IKpt kptReader)
		{
			List<IParcel> parcels = kptReader.GetAllParcels();
			Assert.AreEqual(parcelsCount, parcels.Count, "В Кпт не все записи найдены");

			foreach (var parcelTest in parcelTests)
			{
				var parcel = kptReader.FindParcelByCadastralNumber(parcelTest.cadastralNumber);
				parcelTest.AssertIt(parcel);
			}
		}
	}
	
	[Serializable]
	public class ParcelTestData
	{
		public string cadastralNumber;
		public string area;
		public string address;
		public int    contourCount;

		public ContourTestData[] contourTests;

		public void AssertIt(IParcel parcel)
		{
			Assert.IsNotNull(parcel, $"Не найден парсель '{cadastralNumber}'");
			Assert.AreEqual(area, parcel.GetArea(), $"Не совпадает площадь '{cadastralNumber}'");
			Assert.AreEqual(address, parcel.GetReadableAddress(), $"Не совпадает адресс '{address}' и '{parcel.GetReadableAddress()}'");
			Assert.AreEqual(contourCount, parcel.GetContours().Count, $"Не совпадает колличество контуров '{cadastralNumber}'");
			
			List<IContour> contours = parcel.GetContours();

			if (parcel.GetCadastralNumber() == "91:01:000000:490")
			{
				string msg  = "";
				int    n    = 0;
				int    summ = 0;
				foreach (var countour in contours)
				{
					n++;
					summ += countour.GetPoints().Count;
					msg  += $"n={n} count={countour.GetPoints().Count} summ={summ}" + Environment.NewLine;
				}

				Debug.Log(msg);
			}

			if (contourTests != null)
			{
				foreach (var contourTest in contourTests)
				{
					IContour contour = contours[contourTest.contourIndex];
					contourTest.AssertIt(contour, cadastralNumber);
				}
			}
		}
	}

	[Serializable]
	public struct ContourTestData
	{
		public int    contourIndex;
		public string id;
		public int    pointCount;

		public void AssertIt(IContour contour, string cadastralNumber)
		{
			Assert.AreEqual(pointCount, contour.GetPoints().Count, $"Не совпадает колличество точке в контуре'{cadastralNumber}-{contourIndex}'");
			Assert.AreEqual(id, contour.ID, $"Не совпадает ID'{cadastralNumber}-{contourIndex}'");
		}
	}
}