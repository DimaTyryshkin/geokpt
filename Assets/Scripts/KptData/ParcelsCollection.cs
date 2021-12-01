using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine.Assertions;

namespace Geo.KptData
{
	public class ParcelsCollection
	{
		IReadOnlyList<IParcel> parcels;

		public ParcelsCollection([NotNull] IReadOnlyList<IParcel> parcels)
		{
			Assert.IsNotNull(parcels);
			this.parcels = parcels;
		}

		public List<IParcel> FilterParcels(string filterInput)
		{
			List<IParcel> result = new List<IParcel>(parcels.Count);

			// cadastral number
			{
				string filter2 = filterInput.Replace(" ", ":");
				foreach (var p in parcels)
				{
					string cadastralNumber = p.GetCadastralNumber();
					if (cadastralNumber.Contains(filterInput) || cadastralNumber.Contains(filter2))
						result.Add(p);
				}
			}

			// cadastral number
			{
				var filters = filterInput
					.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
					.Select(f => f.ToLower())
					.ToArray();

				Dictionary<IParcel, int> filterEntryCounter = new Dictionary<IParcel, int>();

				foreach (IParcel parcel in parcels)
				{
					string address = parcel.GetReadableAddress().ToLower();

					foreach (var filter in filters)
					{
						if (address.Contains(filter))
						{
							if (filterEntryCounter.ContainsKey(parcel))
								filterEntryCounter[parcel]++;
							else
								filterEntryCounter[parcel] = 1;
						}
					}
				}

				var filteredParcels = filterEntryCounter
					.Where(p => p.Value== filters.Length)
					.ToArray();

				foreach (KeyValuePair<IParcel, int> p in filteredParcels)
					result.Add(p.Key);
			}

			return result;
		}
	}
}