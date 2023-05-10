using System;
using System.Text.Json;

namespace DNCEcommerceApi.Data.Dtos;

public class UnprocessableResponse
{
    public string Code { get; set; }
    public string Message { get; set; }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}

