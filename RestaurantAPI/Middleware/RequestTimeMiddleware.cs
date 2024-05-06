using System.Diagnostics;

namespace RestaurantAPI;
public class RequestTimeMiddleware : IMiddleware
{
    private Stopwatch _stopwatch;
    private readonly ILogger<RequestTimeMiddleware> _logger;
    
    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _stopwatch = new Stopwatch();
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //przed rozpoczęciem wywołania kolejnego middleware chcemy rozpocząć pomiar czasu
        _stopwatch.Start();
        await next.Invoke(context);
        //zaraz po wywołaniu chcemy pomiar zakończyć, jeżeli czasd przekroczy 4s to chemy zapisać 
        // o tym informację do loggera
        _stopwatch.Stop();

        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        if (elapsedMilliseconds / 1000 > 4)
        {
          var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms";
          _logger.LogInformation(message);  
        }
    }
}
