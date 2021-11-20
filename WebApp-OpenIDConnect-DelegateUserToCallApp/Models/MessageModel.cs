using System.ComponentModel.DataAnnotations;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp.Models
{
    public class MessageModel
    {
        [Required]
        public string Message { get; set; }
    }
}
