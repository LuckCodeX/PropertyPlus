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
        private GenericRepository<career> _careerRepository;
        private GenericRepository<career_content> _careerContentRepository;
        public GenericRepository<career> CareerRepository
        {
            get
            {
                if (this._careerRepository == null)
                    this._careerRepository = new GenericRepository<career>(_context);
                return _careerRepository;
            }
        }
        public GenericRepository<career_content> CareerContentRepository
        {
            get
            {
                if (this._careerContentRepository == null)
                    this._careerContentRepository = new GenericRepository<career_content>(_context);
                return _careerContentRepository;
            }
        }

        public List<career> SearchCareerList(string search)
        {
            return CareerRepository.FindBy(p => p.career_content.Any(q => q.language == 0 && (Equals(search, null) || q.title.Contains(search)))).Include(p => p.career_content).OrderByDescending(p => p.career_id).ToList();
        }

        public CareerContentModel ConvertCareerContentToModel(career_content content)
        {
            return new CareerContentModel()
            {
                Id = content.career_content_id,
                Content = content.content,
                Language = content.language,
                Title = content.title
            };
        }

        public void DeleteCareer(int id)
        {
            CareerRepository.Delete(id);
        }

        public career GetCareerById(int id)
        {
            return CareerRepository.FindBy(p => p.career_id == id).Include(p => p.career_content).FirstOrDefault();
        }

        public void SaveCareer(career career)
        {
            CareerRepository.Save(career);
        }

        public career_content GetCareerContentById(int id)
        {
            return CareerContentRepository.FindBy(p => p.career_content_id == id).FirstOrDefault();
        }

        public void SaveCareerContent(career_content content)
        {
            CareerContentRepository.Save(content);
        }

        public List<career> SearchCareerList(int categoryId, int language, string search)
        {
            return CareerRepository
                .FindBy(p =>
                    p.category_id == categoryId && p.career_content.Any(q => q.language == language) &&
                    (Equals(search, null) || p.career_content.Any(q => q.title.Contains(search))))
                .Include(p => p.career_content).ToList();
        }
    }
}