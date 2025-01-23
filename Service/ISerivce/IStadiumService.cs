// ISportService.cs
using PlatformSport.Models.Dto;

public interface ISportService
{
    Task<List<SportDto>> GetAllSportsAsync();
    Task<SportDto> GetSportByIdAsync(int sportId);
    Task<int> CreateSportAsync(SportDto sportDto);
    Task<bool> UpdateSportAsync(int sportId, SportDto sportDto); // Add this method
    Task<bool> DeleteSportAsync(int sportId); // Add this method
}
