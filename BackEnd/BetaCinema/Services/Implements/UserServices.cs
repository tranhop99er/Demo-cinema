using Azure.Core;
using BetaCinema.Contants;
using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Converters;
using BetaCinema.Payloads.DataRequests;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BetaCinema.Services.Implements
{
    public class UserServices : BaseService, IUserServices
    {
        private readonly ResponseObject<Bill> _responseObjectBill;
        private readonly ResponseObject<Tickets> _responseObjectTicket;
        private readonly ResponseObject<Movies> _responseObjectMovie;
        private readonly ResponseObject<Cinema> _responseObjectCinema;
        private readonly ResponseObject<Food> _responseObjectFood;
        private readonly ResponseObject<Seat> _responseObjectSeat;
        private readonly ResponseObject<Room> _responseObjectRoom;
        private readonly ResponseObject<List<Room>> _responseObjectListRoom;
        private readonly ResponseObject<DataResponse> _responseObject;
        private readonly UserConverter _userConverter;
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<DataResponseToken> _responseTokenObject;
        private readonly ResponseObject<Schedules> _responseObjectSchedule;

        public UserServices(IConfiguration configuration)
        {
            _userConverter = new UserConverter();
            _responseObject = new ResponseObject<DataResponse>();
            _configuration = configuration;
            _responseTokenObject = new ResponseObject<DataResponseToken>();
            _responseObjectSchedule = new ResponseObject<Schedules>();
            _responseObjectListRoom = new ResponseObject<List<Room>>();
            _responseObjectRoom = new ResponseObject<Room>();
            _responseObjectSeat = new ResponseObject<Seat>();
            _responseObjectFood = new ResponseObject<Food>();
            _responseObjectCinema = new ResponseObject<Cinema>();
            _responseObjectMovie = new ResponseObject<Movies>();
            _responseObjectTicket = new ResponseObject<Tickets>();
            _responseObjectBill = new ResponseObject<Bill>();
        }


        //-------------------------------** Token ** -------------------------------------------------

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public DataResponseToken GeneraAccessToken(Users user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secrectKeyBytes = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            //lay quyen role
            var role = _appDbContext.roles.SingleOrDefault(x => x.Id == user.RoleId);

            //mo ta token
            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("RoleId", role.Id.ToString()),
                    new Claim(ClaimTypes.Role, role?.Code ?? "")
                }),
                Expires = DateTime.Now.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secrectKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            //tao token dua tren mo ta tokenDescription
            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accesToken = jwtTokenHandler.WriteToken(token);

            //Refresh Token
            var refreshToken = GenerateRefreshToken();

            RefreshTokens rf = new RefreshTokens
            {
                Token = refreshToken,
                ExpiredTime = DateTime.Now.AddDays(1),
                UserId = user?.Id ?? 0,
            };
            _appDbContext.refreshTokens.Add(rf);
            _appDbContext.SaveChanges();

            DataResponseToken result = new DataResponseToken
            {
                UserName = user.Username,
                AccessToken = accesToken,
                RefreshToken = refreshToken
            };
            return result;
        }
        public DataResponseToken RenewAccessToken(Request_RenewAccessToken request)
        {
            throw new NotImplementedException();
        }



        //-------------------------------** Users ** -------------------------------------------------
        public ResponseObject<DataResponse> Register(Request_Register request)
        {
            if (string.IsNullOrWhiteSpace(request.Username)
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.PhoneNumber)
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            if (_appDbContext.users.Any(x => x.Email.Equals(request.Email)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email da ton tai", null);
            }
            //check username ton tai chua
            if (_appDbContext.users.Any(x => x.Username.Equals(request.Username)))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "UserName da ton tai", null);
            }


            //check dinh dang email
            if (!Validate.IsValidEmail(request.Email))
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email khong hop le", null);
            }

            var user = new Users();
            user.Point = 0;
            user.Username = request.Username;
            user.Email = request.Email;
            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.Password = BCryptNet.HashPassword(request.Password);  //max hoa password
            user.RankCustomerId = null;
            user.UserStatusId = 2;
            user.RoleId = 4;
            user.IsActive = true;
            _appDbContext.users.Add(user);
            _appDbContext.SaveChanges();

            DataResponse result = _userConverter.EntityToDTO(user);
            return _responseObject.ResponseSuccess("Dang ky tai khoan thanh cong", result);
        }
        public ErrorMessage ChangePassword( Request_ChangePassword request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    if (request.OldPassword != request.NewPassword)
                    {
                        var userCurrent = _appDbContext.users.FirstOrDefault(x => x.Username == request.UserName);
                        if (userCurrent != null)
                        {
                            bool checkPass = BCryptNet.Verify(request.OldPassword, userCurrent.Password);
                            if (checkPass)
                            {
                                userCurrent.Password = BCryptNet.HashPassword(request.NewPassword);

                                _appDbContext.users.Update(userCurrent);
                                _appDbContext.SaveChanges();
                                checktr.Commit();
                                return ErrorMessage.ThanhCong;
                            }
                            else
                            {
                                return ErrorMessage.Passwordkhongchinhxac;
                            }

                        }
                        else
                        {
                            return ErrorMessage.Userkhongtontai;
                        }
                    }
                    else
                    {
                        return ErrorMessage.Passwordbitrung;
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return ErrorMessage.ThatBai;
                }
            }
        }

        public ResponseObject<DataResponseToken> Login(Request_Login request)
        {   
            // 1. Kiểm tra thiếu thông tin
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            var user = _appDbContext.users.SingleOrDefault(x => x.Email.Equals(request.Email));

            if (user == null)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Dang nhap that bai", null);
            }

            // 2. Kiểm tra mật khẩu
            bool checkPass = BCryptNet.Verify(request.Password, user.Password);
            // 3. Thay đổi phản hồi (response)
            if (checkPass)
            {
                return _responseTokenObject.ResponseSuccess("Dang nhap thanh cong", GeneraAccessToken(user));
            }
            else
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Dang nhap that bai", null);
            }
        }

        public IQueryable<DataResponse> GetAllUser()
        {
            var result = _appDbContext.users.Select(x => _userConverter.EntityToDTO(x));
            return result;
        }
        public Users get_UserById(Request_Id request)
        {
            var userCurrent = _appDbContext.users.Find(request.Id);
            return userCurrent;
        }




        //-------------------------------** Cinema ** -------------------------------------------------
        public ResponseObject<Cinema> AddNewCinema(Request_Cinema request)
        {
            if (string.IsNullOrEmpty(request.NameOfCinema) || string.IsNullOrEmpty(request.Description) ||
              string.IsNullOrEmpty(request.Address) || string.IsNullOrEmpty(request.Code))
            {
                return _responseObjectCinema.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }

            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var CinemaNew = new Cinema();
                CinemaNew.Address = request.Address;
                CinemaNew.Description = request.Description;
                CinemaNew.NameOfCinema = request.NameOfCinema;
                CinemaNew.Code = request.Code;
                CinemaNew.IsActive = true;
                _appDbContext.cinemas.Add(CinemaNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectCinema.ResponseSuccess("Them Cinema Thanh cong", CinemaNew);
            }
        }

        public ResponseObject<Cinema> DelCinema(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var CinemaCurrent = _appDbContext.cinemas.Find(request.Id);
                if (CinemaCurrent != null)
                {
                    _appDbContext.cinemas.Remove(CinemaCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectCinema.ResponseSuccess("Xoa Cinema Thanh cong", null);
                }
                else
                {
                    return _responseObjectCinema.ResponseError(StatusCodes.Status400BadRequest, "CinemaId khong ton tai", null);
                }
            }
        }

        public ResponseObject<Cinema> UpdateCinema(Request_cinemaUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var CinemaCurrent = _appDbContext.cinemas.FirstOrDefault(x => x.Id == request.Id);
                    if (CinemaCurrent != null)
                    {
                        CinemaCurrent.IsActive = true;
                        CinemaCurrent.Address = request.Address;
                        CinemaCurrent.Description = request.Description;
                        CinemaCurrent.NameOfCinema = request.NameOfCinema;
                        CinemaCurrent.Code = request.Code;

                        _appDbContext.cinemas.Update(CinemaCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectCinema.ResponseSuccess("Cap nhat Cinema Thanh cong", CinemaCurrent);
                    }
                    else
                    {
                        return _responseObjectCinema.ResponseError(StatusCodes.Status400BadRequest, "CinemaId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectCinema.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }


        //-------------------------------** Movie ** -------------------------------------------------
        ResponseObject<Movies> IUserServices.AddNewMovie(Request_Movie request)
        {
            if ( request.MovieTypeId <= 0 || string.IsNullOrEmpty(request.Director) || request.EndTime == null || 
              request.PremiereDate == null || string.IsNullOrEmpty(request.Description) ||
              string.IsNullOrEmpty(request.Image) || string.IsNullOrEmpty(request.Language) || string.IsNullOrEmpty(request.Name) ||
              request.RateId <= 0 || string.IsNullOrEmpty(request.Trailer))
            {
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            var RateCurrent = _appDbContext.rate.Find(request.RateId);
            var MovieTypeCurrent = _appDbContext.movieTypes.Find(request.MovieTypeId);
            if (RateCurrent == null || MovieTypeCurrent == null)
            {
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai RateId va MovieTypeId", null);
            }
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var MovieNew = new Movies();
                MovieNew.EndTime = request.EndTime;
                MovieNew.PremiereDate = request.PremiereDate;
                MovieNew.MovieDuration = 2;
                MovieNew.Description = request.Description;
                MovieNew.Director = request.Director;
                MovieNew.Image = request.Image;
                MovieNew.Language = request.Language;
                MovieNew.MovieTypeId = request.MovieTypeId;
                MovieNew.Name = request.Name;
                MovieNew.RateId = request.RateId;
                MovieNew.Trailer = request.Trailer;
                MovieNew.IsActive = true;
                MovieNew.HeroImage = request.Image;
                _appDbContext.movies.Add(MovieNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectMovie.ResponseSuccess("Them Movie Thanh cong", MovieNew);
            }
        }

        public ResponseObject<Movies> UpdateMovie(Request_MovieUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var RateCurrent = _appDbContext.rate.Find(request.RateId);
                    var MovieTypeCurrent = _appDbContext.movieTypes.Find(request.MovieTypeId);
                    if (MovieTypeCurrent == null || RateCurrent == null)
                    {
                        return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai MovieTypeId va RateId", null);
                    }
                    var MovieCurrent = _appDbContext.movies.FirstOrDefault(x => x.Id == request.Id);
                    if (MovieCurrent != null)
                    {
                        MovieCurrent.EndTime = request.EndTime;
                        MovieCurrent.PremiereDate = request.PremiereDate;
                        MovieCurrent.MovieDuration = 2;
                        MovieCurrent.Description = request.Description;
                        MovieCurrent.Director = request.Director;
                        MovieCurrent.Image = request.Image;
                        MovieCurrent.Language = request.Language;
                        MovieCurrent.MovieTypeId = request.MovieTypeId;
                        MovieCurrent.Name = request.Name;
                        MovieCurrent.RateId = request.RateId;
                        MovieCurrent.Trailer = request.Trailer;
                        MovieCurrent.IsActive = true;
                        MovieCurrent.HeroImage = request.Image;

                        _appDbContext.movies.Update(MovieCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectMovie.ResponseSuccess("Cap nhat Movie Thanh cong", MovieCurrent);
                    }
                    else
                    {
                        return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "MovieId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }

        public ResponseObject<Movies> DelMovie(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var MovieCurrent = _appDbContext.movies.Find(request.Id);
                if (MovieCurrent != null)
                {
                    _appDbContext.movies.Remove(MovieCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectMovie.ResponseSuccess("Xoa Movie Thanh cong", null);
                }
                else
                {
                    return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "MovieId khong ton tai", null);
                }
            }
        }
        public List<DataResponseMovie> get_listMovies()
        {
           
            var movies = _appDbContext.movies.Include(x => x.Schedules).ThenInclude(s => s.Tickets).Where(x => x.IsActive == true).ToList(); 

            // Now, project the loaded movies to DataResponseMovie and calculate TotalTicketSold in-memory.
            var result = movies.Select(x => new DataResponseMovie
            {
                Id = x.Id,
                MovieDuration = x.MovieDuration,
                EndTime = x.EndTime,
                PremiereDate = x.PremiereDate,
                Description = x.Description,
                Director = x.Director,
                Image = x.Image,
                Language = x.Language,
                MovieTypedId = x.MovieTypeId,
                Name = x.Name,
                RateId = x.RateId,
                Trailer = x.Trailer,
                HeroImage = x.HeroImage,
                // Calculate the total tickets sold.
                TotalTicketSold = x.Schedules.SelectMany(s => s.Tickets).Count()
            })
            .OrderByDescending(x => x.TotalTicketSold) // This sorting is now done in-memory.
            .ToList();

            return result;
        }

        public Movies get_MovieById(Request_Id request)
        {
            var movieCurrent = _appDbContext.movies.Find(request.Id);
            if(movieCurrent == null)
            {
                return null;
            }
            return movieCurrent;
        }



        ///--------------------------------** Schdules **-----------------------------------------------------------

        //kiểm tra và đảm bảo là trong một phòng không chiếu đồng thời 2 phim trong một khoảng thời gian giao nhau
        private bool CheckScheduleAvailability(int roomIdCheck, Schedules schedulesCheck)
        {
            var schedules = _appDbContext.schedules.Where(s => s.RoomId == roomIdCheck).ToList();
            foreach (var schedule in schedules)
            {
                if ((schedulesCheck.StartAt < schedule.EndAt && schedulesCheck.StartAt >= schedule.StartAt) ||
                    (schedulesCheck.EndAt > schedule.StartAt && schedulesCheck.EndAt <= schedule.EndAt))
                {
                    // Nếu có giao nhau về thời gian
                    return false;
                }
            }
            return true;
        }
        public ResponseObject<Schedules> AddNewSchedule(Request_Schedule request)
        {
            var movieCurrent = _appDbContext.movies.Find(request.MovieId);
            var roomCurrent = _appDbContext.rooms.Find(request.RoomId);
            if (movieCurrent==null || roomCurrent == null || request.Price <= 0 || string.IsNullOrEmpty(request.Code) || request.StartAt == null
            || request.EndAt == null|| request.MovieId == null || request.RoomId == null)
            {
                return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var scheduleNew = new Schedules();
                scheduleNew.Price = request.Price;
                scheduleNew.StartAt = request.StartAt;
                scheduleNew.EndAt = request.EndAt ;
                scheduleNew.Code = request.Code;
                scheduleNew.MovieId = request.MovieId;
                scheduleNew.Name = movieCurrent.Name;
                scheduleNew.RoomId = request.RoomId;
                scheduleNew.IsActive = true;

                if(!CheckScheduleAvailability(request.RoomId, scheduleNew))
                {
                    return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "Trung lich chieu", null);
                }

                _appDbContext.schedules.Add(scheduleNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectSchedule.ResponseSuccess("Them schedule thanh cong", scheduleNew);
            }
        }

        public ResponseObject<Schedules> UpdateSchedule(Request_ScheduleUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var movieCurrent = _appDbContext.movies.Find(request.MovieId);
                    var roomCurrent = _appDbContext.rooms.Find(request.RoomId);
                    if (movieCurrent == null || roomCurrent == null)
                    {
                        return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai MovieId va RoomId", null);
                    }
                    var scheduleCurrent = _appDbContext.schedules.FirstOrDefault(x => x.Id == request.Id);
                    if (scheduleCurrent != null)
                    {
                        scheduleCurrent.IsActive = true;
                        scheduleCurrent.Price = request.Price;
                        scheduleCurrent.StartAt = request.StartAt;
                        scheduleCurrent.EndAt = request.EndAt;
                        scheduleCurrent.MovieId = request.MovieId;
                        scheduleCurrent.Code = request.Code;
                        scheduleCurrent.Name = movieCurrent.Name;
                        scheduleCurrent.RoomId = request.RoomId;

                        _appDbContext.schedules.Update(scheduleCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectSchedule.ResponseSuccess("Cap nhat Schedule Thanh cong", scheduleCurrent);
                    }
                    else
                    {
                        return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "ScheduleId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }

        public IQueryable<Schedules> GetListSchedule()
        {
            var result = _appDbContext.schedules.AsQueryable();
            return result;
        }
        public ResponseObject<Schedules> DelSchedule(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var scheduleCurrent = _appDbContext.schedules.Find(request.Id);
                if (scheduleCurrent != null)
                {
                    _appDbContext.schedules.Remove(scheduleCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectSchedule.ResponseSuccess("Xoa Schedule Thanh cong", null);
                }
                else
                {
                    return _responseObjectSchedule.ResponseError(StatusCodes.Status400BadRequest, "ScheduleId khong ton tai", null);
                }
            }
        }



        //--------------------------------** Room **-----------------------------------------------------------
        public ResponseObject<Room> AddNewRoom(Request_Room request)
        {

            if (request.Capacity <= 0 || string.IsNullOrEmpty(request.Description) ||
              request.CinemaId <= 0 || string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.Name))
            {
                return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            var cinemaCurrent = _appDbContext.cinemas.Find(request.CinemaId);
            if (cinemaCurrent == null)
            {
                return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "CinemaId khong ton tai", null);
            }
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var roomNew = new Room();
                roomNew.Capacity = request.Capacity;
                roomNew.Type = 1;
                roomNew.Description = request.Description;
                roomNew.CinemaId = request.CinemaId;
                roomNew.Code = request.Code;
                roomNew.Name = request.Name;
                roomNew.IsActive = true;
                _appDbContext.rooms.Add(roomNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectRoom.ResponseSuccess("Them Room Thanh cong", roomNew);
            }
        }

        public ResponseObject<Room> DelRoom(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var roomCurrent = _appDbContext.rooms.Find(request.Id);
                if (roomCurrent != null)
                {
                    _appDbContext.rooms.Remove(roomCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectRoom.ResponseSuccess("Xoa Room Thanh cong", null);
                }
                else
                {
                    return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "RoomId khong ton tai", null);
                }
            }
        }

        public ResponseObject<Room> UpdateRoom(Request_RoomUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var cinemaCurrent = _appDbContext.cinemas.Find(request.CinemaId);
                    if (cinemaCurrent == null)
                    {
                        return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "CinemaId khong ton tai", null);
                    }
                    var roomCurrent = _appDbContext.rooms.FirstOrDefault(x => x.Id == request.Id);
                    if (roomCurrent != null)
                    {
                        roomCurrent.IsActive = true;
                        roomCurrent.Type = 1;
                        roomCurrent.Capacity = request.Capacity;
                        roomCurrent.Description = request.Description;
                        roomCurrent.CinemaId = request.CinemaId;
                        roomCurrent.Code = request.Code;
                        roomCurrent.Name = request.Name;

                        _appDbContext.rooms.Update(roomCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectRoom.ResponseSuccess("Cap nhat Room Thanh cong", roomCurrent);
                    }
                    else
                    {
                        return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "RoomId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectRoom.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }


        //--------------------------------** Seat **-----------------------------------------------------------
        public ResponseObject<Seat> AddNewSeat(Request_Seat request)
        {
            if (request.Number <= 0 || string.IsNullOrEmpty(request.Line) ||
              request.SeatTypeId <= 0 || request.RoomId <= 0 || request.SeatStatusId <= 0)
            {
                return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            var seatStatusCurrent = _appDbContext.seatsStatus.Find(request.SeatStatusId);
            var roomCurrent = _appDbContext.rooms.Find(request.RoomId);
            var seatTypeCurrent = _appDbContext.seatTypes.Find(request.SeatTypeId);
            if (seatStatusCurrent == null || roomCurrent == null || seatTypeCurrent == null)
            {
                return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai RoomId, SeatTypeId, SeatStatusId", null);
            }
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var seatNew = new Seat();
                seatNew.Number = request.Number;
                seatNew.SeatStatusId = request.SeatStatusId;
                seatNew.Line = request.Line;
                seatNew.RoomId = request.RoomId;
                seatNew.SeatTypeId = request.SeatTypeId;
                seatNew.IsActive = true;
                _appDbContext.seats.Add(seatNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectSeat.ResponseSuccess("Them Seat Thanh cong", seatNew);
            }
        }

        public ResponseObject<Seat> UpdateSeat(Request_SeatUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var seatStatusCurrent = _appDbContext.seatsStatus.Find(request.SeatStatusId);
                    var roomCurrent = _appDbContext.rooms.Find(request.RoomId);
                    var seatTypeCurrent = _appDbContext.seatTypes.Find(request.SeatTypeId);
                    if (seatStatusCurrent == null || roomCurrent == null || seatTypeCurrent == null)
                    {
                        return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai RoomId, SeatTypeId, SeatStatusId", null);
                    }
                    var seatCurrent = _appDbContext.seats.FirstOrDefault(x => x.Id == request.Id);
                    if (seatCurrent != null)
                    {
                        seatCurrent.IsActive = true;
                        seatCurrent.Number = request.Number;
                        seatCurrent.SeatStatusId = request.SeatStatusId;
                        seatCurrent.Line = request.Line;
                        seatCurrent.RoomId = request.RoomId;
                        seatCurrent.SeatTypeId = request.SeatTypeId;

                        _appDbContext.rooms.Update(roomCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectSeat.ResponseSuccess("Cap nhat Seat Thanh cong", seatCurrent);
                    }
                    else
                    {
                        return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "SeatId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }

        public ResponseObject<Seat> DelSeat(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var seatCurrent = _appDbContext.seats.Find(request.Id);
                if (seatCurrent != null)
                {
                    _appDbContext.seats.Remove(seatCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectSeat.ResponseSuccess("Xoa Seat Thanh cong", null);
                }
                else
                {
                    return _responseObjectSeat.ResponseError(StatusCodes.Status400BadRequest, "SeatId khong ton tai", null);
                }
            }
        }



        //--------------------------------** Foods **-----------------------------------------------------------
        public ResponseObject<Food> AddNewFood(Request_Food request)
        {
            if (request.Price <= 0 || string.IsNullOrEmpty(request.Description) ||
              string.IsNullOrEmpty(request.Image) || string.IsNullOrEmpty(request.NameOfFood))
            {
                return _responseObjectFood.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }

            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var foodtNew = new Food();
                foodtNew.Price = request.Price;
                foodtNew.Description = request.Description;
                foodtNew.Image = request.Image;
                foodtNew.NameOfFood = request.NameOfFood;
                foodtNew.IsActive = true;
                _appDbContext.foods.Add(foodtNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectFood.ResponseSuccess("Them Food Thanh cong", foodtNew);
            }
        }

        public ResponseObject<Food> UpdateFood(Request_FoodUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var foodCurrent = _appDbContext.foods.FirstOrDefault(x => x.Id == request.Id);
                    if (foodCurrent != null)
                    {
                        foodCurrent.IsActive = true;
                        foodCurrent.Price = request.Price;
                        foodCurrent.Description = request.Description;
                        foodCurrent.Image = request.Image;
                        foodCurrent.NameOfFood = request.NameOfFood;

                        _appDbContext.foods.Update(foodCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectFood.ResponseSuccess("Cap nhat Food Thanh cong", foodCurrent);
                    }
                    else
                    {
                        return _responseObjectFood.ResponseError(StatusCodes.Status400BadRequest, "FoodId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectFood.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }

        public ResponseObject<Food> DelFood(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var foodCurrent = _appDbContext.foods.Find(request.Id);
                if (foodCurrent != null)
                {
                    _appDbContext.foods.Remove(foodCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectFood.ResponseSuccess("Xoa Food Thanh cong", null);
                }
                else
                {
                    return _responseObjectFood.ResponseError(StatusCodes.Status400BadRequest, "FoodId khong ton tai", null);
                }
            }
        }



       ///--------------------------------** Schdules **-----------------------------------------------------------
        public ResponseObject<Tickets> AddNewTicket(Request_Ticket request)
        {
            if (request.ScheduleId <= 0 || request.SeatId <= 0 || string.IsNullOrEmpty(request.Code) ||
              request.PriceTicket <= 0)
            {
                return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "Thieu thong tin", null);
            }
            var ScheduleCurrent = _appDbContext.schedules.Find(request.ScheduleId);
            var SeatCurrent = _appDbContext.seats.Find(request.SeatId);
            if (ScheduleCurrent == null || SeatCurrent == null)
            {
                return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai ScheduleId, SeatId", null);
            }

            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var TickettNew = new Tickets();
                TickettNew.Code = request.Code;
                TickettNew.ScheduleId = request.ScheduleId;
                TickettNew.SeatId = request.SeatId;
                TickettNew.PriceTicket = request.PriceTicket;
                TickettNew.IsActive = true;
                _appDbContext.tickets.Add(TickettNew);
                _appDbContext.SaveChanges();
                checktr.Commit();
                return _responseObjectTicket.ResponseSuccess("Them Ticket Thanh cong", TickettNew);
            }
        }

        public ResponseObject<Tickets> UpdateTicket(Request_TicketUpdate request)
        {
            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                try
                {
                    var ScheduleCurrent = _appDbContext.schedules.Find(request.ScheduleId);
                    var SeatCurrent = _appDbContext.seats.Find(request.SeatId);
                    if (ScheduleCurrent == null || SeatCurrent == null)
                    {
                        return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai ScheduleId, SeatId", null);
                    }
                    var TicketCurrent = _appDbContext.tickets.FirstOrDefault(x => x.Id == request.Id);
                    if (TicketCurrent != null)
                    {
                        TicketCurrent.Code = request.Code;
                        TicketCurrent.ScheduleId = request.ScheduleId;
                        TicketCurrent.SeatId = request.SeatId;
                        TicketCurrent.PriceTicket = request.PriceTicket;
                        TicketCurrent.IsActive = true;

                        _appDbContext.tickets.Update(TicketCurrent);
                        _appDbContext.SaveChanges();

                        checktr.Commit();
                        return _responseObjectTicket.ResponseSuccess("Cap nhat Ticket Thanh cong", TicketCurrent);
                    }
                    else
                    {
                        return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "TicketId khong ton tai", null);
                    }
                }
                catch (Exception ex)
                {
                    checktr.Rollback();
                    return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "That bai", null);
                }
            }
        }

        public ResponseObject<Tickets> DelTicket(Request_Id request)
        {
            using (var check = _appDbContext.Database.BeginTransaction())
            {
                var TicketCurrent = _appDbContext.tickets.Find(request.Id);
                if (TicketCurrent != null)
                {
                    _appDbContext.tickets.Remove(TicketCurrent);
                    _appDbContext.SaveChanges();
                    check.Commit();
                    return _responseObjectTicket.ResponseSuccess("Xoa Ticket Thanh cong", null);
                }
                else
                {
                    return _responseObjectTicket.ResponseError(StatusCodes.Status400BadRequest, "TicketId khong ton tai", null);
                }
            }
        }


        //-------------------------------** Kiem tra phim a, rap 1 thi co nhung phong nao room nào chieu **--------------------------------------**
        
        public ResponseObject<List<Room>> GetListRoom_By_MoviedId_CinemaId(Request_GetRoom_By_MoviedId_CinemaId request)
        {
            var MovieCurrent = _appDbContext.movies.Find(request.MovieId);
            var CinemaCurrentInput = _appDbContext.cinemas.Find(request.CinemaId);
            if (MovieCurrent == null || CinemaCurrentInput == null)
            {
                return _responseObjectListRoom.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai MoviedId va CinemaId", null);
            }

            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var ListScheduleCurrrent = _appDbContext.schedules.Where(x => x.MovieId == request.MovieId).Include(x => x.Room).ThenInclude(x => x.Cinema).ToList();
                var ListRoomCurrrent = ListScheduleCurrrent.Select(schedule => schedule.Room).ToList();
                var ListCinemaCurrent = ListRoomCurrrent?.Select(room => room?.Cinema).ToList();

                //Tich hop ket qua trung nhau lại
                ListCinemaCurrent = ListCinemaCurrent?.GroupBy(c => c?.Id).Select(group => group.First()).ToList();
                //Check xem CinemaId input co trong list chieu List cinema Chieu movieId hay khong
                bool isCinemaInList = ListCinemaCurrent?.Contains(CinemaCurrentInput) ?? false;
                if (!isCinemaInList)
                {
                    return _responseObjectListRoom.ResponseError(StatusCodes.Status400BadRequest, "That bai vi khong co lich chieu Movie o Cinema", null);
                }

                var ListRoomCurrrentResult = ListRoomCurrrent.Where(x => x.CinemaId == CinemaCurrentInput.Id).ToList();
                checktr.Commit();
                return _responseObjectListRoom.ResponseSuccess("Thanh cong. Lich chieu Movied o Cinema la List Room: ", ListRoomCurrrentResult);
            }
        }




        //-------------------------------** Kiem tra phim a, rap 1 thi co nhung phong nao room nào chieu **--------------------------------------**
        /* "Chọn phim => Chọn rạp => Chọn phòng  => Chọn suất chiếu, đồ ăn => Tạo hóa đơn => thanh toán VNPay*/
        private bool checkContain_T_in_ListT<T>(List<T> List_Check, T ItemCheck)
        {
            bool isCheck = List_Check?.Contains(ItemCheck) ?? false;
            if (!isCheck)
            {
                return false;
            }
            return true;
        }
        public ResponseObject<Bill> Booking_Movie(Request_BookingMovie request)
        {
            var MovieCurrentInput = _appDbContext.movies.Find(request.MovieId);
            var CinemaCurrentInput = _appDbContext.cinemas.Find(request.CinemaId);
            var RoomCurrentInput = _appDbContext.rooms.Find(request.RoomId);
            var ScheduleCurrentInput = _appDbContext.schedules.Find(request.ScheduleId);


            var List_SeatCurrentInput = new List<Seat> ();
            List<Request_Id> Select_SeatId = request.SelectedSeatId;
            foreach (var SeatId in Select_SeatId)
            {
                var SeatCurrentInput = _appDbContext.seats.Find(SeatId.Id);
                List_SeatCurrentInput.Add(SeatCurrentInput);
            }

                
         
            if (MovieCurrentInput == null || CinemaCurrentInput == null || RoomCurrentInput == null ||
                ScheduleCurrentInput == null || List_SeatCurrentInput == null)
            {
                return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai Thong tin nhap", null);
            }

            //Kiểm tra xem với cinemaID nhập vào có Room không
            var ListRoomCurrrent = _appDbContext.rooms.Where(x => x.CinemaId == request.CinemaId).ToList();
            if (!checkContain_T_in_ListT(ListRoomCurrrent, RoomCurrentInput))
            {
                return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai Thong tin roomId", null);
            }
            //Room chon
            var RoomCurrent = _appDbContext.rooms.FirstOrDefault(x => x.Id == request.RoomId);

            //Kiểm tra xem với MovieId nhập vào có Schedule không
            var ListScheduleCurrrent = _appDbContext.schedules.Where(x => x.MovieId == request.MovieId).ToList();

            if (!checkContain_T_in_ListT(ListScheduleCurrrent, ScheduleCurrentInput))
            {
                return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai Thong tin ScheduleId", null);
            }

            //ListScheduleCurrrent thong qua RoomCurrent
            ListScheduleCurrrent = ListScheduleCurrrent.Where(s => s.RoomId == RoomCurrent.Id).ToList();
            if(ListScheduleCurrrent == null)
            {
                return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Khong co Room chieu Movie", null);
            }
            //ScheduleCurrrent thoong qua RoomCurrent
            var ScheduleCurrrent = ListScheduleCurrrent.Where(s => s.Id == ScheduleCurrentInput.Id);

            //Chon seat
            //Kiểm tra xem với RoomId nhập vào có SeatId không
            var ListSeatCurrrentbyRoom = _appDbContext.seats.Where(x => x.RoomId == RoomCurrent.Id).ToList();
            var listSeatResult = new List<Seat>();
            foreach(var SeatCurrentInput in List_SeatCurrentInput)
            {
                if (!checkContain_T_in_ListT(ListSeatCurrrentbyRoom, SeatCurrentInput))
                {
                    return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Kiem tra lai Thong tin SeatId", null);
                }
                if(SeatCurrentInput.SeatStatusId != 1)
                {
                    return _responseObjectBill.ResponseError(StatusCodes.Status400BadRequest, "Seat da duoc dat!", null);
                }
                //var SeatCurrent = ListSeatCurrrentbyRoom.FirstOrDefault(x => x.Id == SeatCurrentInput.Id);
                listSeatResult.Add(SeatCurrentInput);
            }




            //Get ve voi ScheduleId va listSeat Input
            double TotalTicketMoney = 0;
            foreach(var SeatCurrent in listSeatResult)
            {
                var TicketCurrent = _appDbContext.tickets.FirstOrDefault(x => x.SeatId == SeatCurrent.Id && x.ScheduleId == ScheduleCurrentInput.Id);
                TotalTicketMoney += TicketCurrent.PriceTicket;
            }


            //ListFood thoong qua list FooId va list quantity
            double TotalFoodMoney = 0;
            List<Request_Food_Quantity> SelectedFoodId_Quatity = request.SelectedFoodId_Quatity;

            foreach (var FoodId_Quantity in SelectedFoodId_Quatity)
            {
                var FoodCurrent = _appDbContext.foods.Find(FoodId_Quantity.Id);
                TotalFoodMoney += FoodCurrent.Price * FoodId_Quantity.Quantity;
            }


            //Thanh toan tien va tao Bill theo khuyen mai Promotion
            var PromotionCurrent = _appDbContext.promotions.Find(request.PromotionId);
            
            double TotalMoneyBill = (TotalFoodMoney + TotalTicketMoney) * (100 - PromotionCurrent.Percent)/100;
            PromotionCurrent.Quantity -= 1;
            _appDbContext.promotions.Update(PromotionCurrent);
            _appDbContext.SaveChanges();

            //tao chuoi string
            int length = 10; // Độ dài của chuỗi ngẫu nhiên
            string randomString = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());

            using (var checktr = _appDbContext.Database.BeginTransaction())
            {
                var BillNew = new Bill();
                BillNew.TotalMoney = TotalMoneyBill;
                BillNew.TradingCode = randomString;
                BillNew.CreateTime = DateTime.Now;
                BillNew.CustomerId = request.UserId;
                BillNew.Name = $"Ve xem Movie: {MovieCurrentInput.Name}, room: {RoomCurrentInput.Name} by {CinemaCurrentInput.NameOfCinema}, Time: {ScheduleCurrentInput.StartAt} - {ScheduleCurrentInput.EndAt}!";
                BillNew.CreateAt = DateTime.Now;
                BillNew.PromotionId = PromotionCurrent.Id;
                BillNew.BillStatusId = 2;
                BillNew.IsActive = true;
                _appDbContext.bills.Add(BillNew);
                _appDbContext.SaveChanges();


                //Add billFood
                
                foreach (var FoodId_Quantity in SelectedFoodId_Quatity)
                {
                    var billFoodNew = new BillFoods();
                    billFoodNew.Quantity = FoodId_Quantity.Quantity;
                    billFoodNew.BillId = BillNew.Id;
                    billFoodNew.FoodId = FoodId_Quantity.Id;
                    _appDbContext.billFoods.Add(billFoodNew);
                    _appDbContext.SaveChanges();
                }

                //Add billTicketNew
                foreach (var SeatCurrent in listSeatResult)
                {
                    var billTicketNew = new BillTickets();
                    var TicketCurrent = _appDbContext.tickets.FirstOrDefault(x => x.SeatId == SeatCurrent.Id && x.ScheduleId == ScheduleCurrentInput.Id);
                    billTicketNew.Quantity = 1;
                    billTicketNew.BillId = BillNew.Id;
                    billTicketNew.TicketId = TicketCurrent.Id;
                    _appDbContext.billTickets.Add(billTicketNew);
                    _appDbContext.SaveChanges();
                }
                checktr.Commit();
                return _responseObjectBill.ResponseSuccess("Them Bill Thanh cong", BillNew);
            }
        }
    }
}
