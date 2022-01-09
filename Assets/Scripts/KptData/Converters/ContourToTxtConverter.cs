
namespace Geo.KptData.Converters
{
	public class ContourToTxtConverter : ContourToTxtConverterBase
	{
		/// <summary>
		///  Формат вывода, например: 'pt(i), (x), (y), 0'
		/// </summary>
		public string format;

		public ContourToTxtConverter(string decimalSeparator, string format) : base(decimalSeparator)
		{
			this.format = format;
		}

		public override string GetFormat()
		{ 
			return format;
		}
	}
}