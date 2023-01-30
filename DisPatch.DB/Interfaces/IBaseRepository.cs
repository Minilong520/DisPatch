using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.DB.Interfaces
{
    public interface IBaseRepository
    {
        IDbConnection GetConnection(string connStr);

        int ExecuteSqlInt(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30);

        string Query1RecordString(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30);

        T Query1Record<T>(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30);

        IEnumerable<T> QueryRecords<T>(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30);

        DataTable QueryTable(string sql, string connStr = null, IDbTransaction transaction = null, int commandTimeout = 30);

    }
}
