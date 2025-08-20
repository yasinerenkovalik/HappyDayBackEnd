namespace HappyDay.Application.Wrappers;

public class GeneralResponse<T> where T : class, new()
{
    
    
    public T? Data { get; set; }
    public bool isSuccess { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public string Message { get; set; }
        
}
