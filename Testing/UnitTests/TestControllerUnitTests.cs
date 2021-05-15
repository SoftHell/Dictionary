using System;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using WebApp.ViewModels.Test;


namespace Testing.UnitTests
{
    public class TestControllerUnitTests
    {
        private readonly TestController _testController;
        private readonly ITestOutputHelper _testOutPutHelper;
        private readonly AppDbContext _ctx;
        
        
        // ARRANGE - common
        public TestControllerUnitTests(ITestOutputHelper testOutPutHelper)
        {
            _testOutPutHelper = testOutPutHelper;

            // set up db context for testing - using InMemory db engine
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            // InMemory DB needs unique name - Guid is used
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            _ctx = new AppDbContext(optionsBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<TestController>();
            
            // SUT (System Under Testing)
            _testController = new TestController(logger, _ctx);
        }

        [Fact]
        public async void Action_Test__Returns_ViewModel()
        {
            // ACT
            var result = await _testController.Test();
            
            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            var vm = viewResult!.Model;
            Assert.IsType<TestViewModel>(vm);
            var testVm = vm as TestViewModel;
            Assert.NotNull(testVm!.ActivityTypes);
            
            _testOutPutHelper.WriteLine($"Count of elements: {testVm.ActivityTypes.Count}");
            
            Assert.Equal(0, testVm.ActivityTypes.Count);
            
        }
}

    public class FactAttribute : Attribute
    {
    }

    internal interface ITestOutputHelper
    {
    }