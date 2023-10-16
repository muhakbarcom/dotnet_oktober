using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_oktober.Dtos.User
{
    public class AddUserDto
    {
        public required string USERNAME { get; set; }
        public required string PASSWORD { get; set; }
        public string? EMAIL { get; set; }
        public required string FULL_NAME { get; set; }
        public required string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string? CHANGED_BY { get; set; }
        public DateTime? CHANGED_DT { get; set; }
    }
}