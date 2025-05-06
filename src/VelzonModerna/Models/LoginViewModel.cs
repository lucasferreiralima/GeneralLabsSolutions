namespace VelzonModerna.Models
{
    public class LoginViewModel
    {
        public LoginViewModel() { }

        public LoginViewModel(string? login, string? senha)
        {
            Login = login;
            Senha = senha;
        }

        public string? Login { get;  set; }
        public string? Senha { get;  set; }
    }
}
