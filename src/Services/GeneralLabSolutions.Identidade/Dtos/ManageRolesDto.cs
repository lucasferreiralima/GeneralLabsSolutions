namespace GeneralLabSolutions.Identidade.Dtos
{
    public class ManageRolesDto
    {
        public string UserId { get; set; }
        public string NomeCompleto { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
