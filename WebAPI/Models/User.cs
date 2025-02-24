using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    /// <summary>
    /// 用户信息实体类
    /// </summary>
    public class User
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        /// <summary>
        /// 电子邮箱地址
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// 权限列表（JSON格式存储）
        /// </summary>
        [MaxLength(500)]
        public string? Permissions { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}