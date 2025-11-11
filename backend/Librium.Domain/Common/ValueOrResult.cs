using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Common;

public class ValueOrResult
{
    public bool isSuccess { get; }
    public string? ErrorMessage { get; set; }

    protected ValueOrResult(bool isSuccess, string? errorMessage)
    {
        this.isSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ValueOrResult Success() => new(true, null);
    public static ValueOrResult Failure(string error) => new(false, error);
}

public class ValueOrResult<T> : ValueOrResult
{
    public T? Value { get; }
    public ValueOrResult(T? value, bool isSuccess, string? errorMessage) : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public static ValueOrResult<T> Success(T value) => new(value, true, null);
    public static ValueOrResult<T> Failure(string error) => new(default, false, error);
}
