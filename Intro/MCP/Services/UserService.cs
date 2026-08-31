namespace MCP.Services
{
    public class UserService
    {
        private static List<User> _users = new();

        public UserService()
        {
            _users.AddRange(new[]
            {
                new User { Id =1, UserName = "alice", Email = "alice@example.com", City = "Seattle", JobTitle = "Quality assurance engineer." },
                new User { Id =2, UserName = "bob", Email = "bob@example.com", City = "New York", JobTitle = "Product manager" },
                new User { Id =3, UserName = "charlie", Email = "charlie@example.com", City = "Chicago", JobTitle = "Human resources specialist" },
                new User { Id =4, UserName = "tom", Email = "tom@example.com", City = "SanFrancisco", JobTitle = "Junior developer" },
                new User { Id =5, UserName = "andrei", Email = "andrei@example.com", City = "Minsk", JobTitle = "Lead software engineer." },
            });
        }

        public User? GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User CreateUser(User user)
        {
            Random rand = new Random();
            rand.Next(10, DateTime.Now.Microsecond);
            _users.Add(user);
            return user;
        }

        public bool UpdateUser(User u)
        {
            var user = _users.FirstOrDefault(u => u.Id == u.Id);
            if (user == null)
                return false;

            user.UserName = u.UserName;
            user.UserName = u.Email;
            user.UserName = u.City;
            user.UserName = u.JobTitle;
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            _users.Remove(user);
            return true;
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string City { get; set; }

        public string JobTitle { get; set; }
    }
}
