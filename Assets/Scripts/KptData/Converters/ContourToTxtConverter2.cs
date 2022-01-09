using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter2 : ContourToTxtConverterBase
	{
		string separator;
		string pointIndexFormat;
		bool   height;

		public ContourToTxtConverter2(string decimalSeparator, string separator, string pointIndexFormat, bool height) : base(decimalSeparator)
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

			if (height)
				formatPaths.Add("0");

			return string.Join(separator, formatPaths);
		}
	}
}