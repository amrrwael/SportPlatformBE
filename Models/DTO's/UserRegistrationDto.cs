﻿using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string Name { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}