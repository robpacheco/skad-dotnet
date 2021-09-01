namespace Skad.Common.Api.Common.Representations
{
    public class ApiErrorRep
    {
        public ApiErrorRep(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}