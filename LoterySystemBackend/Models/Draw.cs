using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LoterySystemBackend.Models
{
    public class Draw
    {
        public Draw(DateTime DrawDatetime) 
        {
            this.DrawDatetime = DrawDatetime;
            this.UpdateDatetime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? WinnerTicketNo { get; set; }
        [Required]
        public DateTime DrawDatetime { get; set; }
        [Required]
        public DateTime UpdateDatetime { get; set; }
    }
}
