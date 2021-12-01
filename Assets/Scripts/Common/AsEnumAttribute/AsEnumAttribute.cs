using System; 
using UnityEngine;

namespace SiberianWellness.Common
{
     /// <summary>
     /// Позволяет отрисовывать в редакторе строку с доступными значениями.
     /// Так же позволяет автоматически проверять корректность значения поля в редакторе.
     /// </summary>
     [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
     public class AsEnumAttribute : PropertyAttribute
     {
          public AsEnumAttribute(Type valuesSource)
          {
               ValuesSource = valuesSource;
          }

          public Type ValuesSource { get; }
     }
}