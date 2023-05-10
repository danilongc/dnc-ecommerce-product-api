using System;
namespace DNCEcommerceApi.Exceptions;


[Serializable]
public class BusinessException : Exception
{
	public BusinessException() : base() { }

    public ErrorCodeEnum Code { get; set; }
	public string Message { get; set; }
	public int StatusCode { get; init; }
}



