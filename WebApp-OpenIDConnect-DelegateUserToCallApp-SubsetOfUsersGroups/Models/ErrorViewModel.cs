using System;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Groups.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
