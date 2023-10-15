using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_oktober.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public required string USERNAME { get; set; }

        [EmailAddress]
        public string? EMAIL { get; set; }

        [Required]
        [MaxLength(100)]
        public required string FULL_NAME { get; set; }

        [MaxLength(50)]
        public string? CREATED_BY { get; set; }

        public DateTime CREATED_DT { get; set; }

        [MaxLength(50)]
        public string? CHANGED_BY { get; set; }

        public DateTime? CHANGED_DT { get; set; }
    }
}