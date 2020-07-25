namespace Msa.Core.Infra
{
    using System;

    public class Outcome
    {

        public Outcome(
            Boolean success, 
            String message = null,
            Exception exception = null)
        {
            if (!success && String.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException("Message is required for failure outcomes");
            }

            this.Success = success;
            this.Message = $"{(message ?? String.Empty)}. {exception?.ToString() ?? String.Empty}";
        }

        public Boolean Success { get; private set; }
        public String Message { get; private set; }
        public String StatusText => this.Success ? "Success" : "Failure";

        public static Outcome Succcess()
        {
            return new Outcome(true);
        }

        public static Outcome Failure(String message, Exception exception = null)
        {
            return new Outcome(false, message, exception);
        }

        public override String ToString()
        {
            if (this.Success)
            {
                return this.StatusText;
            }
            
            return $"{this.StatusText}: {this.Message}";
        }
    }
}