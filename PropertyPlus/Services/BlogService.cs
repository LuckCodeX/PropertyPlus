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
        public GenericRepository<blog> BlogRepository
        {
            get
            {
                if (this._blogRepository == null)
                    this._blogRepository = new GenericRepository<blog>(_context);
                return _blogRepository;
            }
        }

        public IEnumerable GetAllBlog()
        {
            return BlogRepository.GetAll().Include(p => p.blog_content);
        }
    }
}