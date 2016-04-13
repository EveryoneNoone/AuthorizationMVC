using AuthorizationMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizationMVC
{
    public class DbInit
    {
        public static void DbInitialize()
        {
            ApplicationDbContext.DbInitialize();
        }
    }
}