using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoterySystemBackend.Models
{
    public class Lottery
    {
        public Lottery(string ticketNo)
        {
            TicketNo = ticketNo;
            Discarded = false;
            IssueDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string TicketNo { get; set; }
        [Required]
        public bool Discarded { get; set; }
        [Required]
        public DateTime IssueDateTime { get; set; }
    }
}
