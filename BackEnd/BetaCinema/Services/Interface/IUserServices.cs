using BetaCinema.Contants;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataRequests;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IUserServices
    {
        //** -----------------------------------------Token----------------------------**
       
        DataResponseToken GeneraAccessToken(Users user);
        DataResponseToken RenewAccessToken(Request_RenewAccessToken request);


        //** -----------------------------------------User----------------------------**
        ResponseObject<DataResponse> Register(Request_Register request);
        ResponseObject<DataResponseToken> Login(Request_Login request);
        IQueryable<DataResponse> GetAllUser();
        Users get_UserById(Request_Id request);
        ErrorMessage ChangePassword(string UserName, Request_ChangePassword request);



        // **----------------------------------Cinema---------------------------------------**
        ResponseObject<Cinema> AddNewCinema(Request_Cinema request);
        ResponseObject<Cinema> DelCinema(Request_Id request);
        ResponseObject<Cinema> UpdateCinema(Request_cinemaUpdate request);


        // **----------------------------------Room---------------------------------------**
        ResponseObject<Room> AddNewRoom(Request_Room request);
        ResponseObject<Room> DelRoom(Request_Id request);
        ResponseObject<Room> UpdateRoom(Request_RoomUpdate request);


        // **----------------------------------Movie---------------------------------------**
        List<DataResponseMovie> get_listMovies();
        ResponseObject<Movies> AddNewMovie(Request_Movie request);
        ResponseObject<Movies> UpdateMovie(Request_MovieUpdate request);
        ResponseObject<Movies> DelMovie(Request_Id request);
        Movies get_MovieById(Request_Id request);


        //-------------------------------** Schedule **--------------------------------------**
        ResponseObject<Schedules> AddNewSchedule(Request_Schedule request);
        ResponseObject<Schedules> UpdateSchedule(Request_ScheduleUpdate request);
        ResponseObject<Schedules> DelSchedule(Request_Id request);
        IQueryable<Schedules> GetListSchedule();


        //-------------------------------** Seats **--------------------------------------**
        ResponseObject<Seat> AddNewSeat(Request_Seat request);
        ResponseObject<Seat> UpdateSeat(Request_SeatUpdate request);
        ResponseObject<Seat> DelSeat(Request_Id request);

        //-------------------------------** Foods **--------------------------------------**
        ResponseObject<Food> AddNewFood(Request_Food request);
        ResponseObject<Food> UpdateFood(Request_FoodUpdate request);
        ResponseObject<Food> DelFood(Request_Id request);


        //-------------------------------** Ticket **--------------------------------------**
        ResponseObject<Tickets> AddNewTicket(Request_Ticket request);
        ResponseObject<Tickets> UpdateTicket(Request_TicketUpdate request);
        ResponseObject<Tickets> DelTicket(Request_Id request);


        //-------------------------------** Kiem tra phim a, rap 1 thi co nhung phong nao room nào chieu **--------------------------------------**
        ResponseObject<List<Room>> GetListRoom_By_MoviedId_CinemaId(Request_GetRoom_By_MoviedId_CinemaId request);


        //-------------------------------** Booking Movie - BillTicket **--------------------------------------**
        /* "Chọn phim => Chọn rạp => Chọn phòng  => Chọn suất chiếu, đồ ăn => Tạo hóa đơn => thanh toán VNPay*/
        ResponseObject<Bill> Booking_Movie(Request_BookingMovie request);
    }
}
