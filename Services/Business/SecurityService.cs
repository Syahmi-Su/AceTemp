using AceTC.Models;
using AceTC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTC.Services.Business
{
    public class SecurityService
    {
        SecurityDAO daoService = new SecurityDAO();

        public bool Authenticate(UserModel user)
        {
            return daoService.FindByUser(user);
        }
    }
}