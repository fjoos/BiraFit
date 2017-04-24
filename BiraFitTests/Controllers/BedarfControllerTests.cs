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
            Bedarf bedarf = new Bedarf();
            bedarf.Titel = "TestBedarf";
            //BedarfController.BedarfList.Add(bedarf);

            /*
            var controller = new ProductController();
            var result = controller.Details(2) as ViewResult;
            var product = (Product)result.ViewData.Model;
            Assert.AreEqual("Laptop", product.Name);*/
        }

      
[TestMethod()]
        public void IndexTestBed()
        {
          /*  BedarfController controller = new BedarfController();
            var result = controller.Index();
            //var model = result.Model as Model;
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.AreEqual(typeof(BedarfViewModel), viewResult);*/
        }

      
    }
}