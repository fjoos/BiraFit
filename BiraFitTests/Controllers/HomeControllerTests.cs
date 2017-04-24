using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiraFit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BiraFit.ViewModel;
using Microsoft.CSharp;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            /*HomeController controller = new HomeController();
            var result = controller.Index();
            //var model = result.Model as Model;
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(typeof(BedarfViewModel), viewResult);

            */

        }

        [TestMethod()]
        public void AboutTest()
        {
          /*  HomeController controller = new HomeController();
            // Act
            ViewResult result = controller.About() as ViewResult;
            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);*/
        }
          

   
    }
}