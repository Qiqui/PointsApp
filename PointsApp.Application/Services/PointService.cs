using PointsApp.Application.DTOs;
using PointsApp.Application.Helpers;
using PointsApp.Application.Interfaces;
using PointsApp.Domain.Entities;
using PointsApp.Domain.Interfaces;

namespace PointsApp.Application.Services
{
    public class PointService : IPointService
    {
        private readonly IPointsRepository _pointsRepository;

        public PointService(IPointsRepository pointsRepository)
        {
            _pointsRepository = pointsRepository;
        }


        public async Task<PointDto?> GetByIdAsync(int id)
        {
            var point = await _pointsRepository.GetByIdAsync(id);
            var pointDto = point?.ToPointDto();

            return pointDto;
        }
        public async Task<List<PointDto>> GetAllAsync()
        {
            var points = await _pointsRepository.GetAllAsync();
            var pointsDto = points.ToPointsDto();
            return pointsDto;
        }

        public async Task<PointDto> AddAsync(PointDto pointDto)
        {
            var point = pointDto.ToPoint();
            point = await _pointsRepository.AddAsync(point);
            pointDto = point.ToPointDto();

            return pointDto;
        }

        public async Task UpdateAsync(PointDto pointDto)
        {
            var point = pointDto.ToPoint();
            await _pointsRepository.UpdateAsync(point);
        }

        public async Task DeleteAsync(int id)
        {
            await _pointsRepository.DeleteAsync(id);
        }

        public async Task MoveAsync(int id, int x, int y)
        {
            var point = await _pointsRepository.GetByIdAsync(id);
            if (point == null)
                throw new KeyNotFoundException($"Point id: {id} is not found");

            point.X = x;
            point.Y = y;

            await _pointsRepository.UpdateAsync(point);
        }
    }
}
