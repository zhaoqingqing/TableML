using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 扩展C#的方法
/// NOTE 当类型转换失败时，不抛出异常，而是=默认，让程序可以正常运行
/// 此同名文件在Unity的Project中已有， 所以方法命名修改
/// </summary>
public static class CSharpEx
{
    public static bool ToBool_(this string val)
    {
        bool ret = false;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = val.ToLower() == "true" || val.ToLower() != "0";
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static sbyte ToSByte_(this string val)
    {
        sbyte ret = 0;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToSByte(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static byte ToByte_(this string val)
    {
        byte ret = 0;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToByte(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    static public uint ToUInt_(this string str)
    {
        uint ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToUInt32(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    static public Int32 ToInt32_(this string str)
    {
        Int32 ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToInt32(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    static public short ToShort_(this string str)
    {
        short ret = 0;

        try
        {
            if (!String.IsNullOrEmpty(str))
            {
                ret = Convert.ToInt16(str);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static long ToLong_(this string val)
    {
        return ToInt64_(val);
    }

    public static long ToInt64_(this string val)
    {
        long ret = 0;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToInt64(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static float ToSingle_(this string val)
    {
        return ToFloat_(val);
    }

    public static float ToFloat_(this string val)
    {
        float ret = 0;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToSingle(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static double ToDouble_(this string val)
    {
        double ret = 0;
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToDouble(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static char ToChar_(this string val)
    {
        char ret = default(char);
        try
        {
            if (!String.IsNullOrEmpty(val))
            {
                ret = Convert.ToChar(val);
            }
        }
        catch (Exception)
        {
        }
        return ret;
    }

    public static Int32 ToInt32_(this object obj)
    {
        Int32 ret = 0;
        try
        {
            if (obj != null)
            {
                ret = Convert.ToInt32(obj);
            }
        }
        catch (Exception)
        {
        }

        return ret;
    }

    /// <summary>
    /// .net4.0 stringbuild 会有Clear 函数，到时可以删掉这个函数
    /// </summary>
    /// <param name="sb"></param>
    public static void Clear_(this StringBuilder sb)
    {
        sb.Length = 0;
    }


    /// <summary>
    /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
    /// </summary>
    public static Dictionary<TKey, TValue> TryAdd_<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key) == false) dict.Add(key, value);
        return dict;
    }

    /// <summary>
    /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
    /// </summary>
    public static Dictionary<TKey, TValue> AddOrReplace_<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key) == false)
        {
            dict.Add(key, value);
        }
        else
        {
            dict[key] = value;
        }
        return dict;
    }

}
