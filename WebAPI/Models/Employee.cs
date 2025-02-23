using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(18)]
        public string? IDCardNumber { get; set; }

        public string? Photo { get; set; }
        public string? Education { get; set; }
        public string? Title { get; set; }
        public string? Level { get; set; }
        public string? LevelJobType { get; set; }
        public string? Position { get; set; }
        public string? UnitName { get; set; }
        public DateTime RuzhiDate { get; set; }
        public string? SchoolName { get; set; }
        public string? ZhuanYe { get; set; }

        // 用于搜索的额外属性
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime RuzhiDateStart { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime RuzhiDateEnd { get; set; }
    }
}