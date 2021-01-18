using System;

namespace App
{
    public sealed class Envelope
    {
        public object Result { get; }
        public string ErrorMessage { get; }
        public DateTime TimeGenerated { get; }


        private Envelope(object result, string error)
        {
            Result = result;
            ErrorMessage = error;
            TimeGenerated = DateTime.Now;
        }


        public static Envelope Ok(object result = null)
        {
            return new Envelope(result, null);
        }


        public static Envelope Error(string error)
        {
            return new Envelope(null, error);
        }
    }
}
