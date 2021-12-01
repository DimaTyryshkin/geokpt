using System.Collections.Generic;
using System.Linq;
using Geo.Data;
using Geo.KptData;
using Geo.KptData.Converters;
using SiberianWellness.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Geo
{
	public class ContourToTxtConverterWrapper
	{
		public class SeparatorData
		{
			int               separatorIndex;
			readonly string[] separators;
			readonly string   separatorBsdKey;

			public int Count => separators.Length;
			
			public int Index
			{
				get => separatorIndex;
				set
				{
					separatorIndex = value;
					separatorIndex = Mathf.Min(separatorIndex, separators.Length - 1);
					PlayerPrefs.SetInt(separatorBsdKey, separatorIndex);
				}
			}

			public string Separator => separators[separatorIndex];

			public SeparatorData(string separatorBsdKey, string[] separators)
			{
				AssertWrapper.IsAllNotNull(separators);
				Assert.IsFalse(string.IsNullOrWhiteSpace(separatorBsdKey));

				this.separators      = separators;
				this.separatorBsdKey = separatorBsdKey;

				separatorIndex = PlayerPrefs.GetInt(separatorBsdKey, 0);
			}
		}
 
		public readonly SeparatorData separator;
		public readonly SeparatorData decimals;

		public ContourToTxtConverterWrapper()
		{
			separator = new SeparatorData("ContourToTxtConverterWrapper.separatorIndex", ContourToTxtConverter.separators);
			decimals  = new SeparatorData("ContourToTxtConverterWrapper.decimalIndex", ContourToTxtConverter.decimals);
		}

		public string ConvertToString(IContour contour, IParcel parcel)
		{
			Assert.IsNotNull(contour);
			Assert.IsNotNull(parcel);
			 
			var converter = new ContourToTxtConverter(contour, parcel); 
			return converter.ConvertToString(separator.Index, decimals.Index);
		}

		public string ConvertToFile(string folderToSaveFile, IContour contour, IParcel parcel)
		{ 
			Assert.IsNotNull(contour);
			Assert.IsNotNull(parcel);
			
			var converter = new ContourToTxtConverter(contour, parcel); 
			return converter.ConvertToFile(folderToSaveFile, separator.Index, decimals.Index);
		}
	}
}