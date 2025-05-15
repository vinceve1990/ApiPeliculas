using System.Net;

namespace ApiPeliculas.Models
{
    public class RespuestaAPI
    {
        public RespuestaAPI() { 
            ErrorMensaje = new List<string>();
        }

        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMensaje { get; set; }
        public object Result { get; set; }
    }
}
