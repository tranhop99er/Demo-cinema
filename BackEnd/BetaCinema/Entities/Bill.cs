using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Entities
{
    public class Bill : BaseEntity
    {
        public double? TotalMoney { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public int CustomerId { get; set; }
        public Users? Customer { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public int? PromotionId { get; set; }
        public Promotions? Promotion { get; set; }
        public bool? IsActive { get; set; } = true;
        public int BillStatusId { get; set; }
        public BillStatuses? BillStatus { get; set; }
        public IEnumerable<BillFoods>? BillFood { get; set; }
        public IEnumerable<BillTickets>? BillTickets { get; set; }
    }
}
