using APIPCOS_CRM.Models;

namespace APIPCOS_CRM.Repository
{
    public interface IRepository<T>
    {
        Task<MsgModel> Create(T model);
        Task<MsgModel> Update(T model);
        Task<MsgModel> Delete(int id);
        Task<T> FindByID(int id);
        Task<List<T>> GetAll();
    }
}
