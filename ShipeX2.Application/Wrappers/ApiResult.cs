namespace ShipeX2.Application.Wrappers
{
    public class ApiResult
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
    public class ApiResult<T>
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }

}
