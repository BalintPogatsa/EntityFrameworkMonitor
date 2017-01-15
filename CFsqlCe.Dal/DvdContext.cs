using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using CFSqlCe.Model;

namespace CFSqlCe.Dal
{
    public class DvdContext : DbContext
    {
        static DvdContext()
        {
            // Not initialize database
            //  Database.SetInitializer<ProjectDatabase>(null);
            // Database initialize
            Database.SetInitializer<DvdContext>(new DbInitializer());
            using (DvdContext db = new DvdContext())
                db.Database.Initialize(false);
        }

        public DbSet<Actor> Actors { get; set; }
        public DbSet<FilmActorRole> FilmActorRoles { get; set; }
        public DbSet<FilmGenere> FilmGeneres { get; set; }
        public DbSet<FilmTitle> FilmTitles { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    class DbInitializer : DropCreateDatabaseAlways<DvdContext>
    {
        protected override void Seed(DvdContext context)
        {
            // insert some file generes
            context.FilmGeneres.Add(new FilmGenere()
            {
                Name = "Action"
            });
            context.FilmGeneres.Add(new FilmGenere()
            {
                Name = "SciFi"
            });
            context.FilmGeneres.Add(new FilmGenere()
            {
                Name = "Comedy"
            });
            context.FilmGeneres.Add(new FilmGenere()
            {
                Name = "Romance"
            });
            // some roles
            context.Roles.Add(new Role()
            {
                Name = "Lead"
            });
            context.Roles.Add(new Role()
            {
                Name = "Supporting"
            });
            context.Roles.Add(new Role()
            {
                Name = "Background"
            });
            // some actors
            context.Actors.Add(new Actor()
            {
                Name ="Chris",
                Surname ="Pine",
                Note = "Born in Los Angeles, California"
            });
            context.Actors.Add(new Actor()
            {
                Name = "Zachary",
                Surname = "Quinto",
                Note = "Zachary Quinto graduated from Central Catholic High School in Pittsburgh"
            });
            context.Actors.Add(new Actor()
            {
                Name = "Tom",
                Surname = "Cruise"
            });
            // producers
            context.Producers.Add(new Producer()
            {
                FullName = "J.J. Abrams",
                Email = "jj.adams@producer.com",
                Note = "Born: Jeffrey Jacob Abrams"
            });
            base.Seed(context);
        }
    }
}
