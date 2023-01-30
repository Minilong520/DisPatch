using DisPatch.DB.Implements;
using DisPatch.DB.Interfaces;
using DisPatch.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisPatch.DB.Factories
{
    public class DBFactory
    {
        public static IBaseRepository GetRepository(DBType type, string connStr = null)
        {
            switch (type)
            {
                case DBType.sqlserver:
                    return new MSRepository(connStr);
                case DBType.oracle:
                    return new OracleRepository(connStr);
                case DBType.mysql:
                    return new MySqlRepository(connStr);
                default:
                    return null;
            }
        }
    }
}
