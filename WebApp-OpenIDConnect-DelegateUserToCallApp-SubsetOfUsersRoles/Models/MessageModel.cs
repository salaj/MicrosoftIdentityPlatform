using System.ComponentModel.DataAnnotations;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp_SubsetOfUsers_Roles.Models
{
    public class MessageModel
    {
        [Required]
        public string Message { get; set; }
    }
}
