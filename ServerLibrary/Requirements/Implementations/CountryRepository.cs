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
  public class CountryRepository(AppDbContext appDbContext) : IGenericRepositoryInterface1<Country>
    {

        public async Task<GeneralResponse> DeleteId(int id)
        {
            var dep = await appDbContext.Countries.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Countries.Remove(dep);
            await Commit();
            return Success();
        }
        public async Task<List<Country>> GetAll() => await appDbContext.Countries.ToListAsync();
        public async Task<Country> GetbyId(int id) => await appDbContext.Countries.FindAsync(id);




        public async Task<GeneralResponse> Insert(Country item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Country Alreaday Added");
            appDbContext.Countries.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Country item)
        {
            var dep = await appDbContext.Countries.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();
        public static GeneralResponse NotFound() => new(false, "Sorry Country not found");
        public static GeneralResponse Success() => new(false, "Process Completed");


        private async Task<bool> CheckName(string Name)
        {
            var item = await appDbContext.Countries.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(Name.ToLower()));
            return item is null;
        }
    }
}
