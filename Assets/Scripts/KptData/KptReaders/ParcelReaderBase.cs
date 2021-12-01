using System.Collections.Generic;
using Geo.KptData.KptReaders;

namespace Geo.KptData.KptReaders
{
	public abstract class ParcelReaderBase : XmlReaderBase, IParcel
	{
		protected string cadastralNumberCache = null; 
		protected string readableAddressCache = null;
		
		public abstract string GetCadastralNumber();
		public abstract string GetArea(); 
		public abstract string GetReadableAddress();
		public abstract List<IContour> GetContours();
	}
}