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

                // 创建数据表
                string sql = string.Format("create table [{0}] ([id] INTEGER PRIMARY KEY, [value] TEXT COLLATE NOCASE)", tabName);
                DbCommand cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                // 添加参数
                cmd.Parameters.Add(cmd.CreateParameter());

                // 开始计时
                Stopwatch watch = new Stopwatch();
                watch.Start();

                //NOTE 为所有的插入操作只开启一个事务
                DbTransaction trans = conn.BeginTransaction();
                try
                {

                    // 连续插入1000条记录
                    for (int i = 0; i < maxCount; i++)
                    {
                        cmd.CommandText = string.Format("insert into [{0}] ([value]) values (?)", tabName);
                        cmd.Parameters[0].Value = i.ToString();
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();
                    ConsoleHelper.WriteLine("提交事务");
                    ConsoleHelper.Confirmation("创建表{0},并插入{1}条测试数据", tabName, maxCount);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ConsoleHelper.WriteLine("回滚事务,Exception:{0}", ex.Message);
                    throw;
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
}