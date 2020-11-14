using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

/// <summary>
/// 对console的扩充，用于打印出不同颜色的字
/// </summary>
public class ConsoleHelper
{
    /// <summary>
    /// default 白色字
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Log(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args));
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void InfoWithNewLine(string message, params object[] args)
    {
        Info(message,args);
        Console.WriteLine();
    }

    /// <summary>
    /// info 蓝色字
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Info(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args));
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    /// <summary>
    /// warn 黄色字
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Warning(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Yellow;
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args));
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    /// <summary>
    /// error 红色字
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Error(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args));
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    
    public static void ErrorWithStack(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        //打印出错堆栈
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args),new StackTrace());
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}

