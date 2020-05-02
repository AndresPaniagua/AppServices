﻿using AppServices.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppServices.Common.Models
{
    public class UserResponse 
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Document { get; set; }
        
        public string FullName { get; set; }

        public string Address { get; set; }

        public UserType UserType { get; set; }
    }
}