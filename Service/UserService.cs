using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCoreCRUD.Service
{
	public class UserService
	{
		private readonly ApplicationDbContext _context;
		public UserService(ApplicationDbContext context) { 
			_context=context;
		}

        public bool IsUsernameTaken(string username)
        {
            return _context.Users.Any(u => u.username == username);
        }
        public User CheckUsernamePassword(string username, string password)
		{
			return _context.Users.FirstOrDefault(u => u.username == username && u.password ==password);
		}
		public void AddUser(User user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
		}
	}
}
