using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Domain.DTO
{
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
