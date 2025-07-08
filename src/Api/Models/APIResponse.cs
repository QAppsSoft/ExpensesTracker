using System.Net;

namespace Api.Models;

public class APIResponse
{
    private APIResponse(bool isSuccess, object? result, HttpStatusCode statusCode)
    {
        IsSuccess = isSuccess;
        Result = result;
        StatusCode = statusCode;
        ErrorMessages = [];
    }

    public bool IsSuccess { get; }
    public object? Result { get; }
    public HttpStatusCode StatusCode { get; }
    public List<string> ErrorMessages { get; }

    public static APIResponse CreateSuccess(object result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new APIResponse(true, result ?? throw new ArgumentNullException(nameof(result)), statusCode);
    }

    public static APIResponse CreateError(HttpStatusCode statusCode, params string[] errorMessages)
    {
        var response = new APIResponse(false, null, statusCode);
        if (errorMessages != null)
        {
            response.ErrorMessages.AddRange(errorMessages);
        }
        return response;
    }
}

public class APIResponse<TResult>
{
    private APIResponse(bool isSuccess, TResult? result, HttpStatusCode statusCode, IReadOnlyList<string> errorMessages)
    {
        IsSuccess = isSuccess;
        Result = result;
        StatusCode = statusCode;
        ErrorMessages = errorMessages;
    }

    public bool IsSuccess { get; }
    public TResult? Result { get; }
    public HttpStatusCode StatusCode { get; }
    public IReadOnlyList<string> ErrorMessages { get; }

    public static APIResponse<TResult> CreateSuccess(TResult result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new APIResponse<TResult>(isSuccess: true, result ?? throw new ArgumentNullException(nameof(result)), statusCode, []);
    }

    public static APIResponse<TResult> CreateError(HttpStatusCode statusCode, params string[] errorMessages)
    {
        return new APIResponse<TResult>(isSuccess: false, default, statusCode, errorMessages.AsReadOnly());
    }
}