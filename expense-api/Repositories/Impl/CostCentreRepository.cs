using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using expense_api.Models;

namespace expense_api.Repositories
{
    public class CostCentreRepository : ICostCentreRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public CostCentreRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<IEnumerable<CostCentre>> GetAll()
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select id, description
                                    from [dbo].[cost_centre]";

                    conn.Open();
                    var result = await conn.QueryAsync<CostCentre>(sql);
                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching cost centres: " + ex.Message);
            }
        }
    }
}
