using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 对console的扩充
/// </summary>
public class ConsoleHelper
{
    public static void WriteLine(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        var msg = string.Concat("[", System.DateTime.Now.ToString("u"), "]", string.Format(message, args));
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public static void Log(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message, args);
        Console.ResetColor();
    }

    public static void ConfirmationWithBlankLine(string message, params object[] args)
    {
        Confirmation(message,args);
        Console.WriteLine();
    }

    public static void Confirmation(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message, args);
        Console.ResetColor();
    }

    public static void Warning(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message, args);
        Console.ResetColor();
    }

    public static void Error(string message, params object[] args)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message, args);
        Console.ResetColor();
        //TODO 打印出错堆栈
    }
}

