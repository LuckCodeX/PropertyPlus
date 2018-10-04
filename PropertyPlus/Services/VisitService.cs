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


        public user_visit GetUserVisitByUserProfileIdAndApartmentId(int userProfileId, int apartmentId)
        {
            return UserVisitRepository.FindBy(p => p.user_profile_id == userProfileId && p.apartment_id == apartmentId).FirstOrDefault();
        }

        public void SaveUserVisit(user_visit userVisit)
        {
            UserVisitRepository.Save(userVisit);
        }

        public void DeleteUserVisit(user_visit userVisit)
        {
            UserVisitRepository.Delete(userVisit);
        }

        public List<apartment> GetListVisitApartmentByUserProfileId(int userProfileId)
        {
            return ApartmentRepository.FindBy(p => p.user_visit.Any(q => q.user_profile_id == userProfileId)).ToList();
        }

        public List<user_visit> GetListUserVisitByUserProfileId(int userProfileId)
        {
            return UserVisitRepository.FindBy(p => p.user_profile_id == userProfileId).Include(p => p.apartment.apartment_content)
                .ToList();
        }

        public user_visit GetUserVisitById(int id)
        {
            return UserVisitRepository.FindBy(p => p.user_visit_id == id).FirstOrDefault();
        }
    }
}