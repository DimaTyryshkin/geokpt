using System;
using System.Reflection;
using Object = UnityEngine.Object;

namespace SiberianWellness.Validation
{
	public abstract class FieldValidator
	{
		public abstract void FindProblems(object value, Type valueType, FieldInfo ownerFieldInfo, Object rootObject);
	}
} 