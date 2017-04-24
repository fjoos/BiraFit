using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiraFit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiraFit.Models;
using System.Web.Mvc;
using System.Web;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {

        private AccountController controller;
    

        [TestInitialize]
        public void TestInitialize()
        {
            controller = new AccountController();
         
           }


        /*
         testuser: 
        */
        [TestMethod()]
        public void LoginView()
        {
        /*    ViewResult result = controller.Login("/login") as ViewResult;
            var actualModel = result.Model as LoginViewModel;
            Assert.IsNull(actualModel);*/
        }

        [TestMethod()]
        public void LoginSuccess()
        {
            LoginViewModel lvmodel = new LoginViewModel() {
                Sportler = true,
                Email = "sportler@hotmail.com",
                Password = "Hsr-123"
            };
            String returnUrl = "/";
            var result = controller.Login(lvmodel, returnUrl);
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
            var result = controller.Login(lvmodel, returnUrl);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void RegisterView()
        {
         /* var result = controller.Register() as ViewResult;
            Assert.IsNotNull(result);*/
        }

        [TestMethod()]
        public void RegisterViewFull()
        {
           RegisterViewModel rvm = new RegisterViewModel();
            var result = controller.Register(rvm);
            Assert.IsNotNull(result);
        }

    }
}