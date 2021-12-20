using System.ComponentModel.DataAnnotations;

namespace WebApp_OpenIDConnect_DelegateUserToCallApp2.Models
{
    public class MessageModel
    {
        [Required]
        public string Message { get; set; }
    }
}
