using BlogWebSite.Core.ResultMessages;
using NToastNotify;

namespace BlogWebSite.Service.Helpers.ToastMessage
{
    public class ToastMsg : IToastMsg
    {
        private readonly IToastNotification _toast;

        public ToastMsg(IToastNotification toast)
        {
            _toast = toast;
        }
        public void Success(string message)
        {
            _toast.AddSuccessToastMessage(message,
                       new ToastrOptions { Title = Messages.ToastTitle.Success });
        }
        public void Error()
        {
            _toast.AddErrorToastMessage(Messages.GlobalMessage.Error,
                        new ToastrOptions { Title = Messages.ToastTitle.Error });
        }
    }
}
