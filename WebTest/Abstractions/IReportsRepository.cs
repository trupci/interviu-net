using Reports.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reports.Api.Abstractions {
    public interface IReportsRepository {
        Task<IEnumerable<Report>> GetReports(string user);
    }
}
