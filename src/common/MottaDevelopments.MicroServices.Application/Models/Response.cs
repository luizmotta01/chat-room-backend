using System.Collections.Generic;
using System.Net;

namespace MottaDevelopments.MicroServices.Application.Models
{
    public class Response<T> where T : class
    {
        public T Payload { get; private set; }

        public HttpStatusCode StatusCode { get; set; }

        public List<string> Messages { get; set; }
        
        public Response(T payload)
        {
            Payload = payload;
        }
        
    }
}