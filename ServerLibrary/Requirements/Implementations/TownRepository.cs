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
   public class TownRepository(AppDbContext appDbContext) : IGenericRepositoryInterface1<Town>
    {
        public async Task<GeneralResponse> DeleteId(int id)
        {
            var dep = await appDbContext.Towns.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Towns.Remove(dep);
            await Commit();
            return Success();
        }
        public async Task<List<Town>> GetAll() => await appDbContext.Towns.ToListAsync();
        public async Task<Town> GetbyId(int id) => await appDbContext.Towns.FindAsync(id);




        public async Task<GeneralResponse> Insert(Town item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false,"Town Alreaday Added");
            appDbContext.Towns.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var dep = await appDbContext.Towns.FindAsync(item.Id);
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
