using Dapper;
using expense_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace expense_api.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public AuthenticationRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<bool> IsValidUser(string userid, string password)
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select u.user_id as UserId, uc.password from [dbo].[users] u inner join [dbo].[user_credentials] uc
                                    on u.user_id = uc.user_id
                                    where u.active = 'A' 
                                    and u.user_id = @userid 
                                    and uc.password = @password";
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("userid", userid, System.Data.DbType.String, System.Data.ParameterDirection.Input, 100);
                    dp.Add("password", password, System.Data.DbType.String, System.Data.ParameterDirection.Input, 100);

                    conn.Open();
                    var result = await conn.QueryAsync<UserCredentials>(sql, dp);
                    if (result.Count() == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
