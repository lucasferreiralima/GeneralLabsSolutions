namespace GeneralLabSolutions.Identidade.Configuration
{
    /// <summary>
    /// Classe que guardará os dados do nó
    /// SeedUsersSettings em secrets.json
    /// </summary>
    public class SeedUserSettings
    {
        public string SuperAdminEmail { get; set; } = string.Empty;

        public string SuperAdminPassword { get; set; } = string.Empty;

        public string AdminEmail { get; set; } = string.Empty;
        public string AdminPassword { get; set; } = string.Empty;
        public string DefaultUserEmail { get; set; } = string.Empty;
        public string DefaultUserPassword { get; set; } = string.Empty;
    }
}