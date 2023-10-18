﻿using System.ComponentModel.DataAnnotations;

namespace RailReserve.Dtos
{
    public class TravelerRegisterRequest
    {
        [Required]
        public string NIC { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
