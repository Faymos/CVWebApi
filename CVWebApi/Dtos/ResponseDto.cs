using System.Net;

namespace CVWebApi.Dtos
{
    public class ResponseDto
    {
        public string? Message { get; set; }
        public bool  Success { get; set; } = false;
        public HttpStatusCode? Status { get; set; } = HttpStatusCode.BadGateway;
    }
}
