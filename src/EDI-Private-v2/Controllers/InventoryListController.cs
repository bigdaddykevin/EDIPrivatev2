using System;
using System.Linq;
using EDIPrivate.Models;
using EDIPrivate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EDIPrivate.Controllers
{
    [Route("[controller]")]
    [ResponseCache(Duration = 300)]
    public sealed class InventoryListController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<StudyStubViewModel> CollectionStubs;

        public InventoryListController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            CollectionStubs = from d in Context.TblDetail
                              where d.Releasable == true
                              let fu = d.FollowUp
                              let s = fu.Study
                              where this.IsStringValid(s.Name) || this.IsStringValid(fu.Name)
                              select new StudyStubViewModel()
                              {
                                  Id = d.Id,
                                  Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                  Series = new SeriesStubViewModel()
                                  {
                                      Id = s.ProgramId,
                                      Name = this.StringFormatter(s.Program.Name)
                                  }
                              };
        }

        // Get: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            if (!CollectionStubs.Any())
            {
                return NotFound();
            }

            return View(CollectionStubs.ToList());
        }
    }
}