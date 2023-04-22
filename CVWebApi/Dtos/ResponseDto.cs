using System.Net;

namespace CVWebApi.Dtos
{
    public class ResponseDto
    {
        public string? Message { get; set; }
        public bool  Success { get; set; }
        public HttpStatusCode? Status { get; set; }
    }
}
