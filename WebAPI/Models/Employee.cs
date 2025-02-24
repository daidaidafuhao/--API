using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// 员工信息实体类
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// 员工姓名，最大长度50个字符
        /// </summary>
        [MaxLength(50)]
        public string? Name { get; set; }

        /// <summary>
        /// 身份证号码，最大长度18个字符
        /// </summary>
        [MaxLength(18)]
        public string? IDCardNumber { get; set; }

        /// <summary>
        /// 员工照片路径
        /// </summary>
        public string? Photo { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public string? Education { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public string? Level { get; set; }

        /// <summary>
        /// 职级对应的工作类型
        /// </summary>
        public string? LevelJobType { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string? Position { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string? UnitName { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime RuzhiDate { get; set; }

        /// <summary>
        /// 毕业院校名称
        /// </summary>
        public string? SchoolName { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string? ZhuanYe { get; set; }

        /// <summary>
        /// 用于搜索的入职开始日期
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime RuzhiDateStart { get; set; }

        /// <summary>
        /// 用于搜索的入职结束日期
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime RuzhiDateEnd { get; set; }
    }
}