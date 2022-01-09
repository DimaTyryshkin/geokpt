using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter2 : ContourToTxtConverterBase
	{
		string separator;
		string pointIndexFormat;
		string height;

		public ContourToTxtConverter2(string decimalSeparator, string separator, string pointIndexFormat, string height) : base(decimalSeparator)
		{
			Assert.IsFalse(string.IsNullOrEmpty(separator));

			this.separator        = separator;
			this.pointIndexFormat = pointIndexFormat;
			this.height           = height;
		}

		public override string GetFormat()
		{
			List<string> formatPaths = new List<string>();

			if (!string.IsNullOrEmpty(pointIndexFormat))
				formatPaths.Add(pointIndexFormat);

			formatPaths.Add("(x)");
			formatPaths.Add("(y)");

			if (!string.IsNullOrEmpty(height))
				formatPaths.Add(height);

			return string.Join(separator, formatPaths);
		}
	}
}