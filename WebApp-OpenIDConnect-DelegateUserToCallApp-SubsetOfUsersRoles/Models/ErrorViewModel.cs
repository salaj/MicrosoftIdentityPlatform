using System;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Roles.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
