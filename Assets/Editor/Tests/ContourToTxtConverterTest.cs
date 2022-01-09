﻿using Geo.KptData;
using Geo.KptData.Converters;
using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace Tests
{
	public class ContourToTxtConverterTest
	{
		[Test]
		public void ContourToTxtConverter1_1()
		{
			string                format     = "pt(i),(x),(y)";
			ContourToTxtConverter converter1 = new ContourToTxtConverter(".", format);
			Point                 p          = new Point("9.9", "1.1"); 
			string                text       = converter1.PointToString(5, p);
			
			Assert.AreEqual("pt5,9.9,1.1",text); 
		}
		
		[Test]
		public void ContourToTxtConverter1_2()
		{
			string                format     = "n=(n) (x) (y) 0";
			ContourToTxtConverter converter1 = new ContourToTxtConverter(",", format);
			Point                 p          = new Point("9.9", "1.1"); 
			string                text       = converter1.PointToString(5, p);
			
			Assert.AreEqual("n=6 9,9 1,1 0",text); 
		}
		
		[Test]
		public void ContourToTxtConverter2_1()
		{
			string                 indexFormat = "pt(i)";
			ContourToTxtConverter2 converter2  = new ContourToTxtConverter2(".", ",", indexFormat, "");
			Point                  p           = new Point("9.9", "1.1"); 
			string                 text        = converter2.PointToString(5, p);
			
			Assert.AreEqual("pt5,9.9,1.1",text); 
			Assert.AreEqual("pt(i),(x),(y)",converter2.GetFormat()); 
		}
		
		[Test]
		public void ContourToTxtConverter2_2()
		{
			string                 indexFormat = "";
			ContourToTxtConverter2 converter2  = new ContourToTxtConverter2(",", " ", indexFormat, "0");
			Point                  p           = new Point("9.9", "1.1"); 
			string                 text        = converter2.PointToString(5, p);
			
			Assert.AreEqual("9,9 1,1 0",text); 
			Assert.AreEqual("(x) (y) 0",converter2.GetFormat()); 
		}
	}
}