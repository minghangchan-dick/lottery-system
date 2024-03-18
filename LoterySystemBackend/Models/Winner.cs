namespace LoterySystemBackend.Models
{
    public class Winner
    {
        public Winner(string ticketNo)
        {
            TicketNo = ticketNo;
        }

        public string TicketNo { get; set; }
    }
}
