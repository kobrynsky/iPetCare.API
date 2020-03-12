using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos.Users
{
    public class GetAllDtoResponse
    {
        public List<UserGetAllDtoResponse> Users { get; set; }
    }

    public class UserGetAllDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }
    }

}
