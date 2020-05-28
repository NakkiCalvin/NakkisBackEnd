using BLL.DataAccess;
using BLL.Entities;
using BLL.Finders;
using BLL.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IRepository<Availability> _repository;
        readonly IAvailabilityFinder _finder;

        public AvailabilityService(IUnitOfWork unitOfWork, IRepository<Availability> repository, IAvailabilityFinder finder)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _finder = finder;
        }


        public async Task<List<Availability>> GetAvailabilites()
        {
            return await _finder.GetAvailabilites();
        }
        public Availability GetAvailabilityById(int id)
        {
            return _finder.GetAvailabilityById(id);
        }

        public Availability GetAvailabilityByVariantId(int id, int size)
        {
            return _finder.GetAvailabilityByVariantId(id, size);
        }

        public void Update(Availability availability)
        {
            if (availability == null) return;
            _repository.Update(availability);
            _unitOfWork.Commit();
        }
    }
}
