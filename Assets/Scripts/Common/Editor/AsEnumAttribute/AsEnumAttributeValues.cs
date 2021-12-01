using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Assertions;

namespace SiberianWellness.Common
{
	public static class AsEnumAttributeValues
	{
		static Dictionary<Type, string[]> typeToValues = new Dictionary<Type, string[]>();
  
		public static bool IsValidValue(Type sourceType, string value)
		{
			var labels = GetAvailableLabels(sourceType);

			foreach (var label in labels)
			{
				if (GetValueFromLabel(label) == value)
					return true;
			}

			return false;
		}
		
		public static string GetValueFromLabel(string label)
		{
			string value = label;
			if (value.Contains('/'))
			{
				value = label.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Last();
			}

			return value;
		}

		public static string[] GetAvailableLabels(Type type)
		{
			Assert.IsNotNull(type);

			string[] allLabels;
			if (!typeToValues.TryGetValue(type, out allLabels))
			{
				List<string> labels = new List<string>();
				List<string> values = new List<string>();
				GetAvailableLabels(type, "", labels, values);

				allLabels = labels.ToArray();
				typeToValues.Add(type, allLabels);
			}

			return allLabels;
		}

		static void GetAvailableLabels(Type type, string prefix, List<string> labels, List<string> values)
		{
			Assert.IsNotNull(type);
			var          allFields           = type.GetFields(BindingFlags.Public | BindingFlags.Static);
			foreach (var f in allFields)
			{
				if (f.FieldType == typeof(string) && f.IsInitOnly)
				{
					string value =(string) f.GetValue(null);
					Assert.IsFalse(values.Contains(value),$"Список значений {type.Name} содержит дублирующееся начение {value}");
					values.Add(value);
					
					string label = prefix + value; 
					labels.Add(label);
				}
			}

			var nestedTypes = type.GetNestedTypes(BindingFlags.Static | BindingFlags.Public);

			foreach (var t in nestedTypes)
			{
				GetAvailableLabels(t, prefix + t.Name + "/", labels, values);
			}
		}
	}
}