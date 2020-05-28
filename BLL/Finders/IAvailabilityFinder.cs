using BLL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Finders
{
    public interface IAvailabilityFinder
    {
        Availability GetAvailabilityById(int id);

        Availability GetAvailabilityByVariantId(int id, int size);

        Task<List<Availability>> GetAvailabilites();
    }
}
