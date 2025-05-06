using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using GeneralLabSolutions.Identidade.Dtos;

namespace VelzonModerna.Services
{
    public class UserAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserAdminService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }

        // Definir o token JWT no cabeçalho
        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }



        public async Task<List<UserDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/admin/usuarios");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(json);

            var users = new List<UserDto>();

            foreach (var user in jsonObject.EnumerateArray())
            {
                var userDto = new UserDto
                {
                    UserId = user.GetProperty("userId").GetString(),
                    NomeCompleto = user.GetProperty("nomeCompleto").GetString(),
                    Email = user.GetProperty("email").GetString(),
                    EmailConfirmado = user.GetProperty("emailConfirmado").GetBoolean() ? "Sim" : "Não"
                };
                users.Add(userDto);
            }

            return users;
        }

        // Criar usuário
        public async Task<bool> CriarUsuarioAsync(CriarUsuarioDto usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/admin/criar-usuario", content);
            return response.IsSuccessStatusCode;
        }

        // Obter detalhes de um usuário
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"api/admin/usuarios/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Atualizar usuário
        public async Task<bool> AtualizarUsuarioAsync(AtualizarUsuarioDto usuario)
        {
            var json = JsonSerializer.Serialize(usuario);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/admin/usuarios/{usuario.UserId}", content);
            return response.IsSuccessStatusCode;
        }

        // Excluir usuário
        public async Task<bool> ExcluirUsuarioAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/usuarios/{userId}");
            return response.IsSuccessStatusCode;
        }

        // Adicionar usuário a uma Role
        public async Task<bool> AdicionarUsuarioRoleAsync(AdicionarUsuarioRoleDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/admin/adicionar-usuario-role", content);
            return response.IsSuccessStatusCode;
        }


        // Obter roles do usuário
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"api/admin/usuarios/{userId}/roles");
            if (!response.IsSuccessStatusCode) return new List<string>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Adicionar usuário a uma Role
        //public async Task<bool> AdicionarUsuarioRoleAsync(AdicionarUsuarioRoleDto dto)
        //{
        //    var json = JsonSerializer.Serialize(dto);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync("api/admin/adicionar-usuario-role", content);
        //    return response.IsSuccessStatusCode;
        //}

        // Listar todas as Claims
        //public async Task<List<ClaimDto>> GetAllClaimsAsync()
        //{
        //    var response = await _httpClient.GetAsync("api/admin/claims");
        //    if (!response.IsSuccessStatusCode) return null;

        //    var json = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<List<ClaimDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}

        //// Listar Claims de um usuário
        //public async Task<List<ClaimDto>> GetUserClaimsAsync(string userId)
        //{
        //    var response = await _httpClient.GetAsync($"api/admin/usuarios/{userId}/claims");
        //    if (!response.IsSuccessStatusCode) return null;

        //    var json = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<List<ClaimDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}

        // Excluir uma Claim de um usuário
        public async Task<bool> RemoveUserClaimAsync(string userId, string claimType, string claimValue)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/usuarios/{userId}/claims?claimType={claimType}&claimValue={claimValue}");
            return response.IsSuccessStatusCode;
        }

        // Ativar/Inativar usuário
        public async Task<bool> ToggleUserActivationAsync(string userId, bool ativar)
        {
            var response = await _httpClient.PutAsync($"api/admin/usuarios/{userId}/ativar-inativar?ativar={ativar}", null);
            return response.IsSuccessStatusCode;
        }

        // Atualizar senha do usuário
        public async Task<bool> UpdateUserPasswordAsync(AtualizarSenhaDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("api/admin/usuarios/senha", content);
            return response.IsSuccessStatusCode;
        }

        // Upload de imagem de perfil
        public async Task<bool> UploadProfileImageAsync(UploadImagemDto dto)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StreamContent(dto.Imagem.OpenReadStream()), "imagem", dto.Imagem.FileName);
            content.Add(new StringContent(dto.UserId), "userId");

            var response = await _httpClient.PostAsync("api/admin/usuarios/imagem", content);
            return response.IsSuccessStatusCode;
        }
    }
}
