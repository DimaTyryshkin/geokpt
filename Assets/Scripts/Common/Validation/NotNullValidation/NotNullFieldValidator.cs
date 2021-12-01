using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using SiberianWellness.Validation;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR

#endif

namespace SiberianWellness.NotNullValidation.Internal
{
    public class NotNullFieldValidator : FieldValidator
    {
        static bool IsSimilarToNull(object obj)
        {
            if (obj == null || obj.Equals(null))
            {
                return true;
            }
            else
            {
                if (obj is string s)
                    return string.IsNullOrWhiteSpace(s);

                return false;
            }
        }
 
        public override void FindProblems(object value, Type valueType, FieldInfo ownerFieldInfo, Object rootObject)
        {
            if(ownerFieldInfo.Name=="s2") 
                Debug.Log("find s2");
            
            if (IsSimilarToNull(value))
                AddErrorIfNeed(ownerFieldInfo, rootObject);
        }

        void AddErrorIfNeed(FieldInfo field, Object errorObject)
        { 
            if (field.GetCustomAttribute<IsntNullAttribute>() != null)
            {
                ObjectValidator.Problems.Add(new ValidationProblem(field, errorObject,nameof(IsntNullAttribute)));
            }
        }
    }
}