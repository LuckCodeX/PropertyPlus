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
        private GenericRepository<project> _projectRepository;
        public GenericRepository<project> ProjectRepository
        {
            get
            {
                if (this._projectRepository == null)
                    this._projectRepository = new GenericRepository<project>(_context);
                return _projectRepository;
            }
        }

        private GenericRepository<project_content> _projectContentRepository;
        public GenericRepository<project_content> ProjectContentRepository
        {
            get
            {
                if (this._projectContentRepository == null)
                    this._projectContentRepository = new GenericRepository<project_content>(_context);
                return _projectContentRepository;
            }
        }

        private GenericRepository<project_overview> _projectOverviewRepository;
        public GenericRepository<project_overview> ProjectOverviewRepository
        {
            get
            {
                if (this._projectOverviewRepository == null)
                    this._projectOverviewRepository = new GenericRepository<project_overview>(_context);
                return _projectOverviewRepository;
            }
        }

        private GenericRepository<project_facility> _projectFacilityRepository;
        public GenericRepository<project_facility> ProjectFacilityRepository
        {
            get
            {
                if (this._projectFacilityRepository == null)
                    this._projectFacilityRepository = new GenericRepository<project_facility>(_context);
                return _projectFacilityRepository;
            }
        }

        public List<project> SearchProjectList(string search)
        {
            return ProjectRepository.FindBy(p => p.status == 1 && (Equals(search, null) || p.project_content.Any(q => q.name.Contains(search)))).Include(p => p.project_content).ToList();
        }

        public ProjectContentModel ConvertProjectContentToModel(project_content model)
        {
            return new ProjectContentModel()
            {
                Id = model.project_content_id,
                Name = model.name,
                Language = model.language,
                Description = model.description
            };
        }

        public project GetProjectById(int id)
        {
            return ProjectRepository.FindBy(p => p.project_id == id && p.status == 1).Include(p => p.project_content).Include(p => p.project_overview).Include(p => p.project_facility.Select(q => q.facility.facility_content)).FirstOrDefault();
        }

        public void SaveProject(project project)
        {
            ProjectRepository.Save(project);
        }

        public project_content GetProjectContentById(int id)
        {
            return ProjectContentRepository.FindBy(p => p.project_content_id == id).FirstOrDefault();
        }

        public void SaveProjectContent(project_content content)
        {
            ProjectContentRepository.Save(content);
        }

        public List<project> GetAllProject()
        {
            return ProjectRepository.FindBy(p => p.status == 1).ToList();
        }

        public project_overview GetProjectOverviewById(int id)
        {
            return ProjectOverviewRepository.FindBy(p => p.project_overview_id == id).FirstOrDefault();
        }

        public void SaveProjectOverview(project_overview content)
        {
            ProjectOverviewRepository.Save(content);
        }

        public void DeleteAllProjectFacilities(List<project_facility> projectFacility)
        {
            foreach (var item in projectFacility)
            {
                ProjectFacilityRepository.Delete(item);
            }
        }

        public void SaveProjectFacility(project_facility fac)
        {
            ProjectFacilityRepository.Save(fac);
        }
    }
}