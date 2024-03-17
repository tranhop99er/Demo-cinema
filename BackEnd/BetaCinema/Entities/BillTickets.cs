using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class BillTickets : BaseEntity
    {
        public int Quantity { get; set; }
        public int BillId { get; set; }
        public Bill? Bill { get; set; }
        public int TicketId { get; set; }
        public Tickets? Ticket { get; set; }
    }
}
