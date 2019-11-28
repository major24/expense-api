using expense_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace expense_api.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public TransactionRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select Id, user_id as userid, trans_type as transtype, description, amount, tax, trans_date as transdate, category, status 
                                    from [dbo].[transactions]";
                    conn.Open();
                    var result = await conn.QueryAsync<Transaction>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public async Task<Transaction> GetById(int id)
        {
            using (var conn = _sqlConnHelper.Connection)
            {
                string sql = @"select Id, user_id as userid, trans_type, description, amount, tax, trans_date, status 
                                    from transactions
                                    where id = @id";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("id", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input, 100);

                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<Transaction>(sql, dp);
                return result;
            }
        }


    }
}
