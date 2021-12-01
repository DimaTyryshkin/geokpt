using System.Collections.Generic;

namespace Geo.KptData.KptReaders
{
	public abstract class KptReaderBase : XmlReaderBase, IKpt
	{
		public abstract List<IParcel> GetAllParcels();
		
		public abstract string KptVersionNumber { get; }

		public IParcel FindParcelByCadastralNumber(string number)
		{
			foreach (var parcel in GetAllParcels())
			{
				if (parcel.GetCadastralNumber() == number)
					return parcel;
			}

			return null;
		}
	}
}