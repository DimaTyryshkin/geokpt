using System.Collections.Generic;

namespace Geo.KptData
{
	public interface IContour
	{
		string      ID { get; }
		List<Point> GetPoints();
	}

	public abstract class Contour : IContour
	{
		public static string defaultId = "-1";

		public abstract string      ID { get; }
		public abstract List<Point> GetPoints();
	}
}