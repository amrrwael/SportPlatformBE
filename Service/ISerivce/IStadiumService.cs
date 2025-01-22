// IStadiumService.cs
using PlatformSport.Models.Dto;

public interface IStadiumService
{
    Task<List<StadiumDto>> GetAllStadiumsAsync(int sportId);
    Task<StadiumDto> GetStadiumByIdAsync(int stadiumId);
    Task<int> CreateStadiumAsync(StadiumDto stadiumDto);
    Task<bool> UpdateStadiumAsync(int stadiumId, StadiumDto stadiumDto); // Add this method
    Task<bool> DeleteStadiumAsync(int stadiumId); // Add this method
}
