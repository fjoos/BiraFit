using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiraFit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        private AccountController _controller;
        private Mock<HttpContextBase> moqContext;
        private Mock<HttpRequestBase> moqRequest;

        [TestMethod()]
        public void RegisterTest()
        {
            Assert.Fail();
        }
    }
}