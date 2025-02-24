using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// 培训记录实体类
    /// </summary>
    public class TrainingRecord
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 培训记录序号，必填
        /// </summary>
        [Required]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 培训日期，必填
        /// </summary>
        [Required]
        public DateTime TrainingDate { get; set; }

        /// <summary>
        /// 培训内容
        /// </summary>

        public string? TrainingContent { get; set; }

        /// <summary>
        /// 培训单位
        /// </summary>
 
        public string? TrainingUnit { get; set; }

        /// <summary>
        /// 培训地点
        /// </summary>

        public string? TrainingLocation { get; set; }

        /// <summary>
        /// 培训评估结果
        /// </summary>
        public string? Assessment { get; set; }

        /// <summary>
        /// 培训费用
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string? Remarks { get; set; }

        /// <summary>
        /// 关联的员工ID，必填
        /// </summary>
        [Required]
        public String EmployeeId { get; set; }

        /// <summary>
        /// 关联的员工信息
        /// </summary>
        public Employee? Employee { get; set; }
    }
}