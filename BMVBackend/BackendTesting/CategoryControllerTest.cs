using Backend.Controllers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BackendTesting
{
    [TestFixture]
    public class CategoryControllerTest
    {
        Mock<CategoryService> service;
        CategoryController controller;

        [OneTimeSetUp]
        public void SetUp()
        {
            service = new Mock<CategoryService>();
            controller = new CategoryController();
        }

        [Test]
        public void GetCategoryPositiveTest()
        {
            int id = 1;
            service.Setup(s => s.GetCategoryById(id)).Returns(new Category());
            typeof(CategoryController)
            .GetField("cService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(controller, service.Object);
            var result = controller.Get(id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetCategoryNegativeTest() {
            int id = 1;
            service.Setup(s => s.GetCategoryById(id)).Returns((Category)null);
            typeof(CategoryController)
            .GetField("cService", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(controller, service.Object);
            var result = controller.Get(id);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}
