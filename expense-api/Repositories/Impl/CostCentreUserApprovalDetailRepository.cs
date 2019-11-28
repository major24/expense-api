using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using expense_api.Models;

namespace expense_api.Repositories
{
    public class CostCentreUserApprovalDetailRepository : ICostCentreUserApprovalDetailRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public CostCentreUserApprovalDetailRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<IEnumerable<CostCentreUserApprovalDetail>> GetAll()
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    // Use multiple queries
                    string sqlcc = @"select * from cost_centre;";
                    string sqcc_user = @"select ccusr.id as id, ccusr.user_id as userid, ccusr.limit as limit, u.first_name as firstname, u.last_name as lastname
                                    from[dbo].[cost_centre_user_approval] ccusr
                                    join[dbo].[users] u on ccusr.user_id = u.user_id;";

                    var queries = $"{sqlcc} {sqcc_user}";
                    conn.Open();
                    var result = await conn.QueryMultipleAsync(queries);
                    // retieve the results
                    var ccs = result.Read<CostCentre>();
                    var cc_users = result.Read<CostCentreUserApproval>();

                    // Map manually for now. Explore later on mapping????
                    IList <CostCentreUserApprovalDetail> list = new List<CostCentreUserApprovalDetail>();
                    foreach(var cc in ccs)
                    {
                        CostCentreUserApprovalDetail ccUserApproval = new CostCentreUserApprovalDetail();
                        ccUserApproval.CostCentre = cc;
                        // get all the approves associated with cc
                        var approvers = from userapr in cc_users where userapr.Id == cc.Id select userapr;
                        ccUserApproval.Approvals = approvers.ToArray();
                        list.Add(ccUserApproval);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error fetching cost centre user approval details: " + ex.Message);
            }
        }
    }
}
