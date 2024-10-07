using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatedParameter<T> : MonoBehaviour
{
    protected Dictionary<string, object> fieldInfos = new();

    public StatedParameter()
    {
    }

    /// <summary>
    /// 获得StatedPara数据快照
    /// </summary>
    /// <param name="var">被控制对象</param>
    protected void LookPara(T var)
    {
        // 获取字段
        var FIs = typeof(T).GetFields();
        foreach (var item in FIs)
        {
            if (item.IsDefined(typeof(StatedPara), false))
            {
                if (fieldInfos.ContainsKey(item.Name))
                {
                    fieldInfos[item.Name] = item.GetValue(var);
                    continue;
                }
                fieldInfos.Add(item.Name, item.GetValue(var));
            }
        }

        // 获取属性
        var PIs = typeof(T).GetProperties();
        foreach (var item in PIs)
        {
            if (item.IsDefined(typeof(StatedPara), false) && item.CanRead && item.CanWrite)
            {
                if (fieldInfos.ContainsKey(item.Name))
                {
                    fieldInfos[item.Name] = item.GetValue(var);
                    continue;
                }
                fieldInfos.Add(item.Name, item.GetValue(var));
            }
        }
    }

    /// <summary>
    /// 恢复上一次StatedPara数据快照
    /// </summary>
    /// <param name="var">被控制对象</param>
    public void ResetPara(T var)
    {
        // 恢复字段
        var FIs = typeof(T).GetFields();
        foreach (var item in FIs)
        {
            if (fieldInfos.ContainsKey(item.Name))
            {
                item.SetValue(var, fieldInfos[item.Name]);
            }
        }

        // 恢复属性
        var PIs = typeof(T).GetProperties();
        foreach (var item in PIs)
        {
            if (fieldInfos.ContainsKey(item.Name) && item.CanWrite)
            {
                item.SetValue(var, fieldInfos[item.Name]);
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class StatedPara : Attribute
{
}