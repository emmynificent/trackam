public interface IUser
{
    Task <IEnumerable<User>> GetAllUsers();
    Task <User> CreateUser (User user);
    Task  DeleteUser (User user);
    Task<User?> GetUserById(int Id);
    Task<User?> GetUserByGoogleId(string googleId);

}