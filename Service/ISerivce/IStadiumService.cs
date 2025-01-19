using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformSport.Models.Dto;

namespace PlatformSport.Services
{
    public interface IStadiumService
    {
        Task<List<StadiumDto>> GetAllStadiumsAsync(int sportId);
        Task<StadiumDto> GetStadiumByIdAsync(int stadiumId);
        Task<int> CreateStadiumAsync(StadiumDto stadiumDto);
    }
}
