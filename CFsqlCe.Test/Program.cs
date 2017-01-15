using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFSqlCe.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Dal.DvdContext db = new Dal.DvdContext())
            {
                db.Database.Log = (s) => System.Diagnostics.Trace.WriteLine(s);

                // film generes
                Model.FilmGenere actionGenere = db.FilmGeneres.Where(g => g.Name == "Action").SingleOrDefault();
                Model.FilmGenere scifiGenere = db.FilmGeneres.Where(g => g.Name == "SciFi").SingleOrDefault();
                // find the producer
                Model.Producer jjAbrams = db.Producers.Where(p => p.FullName == "J.J. Abrams").SingleOrDefault();
                // we found the producer
                if (jjAbrams != null)
                {
                    // add some films to that producer
                    Model.FilmTitle film1 = new Model.FilmTitle()
                    {
                        Title = "Mission: Impossible III",
                        ReleaseYear = 2006,
                        Duration = 126,
                        Story = "Ethan Hunt comes face to face with a dangerous and ...",
                        FilmGenere = actionGenere
                    };
                    film1.Producers = new List<Model.Producer>();
                    film1.Producers.Add(jjAbrams);
                    db.FilmTitles.Add(film1);
                    Model.FilmTitle film2 = new Model.FilmTitle()
                    {
                        Title = "Star Trek Into Darkness",
                        ReleaseYear = 2013,
                        Duration = 132,
                        Story = "After the crew of the Enterprise find an unstoppable force  ...",
                        FilmGenere = scifiGenere
                    };
                    film2.Producers = new List<Model.Producer>();
                    film2.Producers.Add(jjAbrams);
                    db.FilmTitles.Add(film2);

                    // add some film roles
                    Model.Role leadRole = db.Roles.Where(r => r.Name == "Lead").SingleOrDefault();
                    Model.Role supportingRole = db.Roles.Where(r => r.Name == "Supporting").SingleOrDefault();
                    // load the actors
                    Model.Actor tom = db.Actors.Where(a => a.Surname == "Cruise").SingleOrDefault();
                    Model.Actor quinto = db.Actors.Where(a => a.Surname == "Quinto").SingleOrDefault();
                    Model.Actor pine = db.Actors.Where(a => a.Surname == "Pine").SingleOrDefault();
                    // add filmroles
                    db.FilmActorRoles.Add(new Model.FilmActorRole()
                    {
                        Actor = tom,
                        Role = leadRole,
                        FilmTitle = film1,
                        Character = "Ethan",
                        Description = "Ethan Hunt comes face to face with a dangerous and sadistic arms dealer while trying to keep his identity secret in order to protect his girlfriend."
                    });
                    db.FilmActorRoles.Add(new Model.FilmActorRole()
                    {
                        Actor = pine,
                        Role = leadRole,
                        FilmTitle = film2,
                        Character = "Kirk",
                        Description = "Captain Kirk"
                    });
                    db.FilmActorRoles.Add(new Model.FilmActorRole()
                    {
                        Actor = quinto,
                        Role = supportingRole,
                        FilmTitle = film2,
                        Character = "Spock",
                        Description = "Spock was born in 2230, in the city of Shi'Kahr on the planet Vulcan"
                    });
                }
                // save data to db
                db.SaveChanges();
            }
        }
    }
}
