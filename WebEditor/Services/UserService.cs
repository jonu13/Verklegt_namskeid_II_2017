using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebEditor.Models;
using WebEditor.Models.Entities;

namespace WebEditor.Services
{
    public class UserService
    {
        private readonly IAppDataContext _db;
        public UserService(IAppDataContext context)
        {
            _db = context ?? new ApplicationDbContext();
        }

        private ApplicationDbContext _dbContext;
        public UserService()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ApplicationDbContext getContext()
        {
            return _dbContext;
        }

        public bool checkUserName(string userName)
        {
            // TODO
            return true;
        }
    }
}