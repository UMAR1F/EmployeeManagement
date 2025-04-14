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
    public class GeneralDepartmentRepository (AppDbContext appDbContext): IGenericRepositoryInterface1<Generaldepartment>
    {
        public async Task<GeneralResponse> DeleteId(int id)
        {
           var dep = await appDbContext.Generaldepartment.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Generaldepartment.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Generaldepartment>> GetAll() => await appDbContext.Generaldepartment.ToListAsync();


        public async Task<Generaldepartment> GetbyId(int id) => await appDbContext.Generaldepartment.FindAsync(id);



        public async Task<GeneralResponse> Insert(Generaldepartment item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false,"Department Alreday Added");
            appDbContext.Generaldepartment.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Generaldepartment item)
        {
           var dep = await appDbContext.Generaldepartment.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }


        public static GeneralResponse NotFound() => new(false, "Sorry Department not found");
        public static GeneralResponse Success() => new(false, "Process Completed");
        private async Task Commit() => await appDbContext.SaveChangesAsync();

        private async Task <bool> CheckName(string Name)
        {
            var item=await appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(Name.ToLower()));
            return item is null;
        }
    }
}
