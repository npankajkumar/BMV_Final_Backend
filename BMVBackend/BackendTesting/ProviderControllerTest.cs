using Backend.Controllers;
using Backend.DTO.Provider;
using Backend.Models;
using Backend.Services;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BackendTesting
{
    [TestFixture]
    public class ProviderControllerTest
    {
        Mock<IProvidersService> service;
        ProvidersController controller;
        PostProviderDTO postProviderDto;
        PutProviderDTO putProviderDto;
        ProviderLoginDTO loginDto;
        Provider provider;
        [OneTimeSetUp]
        public void SetUp()
        {
            service = new Mock<IProvidersService>();
            controller = new ProvidersController(service.Object);
            provider = new Provider { Id = 1, Name = "Niketh Donthula", Mobile = "1234567890", Email = "nikethdonthula@gmail.com" };
            postProviderDto = new PostProviderDTO
            {
                Email = "test@gmail.com",
                Name = "Test",
                Mobile = "1234567890",
                Password = "test@1234"
            };
            loginDto = new ProviderLoginDTO
            {
                Email = "test@gmail.com",
                Password = "pass@1234"
            };
            putProviderDto = new PutProviderDTO
            {
                Email = "sathvik@gmail.com",
                Name = "Test",
                Mobile = "1234567890",
                Password = "test@1234"
            };
        }

        [Test]
        public void GetProviderPositiveTest()
        {
            var providerId = "1";
            service.Setup(s => s.GetProviderById(1)).Returns(provider);
            var userClaims = new List<Claim> { new Claim("ProviderId", providerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Get();
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetProviderNegativeTest()
        {
            var providerId = "1";
            service.Setup(s => s.GetProviderById(1)).Returns((Provider)null);
            var userClaims = new List<Claim> { new Claim("ProviderId", providerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Get();
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void PostProviderPositiveTest()
        {
            service.Setup(s=> s.AddProvider(It.IsAny<Provider>())).Returns(true);
            var result = controller.Post(postProviderDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        [Test]
        public void PostProviderNegativeTest() {
            service.Setup(s => s.AddProvider(It.IsAny<Provider>())).Returns(false);
            var result = controller.Post(postProviderDto);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

        [Test]
        public void PutProviderPositiveTest()
        {
            var providerId = "1";
            var updatedProvider = new Provider
            {
                Id = 1,
                Name = putProviderDto.Name,
                Mobile = putProviderDto.Mobile,
                Email = putProviderDto.Email,
                
            };
            service.Setup(s => s.UpdateProvider(It.IsAny<int>(), It.IsAny<Provider>())).Returns(updatedProvider);
            var userClaims = new List<Claim> { new Claim("ProviderId", providerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Put(putProviderDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void PutProviderNegativeTest() {
            var providerId = "1";
            var updatedProvider = new Provider
            {
                Id = 1,
                Name = putProviderDto.Name,
                Mobile = putProviderDto.Mobile,
                Email = putProviderDto.Email,
                
            };
            service.Setup(s => s.UpdateProvider(It.IsAny<int>(), It.IsAny<Provider>())).Returns((Provider)null);
            var userClaims = new List<Claim> { new Claim("ProviderId", providerId) };
            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };
            var result = controller.Put(putProviderDto);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }


        [Test]
        public void ValidateProviderPositiveTest()
        {

            service.Setup(s => s.ValidateProvider(loginDto)).Returns(provider);
            var result = controller.Validate(loginDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ValidateProviderNegativeTest()
        {
            var result = controller.Validate(null);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public void ValidateProviderNotFound()
        {
            service.Setup(s => s.ValidateProvider(loginDto)).Returns((Provider)null);
            var result = controller.Validate(loginDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

    }
}
