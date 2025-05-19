using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot.DAL.Entities
{
    public class CustomerTicket
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string MobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int IssueTypeId { get; set; }
        public IssueType? IssueType { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TicketPriority Priority { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
