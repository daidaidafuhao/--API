using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI.Tests.Controllers
{
    [TestClass]
    public class DatabaseControllerTests
    {
        private DatabaseController _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new DatabaseController();
        }

        [TestMethod]
        public void GetEmployees_ReturnsOkResult()
        {
            // Arrange
            string searchTerm = "张";

            // Act
            var result = _controller.GetEmployees(searchTerm);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void SearchEmployees_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "测试员工",
                IDCardNumber = "123456789"
            };

            // Act
            var result = _controller.SearchEmployees(employee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void InsertEmployee_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "新员工",
                IDCardNumber = "987654321",
                Gender = "男",
                BirthDate = DateTime.Now.AddYears(-25),
                PhoneNumber = "13800138000",
                Department = "技术部",
                Position = "工程师"
            };

            // Act
            var result = _controller.InsertEmployee(employee);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateEmployee_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee
            {
                Name = "更新员工",
                IDCardNumber = "987654321",
                Gender = "男",
                BirthDate = DateTime.Now.AddYears(-25),
                PhoneNumber = "13800138000",
                Department = "技术部",
                Position = "高级工程师"
            };
            string oldId = "987654321";

            // Act
            var result = _controller.UpdateEmployee(employee, oldId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetTrainingRecords_ReturnsOkResult()
        {
            // Arrange
            string employeeId = "123456789";

            // Act
            var result = _controller.GetTrainingRecords(employeeId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAllTrainingRecords_ReturnsOkResult()
        {
            // Act
            var result = _controller.GetAllTrainingRecords();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void SearchTrainingRecords_ReturnsOkResult()
        {
            // Arrange
            string content = "安全培训";
            string unit = "培训部";
            string location = "会议室";

            // Act
            var result = _controller.SearchTrainingRecords(content, unit, location);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void InsertTrainingRecord_ReturnsOkResult()
        {
            // Arrange
            var record = new TrainingRecord
            {
                EmployeeId = "123456789",
                TrainingContent = "新员工培训",
                TrainingUnit = "人力资源部",
                TrainingLocation = "培训室",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                TrainingHours = 8,
                Trainer = "王教练",
                TrainingType = "入职培训",
                TrainingResult = "通过"
            };

            // Act
            var result = _controller.InsertTrainingRecord(record);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UpdateTrainingRecord_ReturnsOkResult()
        {
            // Arrange
            var record = new TrainingRecord
            {
                Id = 1,
                EmployeeId = "123456789",
                TrainingContent = "更新后的培训内容",
                TrainingUnit = "人力资源部",
                TrainingLocation = "培训室",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                TrainingHours = 16,
                Trainer = "李教练",
                TrainingType = "技能提升",
                TrainingResult = "优秀"
            };

            // Act
            var result = _controller.UpdateTrainingRecord(record);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}