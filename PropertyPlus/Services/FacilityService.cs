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
        private GenericRepository<facility> _facilityRepository;
        private GenericRepository<facility_content> _facilityContentRepository;
        public GenericRepository<facility> FacilityRepository
        {
            get
            {
                if (this._facilityRepository == null)
                    this._facilityRepository = new GenericRepository<facility>(_context);
                return _facilityRepository;
            }
        }
        public GenericRepository<facility_content> FacilityContentRepository
        {
            get
            {
                if (this._facilityContentRepository == null)
                    this._facilityContentRepository = new GenericRepository<facility_content>(_context);
                return _facilityContentRepository;
            }
        }

        public List<facility> GetAllFacility()
        {
            return FacilityRepository.GetAll().Include(p => p.facility_content).ToList();
        }

        public FacilityContentModel ConvertFacilityContentToModel(facility_content model)
        {
            return new FacilityContentModel()
            {
                Id = model.facility_content_id,
                Name = model.name,
                Language = model.language
            };
        }

        public facility GetFacilityById(int id)
        {
            return FacilityRepository.FindBy(p => p.facility_id == id && p.status == 1).Include(p => p.facility_content).FirstOrDefault();
        }

        public void SaveFacility(facility facility)
        {
            FacilityRepository.Save(facility);
        }

        public facility_content GetFacilityContentById(int id)
        {
            return FacilityContentRepository.FindBy(p => p.facility_content_id == id).FirstOrDefault();
        }

        public void SaveFacilityContent(facility_content content)
        {
            FacilityContentRepository.Save(content);
        }

        public List<facility> GetAllFacilities()
        {
            return FacilityRepository.FindBy(p => p.status == 1).Include(p => p.facility_content).ToList();
        }
    }
}