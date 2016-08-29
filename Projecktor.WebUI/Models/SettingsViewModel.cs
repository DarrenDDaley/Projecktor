﻿using System.ComponentModel.DataAnnotations;

namespace Projecktor.WebUI.Models
{
    public class SettingsViewModel
    {
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string Username { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string CurrentPassword { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}