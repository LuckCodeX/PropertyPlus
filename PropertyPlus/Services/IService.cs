using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyPlus.Models;

namespace PropertyPlus.Services
{
    public interface IService
    {
        admin GetAdminByToken(TokenModel token);
        admin LoginAdmin(AdminModel model);
        IEnumerable GetAllBlog();
        void Dispose();
    }
}
