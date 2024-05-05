namespace RestaurantAPI;
public class ErrorHandlingMiddleware: IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    //wtrzyknij loggera poprzez konstruktor
    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            //wywołaj następny middleware bo tutaj nie chcemy wpływać na przychodzące zapytanie
            await next.Invoke(context);
        }
        catch (Exception e)
        {
            //ale za to chcemy reagować na wyłapane wyjątki żeby móc zapisać je poprzez loggera
            _logger.LogError(e, e.Message);

            //dodatkowo wysłanie generycznej odpowiedzi klientowi że wystąpił wyjątek
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
