using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PropertyPlus.Models;

namespace PropertyPlus.Services
{
    public partial class Service
    {
        private GenericRepository<user_visit> _userVisitRepository;
        public GenericRepository<user_visit> UserVisitRepository
        {
            get
            {
                if (this._userVisitRepository == null)
                    this._userVisitRepository = new GenericRepository<user_visit>(_context);
                return _userVisitRepository;
            }
        }

        private GenericRepository<user_visit_item> _userVisitItemRepository;
        public GenericRepository<user_visit_item> UserVisitItemRepository
        {
            get
            {
                if (this._userVisitItemRepository == null)
                    this._userVisitItemRepository = new GenericRepository<user_visit_item>(_context);
                return _userVisitItemRepository;
            }
        }

        public void SaveUserVisit(user_visit userVisit)
        {
            UserVisitRepository.Save(userVisit);
        }

        public void DeleteUserVisit(user_visit userVisit)
        {
            UserVisitRepository.Delete(userVisit);
        }

        public List<user_visit> GetListUserVisitByUserProfileId(int userProfileId)
        {
            return UserVisitRepository.FindBy(p => p.user_profile_id == userProfileId).Include(p => p.user_visit_item).ToList();
        }

        public user_visit GetUserVisitById(int id)
        {
            return UserVisitRepository.FindBy(p => p.user_visit_id == id).FirstOrDefault();
        }

        public user_visit_item GetUserVisitItemByUserProfileIdAndApartmentId(int userProfileId, int apartmentId)
        {
            return UserVisitItemRepository
                .FindBy(p => p.user_visit.user_profile_id == userProfileId && p.apartment_id == apartmentId)
                .FirstOrDefault();
        }

        public void SaveUserVisitItem(user_visit_item visitItem)
        {
            UserVisitItemRepository.Save(visitItem);
        }

        public List<user_visit_item> GetListUserVisitItemByUserProfileId(int userProfileId)
        {
            return UserVisitItemRepository.FindBy(p => p.user_visit.user_profile_id == userProfileId).ToList();
        }

        public user_visit_item GetUserVisitItemByIdAndUserProfileId(int id, int userProfileId)
        {
            return UserVisitItemRepository.FindBy(p => p.user_visit_item_id == id && p.user_visit.user_profile_id == userProfileId).FirstOrDefault();
        }

        public void DeleteUserVisitItem(user_visit_item userVisit)
        {
            UserVisitItemRepository.Delete(userVisit);
        }
    }
}