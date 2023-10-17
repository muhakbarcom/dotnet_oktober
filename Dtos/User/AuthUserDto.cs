using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_oktober.Dtos.User
{
    public class AuthUserDto
    {
        public required string USERNAME { get; set; }
        public required string PASSWORD { get; set; }
    }

    public class AuthResDto
    {
        public string? USERNAME { get; set; }
        public string? TOKEN { get; set; }
    }
}