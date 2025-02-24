using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    /// <summary>
    /// 导入历史记录实体类
    /// </summary>
    public class ImportHistory
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 身份证号码，必填，最大长度18个字符
        /// </summary>
        [Required]
        [MaxLength(18)]
        public string IDCardNumber { get; set; }

        /// <summary>
        /// 导入数量，必填
        /// </summary>
        [Required]
        public string ImportCount { get; set; }

        /// <summary>
        /// 导入时间，必填
        /// </summary>
        [Required]
        public string ImportTime { get; set; }

        /// <summary>
        /// 关联的员工信息
        /// </summary>
        [ForeignKey("IDCardNumber")]
        public Employee? Employee { get; set; }
    }
}