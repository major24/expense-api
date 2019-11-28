using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using expense_api.Models;

namespace expense_api.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public ExpenseRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<Expense> GetById(int id)
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sql = @"select Id, user_id as userid, cost_centre as costcentre, approver_id as approverid, status, submitted_date as submitteddate, updated_date as updateddate
                                    from [dbo].[expenses]";
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("id", id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);

                    conn.Open();
                    var result = await conn.QueryFirstOrDefaultAsync<Expense>(sql, dp);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting expense by id" + ex.Message);
            }
        }

        public Task<ExpenseReport> GetByIdForReport(int expenseid)
        {
            try
            {
                ExpenseReport expenseReport = new ExpenseReport();
                using (var conn = _sqlConnHelper.Connection)
                {
                    // multi query
                    string sqlExp = @"SELECT [id], [user_id] as userid, [cost_centre] as costcentre, [approver_id] as approverid, [status], [submitted_date] as submitteddate
                                        FROM [dbo].[expenses] where id = @expenseid;";

                    string sqlExpItems = @"select ei.id, ei.expense_id as expenseid, t.trans_type as transtype, t.description, t.amount, t.tax, clkp.description as category, t.trans_date as transdate
		                                    from [dbo].[expense_items] ei
		                                    inner join [dbo].[transactions] t
		                                    on ei.trans_id = t.id
		                                    inner join [dbo].[category_lookup] clkp
		                                    on t.category = clkp.category
		                                    where ei.expense_id = @expenseid;";

                    string sqlUser = @"select u.user_id as userid, u.first_name as firstname, u.last_name as lastname from [dbo].[users] u
	                                        inner join [dbo].[expenses] e
	                                        on e.user_id = u.user_id
	                                        where e.id = @expenseid;";

                    var queries = $"{sqlExp} {sqlExpItems} {sqlUser}";

                    conn.Open();
                    using (var multi = conn.QueryMultiple(queries, new { expenseid = expenseid }))
                    {
                        var expense = multi.Read<Expense>();
                        var expItems = multi.Read<ExpenseItem>().ToArray();
                        var user = multi.Read<User>();

                        expenseReport.Expense = expense.FirstOrDefault();
                        expenseReport.ExpenseItems = expItems;
                        expenseReport.User = user.FirstOrDefault();
                    }

                    return Task.FromResult(expenseReport);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting expense by id" + ex.Message);
            }
        }

        public Task<int> Save(Expense expense)
        {

            // expense contains one parent record and expense items in an array
            // items array contains new OOP and exiting CR transactions
            // for OOP, id is set as 0  
            // CR trans comes with existing id from sql table
            // If id == 0, then Insert to trans table, else update trans table
            // Once inserted, get the id from sql server and add to array. then use to insert to items table
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sqlTransInsert = @"insert into transactions (user_id, trans_type, description, amount, tax, trans_date, category, status, updated_date)
                                values (@userid, @transtype, @description, @amount, @tax, @transdate, @category, @status, @updateddate);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";

                    string sqlTransUpdate = @"update transactions 
                                                    set category = @category,
                                                        status = @status,
                                                        updated_date = SYSDATETIME()
                                                where id = @id";

                    string sqlExps = @"insert into expenses (user_id, cost_centre, approver_id, status, submitted_date)
                                values (@userid, @costcentre, @approverid, @status, convert(datetime, getdate(), 0));
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                    string sqlItem = @"insert into expense_items (expense_id, trans_id)
                                values (@expenseid, @transid)";

                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        // Save OOP expense items to trans table and get the id...
                        var items = expense.ExpenseItems;
                        foreach (var item in items)
                        {
                            DynamicParameters dp = new DynamicParameters();
                            string insertTransType = "OOP";
                            string insertStatus = "Submitted";
                            DateTime updatedDateTime = DateTime.Now;
                            var resultIdTrans = 0;
                            if (item.Id == 0)
                            {
                                // New items: Insert - OOP
                                dp.Add("userid", expense.UserId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("transtype", insertTransType, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("description", item.Description, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("amount", item.Amount, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                                dp.Add("tax", item.Tax, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                                dp.Add("transdate", item.TransDate, System.Data.DbType.Date, System.Data.ParameterDirection.Input);
                                dp.Add("category", item.Category, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("status", insertStatus, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("updateddate", updatedDateTime, System.Data.DbType.DateTime, System.Data.ParameterDirection.Input);
                                resultIdTrans = conn.Query<int>(sqlTransInsert, dp, transaction).Single();
                                // Assign id to items
                                item.Id = resultIdTrans;
                            }
                            else
                            {
                                // Existing trans: Update (CR)
                                dp.Add("category", item.Category, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("status", insertStatus, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                                dp.Add("id", item.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                                resultIdTrans = conn.Execute(sqlTransUpdate, dp, transaction, commandType: System.Data.CommandType.Text);
                            }
                        }

                        DynamicParameters dpExps = new DynamicParameters();
                        dpExps.Add("userid", expense.UserId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                        dpExps.Add("costcentre", expense.CostCentre, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                        dpExps.Add("approverid", expense.ApproverId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                        dpExps.Add("status", expense.Status, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                        var resultIdExps = conn.Query<int>(sqlExps, dpExps, transaction).Single();

                        // Save each items
                        foreach (var item in items)
                        {
                            DynamicParameters dpItem = new DynamicParameters();
                            dpItem.Add("expenseid", resultIdExps, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                            dpItem.Add("transid", item.Id, System.Data.DbType.Int32, System.Data.ParameterDirection.Input);
                            var result = conn.Execute(sqlItem, dpItem, transaction, commandType: System.Data.CommandType.Text);
                        }
                        transaction.Commit();
                        return Task.FromResult(resultIdExps);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error inserting expense data: " + ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                // Log error
                throw new Exception("Error accessing db: " + ex.Message);
            }
        }
    }
}
