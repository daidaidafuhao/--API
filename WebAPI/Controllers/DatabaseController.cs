using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseManager _databaseManager;

        public DatabaseController()
        {
            _databaseManager = new DatabaseManager();
        }


        /// <summary>
        /// 获取下拉框选项数据
        /// </summary>
        /// <param name="query">SQL查询语句</param>
        /// <returns>选项列表</returns>
        [HttpGet("combobox-items")]
        public ActionResult<List<string>> GetComboBoxItems([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest(new { success = false, message = "查询语句不能为空" });
                }

                var items = _databaseManager.LoadComboBoxItems(query);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        /// <summary>
        /// 获取所有员工信息
        /// </summary>
        /// <param name="searchTerm">搜索关键词（可选）</param>
        /// <returns>员工列表</returns>
        [HttpGet("employees")]
        public ActionResult<List<Employee>> GetEmployees([FromQuery] string searchTerm = null)
        {
            try
            {
                var employees = _databaseManager.GetEmployees(searchTerm);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                 return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 根据条件搜索员工
        /// </summary>
        /// <param name="employee">员工搜索条件</param>
        /// <returns>符合条件的员工列表</returns>
        [HttpPost("employees/search")]
        public ActionResult<List<Employee>> SearchEmployees([FromBody] Employee employee)
        {
            try
            {
                // 验证请求数据
                if (employee == null)
                {
                    return BadRequest("搜索条件不能为空");
                }

                // 记录请求参数
                Console.WriteLine($"搜索条件: Name={employee.Name}, IDCardNumber={employee.IDCardNumber}");

                var employees = _databaseManager.SearchEmployees(employee);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索员工时发生错误: {ex.Message}");
                 return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 添加新员工
        /// </summary>
        /// <param name="employee">员工信息</param>
        [HttpPost("addEmployees")]
        public ActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                _databaseManager.InsertEmployee(employee);
                return Ok(new { success = true, message = "员工添加成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="employee">更新后的员工信息</param>
        /// <param name="oldId">原身份证号</param>
        [HttpPut("employees/UpdateEmployeeAndTrainingRecord/{oldId}")]
        public ActionResult UpdateEmployee([FromBody] Employee employee, string oldId)
        {
            try
            {
                _databaseManager.UpdateEmployeeAndTrainingRecord(employee, oldId);
       
                return Ok(new { success = true, message = "员工信息更新成功" });
            }
            catch (Exception ex)
            {
               return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 获取指定员工的培训记录
        /// </summary>
        /// <param name="employeeId">员工ID</param>
        /// <returns>培训记录列表</returns>
        [HttpGet("training-records/{employeeId}")]
        public ActionResult<List<TrainingRecord>> GetTrainingRecords(string employeeId)
        {
            try
            {
                var records = _databaseManager.GetTrainingRecordsByEmployeeId(employeeId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 获取所有培训记录
        /// </summary>
        /// <returns>所有培训记录列表</returns>
        [HttpGet("training-records")]
        public ActionResult<List<TrainingRecord>> GetAllTrainingRecords()
        {
            try
            {
                var records = _databaseManager.GetALLTrainingRecords();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 根据条件搜索培训记录
        /// </summary>
        /// <param name="content">培训内容</param>
        /// <param name="unit">培训单位</param>
        /// <param name="location">培训地点</param>
        /// <returns>符合条件的培训记录列表</returns>
        [HttpGet("training-records/search")]
        public ActionResult<List<TrainingRecord>> SearchTrainingRecords(
            [FromQuery] string content,
            [FromQuery] string unit,
            [FromQuery] string location)
        {
            try
            {
                var records = _databaseManager.GetTrainingRecordsByCriteria(content, unit, location);
                return Ok(records);
            }
            catch (Exception ex)
            {
                 return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 添加培训记录
        /// </summary>
        /// <param name="record">培训记录信息</param>
        [HttpPost("training-records")]
        public ActionResult InsertTrainingRecord([FromBody] TrainingRecord record)
        {
            try
            {
                _databaseManager.InsertTrainingRecord(record);

                return Ok(new { success = true, message = "培训记录添加成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 更新培训记录
        /// </summary>
        /// <param name="record">更新后的培训记录信息</param>
        [HttpPut("InsertTrainingRecordOrUpdate")]
        public ActionResult UpdateTrainingRecord([FromBody] TrainingRecord record)
        {
            try
            {
                _databaseManager.InsertTrainingRecordOrUpdate(record);
                return Ok(new { success = true, message = "培训记录更新成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = true, message = ex.Message });
            }
        }

        /// <summary>
        /// 删除指定的培训记录
        /// </summary>
        /// <param name="serialNumber">培训记录序号</param>
        /// <param name="employeeId">员工ID</param>
        /// <returns>操作结果</returns>
        [HttpDelete("training-records/DeleteTrainingRecord/{employeeId}/{serialNumber}")]
        public ActionResult DeleteTrainingRecord(string employeeId, string serialNumber)
        {
            try
            {
                _databaseManager.DeleteTrainingRecordBySerialNumber(serialNumber, employeeId);
                return Ok(new { success = true, message = "培训记录删除成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 批量更新培训记录
        /// </summary>
        /// <param name="records">培训记录列表</param>
        /// <returns>操作结果</returns>
        [HttpPut("BatchInsertTrainingRecordOrUpdate")]
        public ActionResult BatchUpdateTrainingRecords([FromBody] List<TrainingRecord> records)
        {
            try
            {
                if (records == null || !records.Any())
                {
                    return BadRequest(new { success = false, message = "培训记录列表不能为空" });
                }

                int successCount = 0;
                var errors = new List<string>();

                foreach (var record in records)
                {
                    try
                    {
                        _databaseManager.InsertTrainingRecordOrUpdate(record);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"处理培训记录时出错：{ex.Message}");
                    }
                }
                // 返回更新结果
                var TrainingRecords = _databaseManager.GetTrainingRecordsByEmployeeId(records[0].EmployeeId);
                return Ok(TrainingRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 批量插入或更新员工信息
        /// </summary>
        /// <param name="employees">员工信息列表</param>
        /// <returns>操作结果</returns>
        [HttpPost("employees/BatchInsertOrUpdateEmployees")]
        public ActionResult BatchInsertOrUpdateEmployees([FromBody] List<Employee> employees)
        {
            try
            {
                if (employees == null || employees.Count == 0)
                {
                    return BadRequest(new { success = false, message = "员工列表不能为空" });
                }

                int successCount = 0;
                var errors = new List<string>();

                foreach (var employee in employees)
                {
                    try
                    {
                        _databaseManager.InsertOrUpdateEmployee(employee);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"处理员工 {employee.Name}（身份证号：{employee.IDCardNumber}）时出错：{ex.Message}");
                    }
                }

                return Ok(new { 
                    success = true, 
                    message = $"批量处理完成。成功：{successCount}条，失败：{errors.Count}条",
                    errors = errors
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 批量插入或更新导入履历记录
        /// </summary>
        /// <param name="importHistories">导入履历记录列表</param>
        /// <returns>操作结果</returns>
        [HttpPost("importhistories/BatchInsertOrUpdateImportHistories")]
        public ActionResult BatchInsertOrUpdateImportHistories([FromBody] List<ImportHistory> importHistories)
        {
            try
            {
                if (importHistories == null || !importHistories.Any())
                {
                    return BadRequest(new { success = false, message = "导入履历记录列表不能为空" });
                }

                int successCount = 0;
                var errors = new List<string>();

                foreach (var history in importHistories)
                {
                    try
                    {
                        _databaseManager.InsertOrUpdateImportHistory(history);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"处理导入履历记录（身份证号：{history.IDCardNumber}）时出错：{ex.Message}");
                    }
                }

                return Ok(new { 
                    success = true, 
                    message = $"批量处理完成。成功：{successCount}条，失败：{errors.Count}条",
                    errors = errors
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 根据导入时间获取导入履历记录
        /// </summary>
        /// <param name="importTime">导入时间</param>
        /// <returns>导入履历记录列表</returns>
        [HttpGet("importhistories/byTime/{importTime}")]
        public ActionResult<List<ImportHistory>> GetImportHistoriesByImportTime(string importTime)
        {
            try
            {
                if (string.IsNullOrEmpty(importTime))
                {
                    return BadRequest(new { success = false, message = "导入时间不能为空" });
                }

                var histories = _databaseManager.GetImportHistoriesByImportTime(importTime);
                return Ok(histories);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 更新或插入员工照片
        /// </summary>
        /// <param name="idCardNumber">身份证号</param>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>操作结果</returns>
        [HttpPost("employees/photo/{idCardNumber}")]
        public ActionResult UpdateEmployeePhoto(string idCardNumber, [FromBody] byte[] imageBytes)
        {
            try
            {
                if (string.IsNullOrEmpty(idCardNumber))
                {
                    return BadRequest(new { success = false, message = "身份证号不能为空" });
                }

                if (imageBytes == null || imageBytes.Length == 0)
                {
                    return BadRequest(new { success = false, message = "图片数据不能为空" });
                }

                _databaseManager.InsertEmployeePhotoByIDCard(idCardNumber, imageBytes);
                return Ok(new { success = true, message = "员工照片更新成功" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 根据身份证号获取员工照片
        /// </summary>
        /// <param name="idCardNumber">身份证号</param>
        /// <returns>照片字节数组</returns>
        [HttpGet("employees/GetEmployeePhoto/{idCardNumber}")]
        public ActionResult GetEmployeePhoto(string idCardNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(idCardNumber))
                {
                    return BadRequest(new { success = false, message = "身份证号不能为空" });
                }

                byte[] photoData = _databaseManager.GetPhotoByIDCard(idCardNumber);
                if (photoData == null)
                {
                    return NotFound(new { success = false, message = "未找到该员工的照片" });
                }
               return Ok(photoData);

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 根据身份证号删除员工的所有相关数据
        /// </summary>
        /// <param name="idCardNumber">身份证号</param>
        /// <returns>删除操作的结果</returns>
        [HttpDelete("employees/DeleteEmployeeByIdCard/{idCardNumber}")]
        public ActionResult DeleteEmployeeByIdCard(string idCardNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(idCardNumber))
                {
                    return BadRequest(new { success = false, message = "身份证号不能为空" });
                }

                _databaseManager.DeleteEmployeeDataByIDCard(idCardNumber);
                return Ok(new { success = true, message = $"身份证号为 {idCardNumber} 的员工数据已成功删除" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        [HttpGet("roles")]
        public ActionResult<List<string>> GetRoles()
        {
            try
            {
                // 这里可以根据实际需求从数据库中获取角色列表
                // 示例：使用现有的LoadComboBoxItems方法获取角色
                var query = "SELECT DISTINCT UnitName FROM Employee";
                var roles = _databaseManager.LoadComboBoxItems(query);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// 测试API连接状态
        /// </summary>
        /// <returns>服务器状态信息</returns>
        [HttpGet("test-connection")]
        public ActionResult TestConnection()
        {
            try
            {
                return Ok(new { 
                    success = true, 
                    message = "服务器连接正常",
                    timestamp = DateTime.Now,
                    serverStatus = "running"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}