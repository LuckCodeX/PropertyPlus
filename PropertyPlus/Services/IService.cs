﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        List<user_profile> SearchUserProfile(string search);
        void DeleteAccount(int id);
        List<project> SearchProjectList(string search);
        ProjectContentModel ConvertProjectContentToModel(project_content model);
        project GetProjectById(int id);
        void SaveProject(project project);
        project_content GetProjectContentById(int id);
        void SaveProjectContent(project_content content);
        List<apartment> GetListApartmentByUserProfileId(int userProfileId);
        void SaveApartment(apartment apartment);
        void SaveApartmentContent(apartment_content apartmentContent);
        void SaveApartmentImage(aparment_image apartmentImage);
        void SaveApartmentFacility(apartment_facility apartmentFacility);
        List<project> GetAllProject();
        List<facility> GetAllFacility();
        FacilityContentModel ConvertFacilityContentToModel(facility_content model);
        facility GetFacilityById(int id);
        void SaveFacility(facility facility);
        facility_content GetFacilityContentById(int id);
        void SaveFacilityContent(facility_content content);
        List<apartment> SearchListApartment();
        apartment GetActiveApartmentById(int id);
        List<user_visit> GetListUserVisitByUserProfileId(int userProfileId);
        user_visit GetUserVisitById(int id);
        List<apartment> SearchListApartmentByUserProfileId(int status, int userProfileId);
    }
}
