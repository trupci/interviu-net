using Dapper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Reports.Api.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ReportsRepository
    {
        private string _connString;

        public ReportsRepository(IConfiguration config)
        {
            _connString = config.GetConnectionString("reportsConnection");
        }

        public async Task<IEnumerable<Report>> GetReports(string userName)
        {
            using (DbConnection conn = new SqlConnection(_connString))
            {
                await conn.OpenAsync();
                var tables = await conn.QueryAsync<Report>("[dbo].[GetReports]",
                    new
                    {
                        user_type_id = userName
                    }, commandType: CommandType.StoredProcedure);

                return tables;
            }
        }
    }
}
