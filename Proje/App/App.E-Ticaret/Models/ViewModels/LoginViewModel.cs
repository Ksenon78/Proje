﻿using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{

    [Required(ErrorMessage = "E-posta adresi gereklidir.")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Şifre gereklidir.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }


}
