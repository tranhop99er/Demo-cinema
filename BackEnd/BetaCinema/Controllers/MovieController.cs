using BetaCinema.Contants;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataRequests;
using BetaCinema.Services.Implements;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace BetaCinema.Controllers
{
    public class MovieController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public MovieController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        //---------------------------** Movie ** ------------------------------------------
        //get Movie
        [HttpGet("/api/getMovie")]
        public IActionResult GetMovie()
        {
            var lstMovie = _userServices.get_listMovies();
            return Ok(lstMovie);
        }

        //Lay movie
        [HttpGet("api/getMovieById")]
        public IActionResult GetMovieById([FromQuery] Request_Id request)
        {
            var ret = _userServices.get_MovieById(request);
            if (ret != null)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest("Khong co MovieId");
            }
        }

        [HttpPost("/api/addNewMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult addNewMovie([FromBody] Request_Movie request)
        {
            return Ok(_userServices.AddNewMovie(request));
        }

        [HttpDelete("/api/DelMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelMovie([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelMovie(request));
        }

        [HttpPut("/api/UpdateMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateMovie([FromBody] Request_MovieUpdate request)
        {
            return Ok(_userServices.UpdateMovie(request));
        }


        //---------------------------** Schedule ** ------------------------------------------

        [HttpPost("/api/addNewSchedule")]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddNewSchedule([FromBody] Request_Schedule request)
        {
            return Ok(_userServices.AddNewSchedule(request));
        }


        [HttpPut("/api/UpdateSchedule")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSchedule([FromBody] Request_ScheduleUpdate request)
        {
            return Ok(_userServices.UpdateSchedule(request));
        }

        [HttpDelete("/api/DelSchedule")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelSchedule([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelSchedule(request));
        }


        [HttpGet("/api/getListSchedule")]
        public IActionResult GetListSchedule()
        {
            return Ok(_userServices.GetListSchedule());
        }


        //---------------------------** Ticket ** ------------------------------------------
        [HttpPost("/api/AddNewTicket")]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddNewTicket([FromBody] Request_Ticket request)
        {
            return Ok(_userServices.AddNewTicket(request));
        }

        [HttpDelete("/api/DelTicket")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelTicket([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelTicket(request));
        }

        [HttpPut("/api/UpdateTicket")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateTicket([FromBody] Request_TicketUpdate request)
        {
            return Ok(_userServices.UpdateTicket(request));
        }

        //-------------------------------** Kiem tra phim a, rap 1 thi co nhung phong nao room nào chieu **--------------------------------------**
        [HttpGet("/api/GetListRoomByMoviedIdCinemaId")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetListRoom_By_MoviedId_CinemaId([FromQuery] Request_GetRoom_By_MoviedId_CinemaId request)
        {
            return Ok(_userServices.GetListRoom_By_MoviedId_CinemaId(request));
        }


        //---------------------------** Booking Movie - BillTicket ** ------------------------------------------
        [HttpPost("/api/NewBill")]
        //[Authorize(Roles = "Admin")]
        public IActionResult NewBill([FromBody] Request_BookingMovie request)
        {
            return Ok(_userServices.Booking_Movie(request));
        }
    }
}
