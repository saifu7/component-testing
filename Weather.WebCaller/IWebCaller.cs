namespace Weather.WebCaller
{
    public interface IWebCaller
    {
        Task<T> SendAsync<T>(HttpMethod method, string uri, object? content = null);
    }
}
