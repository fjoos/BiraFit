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
            BedarfController.BedarfList.Add(bedarf);
        }


 

    }
}