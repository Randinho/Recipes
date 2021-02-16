using Recipes.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Recipes.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public ApplicationUserDTO Receiver { get; set; }
        public string ReceiverId { get; set; }
        public bool IsReceived { get; set; }
    }
}
