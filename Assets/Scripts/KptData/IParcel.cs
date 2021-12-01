using System.Collections.Generic;

namespace Geo.KptData
{
	public interface IParcel
	{
		string      GetCadastralNumber();
		string      GetArea();
		string      GetReadableAddress();
		List<IContour> GetContours();
	}
}