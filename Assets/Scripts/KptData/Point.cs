namespace Geo.KptData
{
	public class Point
	{
		public Point(string x, string y)
		{
			this.x = x;
			this.y = y;
		}

		public string x;
		public string y;

		public override string ToString()
		{
			return x + " " + y;
		}
	}
}