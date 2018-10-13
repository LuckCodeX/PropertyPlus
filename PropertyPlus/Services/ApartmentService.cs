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
        private GenericRepository<apartment> _apartmentRepository;
        public GenericRepository<apartment> ApartmentRepository
        {
            get
            {
                if (this._apartmentRepository == null)
                    this._apartmentRepository = new GenericRepository<apartment>(_context);
                return _apartmentRepository;
            }
        }

        private GenericRepository<apartment_content> _apartmentContentRepository;
        public GenericRepository<apartment_content> ApartmentContentRepository
        {
            get
            {
                if (this._apartmentContentRepository == null)
                    this._apartmentContentRepository = new GenericRepository<apartment_content>(_context);
                return _apartmentContentRepository;
            }
        }

        private GenericRepository<aparment_image> _apartmentImageRepository;
        public GenericRepository<aparment_image> ApartmentImageRepository
        {
            get
            {
                if (this._apartmentImageRepository == null)
                    this._apartmentImageRepository = new GenericRepository<aparment_image>(_context);
                return _apartmentImageRepository;
            }
        }

        private GenericRepository<apartment_facility> _apartmentFacilityRepository;
        public GenericRepository<apartment_facility> ApartmentFacilityRepository
        {
            get
            {
                if (this._apartmentFacilityRepository == null)
                    this._apartmentFacilityRepository = new GenericRepository<apartment_facility>(_context);
                return _apartmentFacilityRepository;
            }
        }

        public List<apartment> GetListApartmentByUserProfileId(int userProfileId)
        {
            return ApartmentRepository.FindBy(p => p.user_profile_owner_id == userProfileId).ToList();
        }

        public void SaveApartment(apartment apartment)
        {
            ApartmentRepository.Save(apartment);
        }

        public void SaveApartmentContent(apartment_content apartmentContent)
        {
            ApartmentContentRepository.Save(apartmentContent);
        }

        public void SaveApartmentImage(aparment_image apartmentImage)
        {
            ApartmentImageRepository.Save(apartmentImage);
        }

        public void SaveApartmentFacility(apartment_facility apartmentFacility)
        {
            ApartmentFacilityRepository.Save(apartmentFacility);
        }

        public List<apartment> SearchListApartment(FilterModel filter)
        {
            return ApartmentRepository
                .FindBy(p => p.status == 1 &&
                             (Equals(filter.Search, null) || p.city.Contains(filter.Search) || p.address.Contains(filter.Search) || (!Equals(p.project_id, null) && p.project.project_content.Any(q => q.name.Contains(filter.Search)))) &&
                             filter.FilterArea.MinValue <= p.area && p.area <= filter.FilterArea.MaxValue &&
                             filter.FilterPrice.MinValue <= p.price && p.price <= filter.FilterPrice.MaxValue &&
                             (filter.FilterRoom.NoBathRoom == 0 || filter.FilterRoom.NoBathRoom == p.no_bathroom) && (filter.FilterRoom.NoBedRoom == 0 || filter.FilterRoom.NoBedRoom == p.no_bedroom) &&
                             (filter.FilterFacility.FacilityIds.Count == 0 || filter.FilterFacility.FacilityIds.All(x => p.apartment_facility.Any(y => x == y.facility_id))))
                .OrderByDescending(p => p.apartment_id)
                .Include(p => p.aparment_image).Include(p => p.apartment_content)
                .Include(p => p.apartment_facility).Include(p => p.user_profile).Include(p => p.project.project_content).ToList();
        }

        public apartment GetActiveApartmentById(int id)
        {
            return ApartmentRepository.FindBy(p => p.status == 1 && p.apartment_id == id)
                .Include(p => p.aparment_image).Include(p => p.apartment_content)
                .Include(p => p.apartment_facility).Include(p => p.user_profile).Include(p => p.project.project_content).FirstOrDefault();
        }

        public List<apartment> SearchListApartmentByUserProfileId(int status, int userProfileId)
        {
            return ApartmentRepository.FindBy(p => p.user_profile_owner_id == userProfileId && (status == -1 || p.status == status)).OrderByDescending(p => p.apartment_id)
                .Include(p => p.aparment_image).Include(p => p.apartment_content)
                .Include(p => p.apartment_facility).Include(p => p.user_profile).Include(p => p.project.project_content).ToList();
        }

        public apartment GetApartmentById(int id)
        {
            return ApartmentRepository.FindBy(p => p.apartment_id == id && p.status != 2).OrderByDescending(p => p.apartment_id)
                .Include(p => p.aparment_image).Include(p => p.apartment_content)
                .Include(p => p.apartment_facility).Include(p => p.user_profile).Include(p => p.project.project_content).FirstOrDefault();
        }
    }
}