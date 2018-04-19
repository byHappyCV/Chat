using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatWhitAuth.Interfaces;
using ChatWhitAuth.Models;
using ChatWhitAuth.Repositories;
using Ninject.Modules;

namespace ChatWhitAuth.IoC
{
    public class NinjectRegistrations : NinjectModule
    {

        public override void Load()
        {
            Bind<IUnitOfWork>()
                .To<UnitOfWork>()
                .InSingletonScope();
            Bind(typeof(IBaseRepository<>))
                .To(typeof(BaseRepository<>));
            Bind<ApplicationDbContext>()
                .To<ApplicationDbContext>();
        }
    }
}