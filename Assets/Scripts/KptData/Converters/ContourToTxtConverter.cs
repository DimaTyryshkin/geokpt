
namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter : ContourToTxtConverterBase
	{
		/// <summary>
		///  Формат вывода, например: 'pt(i), (x), (y), 0'
		/// </summary>
		public string format;

		public ContourToTxtConverter(int decimalSeparatorIndex, string format) : base(decimalSeparatorIndex)
		{
			this.format = format;
		}

		protected override string PointToString(int index, Point p, string decimalSeparator)
		{
			int    n = index + 1;
			string x = ReplaceDecimal(p.x, decimalSeparator);
			string y = ReplaceDecimal(p.y, decimalSeparator);

			string result = format;
			result = result.Replace("(i)", index.ToString());
			result = result.Replace("(n)", (n).ToString());
			result = result.Replace("(x)", x);
			result = result.Replace("(y)", y);

			return result;
		}
	}
}