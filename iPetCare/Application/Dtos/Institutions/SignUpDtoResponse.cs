using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Institutions
{
    public class SignUpDtoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
