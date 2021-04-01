using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Settings
{
    public class GlobalSettings
    {
        public Guid EnvironmentGuid { get; set; }
        public string EnvironmentLevel { get; set; }
        public Guid SourceSystemGuid { get; set; }
        public string SourceSystemName { get; set; }
        public string ApplicationName { get; set; }
    }
}
