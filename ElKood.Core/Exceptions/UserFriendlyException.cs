using System.Net;

namespace ElKood.Core.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public HttpStatusCode Code { get; }

        public UserFriendlyException(string message, HttpStatusCode code) : base(message)
        {
            Code = code;
        }
    }
}
