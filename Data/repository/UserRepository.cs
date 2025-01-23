using Data.entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.repository
{
    public class UserRepository
    {
        private readonly MonedasContext _context;
        public UserRepository(MonedasContext context)
        {
            _context = context;
        }

        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }


        public List<User> GetUsers() 
        {
            return _context.Users.ToList();
        }
        public User GetById(int id)
        {
            return _context.Users.Include(u => u.Subscription)
                               .FirstOrDefault(u => u.Id == id);
            
        }
        

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
