using System.Collections.Generic;

namespace Geo.KptData
{
	public interface IKpt
	{
		string KptVersionNumber { get; }
		List<IParcel> GetAllParcels();
		IParcel       FindParcelByCadastralNumber(string number);
	}
}