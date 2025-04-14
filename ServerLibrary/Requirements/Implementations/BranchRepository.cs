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
    public class BranchRepository(AppDbContext appDbContext) : IGenericRepositoryInterface1<Branch>
    {
        public async Task<GeneralResponse> DeleteId(int id)
        {
            var dep = await appDbContext.Branches.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Branches.Remove(dep);
            await Commit();
            return Success();
        }
        public async Task<List<Branch>> GetAll() => await appDbContext.Branches.ToListAsync();
        public async Task<Branch> GetbyId(int id) => await appDbContext.Branches.FindAsync(id);




        public async Task<GeneralResponse> Insert(Branch item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Branch Alreday Added");
            appDbContext.Branches.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Branch item)
        {
            var dep = await appDbContext.Branches.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

            private async Task Commit() => await appDbContext.SaveChangesAsync();
        public static GeneralResponse NotFound() => new(false, "Sorry Branch not found");
        public static GeneralResponse Success() => new(false, "Process Completed");


        private async Task<bool> CheckName(string Name)
        {
            var item = await appDbContext.Branches.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(Name.ToLower()));
            return item is null;
        }
    }
}
