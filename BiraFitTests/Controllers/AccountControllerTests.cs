using BiraFit.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BiraFit.Models;
using System.Web.Mvc;
using System.Threading.Tasks;

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
            var result = _controller.Login("/login") as ViewResult;
            var returnUrl = result.ViewBag.ReturnUrl;
            Assert.AreEqual("/login", returnUrl);
        }

        [TestMethod()]
        public async Task LoginWithNoModel()
        {
            LoginViewModel lvm = new LoginViewModel {
                Email = "",
                Password = ""                
            };
            var result = await _controller.Login(null, "/Login");
            Assert.IsNotNull(result);
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