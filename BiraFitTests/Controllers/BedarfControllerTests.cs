using Microsoft.VisualStudio.TestTools.UnitTesting;
using BiraFit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BiraFit.Models;
using BiraFit.ViewModel;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class BedarfControllerTests
    {

        private BedarfController controller;


        [TestInitialize]
        public void TestInitialize()
        {
            controller = new BedarfController();          
        }

      
[TestMethod()]
        public void IndexTestBed()
        {
         
/*            var result = controller.Index();
            //var model = result.Model as Model;
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(typeof(BedarfViewModel), viewResult);*/
        }

      
    }
}