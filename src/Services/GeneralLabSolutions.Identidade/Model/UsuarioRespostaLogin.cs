namespace GeneralLabSolutions.Identidade.Model
{
    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; } = string.Empty;
        public Guid RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken? UsuarioToken { get; set; }
    }
}
