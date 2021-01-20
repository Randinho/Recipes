using Recipes.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        
        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }
        public bool IsReceived { get; set; }
    }
}
