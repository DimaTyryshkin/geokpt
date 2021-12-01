using System;
using System.Collections.Generic; 
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo.KptData.KptReaders.Kpt11
{
	public class Kpt11Reader : KptReaderBase
	{
		XmlDocument doc;

		XmlNodeList _parcels;

		public override string KptVersionNumber => "11";
		
		XmlNodeList Parcels
		{
			get
			{
				if (_parcels == null)
					_parcels = doc.DocumentElement.SelectNodes("cadastral_blocks/cadastral_block/record_data/base_data/land_records/land_record");

				return _parcels;
			}
		}

		public Kpt11Reader(XmlDocument doc)
		{
			Assert.IsNotNull(doc);

			this.doc = doc;
		}

		public override List<IParcel> GetAllParcels()
		{
			List<IParcel> r = new List<IParcel>();

			DateTime t      = DateTime.Now;
			
			foreach (var percel in Parcels)
			{  
				r.Add(new Kpt11ParcelReader(percel as XmlNode));
			}

			Debug.Log("Load parcels time="+(DateTime.Now -t).ToString("g"));
			return r;
		} 
	}
}