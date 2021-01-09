using Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string ReceiverId { get; set; }
        public bool IsReceived { get; set; }
    }
}
