using System.Xml.Serialization;

namespace DVLDApi.Helpers
{
    public class ApiResponse
    {
        public static string StatusSuccess = "success";
        public static string StatusFail = "fail";
        public static string StatusError = "error";


        public static object CreateResponse(string Status)
        {
            return new
            {
                status = Status
            };
        }

        public static object CreateResponse(string Status, string Message)
        {
            return new
            {
                status = Status,
                message = Message
            };
        }

        public static object CreateResponse(string Status, object Data)
        {
            return new
            {
                status = Status,
                data = Data
            };
        }

        public static object CreateResponse(string Status, string Message, object Data)
        {
            return new
            {
                status = Status,
                message = Message,
                data = Data
            };
        }
    }
}
