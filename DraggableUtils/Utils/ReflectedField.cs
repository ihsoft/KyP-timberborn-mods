// Kerbal Development tools.
// Author: igor.zavoychinskiy@gmail.com
// This software is distributed under Public domain license.

using System.Reflection;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace KSPDev.ProcessingUtils {

/// <summary>Wrapper to implement efficient access to the class fields via reflection.</summary>
/// <remarks>It ignores the access scope. Use <see cref="IsValid"/> to determine if the reflection was successful.
/// </remarks>
/// <example>
/// Below is an accessor to deal with a boolean field, named "isTrueField", from type <c>MyType</c>:
/// <code><![CDATA[
/// static readonly ReflectedField<MyType, bool> check = new("isTrueField");
/// ]]></code>
/// </example>
/// <typeparam name="T">type of the class. It's inferred at the instantiation.</typeparam>
/// <typeparam name="TV">type of the field value.</typeparam>
public class ReflectedField<T, TV> {
  readonly FieldInfo _fieldInfo;

  public ReflectedField(string fieldName) {
    _fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (_fieldInfo == null) {
      Debug.LogErrorFormat("Cannot obtain field {0} from {1}", fieldName, typeof(T));
    }
  }

  /// <summary>Indicates if the target field was found and ready to use.</summary>
  public bool IsValid() {
    return _fieldInfo != null;
  }

  /// <summary>Gets the field value or returns a default value if the field is not found.</summary>
  public TV Get(T instance) {
    return _fieldInfo != null ? (TV)_fieldInfo.GetValue(instance) : default(TV);
  }

  /// <summary>Gets the field value or returns the provided default value if the field is not found.</summary>
  public TV Get(T instance, TV defaultValue) {
    return _fieldInfo != null ? (TV)_fieldInfo.GetValue(instance) : defaultValue;
  }

  /// <summary>Sets the field value or does nothing if the field is not found.</summary>
  public void Set(T instance, TV value) {
    if (_fieldInfo != null) {
      _fieldInfo.SetValue(instance, value);
    }
  }
}

}
