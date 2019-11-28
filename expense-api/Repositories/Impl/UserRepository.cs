using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using expense_api.Models;

namespace expense_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public UserRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select user_id as userid, first_name as firstname, last_name as lastname, manager_id as managerid, cost_centre as costcentre, active, email
                                    from [dbo].[users]";

                    conn.Open();
                    var result = await conn.QueryAsync<User>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching users: " + ex.Message);
            }
        }

        public async Task<User> GetById(string userid)
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select user_id as userid, first_name as firstname, last_name as lastname, manager_id as managerid, cost_centre as costcentre, active, email
                                    from [dbo].[users]
                                    where user_id = @userid";
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("userid", userid, System.Data.DbType.String, System.Data.ParameterDirection.Input, 100);

                    conn.Open();
                    var result = await conn.QueryFirstOrDefaultAsync<User>(sql, dp);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching user data: " + ex.Message);
            }
        }
    }
}
