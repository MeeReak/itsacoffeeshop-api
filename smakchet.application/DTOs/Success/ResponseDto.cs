using smakchet.application.DTOs.Error;

namespace smakchet.application.DTOs.Success
{
  public class ResponseDto<T>
  {
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public ErrorDto? Error { get; set; }

    public static ResponseDto<T> Ok(T? data, string message = "Success") =>
        new()
        {
          Success = true,
          Data = data,
          Message = message
        };

    public static ResponseDto<T> Fail(string message, ErrorDto error) =>
        new()
        {
          Success = false,
          Message = message,
          Error = error
        };
  }
}
