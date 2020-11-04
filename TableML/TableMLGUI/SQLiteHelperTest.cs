using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

public partial class SQLiteHelper
{
    /// <summary>
    /// 插入测试数据
    /// </summary>
    public static void TestInsert()
    {
        if (File.Exists(dbfile) == false)
        {
            // 创建数据库文件
            SQLiteConnection.CreateFile(dbfile);
        }
        int maxCount = 100000;
        string tabName = "TestTable";
        DbProviderFactory factory = SQLiteFactory.Instance;
        using (DbConnection conn = factory.CreateConnection())
        {
            // 连接数据库
            if (conn != null)
            {
                conn.ConnectionString = string.Format("Data Source={0}", dbfile);
                conn.Open();
                DbCommand dbCmd = conn.CreateCommand();

                //执行创建表，表名如果有数字，需要加[]
                var tableSql = string.Format("DROP TABLE IF EXISTS [{0}]", tabName);
                ConsoleHelper.InfoWithNewLine("创建表sql:{0}", tableSql);
                dbCmd.CommandText = tableSql;
                dbCmd.ExecuteNonQuery();

                // 创建数据表
                string sql = string.Format("create table [{0}] ([id] INTEGER PRIMARY KEY, [value] TEXT COLLATE NOCASE)", tabName);
                dbCmd.Connection = conn;
                dbCmd.CommandText = sql;
                dbCmd.ExecuteNonQuery();

                // 添加参数
                dbCmd.Parameters.Add(dbCmd.CreateParameter());

                // 开始计时
                Stopwatch watch = new Stopwatch();
                watch.Start();

                //NOTE 为所有的插入操作只开启一个事务
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    // 连续插入xxx条记录
                    for (int i = 0; i < maxCount; i++)
                    {
                        dbCmd.CommandText = string.Format("insert into [{0}] ([value]) values (?)", tabName);
                        dbCmd.Parameters[0].Value = i.ToString();
                        dbCmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                    ConsoleHelper.Log("提交事务");
                    ConsoleHelper.Info("创建表{0},并插入{1}条测试数据", tabName, maxCount);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ConsoleHelper.Log("回滚事务,Exception:{0}", ex.Message);
                    throw;
                }

                // 停止计时
                watch.Stop();
                ConsoleHelper.Info("执行耗时：{0} s", watch.Elapsed.TotalSeconds);
            }
            else
            {
                ConsoleHelper.Error("{0} 连接失败！", dbfile);
            }
        }
    }
}