using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Requirements.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Requirements.Implementations
{
  public class CityRepository(AppDbContext appDbContext) : IGenericRepositoryInterface1<City>
    {
        public async Task<GeneralResponse> DeleteId(int id)
        {
            var dep = await appDbContext.Cities.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Cities.Remove(dep);
            await Commit();
            return Success();
        }
        public async Task<List<City>> GetAll() => await appDbContext.Cities.ToListAsync();
        public async Task<City> GetbyId(int id) => await appDbContext.Cities.FindAsync(id);




        public async Task<GeneralResponse> Insert(City item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "City Alreaday Added");
            appDbContext.Cities.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City item)
        {
            var dep = await appDbContext.Cities.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();
        public static GeneralResponse NotFound() => new(false, "Sorry City not found");
        public static GeneralResponse Success() => new(false, "Process Completed");


        private async Task<bool> CheckName(string Name)
        {
            var item = await appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(Name.ToLower()));
            return item is null;
        }
    }
}
