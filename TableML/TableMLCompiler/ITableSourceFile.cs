using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableML.Compiler
{
    /// <summary>
    /// 读取带有头部、声明和注释的文件表格
    /// </summary>
    public interface ITableSourceFile
    {
        Dictionary<string, int> ColName2Index { get; set; }
        Dictionary<int, string> Index2ColName { get; set; }
        Dictionary<string, string> ColName2Statement { get; set; }//  string,or something
        Dictionary<string, string> ColName2Comment { get; set; }// string comment
        int GetRowsCount();
        int GetColumnCount();
        string GetString(string columnName, int row);
        string ExcelFileName { get; set; }
    }
}
