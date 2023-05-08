namespace BlogWebSite.Service.Helpers.ToastMessage
{
    public interface IToastMsg
    {
        public void Success(string message);

        public void Error();
    }
}
