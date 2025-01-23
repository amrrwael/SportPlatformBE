// IStadiumService.cs
using PlatformSport.Models.Dto;

public interface IStadiumService
{
    Task<List<StadiumDto>> GetAllStadiumsAsync(int sportId);
    Task<List<StadiumDto>> GetAllStadiumsAsync(); // Add this method
    Task<StadiumDto> GetStadiumByIdAsync(int stadiumId);
    Task<int> CreateStadiumAsync(StadiumDto stadiumDto);
    Task<bool> UpdateStadiumAsync(int stadiumId, StadiumDto stadiumDto);
    Task<bool> DeleteStadiumAsync(int stadiumId);
}
