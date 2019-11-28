using Dapper;
using expense_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace expense_api.Repositories
{
    public class UsersTransactionRepository : IUsersTransactionRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public UsersTransactionRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByUserId(string userId)
        {
            using (var conn = _sqlConnHelper.Connection)
            {
                //select * from transactions tr inner join [dbo].[category_lookup] lk
                // on tr.category = lk.category

                string sql = @"select t.Id, t.user_id as userid, t.trans_type as transtype, t.description, t.amount, t.tax, t.trans_date as transdate, t.category, lk.description as categoryDescription, t.status 
                                    from transactions t inner join category_lookup lk
                                    on t.category = lk.category
                                    where user_id = @userid
                                    and status='New'";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("userid", userId, System.Data.DbType.String, System.Data.ParameterDirection.Input, 100);

                try
                {
                    conn.Open();
                    var result = await conn.QueryAsync<Transaction>(sql, dp);
                    return result;
                }
                catch (Exception ex)
                {
                    // Log error
                    throw new Exception("Error fetching user transaction data: " + ex.Message);
                }


            }
        }
    }
}
