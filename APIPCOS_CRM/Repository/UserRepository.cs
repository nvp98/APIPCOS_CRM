using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Repository
{
    public class UserRepository : IRepository<UserModel>
    {
        private PLCOS_Context context;

        public UserRepository(PLCOS_Context context)
        {
            this.context = context;
        }
        public async Task<MsgModel> Create(UserModel model)
        {
            MsgModel msg;
            try
            {
                await context.Database.ExecuteSqlRawAsync($"sp_users_api_inset '{model.username}','{model.password}',{1}");
                msg = new MsgModel() { status = 1, msg = "Thêm mới nhân viên thành công" };
                return msg;
            }
            catch (Exception ex)
            {
                msg = new MsgModel() { status = 0, msg = "Đã có lỗi xãy ra:" + ex.Message };
                return msg;
            }
        }

        public Task<MsgModel> Delete(int id)
        {
            throw new NotImplementedException();

        }

        public Task<UserModel> FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Unique(string check)
        {
            var res = await context.USER.Where(x => x.username == check).FirstOrDefaultAsync();
            return (res == null) ? true : false;
        }
        public async Task<MsgModel> Update(UserModel model)
        {
            MsgModel msg;
            try
            {
                await context.Database.ExecuteSqlRawAsync($"sp_users_api_update '{model.username}','{model.password}',{model.status}");
                msg = new MsgModel() { status = 1, msg = "Cập nhật thông tin nhân viên thành công" };
                return msg;
            }
            catch (Exception ex)
            {
                msg = new MsgModel() { status = 0, msg = "Đã có lỗi xãy ra:" + ex.Message };
                return msg;
            }
        }

        public UserModel Authenticate(string username, string password)
        {
            var user = (from a in context.USER

                        where a.username == username
                        select new UserModel()
                        {
                            username = a.username,
                            password = a.password,
                            status = a.status ?? default
                        }).FirstOrDefault();
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(password, user.password))
                {
                    user = null;
                }
            }

            return user;
        }
    }
}
