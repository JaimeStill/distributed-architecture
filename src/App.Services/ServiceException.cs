namespace App.Services;
public class ServiceException<T> : Exception
{
    public ServiceException(string method, Exception ex)
        : base(BuildMessage(method), ex) { }

    static string BuildMessage(string method)
    {
        string type = typeof(T).ToString();
        return $"{type} {method} method error";
    }
}