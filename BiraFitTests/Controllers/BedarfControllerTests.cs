using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BiraFit.Controllers.Tests
{
    [TestClass()]
    public class BedarfControllerTests
    {
        private BedarfController _controller;


        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new BedarfController();
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