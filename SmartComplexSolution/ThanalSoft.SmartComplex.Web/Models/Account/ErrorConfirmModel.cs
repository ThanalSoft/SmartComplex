namespace ThanalSoft.SmartComplex.Web.Models.Account
{
    public class ErrorConfirmModel
    {
        public string Error { get; set; }

        public ErrorConfirmModel(string pError)
        {
            Error = pError;
        }
    }
}