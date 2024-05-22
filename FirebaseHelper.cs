using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

namespace YourAppNamespace
{
    public class FirebaseHelper
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseHelper()
        {
            _firebaseClient = new FirebaseClient("https://logintest-b489f-default-rtdb.firebaseio.com/");
        }

        public async Task AddUser(string id, string patientId, string name, string birthdate, string phone, string username, string password)
        {
            await _firebaseClient
                .Child("Users")
                .Child(id)
                .PutAsync(new User
                {
                    Id = id,  // 식별자
                    PatientId = patientId, // (랜덤)환자번호
                    Name = name, // 이름
                    Birthdate = birthdate, // 생년월일
                    Phone = phone, // 전화번호
                    Username = username, // 아이디
                    Password = password // 비밀번호
                });
        }

        public async Task<bool> IsUsernameExists(string username) // 아이디 중복
        {
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<User>();

            return users.Any(u => u.Object.Username == username);
        }

        public async Task<bool> IsPhoneExists(string phone)
        {
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<User>();

            return users.Any(u => u.Object.Phone == phone);
        }
        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<User>();

            return users.FirstOrDefault(u => u.Object.Username == username && u.Object.Password == password)?.Object;
        }

        public async Task<bool> IsPatientIdExists(string patientId)
        {
            var users = await _firebaseClient
                .Child("Users")
                .OnceAsync<User>();

            return users.Any(u => u.Object.PatientId == patientId);
        }

        public async Task<string> GenerateUniquePatientId() // 환자번호
        {
            string patientId;
            Random random = new Random();
            do
            {
                patientId = GeneratePatientId(random);
            } while (await IsPatientIdExists(patientId));

            return patientId;
        }

        private string GeneratePatientId(Random random) // 환자번호
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";

            string alphaPart = new string(Enumerable.Repeat(chars, 2).Select(s => s[random.Next(s.Length)]).ToArray());
            string numberPart = new string(Enumerable.Repeat(numbers, 3).Select(s => s[random.Next(s.Length)]).ToArray());

            return alphaPart + numberPart;
        }
    }

    public class User // User 클래스에 저장
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Name { get; set; }
        public string Birthdate { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
