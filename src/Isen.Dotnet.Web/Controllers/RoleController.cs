using System;
using System.Linq;
using Isen.Dotnet.Library.Context;
using Isen.Dotnet.Library.Model;
using Isen.Dotnet.Library.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Isen.Dotnet.Web.Controllers
{
    public class RoleController : BaseController<Role>
    {
        public RoleController(
            ILogger<RoleController> logger,
            ApplicationDbContext context) : base(logger, context)
        {
        }
         protected override IQueryable<Role> BaseQuery() =>
            base.BaseQuery()
                // Inclure BirthCity lors d'une requête faite sur une ville
                .Include(r => r.RolePersonnes)
                .ThenInclude(rp => rp.Person);
                // Filtrer sur les villes qui commencent par Toul
                //.Where(p => p.BirthCity.StartsWith("Toul"))
                // Trier par ordre alpha des villes
                //.OrderBy(p => p.Service.Name);            
    }
}