using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatedParameter<T> : MonoBehaviour
{
    protected Dictionary<string, object> fieldInfos = new();
    private  List<FieldInfo> cachedFields;
    private  List<PropertyInfo> cachedProperties;

    public StatedParameter()
    {
        cachedFields = new List<FieldInfo>();
        cachedProperties = new List<PropertyInfo>();

        // 缓存字段
        var fields = typeof(T).GetFields();
        foreach (var field in fields)
        {
            if (field.IsDefined(typeof(StatedPara), false))
            {
                cachedFields.Add(field);
            }
        }

        // 缓存属性
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            if (property.IsDefined(typeof(StatedPara), false) && property.CanRead && property.CanWrite)
            {
                cachedProperties.Add(property);
            }
        }
    }

    /// <summary>
    /// 获得StatedPara数据快照
    /// </summary>
    /// <param name="var">被控制对象</param>
    protected void LookPara(T var)
    {
        // 获取字段值
        foreach (var field in cachedFields)
        {
            if (fieldInfos.ContainsKey(field.Name))
            {
                fieldInfos[field.Name] = field.GetValue(var);
            }
            else
            {
                fieldInfos.Add(field.Name, field.GetValue(var));
            }
        }

        // 获取属性值
        foreach (var property in cachedProperties)
        {
            if (fieldInfos.ContainsKey(property.Name))
            {
                fieldInfos[property.Name] = property.GetValue(var);
            }
            else
            {
                fieldInfos.Add(property.Name, property.GetValue(var));
            }
        }
    }

    /// <summary>
    /// 恢复上一次StatedPara数据快照
    /// </summary>
    /// <param name="var">被控制对象</param>
    public void ResetPara(T var)
    {
        // 恢复字段值
        foreach (var field in cachedFields)
        {
            if (fieldInfos.ContainsKey(field.Name))
            {
                field.SetValue(var, fieldInfos[field.Name]);
            }
        }

        // 恢复属性值
        foreach (var property in cachedProperties)
        {
            if (fieldInfos.ContainsKey(property.Name))
            {
                property.SetValue(var, fieldInfos[property.Name]);
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class StatedPara : Attribute
{
}