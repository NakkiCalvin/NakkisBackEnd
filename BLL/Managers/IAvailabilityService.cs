using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers
{
    public interface IAvailabilityService
    {
        Availability GetAvailabilityById(int id);
        Availability GetAvailabilityByVariantId(int id, int size);

        void Update(Availability availability);
        Task<List<Availability>> GetAvailabilites();
    }
}
