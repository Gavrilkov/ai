namespace Agent
{
    public class UserService
    {
        private static List<User> _users = new();

        public UserService()
        {
            _users.AddRange(new[]
            {
                new User { UserName = "alice", Email = "alice@example.com", City = "Seattle", JobTitle = "Quality assurance engineer." },
                new User { UserName = "bob", Email = "bob@example.com", City = "New York", JobTitle = "Product manager" },
                new User { UserName = "charlie", Email = "charlie@example.com", City = "Chicago", JobTitle = "Human resources specialist" },
                new User { UserName = "tom", Email = "charlie@example.com", City = "Chicago", JobTitle = "Junior developer" },
                new User { UserName = "andrei", Email = "charlie@example.com", City = "Minsk", JobTitle = "Lead software engineer." },
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

        public User CreateUser(string username, string email)
        {
            var user = new User
            {
                UserName = username,
                Email = email
            };
            _users.Add(user);
            return user;
        }

        public bool UpdateUser(int id, string username, string email)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            user.UserName = username;
            user.Email = email;
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
