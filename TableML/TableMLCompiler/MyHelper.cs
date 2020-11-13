using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// Author：qingqing.zhao (569032731@qq.com)
/// Date：2020/11/13 15:55
/// Desc：
/// </summary>
public class MyHelper
{
    public static bool IsNumber(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        var pattern = @"^\d*$";
        return Regex.IsMatch(str, pattern);
    }
}