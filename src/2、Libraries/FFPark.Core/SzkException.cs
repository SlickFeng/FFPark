using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core
{
    [Serializable]
    public class FFParkException : Exception
    {

        public FFParkException()
        {
        }


        public FFParkException(string message)
            : base(message)
        {
        }


        public FFParkException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }


        protected FFParkException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


        public FFParkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
