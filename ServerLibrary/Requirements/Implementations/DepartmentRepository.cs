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
    public class DepartmentRepository(AppDbContext appDbContext) : IGenericRepositoryInterface1<Department>
    {
        public async Task<GeneralResponse> DeleteId(int id)
        {
            var dep = await appDbContext.Departments.FindAsync(id);
            if (dep is null) return NotFound();

            appDbContext.Departments.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Department>> GetAll() => await appDbContext.Departments.ToListAsync();


        public async Task<Department> GetbyId(int id) => await appDbContext.Departments.FindAsync(id);



        public async Task<GeneralResponse> Insert(Department item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Department Alreday Added");
            appDbContext.Departments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var dep = await appDbContext.Departments.FindAsync(item.Id);
            if (dep is null) return NotFound();
            dep.Name = item.Name;
            await Commit();
            return Success();
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();
        public static GeneralResponse NotFound() => new(false, "Sorry Department not found");
        public static GeneralResponse Success() => new(false, "Process Completed");
       

        private async Task<bool> CheckName(string Name)
        {
            var item = await appDbContext.Departments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(Name.ToLower()));
            return item is null;
        }
    }
}

