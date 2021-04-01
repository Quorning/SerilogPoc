using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Trace
{
    public static class TraceHeaderReader 
    {
        public static Guid GetHeaderValuesGuid(IHeaderDictionary headers, string traceHeaderName)
        {
            if (headers.Any(traceRequestHeader => traceHeaderName.Equals(traceRequestHeader.Key.ToLower()))) return Guid.NewGuid();

            var headerValueAsString = GetHeaderValue(traceHeaderName, headers);
            Guid headerValue = Guid.TryParse(headerValueAsString, out headerValue) ? headerValue : Guid.Empty;

            return headerValue;
        }

        public static string GetHeaderValue(string headerName, IHeaderDictionary headers)
        {
            foreach (var header in headers)
            {
                if (header.Key.ToLower() == headerName.ToLower())
                    return header.Value.First();
            }

            return string.Empty;
        }
    }
}
