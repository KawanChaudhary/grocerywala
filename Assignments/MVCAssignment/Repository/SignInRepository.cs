﻿using Microsoft.AspNetCore.Identity;
using MVCAssignment.Models;
using System.Threading.Tasks;

namespace MVCAssignment.Repository
{
    public class SignInRepository : ISignInRepository
    {

        private readonly SignInManager<ApplicationUser> _accountManager;

        public SignInRepository(SignInManager<ApplicationUser> accountManager)
        {
            _accountManager = accountManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(SignInUserModel userModel)
        {
            var res = await _accountManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);
            return res;
        }

        public async Task SignOutAsync()
        {
            await _accountManager.SignOutAsync();
        }

    }
}
