using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Shared.Response
{
    public class ResponseCus<T>
    {
        public bool Succeeded { get; set; }

        public string Message { get; set; }

        public object Errors { get; set; }

        public T Data { get; set; }

        public ResponseCus()
        {
        }

        public ResponseCus(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ResponseCus(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public ResponseCus(string message, List<string> errors)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
        }
    }
}
