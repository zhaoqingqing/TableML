namespace TableML.Compiler
{
    public static class ExcelConfig
    {
        /// <summary>
        /// excel的格式是否为ksframework形式的配置表
        /// </summary>
        public static bool IsKSFrameworkFormat = true;
        /// <summary>
        /// 默认使用Excel的文件名作为生成类和tsv的名字，如果有特殊需求比如从Excel的指定列读取导出的类名，可以查看定制的范例
        /// </summary>
        public static bool IsSaveCompileResult = false;
    }
}