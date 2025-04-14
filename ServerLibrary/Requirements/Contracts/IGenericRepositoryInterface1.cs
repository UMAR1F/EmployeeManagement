using BaseLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Requirements.Contracts
{
   public interface IGenericRepositoryInterface1<T>
    {
        Task<List<T>> GetAll();
        Task <T> GetbyId(int id);
        Task<GeneralResponse> Insert(T item);
        Task<GeneralResponse> Update(T item);
        Task<GeneralResponse> DeleteId(int id);


    }
}
