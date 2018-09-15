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
        List<blog> GetAllBlog();
        void Dispose();
        blog GetBlogById(int id);
        void SaveBlog(blog blog);
        blog_content GetBlogContentById(int id);
        void SaveBlogContent(blog_content content);
        void DeleteBlog(int id);
        BlogContentModel ConvertBlogContentToModel(blog_content blogContent);
        List<blog> SearchBlogList(int type, int language, string search);
        List<slide> GetListSlideByType(int type);
        slide GetSlideById(int id);
        void SaveSlide(slide slide);
        slide GetRandomSlideByType(int type);
        user_account Login(UserAccountModel model);
        user_profile GetUserProfileByEmail(string email);
        void SaveUserProfile(user_profile userProfile);
        void SaveUserAccount(user_account userAcc);
        user_profile GetUserProfileById(int id);
        user_profile GetActiveUserProfileById(int id);
        string SaveImage(string path, string imageName, string image);
        user_account GetUserAccountByUserProfileId(int userProfileId);
        user_visit GetUserVisitByUserProfileIdAndApartmentId(int userProfileId, int apartmentId);
        void SaveUserVisit(user_visit userVisit);
        void DeleteUserVisit(user_visit userVisit);
        List<apartment> GetListVisitApartmentByUserProfileId(int userProfileId);
    }
}
