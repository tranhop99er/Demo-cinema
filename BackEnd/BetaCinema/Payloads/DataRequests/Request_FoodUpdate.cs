﻿namespace BetaCinema.Payloads.DataRequests
{
    public class Request_FoodUpdate : Request_Id
    {
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string NameOfFood { get; set; }
    }
}
