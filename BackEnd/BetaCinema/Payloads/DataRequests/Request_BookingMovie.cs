namespace BetaCinema.Payloads.DataRequests
{
    public class Request_BookingMovie
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public int RoomId { get; set; }
        public int ScheduleId { get; set; }
        public List<Request_Id> SelectedSeatId { get; set; }
        public List<Request_Food_Quantity>? SelectedFoodId_Quatity { get; set; }
        public int? PromotionId { get; set; }

    }
}
