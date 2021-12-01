using System; 
using SiberianWellness.NotNullValidation.Internal;
using SiberianWellness.Validation; 
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace SiberianWellness.NotNullValidation
{
	public class IsntNull
	{ 
		public static TimeSpan totalTime = new TimeSpan();

		public static void Assert(Object mb)
		{
			using (ObjectValidator.StartSearch(new NotNullFieldValidator(), false))
			{
				ObjectValidator.ValidateObject(mb);
				var errors = ObjectValidator.Problems;

				if (errors.Count > 0)
				{
					string s = "";
					foreach (var e in errors)
					{
						s += e.ToString()+Environment.NewLine;

					}

					Debug.LogError(s, mb);
				}
			}
		}
	}
}