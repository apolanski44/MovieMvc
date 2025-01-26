﻿using Microsoft.AspNetCore.Mvc;

namespace MvcFilm.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
