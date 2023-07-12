﻿using System.ComponentModel.DataAnnotations;

namespace GlobalApi.DataTransfer
{
    public class SignInRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
