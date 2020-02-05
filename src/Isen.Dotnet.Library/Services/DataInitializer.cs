using System;
using System.Collections.Generic;
using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Microsoft.Extensions.Logging;

namespace Isen.Dotnet.Library.Services
{
    public class DataInitializer : IDataInitializer
    {
        private List<string> _firstNames => new List<string>
        {
            "Sang", 
            "Anne",
            "Boris",
            "Pierre",
            "Laura",
            "Hadrien",
            "Camille",
            "Louis",
            "Alicia"
        };
        private List<string> _lastNames => new List<string>
        {
            "Schuck",
            "Arbousset",
            "Lopasso",
            "Jubert",
            "Lebrun",
            "Dutaud",
            "Sarrazin",
            "Vu Dinh"
        };

        // Générateur aléatoire
        private readonly Random _random;

        // DI de ApplicationDbContext
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DataInitializer> _logger;
        public DataInitializer(
            ILogger<DataInitializer> logger,
            ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _random = new Random();
        }

        // Générateur de prénom
        private string RandomFirstName => 
            _firstNames[_random.Next(_firstNames.Count)];
        // Générateur de nom
        private string RandomLastName => 
            _lastNames[_random.Next(_lastNames.Count)];

        // Générateur de ville
        private City RandomCity
        {
            get
            {
                var cities = _context.CityCollection.ToList();
                return cities[_random.Next(cities.Count)];
            }
        }
        private Role RandomRole
        {
            get{
                var roles = _context.RoleCollection.ToList();
                return roles[_random.Next(roles.Count)];
            }
        }
  private Service RandomService
        {
            get
            {
                var services = _context.ServiceCollection.ToList();
                return services[_random.Next(services.Count)];
            }
        }
        
        // Générateur de date
        private DateTime RandomDate =>
            new DateTime(_random.Next(1980, 2010), 1, 1)
                .AddDays(_random.Next(0, 365));
        // Générateur de personne
        private Person RandomPerson {
            get{
                var first =RandomFirstName;
                var last =RandomLastName;
                var role = RandomRole;
               var person = new Person()               
                {
                    FirstName = first,
                    LastName = last,
                    DateOfBirth = RandomDate,
                    BirthCity = RandomCity,
                    ResidenceCity = RandomCity,
                    Service=RandomService,
                    Mail=first + "." + last + "@gmail.com",
                    Tel="06" + _random.Next(100000000),
                    RolePersonnes = new List<RolePersonne>()
                };

                person.RolePersonnes.Add(new RolePersonne()
                {
                    Person = person,
                    PersonId = person.Id,
                    Role = role,
                    RoleId = role.Id
                });
                return person;
            }
        }

      
        // Générateur de personnes
        public List<Person> GetPersons(int size)
        {
            var persons = new List<Person>();
            for(var i = 0 ; i < size ; i++)
            {
                persons.Add(RandomPerson);
            }
            return persons;
        }

        public List<City> GetCities()
        {
            return new List<City>
            {
                new City { Name = "Toulon", Zip = "83000", Lat = 43.1363557, Lon = 5.8984116},
                new City { Name = "Nice", Zip = "06000", Lat = 43.7031691, Lon = 7.1827772},
                new City { Name = "Marseille", Zip = "13000", Lat = 43.2803051, Lon = 5.2404126},
                new City { Name = "Lyon", Zip = "69000", Lat = 45.7579341, Lon = 4.7650812},
                new City { Name = "Bordeaux", Zip = "33000", Lat = 44.8637065, Lon = -0.6561808},
                new City { Name = "Toulouse", Zip = "31000", Lat = 43.6006786, Lon = 1.3628011},
                new City { Name = "Lille", Zip = "59000", Lat = 50.6310623, Lon = 3.0121411}
            };
        }
        public List<Service> GetServices()
        {
            return new List<Service>
            {
                new Service { Name = "Direction"},
                new Service { Name = "Administration"},
                new Service { Name = "Finance"},
                new Service { Name = "Production"},
                new Service { Name = "IT"}
                
            };
        }
        public List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role { Name = "utilisateur"},
                new Role { Name = "manager"},
                new Role { Name = "admin"},
                new Role { Name = "superAdmin"}
            };
        }

        public void DropDatabase()
        {
            _logger.LogWarning("Dropping database");
            _context.Database.EnsureDeleted();
        }
            

        public void CreateDatabase()
        {
            _logger.LogWarning("Creating database");
            _context.Database.EnsureCreated();
        }

        public void AddPersons()
        {
            _logger.LogWarning("Adding persons...");
            // S'il y a déjà des personnes dans la base -> ne rien faire
            if (_context.PersonCollection.Any()) return;
            // Générer des personnes
            var persons = GetPersons(50);
            // Les ajouter au contexte
            _context.AddRange(persons);
            // Sauvegarder le contexte
            _context.SaveChanges();
        }

        public void AddCities()
        {
            _logger.LogWarning("Adding cities...");
            if (_context.CityCollection.Any()) return;
            var cities = GetCities();
            _context.AddRange(cities);
            _context.SaveChanges();
        }

        public void AddServices()
        {
            _logger.LogWarning("Adding service...");
            if (_context.ServiceCollection.Any()) return;
            var services = GetServices();
            _context.AddRange(services);
            _context.SaveChanges();
        }

         public void AddRoles()
        {
            _logger.LogWarning("Adding roles...");
            if (_context.RoleCollection.Any()) return;
            var roles = GetRoles();
            _context.AddRange(roles);
            _context.SaveChanges();
        }
    }
}