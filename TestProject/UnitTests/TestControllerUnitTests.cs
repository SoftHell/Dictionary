using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using WebApp.ViewModels.Test;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.UnitTests
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
            var result = await _testController.TestIndex();

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            var vm = viewResult!.Model;
            Assert.IsType<TestViewModel>(vm);
            var testVm = vm as TestViewModel;
            Assert.NotNull(testVm!.Words);

            _testOutPutHelper.WriteLine($"Count of elements: {testVm.Words.Count}");

            Assert.Equal(0, testVm.Words.Count);

        }

        [Fact]
        public async void Action_Test__Returns_ViewModel_With_Data()
        {
            // ARRANGE
            await SeedData();

            // ACT
            var result = await _testController.TestIndex();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            Assert.NotNull(testVm);
            Assert.Equal(1, testVm!.Words.Count);
            Assert.Equal("Word 0", testVm.Words.First()!.Value);

            _testOutPutHelper.WriteLine($"Count of elements: {testVm.Words.Count}");
        }

        [Fact]
        public async Task Action_Test__Returns_ViewModel_WithNoData__Fails_With_Exception()
        {
            // ACT
            var result = await _testController.TestIndex();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            Assert.NotNull(testVm);
            Assert.ThrowsAny<Exception>(() => testVm!.Words.First());
        }


        [Theory]
        [ClassData(typeof(CountGenerator))]
        public async Task Action_Test__Returns_ViewModel_WithData_Fluent(int count)
        {
            // ARRANGE
            await SeedData(count);

            // ACT
            var result = await _testController.TestIndex();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            testVm.Should().NotBeNull();
            testVm!.Words
                .Should().NotBeNull()
                .And.HaveCount(count)
                .And.Contain(w => w.Value!.ToString() == "Word 0")
                .And.Contain(w => w.Value!.ToString() == $"Word {count - 1}");
        }

        [Fact]
        public async Task Test_Details__Returns_ViewModel_With_Data()
        {
            await SeedData();
            var word = await _ctx.Words
                .Include(x => x.Language)
                .FirstAsync();

            // ACT
            var result = await _testController.TestDetails(word.Id)!;

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            var vm = viewResult!.Model;
            Assert.IsType<Word>(vm);
            var testVm = vm as Word;

            _testOutPutHelper.WriteLine($"Element name: {testVm!.Value}");
            _testOutPutHelper.WriteLine($"Element name: {testVm!.Id}");

            Assert.Equal("Word 0", testVm.Value);
            Assert.Equal(word.Id, testVm.Id);
            Assert.Equal(word.LanguageId, testVm.LanguageId);

        }

        [Fact]
        public async Task Test_Create_Word__Object_Added_To_DB()
        {
            // ARRANGE
            var word = await SeedWord();

            // ACT
            var result = await _testController.TestDetails(word.Id)!;

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult);
            var vm = viewResult!.Model;
            Assert.IsType<Word>(vm);
            var testVm = vm as Word;

            _testOutPutHelper.WriteLine($"Element name: {testVm!.Value}");
            _testOutPutHelper.WriteLine($"Element name: {testVm!.Id}");

            Assert.Equal("TestWord", testVm.Value);
            Assert.Equal(word.Id, testVm.Id);
            Assert.Equal(word.LanguageId, testVm.LanguageId);

        }
        
        [Fact]
        public async Task Test_Delete__Object_Removed_From_DB()
        {
            // ARRANGE
            var word = await SeedWord();

            // ACT
            _ctx.Words.Remove(word);
            await _ctx.SaveChangesAsync();

            var result = await _ctx.Words.FirstOrDefaultAsync(w => w.Id == word.Id);

            // ASSERT
            Assert.Null(result);
        }
        
        
        [Fact]
        public async Task Test_Edit__Object_In_DB_Updated()
        {
            // ARRANGE
            var word = await SeedWord();
            word.Value = "UpdatedValue";

            // ACT
            _ctx.Words.Update(word);
            await _ctx.SaveChangesAsync();

            var result = await _ctx.Words.FirstOrDefaultAsync(w => w.Id == word.Id);

            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<Word>(result);
            Assert.Equal("UpdatedValue", result.Value);
        }
        

        private async Task SeedData(int count = 1)
        {
            var langDTO = new Language()
            {
                Name = new LangString("TestLang", "test")
            };
            var language = _ctx.Languages.Add(langDTO);

            var topicDTO = new Topic()
            {
                Name = new LangString("TestTopic", "test")
            };
            var topic = _ctx.Topics.Add(topicDTO);

            var partOfSpeechDTO = new PartOfSpeech()
            {
                Name = new LangString("TestPartOfSpeech", "test")
            };
            var partOfSpeech = _ctx.PartsOfSpeech.Add(partOfSpeechDTO);


            for (int i = 0; i < count; i++)
            {
                _ctx.Words.Add(new Word
                {
                    Value = $"Word {i}",
                    LanguageId = language.Entity.Id,
                    TopicId = topic.Entity.Id,
                    PartOfSpeechId = partOfSpeech.Entity.Id
                });
            }

            await _ctx.SaveChangesAsync();
        }

        private async Task<Word> SeedWord()
        {
            var langDTO = new Language
            {
                Name = new LangString("TestLang", "test")
            };
            var language = _ctx.Languages.Add(langDTO);

            var topicDTO = new Topic
            {
                Name = new LangString("TestTopic", "test")
            };
            var topic = _ctx.Topics.Add(topicDTO);

            var partOfSpeechDTO = new PartOfSpeech
            {
                Name = new LangString("TestPartOfSpeech", "test")
            };
            var partOfSpeech = _ctx.PartsOfSpeech.Add(partOfSpeechDTO);

            var wordDTO = new Word
            {
                Value = "TestWord",
                LanguageId = language.Entity.Id,
                TopicId = topic.Entity.Id,
                PartOfSpeechId = partOfSpeech.Entity.Id
            };
            
            var word = _ctx.Words.Add(wordDTO);
            await _ctx.SaveChangesAsync();
            return word.Entity;
        }

        public class CountGenerator : IEnumerable<object[]>
        {
            private static List<object[]> _data
            {
                get
                {
                    var res = new List<Object[]>();
                    for (int i = 1; i <= 100; i++)
                    {
                        res.Add(new object[] {i});
                    }

                    return res;
                }
            }

            public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}