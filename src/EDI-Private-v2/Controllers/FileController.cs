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
    public sealed class FileController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<FileViewModel> Files;
        private readonly Func<Guid, IQueryable<StudyStubViewModel>> StudyStubGenerator;
        private readonly Func<Guid, IQueryable<ElementViewModel>> ElementGenerator;

        public FileController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            Files = from d in Context.TblDetail
                    where d.Releasable == true
                    join sd in Context.TblSDLink on d.FollowUp.StudyId equals sd.StudyId
                    join t in Context.TblDataset on sd.DatasetId equals t.Id
                    join df in Context.TblDFLink on sd.Id equals df.SDLinkId
                    join f in Context.TblFile on df.FileId equals f.Id
                    where this.IsStringValid(f.Name)
                    select new FileViewModel()
                    {
                        Id = df.Id,
                        Name = this.StringFormatter(f.Name),
                        Format = this.StringFormatter(f.Format),
                        Dataset = this.StringFormatter(t.Title),
                        Restriction = t.Restriction == 0 ? "Public"
                            : (t.Restriction == 1 ? "Restricted Public"
                            : "Private"),
                        Location = this.StringFormatter(t.Location),
                        LocationDescription = this.StringFormatter(t.LocationDetail)
                    };
            StudyStubGenerator = id =>
                from df in Context.TblDFLink
                where df.Id == id
                join sd in Context.TblSDLink on df.SDLinkId equals sd.Id
                join d in Context.TblDetail on sd.StudyId equals d.FollowUp.StudyId
                where d.Releasable == true
                let fu = d.FollowUp
                let s = fu.Study
                where this.IsStringValid(s.Name) || this.IsStringValid(fu.Name)
                select new StudyStubViewModel()
                {
                    Id = d.Id,
                    Name = this.IsStringValid(fu.Name) ? fu.Name : this.StringFormatter(s.Name)
                };
            ElementGenerator = id =>
            {
                Predicate<string> isElementStringValid = str => this.IsStringValid(str) && str != "<null>";
                return from df in Context.TblDFLink
                       where df.Id == id
                       join fe in Context.TblFELink on df.Id equals fe.DFLinkId
                       join e in Context.TblElement on fe.ElementId equals e.Id
                       where isElementStringValid(e.Name) || isElementStringValid(e.Label)
                       select new ElementViewModel()
                       {
                           Id = fe.Id,
                           Name = isElementStringValid(e.Name) ? this.StringFormatter(e.Name) : string.Empty,
                           Type = isElementStringValid(e.Type) ? this.StringFormatter(e.Type) : string.Empty,
                           Label = isElementStringValid(e.Label) ? this.StringFormatter(e.Label) : string.Empty,
                           LabelExtended = isElementStringValid(e.ExtendedDefinition) ? this.StringFormatter(e.ExtendedDefinition) : string.Empty,
                           Question = isElementStringValid(e.Question) ? this.StringFormatter(e.Question) : string.Empty,
                           Values = Enumerable.Empty<ValueViewModel>()
                       };
            };
        }

        // GET: /<controller>/id?
        [HttpGet("{id:guid?}", Order = 0)]
        public IActionResult Index(Guid? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var result = Files.FirstOrDefault(item => item.Id == id);
            if (result == default(FileViewModel))
            {
                return NotFound();
            }

            result.Studies = StudyStubGenerator(result.Id).ToList();
            result.Elements = ElementGenerator(result.Id).ToList();

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
            if (!queryFrames.Any() || !Files.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<FileStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<FileStubViewModel> inter;
                switch (qf.Key)
                {
                    case "name":
                        inter = from item in Files
                                where this.IsStringValid(item.Name) && item.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "format":
                        inter = from item in Files
                                where this.IsStringValid(item.Format) && item.Format.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "dataset":
                        inter = from item in Files
                                where this.IsStringValid(item.Dataset) && item.Dataset.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "restriction":
                        inter = from item in Files
                                where this.IsStringValid(item.Restriction) && item.Restriction.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "location":
                        inter = from item in Files
                                where this.IsStringValid(item.Location) && item.Location.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "locationdescription":
                        inter = from item in Files
                                where this.IsStringValid(item.LocationDescription) && item.LocationDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "studies":
                        inter = from df in Context.TblDFLink
                                join sd in Context.TblSDLink on df.SDLinkId equals sd.Id
                                join d in Context.TblDetail on sd.StudyId equals d.FollowUp.StudyId
                                where d.Releasable == true
                                let fu = d.FollowUp
                                let s = fu.Study
                                where (this.IsStringValid(s.Name) && s.Name.ToLowerInvariant().Contains(qf.Value)) || (this.IsStringValid(fu.Name) && fu.Name.ToLowerInvariant().Contains(qf.Value))
                                join f in Context.TblFile on df.FileId equals f.Id
                                where this.IsStringValid(f.Name)
                                join t in Context.TblDataset on sd.DatasetId equals t.Id
                                select new FileStubViewModel()
                                {
                                    Id = df.Id,
                                    Name = this.StringFormatter(f.Name),
                                    Format = this.StringFormatter(f.Format),
                                    Restriction = t.Restriction == 0 ? "Public"
                                        : (t.Restriction == 1 ? "Restricted Public"
                                        : "Private")
                                };
                        if (!inter.Any())
                        {
                            inter = inter.Select(item =>
                                new FileStubViewModel()
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Format = item.Format,
                                    Restriction = item.Restriction,
                                    Studies = from df in Context.TblDFLink
                                              where df.Id == item.Id
                                              join sd in Context.TblSDLink on df.SDLinkId equals sd.Id
                                              join s in Context.TblStudy on sd.StudyId equals s.Id
                                              from fu in s.FollowUps
                                              from d in fu.Details
                                              select new StudyStubViewModel()
                                              {
                                                  Id = d.Id,
                                                  Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                                  Series = new SeriesStubViewModel()
                                                  {
                                                      Id = s.ProgramId,
                                                      Name = this.StringFormatter(s.Program.Name)
                                                  }
                                              }
                                });
                        }
                        break;

                    case "elements":
                        Predicate<string> isStringValid = str => this.IsStringValid(str) && str != "<null>";
                        inter = from df in Context.TblDFLink
                                join fe in Context.TblFELink on df.Id equals fe.DFLinkId
                                join e in Context.TblElement on fe.ElementId equals e.Id
                                where (isStringValid(e.Name) && e.Name.ToLowerInvariant().Contains(qf.Value)) || (isStringValid(e.Label) && e.Label.ToLowerInvariant().Contains(qf.Value))
                                join f in Context.TblFile on df.FileId equals f.Id
                                where this.IsStringValid(f.Name)
                                join sd in Context.TblSDLink on df.SDLinkId equals sd.Id
                                join t in Context.TblDataset on sd.DatasetId equals t.Id
                                select new FileStubViewModel()
                                {
                                    Id = df.Id,
                                    Name = this.StringFormatter(f.Name),
                                    Format = this.StringFormatter(f.Format),
                                    Restriction = t.Restriction.Value == 0 ? "Public"
                                        : (t.Restriction.Value == 1 ? "Restricted Public"
                                        : "Private")
                                };
                        if (!inter.Any())
                        {
                            inter = inter.Select(item =>
                                new FileStubViewModel()
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    Format = item.Format,
                                    Restriction = item.Restriction,
                                    Studies = from df in Context.TblDFLink
                                              where df.Id == item.Id
                                              join sd in Context.TblSDLink on df.SDLinkId equals sd.Id
                                              join s in Context.TblStudy on sd.StudyId equals s.Id
                                              from fu in s.FollowUps
                                              from d in fu.Details
                                              select new StudyStubViewModel()
                                              {
                                                  Id = d.Id,
                                                  Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                                  Series = new SeriesStubViewModel()
                                                  {
                                                      Id = s.ProgramId,
                                                      Name = this.StringFormatter(s.Program.Name)
                                                  }
                                              }
                                });
                        }
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
                            results = results.Intersect(inter.ToList(), new FileStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new FileStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new FileStubViewModelComparer());
                        }
                        break;
                }
            }

            ViewBag.queryItems = queryItems;

            return View("Search", results);
        }

        // GET: <controller>/<action>/query
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

            var files = Files.ToList();
            if (!files.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in files
                                         where (!string.IsNullOrWhiteSpace(item.Name) && item.Name.ToLowerInvariant().Contains(term))
                                               || (!string.IsNullOrWhiteSpace(item.Format) && item.Format.ToLowerInvariant().Contains(term))
                                               || (!string.IsNullOrWhiteSpace(item.Dataset) && item.Dataset.ToLowerInvariant().Contains(term))
                                               || (!string.IsNullOrWhiteSpace(item.Restriction) && item.Restriction.ToLowerInvariant().Contains(term))
                                               || (!string.IsNullOrWhiteSpace(item.Location) && item.Location.ToLowerInvariant().Contains(term))
                                               || (!string.IsNullOrWhiteSpace(item.LocationDescription) && item.LocationDescription.ToLowerInvariant().Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new FileStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }

        [AjaxOnly]
        [HttpGet("[action]")]
        public IActionResult GetValueComponent(Guid elementId = default(Guid)) =>
            ViewComponent("Value", elementId);
    }
}