using BiraFit.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BiraFit.Models;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new AccountController();
            moqContext = new Mock<HttpContextBase>();
            moqRequest = new Mock<HttpRequestBase>();
        }

        [TestMethod()]
        public void RegisterSportler()
        {
            var model = new RegisterViewModel() {
                Email = "birfafit17@gmail.com",
                Firstname = "Bira",
                Lastname = "Fit",
                Birthdate = new DateTime(1999,1,1),
                Password = "Hsr-123",
                ConfirmPassword = "Hsr-123",
                Sportler = true };

            ValidateModel(model, _controller);

            var asfd = _controller.Register(model);
            Assert.IsNotNull(asfd.);
        }

        private static void ValidateModel(object model, Controller controller)
        {
            var context = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError("CustomError", validationResult.ErrorMessage);
            }
        }

        /*
         * 
         *  var model = new NewOrderViewModel() {Name = "Michael"};

            var context = new DefaultHttpContext {User = CreateUser(), RequestServices = _serviceProvider};
            
            var controller = new OrderController(new OrderService(_dbContext, new FakeUserManager()));
            controller.ControllerContext = new ControllerContext() {HttpContext = context};

            var view = controller.Create(model);
            Assert.IsType<PartialViewResult>(view);


             if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AnmeldeDatum = DateTime.Now,
                    Name = model.Lastname,
                    Vorname = model.Firstname,
                    Aktiv = 1                                        
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    var msg = "Willkommen bei BiraFit! Bitte bestätigen Sie Ihr Konto. Klicken Sie dazu <a href=\"" + callbackUrl + "\">Email bestätigen</a>";
                    var header = "Konto bestätigen";
                    SendMail(user.Email, msg, header);

                    AllocateUser(user, model);
                    return RedirectToAction("Confirmation", "Account");
                }
                AddErrors(result);
            }
         * */

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