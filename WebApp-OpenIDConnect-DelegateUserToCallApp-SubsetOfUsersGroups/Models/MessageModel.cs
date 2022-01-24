using System.ComponentModel.DataAnnotations;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Groups.Models
{
    public class MessageModel
    {
        [Required]
        public string Message { get; set; }
    }
}
