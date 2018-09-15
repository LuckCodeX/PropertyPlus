using System;
using System.Collections.Generic;
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
    }
}