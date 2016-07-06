﻿using Projecktor.Domain.Entites;
using Projecktor.WebUI.Models;

namespace Projecktor.WebUI.Infrastructure.Abstract
{
    public interface ISecurityService
    {
        bool Authenticate(string username, string password);
        User CreateUser(SignupViewModel signupModel, bool login = true);
        bool DoesUserExist(string username);
        User GetCurrentUser();
        bool IsAuthenticated { get; }
        void Login(User user);
        void Login(string username);
        void Logout();
        int UserId { get; set; }
    }
}
