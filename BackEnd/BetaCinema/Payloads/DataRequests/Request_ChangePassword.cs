﻿namespace BetaCinema.Payloads.DataRequests
{
    public class Request_ChangePassword
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
