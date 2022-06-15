using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Reports.Api.Models {
    [ExcludeFromCodeCoverage]
    public class ReportParam {
        public string Type { get; set; }
        public IDictionary<string, string> Values { get; set; }
    }
}
