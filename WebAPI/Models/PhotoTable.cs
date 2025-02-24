using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// 员工照片信息实体类
    /// </summary>
    public class PhotoTable
    {
        /// <summary>
        /// 身份证号码，作为主键，必填，最大长度18个字符
        /// </summary>
        [Key]
        [Required]
        [MaxLength(18)]
        public required string IDCardNumber { get; set; }

        /// <summary>
        /// 员工照片数据，必填，存储为字节数组
        /// </summary>
        [Required]
        public required byte[] Photo { get; set; }
    }
}