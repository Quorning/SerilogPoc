using System.Runtime.CompilerServices;

namespace Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        public static Serilog.ILogger Here(this Serilog.ILogger logger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return logger
                .ForContext("Caller_MemberName", memberName)
                .ForContext("Caller_FilePath", sourceFilePath)
                .ForContext("Caller_LineNumber", sourceLineNumber);
        }
    }
}
