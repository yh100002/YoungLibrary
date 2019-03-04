using System.Threading.Tasks;
using Data;
using Models;

namespace Web.Auth
{
     public class AuthRepository : IAuthRepository
    {
         private readonly IUnitOfWork uow;
        public AuthRepository(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        //The name of Method should be LoginAsync
        public async Task<User> Login(string userName, string password)
        {   
            var repo = this.uow.GetRepositoryAsync<User>();       
            var user = await repo.SingleAsync(x => x.UserName == userName); 

            if (user == null)
                return null;
            
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var repo = this.uow.GetRepositoryAsync<User>();
            await repo.AddAsync(user);  
            this.uow.SaveChanges();
            return user;
        }

        /*
        Password Hashing        
         */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;//Randomly generated seed
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> PassengerExists(string userName)
        {
            var repo = this.uow.GetRepositoryAsync<User>();
            var user = await repo.SingleAsync(x => x.UserName == userName);
            if (user != null)
                return true;
            return false;
        }      
    }
}