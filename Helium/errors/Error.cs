namespace Helium.errors
{
    class Error
    {
        string message;

        public Error(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return message;
        }
    }
}