using Middleware.Entities;
using Middleware.Helpers;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace Middleware.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUser(string username, string password);
    }

    public class UserService : IUserService
    {
        private List<User> _users = new List<User>();
        public UserService()
        {
            var json = File.ReadAllText(@"users.json");
            var accounts = JArray.Parse(json);
            foreach (var item in accounts)
            {
                _users.Add(new User
                {
                    Id = int.Parse(item["id"].ToString()),
                    Password = item["password"].ToString(),
                    Username = item["username"].ToString(),
                    Permissions = JArray.Parse(item["permissions"].ToString()).Select(r => r.ToString()).ToArray(),
                });
            }

        }
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        //private List<User> _users = new List<User>
        //{
        //    new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test",
        //    Permissions = new string[] {"Staff"} }
        //};
        public async Task<User> GetUser(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            return user;    
        }
        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _users);
        }
    }
}
