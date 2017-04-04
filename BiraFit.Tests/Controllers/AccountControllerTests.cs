using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiraFit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BiraFit.Models;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            AccountController controller = new AccountController();
            LoginViewModel lvmodel = new LoginViewModel();
            String returnUrl = "wwww.google.ch";
            var result = controller.Login(lvmodel, returnUrl);
            Assert.IsNotNull(result);
        }


        [TestMethod()]
        public void RegisterTest()
        {
            AccountController controller = new AccountController();
            RegisterViewModel rvm = new RegisterViewModel();
            var result = controller.Register(rvm);
            Assert.IsNotNull(result);
        }


        [TestMethod()]
        public void ForgotPasswordTest()
        {
            AccountController controller = new AccountController();
            ForgotPasswordViewModel viewmodel = new ForgotPasswordViewModel();
            var result = controller.ForgotPassword(viewmodel);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ConfirmEmailTest()
        {
            AccountController controller = new AccountController();
            string userId = "1234";
            string code = "asdf";
            var result = controller.ConfirmEmail(userId, code);
            Assert.IsNotNull(result);
        }
    }
}