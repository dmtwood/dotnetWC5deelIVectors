using System;
using System.Collections.Generic;
using System.Linq;

namespace Vector
{
    class Email
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        private string from;

        public string From
        {
            get { return from; }
            set {
                if (value.IsValidEmail())
                {
                    from = value;
                }
                else
                {
                    throw new ArgumentException("From field does not contain a valid e-mail adress.");
                }
            }
        }

        private List<string> to;

        public List<string> To
        {
            get { return to; }
            set
            {
                to = value.Where(x => x.IsValidEmail()).ToList();
            }
        }

        public DateTime Received { get; set; }

        public override string ToString()
        {
            return $"From: {From}\n" +
                $"To: {string.Join(", ", To)}\n" +
                $"Received: {Received}\n\n" +
                $"{Body}";

        }
    }
}
