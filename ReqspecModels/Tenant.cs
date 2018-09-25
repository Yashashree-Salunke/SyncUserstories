using System;
using System.Collections.Generic;
using System.Text;

namespace ReqspecModels
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string RepositoryUrl { get; set; }
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
