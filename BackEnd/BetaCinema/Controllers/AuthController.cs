﻿using BetaCinema.Contants;
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
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        //---------------------------** Authencation Account ** ------------------------------------------

        [HttpPost("/auth/register")]
        public IActionResult Register([FromBody] Request_Register request_Register)
        {
            return Ok(_userServices.Register(request_Register));
        }
        [HttpPost("/auth/login")]
        public IActionResult Login([FromBody] Request_Login request)
        {
            return Ok(_userServices.Login(request));
        }

        //change Password
        [HttpPost("/changePassword")]
        [Authorize]
        public IActionResult changePassword([FromBody] Request_ChangePassword request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("Ma token khong hop le"); // Trả về 401 thay vì NotFound
            }
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid user ID");
            }
            Console.WriteLine($"User ID: {userIdString}");

            var ret = _userServices.ChangePassword(userId, request);
            switch (ret)
            {
                case ErrorMessage.ThanhCong:
                    return Ok("Password doi Thanh Cong");
                case ErrorMessage.Userkhongtontai:
                    return NotFound("Khong tim thay Id cua users");
                case ErrorMessage.Passwordkhongchinhxac:
                    return BadRequest("Password khong chinh xac");
                case ErrorMessage.Passwordbitrung:
                    return BadRequest("Password khong thay doi");
                default:
                    return BadRequest("Doi Password That Bai");
            }
        }
    }
}
