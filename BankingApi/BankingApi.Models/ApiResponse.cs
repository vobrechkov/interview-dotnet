using System.Collections.Generic;
using System.Net;

namespace BankingApi.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
