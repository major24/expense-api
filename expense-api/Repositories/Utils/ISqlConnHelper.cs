using System;
using System.Data;

namespace expense_api.Repositories
{
    public interface ISqlConnHelper
    {
        IDbConnection Connection { get; }
    }
}
