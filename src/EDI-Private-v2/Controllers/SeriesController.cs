using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDIPrivate.Models;
using EDIPrivate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EDIPrivate.Controllers
{
    [Route("[controller]")]
    [ResponseCache(Duration = 300)]
    public sealed class SeriesController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<SeriesViewModel> Series;
        private readonly Func<int, IQueryable<StudyStubViewModel>> StudyStubGenerator;

        public SeriesController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            var validIds = (from d in Context.TblDetail
                            where d.Releasable == true
                            select d.FollowUp.Study.ProgramId)
                           .ToList();
            Series = from p in Context.TblProgram
                     where validIds.Contains(p.Id) && this.IsStringValid(p.Name)
                     select new SeriesViewModel()
                     {
                         Id = p.Id,
                         Name = p.Name,
                         Symbol = this.StringFormatter(p.Abbreviation),
                         OldName1 = this.StringFormatter(p.OldName1),
                         OldName1Symbol = this.StringFormatter(p.OldName1Abbreviation),
                         OldName1Duration = this.StringFormatter(p.OldName1Duration),
                         OldName2 = this.StringFormatter(p.OldName2),
                         OldName2Symbol = this.StringFormatter(p.OldName2Abbreviation),
                         OldName2Duration = this.StringFormatter(p.OldName2Duration),
                         ParentOrganization = "U.S. Department of Education" +
                             (this.IsStringValid(p.Division.Unit.PrincipalOffice.Name) ? $", {p.Division.Unit.PrincipalOffice.Name}" : string.Empty +
                             (this.IsStringValid(p.Division.Unit.Name) ? $", {p.Division.Unit.Name}" : string.Empty +
                             (this.IsStringValid(p.Division.Name) ? $", {p.Division.Name}" : string.Empty))),
                         Description = this.StringFormatter(p.Description)
                     };
            StudyStubGenerator = id =>
                from d in Context.TblDetail
                where d.Releasable == true
                let fu = d.FollowUp
                let s = fu.Study
                where s.ProgramId == id && (this.IsStringValid(s.Name) || this.IsStringValid(fu.Name))
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

        // GET: /<controller>/id?
        [HttpGet("{id:int?}", Order = 0)]
        public IActionResult Index(int? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var result = Series.FirstOrDefault(item => item.Id == id.Value);
            if (result == default(SeriesViewModel))
            {
                return NotFound();
            }

            result.Studies = StudyStubGenerator(result.Id).ToList();

            return View("Index", result);
        }

        // GET: /<controller>/query
        [HttpGet("{query}", Order = 1)]
        public IActionResult Index(string query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            query = this.NilQuery() + Uri.UnescapeDataString(query).ToLowerInvariant();
            var queryItems = query.Split('&', '=');
            var queryFrames = this.YieldQueryFrames(queryItems);
            if (!queryFrames.Any() || !Series.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<SeriesStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<SeriesStubViewModel> inter;
                switch (qf.Key)
                {
                    case "name":
                        inter = from item in Series
                                where this.IsStringValid(item.Name) && item.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "symbol":
                        inter = from item in Series
                                where this.IsStringValid(item.Symbol) && item.Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName1) && item.OldName1.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1symbol":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName1Symbol) && item.OldName1Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1duration":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName1Duration) && item.OldName1Duration.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName2) && item.OldName2.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2symbol":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName2Symbol) && item.OldName2Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2duration":
                        inter = from item in Series
                                where this.IsStringValid(item.OldName2Duration) && item.OldName2Duration.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "parentorganization":
                        inter = from item in Series
                                where this.IsStringValid(item.ParentOrganization) && item.ParentOrganization.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "description":
                        inter = from item in Series
                                where this.IsStringValid(item.Description) && item.Description.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "studies":
                        inter = from d in Context.TblDetail
                                where d.Releasable == true
                                let fu = d.FollowUp
                                let s = fu.Study
                                where this.IsStringValid(fu.Name)
                                    ? this.StringFormatter(fu.Name).ToLowerInvariant().Contains(qf.Value)
                                    : (this.IsStringValid(s.Name) ? this.StringFormatter(s.Name).ToLowerInvariant().Contains(qf.Value)
                                    : false)
                                select new SeriesStubViewModel()
                                {
                                    Id = s.Program.Id,
                                    Name = s.Program.Name
                                };
                        break;

                    default:
                        continue;
                }

                switch (qf.Operator)
                {
                    case BooleanOperator.Nil:
                        if (!results.Any())
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.And:
                        if (results.Any())
                        {
                            results = results.Intersect(inter.ToList(), new SeriesStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new SeriesStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new SeriesStubViewModelComparer());
                        }
                        break;
                }
            }

            ViewBag.queryItems = queryItems;

            return View("Search", results);
        }

        // GET: /<controller>/<action>/query
        [HttpGet("[action]/{query}", Order = 2)]
        public IActionResult Search(string query = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var terms = query.ToLowerInvariant().Split(' ').AsEnumerable();
            if (string.IsNullOrWhiteSpace(terms.FirstOrDefault()))
            {
                return BadRequest();
            }

            if (terms.Any(str => str.Contains('"')))
            {
                var quotedTerms = new List<string>();
                var sb = new StringBuilder();
                foreach (var term in terms)
                {
                    if (term.First() == '"' && sb.Length == 0)
                    {
                        sb.Append(term.Remove(0) + " ");
                    }
                    else if (term.Last() == '"' && sb.Length != 0)
                    {
                        sb.Append(term.Remove(term.Length - 1));
                        quotedTerms.Add(sb.ToString());
                        sb.Clear();
                    }
                    else if (sb.Length != 0)
                    {
                        sb.Append(term + " ");
                    }
                    else
                    {
                        quotedTerms.Add(term);
                    }
                }

                terms = quotedTerms;
            }

            var series = Series.ToList();
            if (!series.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in series
                                         where (!string.IsNullOrWhiteSpace(item.Name) && item.Name.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Symbol) && item.Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1) && item.OldName1.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1Symbol) && item.OldName1Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1Duration) && item.OldName1Duration.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2) && item.OldName2.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2Symbol) && item.OldName2Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2Duration) && item.OldName2Duration.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ParentOrganization) && item.ParentOrganization.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Description) && item.Description.ToLowerInvariant().Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new SeriesStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }
    }
}