using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TableML.Compiler;

/// <summary>
/// Author：qingqing.zhao (569032731@qq.com)
/// Date：2020/11/5 16:33
/// Desc：配置表转成lua文件
/// </summary>
public class LuaHelper
{
    /// <summary>
    /// 仅仅把配置的表头生成lua字段，不包含具体的数据部分，用于sqlite中的代码提示
    /// </summary>
    public static void GenLuaFile(TableCompileResult compileResult, string full_path)
    {
        if (compileResult.FieldsInternal != null)
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("return{");
            var count = compileResult.FieldsInternal.Count;
            var end = count - 1;
            for (int i = 0; i < count; i++)
            {
                var t = compileResult.FieldsInternal[i];

                str.AppendLine(t.Name + (i == end ? "" : ","));
            }
            str.AppendLine("}");
            if (File.Exists(full_path))
            {
                File.Delete(full_path);
            }

            File.WriteAllText(full_path, str.ToString());
        }
    }
}