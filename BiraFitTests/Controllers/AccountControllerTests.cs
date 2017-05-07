using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BiraFit.Models;
using System.Web.Mvc;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController _controller;


        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new AccountController();
        }

        [TestMethod()]
        public void LoginView()
        {
            ViewResult result = _controller.Login("/login") as ViewResult;
            var actualModel = result.Model as LoginViewModel;
            Assert.IsNull(actualModel);
        }

        [TestMethod()]
        public void LoginSuccess()
        {
            LoginViewModel lvmodel = new LoginViewModel()
            {
                Sportler = true,
                Email = "sportler@hotmail.com",
                Password = "Hsr-123"
            };
            String returnUrl = "/";
            var result = _controller.Login(lvmodel, returnUrl);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void LoginFail()
        {
            LoginViewModel lvmodel = new LoginViewModel()
            {
                Sportler = true,
                Email = "sportler@hotmail.com",
                Password = "Fail"
            };
            String returnUrl = "/";
            var result = _controller.Login(lvmodel, returnUrl);
            Assert.IsNotNull(result);
        }


        [TestMethod()]
        public void RegisterViewFull()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            var result = _controller.Register(rvm);
            Assert.IsNotNull(result);
        }
    }
}