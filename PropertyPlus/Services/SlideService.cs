using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PropertyPlus.Models;

namespace PropertyPlus.Services
{
    public partial class Service
    {
        private GenericRepository<slide> _slideRepository;
        public GenericRepository<slide> SlideRepository
        {
            get
            {
                if (this._slideRepository == null)
                    this._slideRepository = new GenericRepository<slide>(_context);
                return _slideRepository;
            }
        }

        public List<slide> GetListSlideByType(int type)
        {
            return SlideRepository.FindBy(p => p.type == type).ToList();
        }

        public slide GetSlideById(int id)
        {
            return SlideRepository.FindBy(p => p.slide_id == id).FirstOrDefault();
        }

        public void SaveSlide(slide slide)
        {
            SlideRepository.Save(slide);
        }

        public slide GetRandomSlideByType(int type)
        {
            return SlideRepository.FindBy(p => p.type == type).OrderBy(p => Guid.NewGuid()).FirstOrDefault();
        }
    }
}