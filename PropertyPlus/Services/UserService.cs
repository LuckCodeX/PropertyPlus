using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PropertyPlus.Helper;
using PropertyPlus.Models;

namespace PropertyPlus.Services
{
    public partial class Service
    {
        private GenericRepository<user_profile> _userProfileRepository;
        public GenericRepository<user_profile> UserProfileRepository
        {
            get
            {
                if (this._userProfileRepository == null)
                    this._userProfileRepository = new GenericRepository<user_profile>(_context);
                return _userProfileRepository;
            }
        }

        private GenericRepository<user_account> _userAccountRepository;
        public GenericRepository<user_account> UserAccountRepository
        {
            get
            {
                if (this._userAccountRepository == null)
                    this._userAccountRepository = new GenericRepository<user_account>(_context);
                return _userAccountRepository;
            }
        }

        public user_account Login(UserAccountModel model)
        {
            var password = Encrypt.EncodePassword(model.Password);
            return UserAccountRepository.FindBy(p => p.email == model.Email && p.password == password).FirstOrDefault();
        }

        public user_profile GetUserProfileByEmail(string email)
        {
            return UserProfileRepository.FindBy(p => p.email == email).FirstOrDefault();
        }

        public void SaveUserProfile(user_profile userProfile)
        {
            UserProfileRepository.Save(userProfile);
        }

        public void SaveUserAccount(user_account userAcc)
        {
            UserAccountRepository.Save(userAcc);
        }

        public user_profile GetUserProfileById(int id)
        {
            return UserProfileRepository.FindBy(p => p.user_profile_id == id).FirstOrDefault();
        }

        public user_profile GetActiveUserProfileById(int id)
        {
            return UserProfileRepository.FindBy(p => p.user_profile_id == id && p.status == 1).FirstOrDefault();
        }

        public user_account GetUserAccountByUserProfileId(int userProfileId)
        {
            return UserAccountRepository.FindBy(p => p.user_profile_id == userProfileId).FirstOrDefault();
        }

        public List<user_profile> SearchUserProfile(string search)
        {
            return UserProfileRepository.FindBy(p => p.status == 1 && (Equals(search, null) || (p.first_name + " " + p.last_name).Contains(search) || p.email.Contains(search))).OrderByDescending(p => p.user_profile_id).ToList();
        }

        public void DeleteAccount(int id)
        {
            UserProfileRepository.Delete(id);
        }
    }
}