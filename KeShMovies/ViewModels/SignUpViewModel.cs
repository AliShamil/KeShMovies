﻿using KeShMovies.Commands;
using KeShMovies.Models;
using KeShMovies.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeShMovies.ViewModels;

public class SignUpViewModel : BaseViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly List<User>? _users;

    public ICommand SignUpCommand { get; set; }
    public ICommand LogInCommand { get; set; }

    private static string usernameRegex = @"^(?=[a-zA-Z])[-\w.]{2,23}([a-zA-Z\d]|(?<![-.])_)$";
    private static string passwordRegex = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$";

    public string Email { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    public SignUpViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        _users = _userRepository.GetList() != null ? _userRepository.GetList() : new();

        SignUpCommand = new RelayCommand(ExecuteSignUpCommand, CanExecuteContinueCommand);
        LogInCommand = new RelayCommand(ExecuteLogInCommand);
    }

    private void ExecuteLogInCommand(object? obj)
    {
        
    }

    private void ExecuteSignUpCommand(object? parametr)
    {
        var user = new User()
        {
            Email = this.Email,
            Username = this.Username,
            Password = this.Password,
        };


        if(!IsValidEmail())
        {
            MessageBox.Show($"Incorrect {nameof(Email)} Format");
            return;
        }    

        if(!Regex.IsMatch(Username, usernameRegex))
        {
            MessageBox.Show($"Incorrect {nameof(Username)} Format");
            return;
        }

        if (!Regex.IsMatch(Password, passwordRegex))
        {
            MessageBox.Show($"Incorrect {nameof(Password)} Format");
            return;
        }

        if (_users is not null)
        {
            if (_users.Any(u => u.Email == user.Email))
            {
                MessageBox.Show($"This {nameof(Email)} is already Used");
                return;
            }

            if (_users.Any(u => u.Username == user.Username))
            {
                MessageBox.Show($"This {nameof(Username)}  is already Used");
                return;
            }

            _users.Add(user);
            _userRepository.Add(user);


        }

    }

    private bool CanExecuteContinueCommand(object? parametr)
    {
        if (parametr is StackPanel sp)
        {
            foreach (var txt in sp.Children.OfType<TextBox>())
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                    return false;
            }

            return true;
        }

        return false;
    }


    private bool IsValidEmail()
    {
        if (Email is null)
            return false;

        try
        {
            MailAddress m = new MailAddress(Email);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
