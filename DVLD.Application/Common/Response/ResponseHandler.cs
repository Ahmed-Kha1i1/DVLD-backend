using System.Net;

namespace DVLD.Application.Common.Response
{
    public class ResponseHandler
    {

        public ResponseHandler()
        {

        }
        public Response<T> BadRequest<T>(string? message = "Bad Request")
        {
            return new Response<T>(HttpStatusCode.BadRequest, message);
        }

        public Response<T> ValidationError<T>(T? Error, string? message = "Validation Error")
        {
            return new Response<T>(HttpStatusCode.BadRequest, Error, message);
        }

        public Response<T> NotFound<T>(string? message = "Not Found")
        {
            return new Response<T>(HttpStatusCode.NotFound, message);
        }


        public Response<T> Success<T>(T data, string? message = null)
        {
            return new Response<T>(HttpStatusCode.OK, data, message);
        }


        public Response<T> Created<T>(T data, string? message = null)
        {
            return new Response<T>(HttpStatusCode.Created, data, message);
        }


        public Response<T> Unauthorized<T>(string? message = "Unauthorized")
        {
            return new Response<T>(HttpStatusCode.Unauthorized, message);
        }


        public Response<T> Fail<T>(T? date, string? message = null)
        {
            return new Response<T>(HttpStatusCode.InternalServerError, date, message);
        }


        public Response<T> Custom<T>(HttpStatusCode statusCode, T data, string? message = null)
        {
            return new Response<T>(statusCode, data, message);
        }
    }
}




