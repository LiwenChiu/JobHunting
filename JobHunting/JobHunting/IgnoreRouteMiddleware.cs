public class IgnoreRouteMiddleware
{
    private readonly RequestDelegate next;
    public IgnoreRouteMiddleware(RequestDelegate next)
    {
        this.next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.HasValue && context.Request.Path.Value.EndsWith("aspx"))
        {
            return; //ignore
        }
        await next.Invoke(context);
    }
}