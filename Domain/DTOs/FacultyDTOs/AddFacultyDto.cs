using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AddFacultyDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null;
        public FacultyStatus Status { get; set; }
    }
}

