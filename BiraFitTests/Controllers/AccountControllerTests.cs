using BiraFit.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BiraFit.Models;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Principal;
using TypeMock.ArrangeActAssert;

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

[TestMethod, Isolated]
public async Task TestWhenLoginIsBad_ErrorMessageIsShown()
{
    // Arrange
    // Create the wanted controller for testing 
    var controller = new AccountController(); 
    var loginData = new LoginViewModel { Email = "support@typemock.com", Password = "password", RememberMe = false };

    // Fake the ModelState
    Isolate.WhenCalled(() => controller.ModelState.IsValid).WillReturn(true);

    // Ignore AddModelError (should be called when login fails)
    Isolate.WhenCalled(() => controller.ModelState.AddModelError("", "")).IgnoreCall();

    // Fake HttpContext to return a fake ApplicationSignInManager
    var fakeASIM = Isolate.WhenCalled(() => controller.HttpContext.GetOwinContext().Get<ApplicationSignInManager>()).ReturnRecursiveFake();

    // When password checked it will fail. Note we are faking an async method
    Isolate.WhenCalled(() => fakeASIM.PasswordSignInAsync(null, null, true, true)).WillReturn(Task.FromResult(SignInStatus.Failure));

    // Act
    var result = await controller.Login(loginData, "http://www.typemock.com/");

    // Assert
    // The result contains login data, doesn’t redirect
    Assert.IsInstanceOfType(result, typeof(ViewResult));
    Assert.AreSame(loginData, (result as ViewResult).Model);
    // Make sure that the code added an error
    Isolate.Verify.WasCalledWithExactArguments(() => controller.ModelState.AddModelError("", "Invalid login attempt."));
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
        public async Task LoginWithNullModelAsync()
        {
            var result = await _controller.Login(model: null, returnUrl: "/login");
            var viewResult = (ViewResult)result;
            Assert.AreEqual(viewResult.Model, null);
            Assert.AreEqual("Login", viewResult.ViewName);
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


