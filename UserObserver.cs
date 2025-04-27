namespace PlantGuardian.Models
{
    public class UserObserver : IObserver
    {
        private readonly string _userEmail;
        public string AlertMessage { get; private set; }

        public UserObserver(string userEmail)
        {
            _userEmail = userEmail;
        }

        public void Update(string message)
        {
            AlertMessage = $"Alert for {_userEmail}: {message}";
        }

        public string GetUserEmail()
        {
            return _userEmail;
        }
    }
}
