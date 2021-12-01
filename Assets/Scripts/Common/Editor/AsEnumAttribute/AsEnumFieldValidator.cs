using System;
using System.Linq;
using System.Reflection;
using SiberianWellness.Validation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SiberianWellness.Common
{
    /// <summary>
    /// Класс валидатор работающий только в UnityEditor.
    /// Проверяет значения полей, помеченных фтрибутом <see cref="AsEnumAttribute"/>
    /// </summary>
    public class AsEnumFieldValidator : FieldValidator
    {
        public override void FindProblems(object value, Type valueType, FieldInfo ownerFieldInfo, Object rootObject)
        {
            if (value == null)
                return;

            if (valueType != typeof(string))
                return;

            var attribute = ownerFieldInfo.GetCustomAttribute<AsEnumAttribute>();
            if (attribute == null)
                return;

            var isValidValue = AsEnumAttributeValues.IsValidValue(attribute.ValuesSource, value as string);

            if (!isValidValue)
                ObjectValidator.Problems.Add(new ValidationProblem(ownerFieldInfo, rootObject, nameof(AsEnumAttribute)));
        }
    }
}
