using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TableML.Compiler;

/// <summary>
/// Author：qingqing.zhao (569032731@qq.com)
/// Date：2020/11/5 16:33
/// Desc：配置表转成lua文件
///         使用了C#6的语法
/// </summary>
public class LuaHelper
{
    /// <summary>
    /// 仅仅生成lua字段，不包含具体的数据部分，用于sqlite中的代码提示
    /// </summary>
    public static void GenLuaFile(TableCompileResult compileResult, string exportPath)
    {
        if (compileResult.FieldsInternal != null)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder typeBuilder = new StringBuilder();
            var fileName = Path.GetFileNameWithoutExtension(exportPath);
            typeBuilder.AppendLine($"---@class {fileName}");
            builder.AppendLine("return {");
            var count = compileResult.FieldsInternal.Count;
            var end = count - 1;
            for (int i = 0; i < count; i++)
            {
                var t = compileResult.FieldsInternal[i];
                builder.AppendLine("\t" + t.Name + (i == end ? "" : ","));
                typeBuilder.AppendLine($"---@field public {t.Name} {t.Type}");
            }

            builder.AppendLine("}");
            if (File.Exists(exportPath))
            {
                File.Delete(exportPath);
            }

            var dirName = Path.GetDirectoryName(exportPath);
            if (Directory.Exists(dirName) == false)
            {
                Directory.CreateDirectory(dirName);
            }

            File.WriteAllText(exportPath, typeBuilder.ToString() + builder.ToString());
        }
    }
}