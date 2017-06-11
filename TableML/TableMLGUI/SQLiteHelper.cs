using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using Array = System.Array;

/// <summary>
/// 作者：qingqing-zhao 邮箱：569032731@qq.com
/// 对sqlite的封装，方便快速插入数据到sqlite
/// 笔记：
/// 1. 表名和字段名加上[]，防止sql语句执行失败
/// 2. 数据列数不能大于表头列数
/// 
/// </summary>
public partial class SQLiteHelper
{
    /// <summary>
    /// tsv的最小行数
    /// </summary>
    private const int MinLine = 3;
    /// <summary>
    /// 从第几行开始是有效数据
    /// </summary>
    private const int StartRowIndex = 2;
    //数据库文件
    private static string dbfile = "data.db";
    /// <summary>
    /// sqlite关键字
    /// </summary>
    static string[] SQL_KEYWORDS = new string[]{
        "drop", "index", "group", "sort", "replace"
    };



    public static void Init(string dbPath)
    {
        dbfile = dbPath;
    }

    public static bool CheckIsSqlKeyword(string key)
    {
        foreach (var v in SQL_KEYWORDS)
        {
            if (key.Equals(v, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 更新数据库
    /// </summary>
    /// <param name="fileList">文件列表[完整路径]</param>
    public static void UpdateDB(string[] fileList)
    {
        if (fileList == null)
        {
            ConsoleHelper.Error("文件列表不能为空!");
            return;
        }
        if (fileList.Length < 1)
        {
            ConsoleHelper.Warning("文件列表为空!");
            return;
        }

        if (File.Exists(dbfile) == false)
        {
            // 创建数据库文件
            SQLiteConnection.CreateFile(dbfile);
        }

        DbProviderFactory factory = SQLiteFactory.Instance;
        using (DbConnection conn = factory.CreateConnection())
        {
            // 连接数据库
            if (conn != null)
            {
                conn.ConnectionString = string.Format("Data Source={0}", dbfile);
                conn.Open();

                DbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;

                // 开始计时
                Stopwatch watch = new Stopwatch();
                watch.Start();

                //NOTE 所有的插入操作只开启一个事务,这样插入大数据更快！
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    List<string> failList = new List<string>();
                    List<string> successList = new List<string>();
                    foreach (var filePath in fileList)
                    {
                        bool success = UpdateTable(filePath, cmd);
                        var fileName = Path.GetFileName(filePath);
                        if (!success)
                        {

                            failList.Add(fileName);
                        }
                        else
                        {
                            successList.Add(fileName);
                        }
                    }

                    trans.Commit();
                    ConsoleHelper.WriteLine("提交事务完成");
                    ConsoleHelper.Confirmation("更新成功{0}张表", successList.Count);
                    foreach (var fileName in successList)
                    {
                        ConsoleHelper.Confirmation("{0}", fileName);
                    }
                    if (failList.Count > 0)
                    {   //不打印 没有失败
                        ConsoleHelper.Error("更新失败{0}张表", failList.Count);
                    }
                    foreach (var fileName in failList)
                    {
                        ConsoleHelper.Error("{0}", fileName);
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ConsoleHelper.Error("回滚事务,Message:{0}", ex.Message);
                    ConsoleHelper.Error("Exception:{0}", ex);
                }

                // 停止计时
                watch.Stop();

                ConsoleHelper.Confirmation("执行耗时：{0} s", watch.Elapsed.TotalSeconds);
            }
            else
            {
                ConsoleHelper.Error("{0} 连接失败！", dbfile);
            }
        }
    }

    /// <summary>
    /// 更新数据库
    /// </summary>
    /// <param name="srcPath">文件目录</param>
    public static void UpdateDB(string srcPath)
    {
        if (Directory.Exists(srcPath) == false)
        {
            ConsoleHelper.Error("{0} 目录不存在!", srcPath);
            return;
        }

        var fileList = FileHelper.GetAllFiles(srcPath);
        List<string> pathList = new List<string>();

        foreach (FileInfo fileInfo in fileList)
        {
            pathList.Add(fileInfo.FullName);
        }
        UpdateDB(pathList.ToArray());
    }



    /// <summary>
    /// 更新单张数据表
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="dbCmd"></param>
    public static bool UpdateTable(string filePath, DbCommand dbCmd)
    {
        if (File.Exists(filePath) == false)
        {
            ConsoleHelper.Error("{0} 文件不存在！", filePath);
            return false;
        }
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        string[] lines = File.ReadAllLines(filePath);
        //TODO 当文件内容为空时的处理
        if (lines == null || lines.Length <= 0)
        {
            ConsoleHelper.Error("{0} 内容为空！", fileName);
            return false;
        }
        if (lines.Length < MinLine)
        {
            ConsoleHelper.Error("{0} 至少要有{1}行数据！", fileName, MinLine);
            return false;
        }
        //NOTE tsv是以tab作为分隔符
        var columnNames = lines[0].Split('\t');
        var columnTypes = lines[1].Split('\t');
        if (columnNames.Length != columnTypes.Length)
        {
            ConsoleHelper.Error("{0} 表头列数{1},数据类型列数{2}", fileName, columnNames.Length, columnTypes.Length);
        }

        //执行创建表，表名如果有数字，需要加[]
        var tableSql = string.Format("DROP TABLE IF EXISTS [{0}]", fileName);
        ConsoleHelper.ConfirmationWithBlankLine("创建表sql:{0}", tableSql);
        dbCmd.CommandText = tableSql;
        dbCmd.ExecuteNonQuery();


        //执行创建表的字段名和数据类型
        var sb = new System.Text.StringBuilder();
        sb.AppendFormat("CREATE TABLE [{0}] (", fileName);
        for (int i = 0; i < columnNames.Length; i++)
        {
            //NOTE 如果表字段名是SQLite关键字用[]
            //            if (CheckIsSqlKeyword(columnNames[i]))
            //            {
            //                ConsoleHelper.Warning("表{0}的字段名含有sql关键字{1}", fileName, columnNames[i]);
            //            }
            sb.AppendFormat("[{0}]", columnNames[i]);
            if (i <= columnTypes.Length - 1)
            {
                if (columnTypes[i].Trim().ToLower().Contains("int"))
                {
                    sb.Append(" integer,");
                }
                else
                {
                    sb.Append(" varchar(32),");
                }
            }
            else
            {
                ConsoleHelper.Error("index={0} ,超出索引columnTypes.Length={1}", i, columnTypes.Length);
            }
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(")");
        string sql = sb.ToString();
        ConsoleHelper.ConfirmationWithBlankLine("创建字段和数据类型sql:{0}", sql);
        dbCmd.CommandText = sql;
        dbCmd.ExecuteNonQuery();


        //执行数据插入
        int addNum = 0;
        for (int i = StartRowIndex; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');
            string[] newValue = new string[columnNames.Length];

            //NOTE 数据列数不能大于表头列数！
            if (values.Length > columnNames.Length)
            {
                //列数不一样有可能最后列是空白字符串,但要保证也能插入数据
                ConsoleHelper.Warning("{0},表头{1}列,但数据{2}列", fileName, columnNames.Length, values.Length);

                Array.Copy(values, 0, newValue, 0, columnNames.Length); // 拷贝，确保不会超出表头的
            }
            else
            {
                newValue = values;
            }

            var sb2 = new System.Text.StringBuilder();
            sb2.AppendFormat("INSERT INTO [{0}] VALUES(", fileName);
            for (int j = 0; j < newValue.Length; j++)
            {
                sb2.AppendFormat("\"{0}\",", newValue[j]);
            }
            sb2.Remove(sb2.Length - 1, 1);
            sb2.Append(")");
            var inserSql = sb2.ToString();
            ConsoleHelper.ConfirmationWithBlankLine("插入数据sql:{0}", inserSql);
            dbCmd.CommandText = inserSql;
            addNum += dbCmd.ExecuteNonQuery();

        }
        ConsoleHelper.Confirmation("已创建 {0} 表，并插入了 {1} 条数据", fileName, addNum);
        return true;
    }

}



