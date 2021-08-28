using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Data;
using FFPark.Entity.Park;
namespace FFPark.Services.Park
{
    public class AdPositionServices : IAdPositionServices
    {
        private readonly IRepository<AdPosition> _baseRepository;
        public AdPositionServices(IRepository<AdPosition> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        public async Task<bool> AdPosition(AdPosition entity)
        {
            var result = await _baseRepository.InsertAsync(entity);
            return result;
        }

        public async Task<List<AdPosition>> GetAdPositions()
        {
            var result = await _baseRepository.GetAsyncWithColumn(t => 1 == 1, t => new AdPosition() { Id = t.Id, PositionName = t.PositionName });
            return result;
        }



        public async Task<bool> IsExistAdPositionName(string name)
        {
            var result = await _baseRepository.IsExistAsync(t => t.PositionName == name);
            return result;
        }
        public async Task<bool> IsExistAdPositionId(string id)
        {
            var result = await _baseRepository.IsExistAsync(t => t.Id == int.Parse(id));
            return result;
        }



    }
}
