using System;
using System.Net;
using DNCEcommerceApi.Data.Dtos;
using DNCEcommerceApi.Exceptions;

namespace DNCEcommerceApi;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case BusinessException e :
                handleBusinessException(context, e);
                break;
            default:
                break;
        }
    }

    private async void handleBusinessException(HttpContext context, BusinessException e)
    {
        context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        string jsonErrorMessage = new UnprocessableResponse()
        {
            Code = (e as BusinessException).Code.ToString(),
            Message = (e as BusinessException).Message
        }.ToJson();

        await context.Response.WriteAsync(jsonErrorMessage);
    }
}
