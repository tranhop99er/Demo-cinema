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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }


        //---------------------------** Users ** ------------------------------------------
        [HttpGet("api/getUserById")]
        public IActionResult GetUserById([FromQuery] Request_Id request)
        {
            var ret = _userServices.get_UserById(request);
            if (ret != null)
            {
                return Ok(ret);
            }
            else
            {
                return BadRequest("Khong co UserId");
            }
        }

        [HttpGet("api/auth/get-all")]
        //[Authorize(Roles = "Admin")]
        public IActionResult GetAllUser()
        {
            return Ok(_userServices.GetAllUser());
        }

        //---------------------------** Cinema ** ------------------------------------------
        [HttpPost("/api/AddNewCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewCinema([FromBody] Request_Cinema request)
        {
            return Ok(_userServices.AddNewCinema(request));
        }

        [HttpDelete("/api/DelCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelCinema([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelCinema(request));
        }

        [HttpPut("/api/UpdateCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema([FromBody] Request_cinemaUpdate request)
        {
            return Ok(_userServices.UpdateCinema(request));
        }


        //---------------------------** Room ** ------------------------------------------
        [HttpPost("/api/addNewRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult addNewRoom([FromBody] Request_Room request)
        {
            return Ok(_userServices.AddNewRoom(request));
        }

        [HttpDelete("/api/DelRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelRoom([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelRoom(request));
        }

        [HttpPut("/api/UpdateRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom([FromBody] Request_RoomUpdate request)
        {
            return Ok(_userServices.UpdateRoom(request));
        }


        //---------------------------** Seat ** ------------------------------------------
        [HttpPost("/api/addNewSeat")]
        //[Authorize(Roles = "Admin")]
        public IActionResult addNewSeat([FromBody] Request_Seat request)
        {
            return Ok(_userServices.AddNewSeat(request));
        }

        [HttpDelete("/api/DelSeat")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DelSeat([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelSeat(request));
        }

        [HttpPut("/api/UpdateSeat")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat([FromBody] Request_SeatUpdate request)
        {
            return Ok(_userServices.UpdateSeat(request));
        }

        //---------------------------** Foods ** ------------------------------------------
        [HttpPost("/api/addNewFood")]
        [Authorize(Roles = "Admin")]
        public IActionResult addNewFood([FromBody] Request_Food request)
        {
            return Ok(_userServices.AddNewFood(request));
        }

        [HttpDelete("/api/DelFood")]
        [Authorize(Roles = "Admin")]
        public IActionResult DelFood([FromBody] Request_Id request)
        {
            return Ok(_userServices.DelFood(request));
        }

        [HttpPut("/api/UpdateFood")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateFood([FromBody] Request_FoodUpdate request)
        {
            return Ok(_userServices.UpdateFood(request));
        }
    }
}
