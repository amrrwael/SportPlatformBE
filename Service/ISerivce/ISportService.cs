using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformSport.Models.Dto;

namespace PlatformSport.Services
{
    public interface ISportService
    {
        Task<List<SportDto>> GetAllSportsAsync();
        Task<SportDto> GetSportByIdAsync(int sportId);
        Task<int> CreateSportAsync(SportDto sportDto);
    }
}
