using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PropertyPlus.Models;

namespace PropertyPlus.Services
{
    public partial class Service
    {
        private GenericRepository<blog> _blogRepository;
        private GenericRepository<blog_content> _blogContentRepository;
        public GenericRepository<blog> BlogRepository
        {
            get
            {
                if (this._blogRepository == null)
                    this._blogRepository = new GenericRepository<blog>(_context);
                return _blogRepository;
            }
        }
        public GenericRepository<blog_content> BlogContentRepository
        {
            get
            {
                if (this._blogContentRepository == null)
                    this._blogContentRepository = new GenericRepository<blog_content>(_context);
                return _blogContentRepository;
            }
        }

        public List<blog> GetAllBlog()
        {
            return BlogRepository.GetAll().Include(p => p.blog_content).OrderByDescending(p => p.blog_id).ToList();
        }

        public blog GetBlogById(int id)
        {
            return BlogRepository.FindBy(p => p.blog_id == id).Include(p => p.blog_content).FirstOrDefault();
        }

        public void SaveBlog(blog blog)
        {
            BlogRepository.Save(blog);
        }

        public blog_content GetBlogContentById(int id)
        {
            return BlogContentRepository.FindBy(p => p.blog_content_id == id).Include(p => p.blog).FirstOrDefault();
        }

        public void SaveBlogContent(blog_content content)
        {
            BlogContentRepository.Save(content);
        }

        public void DeleteBlog(int id)
        {
            BlogRepository.Delete(id);
        }

        public BlogContentModel ConvertBlogContentToModel(blog_content blogContent)
        {
            return new BlogContentModel()
            {
                Id = blogContent.blog_content_id,
                Content = blogContent.content,
                Title = blogContent.title,
                Description = blogContent.description
            };
        }
    }
}