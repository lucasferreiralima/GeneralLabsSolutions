using GeneralLabSolutions.Identidade.Dtos;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VelzonModerna.Services
{
    public class RoleAdminService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public RoleAdminService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }

        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var response = await _httpClient.GetAsync("api/role/roles");
            if (!response.IsSuccessStatusCode) return null;
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RoleDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> CreateRoleAsync(CriarRoleDto roleDto)
        {
            var json = JsonSerializer.Serialize(roleDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/role/criar-role", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var response = await _httpClient.DeleteAsync($"api/role/roles/{roleName}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"api/role/usuarios/{userId}/roles");
            if (!response.IsSuccessStatusCode) return null;
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> AddUserToRoleAsync(AdicionarUsuarioRoleDto dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/role/adicionar-usuario-role", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
        {
            var response = await _httpClient.GetAsync($"api/role/roles/{roleName}/usuarios");
            if (!response.IsSuccessStatusCode) return null;
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}

