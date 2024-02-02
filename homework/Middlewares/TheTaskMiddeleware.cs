using System.Diagnostics;

namespace myTask.Middlewares;

public class TheTaskMiddeleware
{
    private RequestDelegate next;
    private readonly string logFilePath;

    public TheTaskMiddeleware(RequestDelegate next,strint logFilePath)
    {
        this.next = next;
        this.logFilePath=logFilePath;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
          WriteLogToFile($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");     
    }    

    public void WriteLogToFile(string logMessage)
    {
        using(StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine(logMessage);
        }
    }
}

public static partial class MyMiddleExtensions
{
    public static IApplicationBuilder UseMyMiddleExtensions(this IApplicationBuilder builder,string FilePath)
    {
        return builder.UseMiddleware<TheTaskMiddeleware>(logFilePath);
    }
}