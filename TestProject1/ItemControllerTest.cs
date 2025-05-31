using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelUp_Task.Controllers;
using TravelUp_Task.Data;
using TravelUp_Task.Models;
using Xunit;

namespace TravelUp_Task.Tests
{
    public class ItemsControllerTests
    {
        
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void Index_Returns_All_Items()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Items.Add(new Item { Name = "Item 1" });
            context.Items.Add(new Item { Name = "Item 2" });
            context.SaveChanges();

            var controller = new ItemsController(context);

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as List<Item>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void CreateAjax_Adds_New_Item()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ItemsController(context);
            string newItemName = "Test Item";

            // Act
            var result = controller.CreateAjax(newItemName) as JsonResult;
            dynamic json = result.Value;

            // Assert
            Assert.True(json.success);
            Assert.Equal("Test Item", (string)json.item.name); // lowercase 'name' as it's JSON
            Assert.Equal(1, context.Items.Count());
        }

        [Fact]
        public void CreateAjax_With_Empty_Name_Returns_Error()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ItemsController(context);

            // Act
            var result = controller.CreateAjax("") as JsonResult;
            dynamic json = result.Value;

            // Assert
            Assert.False(json.success);
            Assert.Equal("Name is required", (string)json.message);
        }
    }
}
