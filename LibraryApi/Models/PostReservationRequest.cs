﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class PostReservationRequest
    {
        [Required]
        public string For { get; set; }
        [Required]
        public string Books { get; set; }
    }
}
