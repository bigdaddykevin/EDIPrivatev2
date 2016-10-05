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
    public sealed class CollectionController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<CollectionViewModel> Collections;
        private readonly Func<int, IQueryable<PackageStubViewModel>> PackageStubGenerator;
        private readonly Func<int, IQueryable<RespondentStubViewModel>> RespondentStubGenerator;

        public CollectionController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            Collections = from d in Context.TblDetail
                          where d.Releasable == true
                          join c in Context.TblCollection on d.Id equals c.DetailId.Value
                          where this.IsStringValid(c.Name)
                          let fu = d.FollowUp
                          let s = fu.Study
                          where this.IsStringValid(s.Name) || this.IsStringValid(fu.Name)
                          select new CollectionViewModel()
                          {
                              Id = c.Id,
                              Name = this.StringFormatter(c.Name),
                              Study = new StudyStubViewModel()
                              {
                                  Id = d.Id,
                                  Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                  Series = new SeriesStubViewModel()
                                  {
                                      Id = s.ProgramId,
                                      Name = this.StringFormatter(s.Program.Name)
                                  }
                              },
                              Type = this.StringFormatter(c.Type),
                              Cost = this.IntegerFormatter(c.Cost),
                              CostYears = this.StringFormatter(c.CostYears),
                              RecruitmentStartDateEstimated = this.DateTimeFormatter(c.RecruitmentStartDate),
                              StartDateEstimated = this.DateTimeFormatter(c.StartDatePlanned),
                              StartDate = this.DateTimeFormatter(c.StartDateActual),
                              EndDateEstimated = this.DateTimeFormatter(c.EndDatePlanned),
                              EndDate = this.DateTimeFormatter(c.EndDateActual),
                              DateDescription = this.StringFormatter(c.DateDetail),
                              DataCollectionAgentType = this.StringFormatter(c.CollectorType),
                              DataCollectionAgentPrimary = this.StringFormatter(c.CollectorsNames),
                              DataCollectionAgentNonPrimary = this.StringFormatter(c.SubcollectorsNames),
                              ConfidentialityLaw = this.StringFormatter(c.RelevantConfidentialityLaw),
                              VoluntaryConfidentialityStatement = this.StringFormatter(c.ConfidentialityLanguage),
                              VoluntaryConfidentialityStatementRespondent = this.StringFormatter(c.VCLanguageA10),
                              ExperimentDescription = this.StringFormatter(c.ExperimentsDescription),
                              ExperimentResults = this.StringFormatter(c.ExperimentResults)
                          };
            RespondentStubGenerator = id =>
                from r in Context.TblRespondent
                let c = r.Collection
                join d in Context.TblDetail on c.DetailId.Value equals d.Id
                let fu = d.FollowUp
                let s = fu.Study
                where d.Releasable == true && r.CollectionId == id && this.IsStringValid(r.Type)
                select new RespondentStubViewModel()
                {
                    Id = r.Id,
                    Description = this.StringFormatter(r.Type) + (this.IsStringValid(r.TypeDetail) ? " – " + this.StringFormatter(r.TypeDetail) : string.Empty),
                    Collection = new CollectionStubViewModel()
                    {
                        Id = r.Id,
                        Name = this.StringFormatter(r.Collection.Name),
                        Study = new StudyStubViewModel()
                        {
                            Id = r.Collection.DetailId.Value,
                            Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                            Series = new SeriesStubViewModel()
                            {
                                Id = s.ProgramId,
                                Name = this.StringFormatter(s.Program.Name)
                            }
                        }
                    }
                };
            PackageStubGenerator = id =>
            {
                var ret =
                    from l in Context.TblLink
                    where l.CollectionId == id
                    from p in Context.TblPackage
                    where p.Id == l.PackageId && new[] { this.IsStringValid(p.ICRIdMain), this.IsStringValid(p.EDICSNumber), this.IsStringValid(p.ICRReferenceNumber), this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) }.Any(b => b)
                    select new PackageStubViewModel()
                    {
                        Id = p.Id,
                        ReferenceNumber = this.IsStringValid(p.ICRIdMain) && this.IsStringValid(p.ICRIdVersion) ? $"ICRAS {p.ICRIdMain}.{p.ICRIdVersion}"
                            : (this.IsStringValid(p.EDICSNumber) ? $"EDICS {p.EDICSNumber}"
                            : (this.IsStringValid(p.ICRReferenceNumber) ? p.ICRReferenceNumber
                            : (this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) ? $"{p.OfficeOMBNumber}-{p.StudyOMBNumber} v." + (this.IsStringValid(p.VersionOMBNumber) ? p.VersionOMBNumber : "NEW")
                            : string.Empty)))
                    };
                if (ret.Any())
                {
                    return ret.Select(item =>
                        new PackageStubViewModel()
                        {
                            Id = item.Id,
                            ReferenceNumber = item.ReferenceNumber,
                            Collections = from l in Context.TblLink
                                          where l.PackageId == item.Id
                                          join d in Context.TblDetail on l.Collection.DetailId.Value equals d.Id
                                          let fu = d.FollowUp
                                          let s = fu.Study
                                          select new CollectionStubViewModel()
                                          {
                                              Id = l.CollectionId,
                                              Name = this.StringFormatter(l.Collection.Name),
                                              Study = new StudyStubViewModel()
                                              {
                                                  Id = d.Id,
                                                  Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                                  Series = new SeriesStubViewModel()
                                                  {
                                                      Id = s.ProgramId,
                                                      Name = this.StringFormatter(s.Program.Name)
                                                  }
                                              }
                                          }
                        });
                }
                else
                {
                    return ret;
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

            var result = Collections.FirstOrDefault(item => item.Id == id.Value);
            if (result == default(CollectionViewModel))
            {
                return NotFound();
            }

            result.Respondents = RespondentStubGenerator(result.Id).ToList();
            result.Packages = PackageStubGenerator(result.Id).ToList();

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
            if (!queryFrames.Any() || !Collections.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<CollectionStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<CollectionStubViewModel> inter;
                switch (qf.Key)
                {
                    case "name":
                        inter = from item in Collections
                                where this.IsStringValid(item.Name) && item.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "study":
                        inter = from item in Collections
                                where item.Study != default(StudyStubViewModel) && this.IsStringValid(item.Study.Name) && item.Study.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "type":
                        inter = from item in Collections
                                where this.IsStringValid(item.Type) && item.Type.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "cost":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.Cost >= value0 && item.Cost <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            int value;
                            if (int.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.Cost == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "costyears":
                        inter = from item in Collections
                                where this.IsStringValid(item.CostYears) && item.CostYears.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "costdescription":
                        inter = from item in Collections
                                where this.IsStringValid(item.CostDescription) && item.CostDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "recruitmentstartdateestimated":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.RecruitmentStartDateEstimated >= value0 && item.RecruitmentStartDateEstimated <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            DateTime value;
                            if (DateTime.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.RecruitmentStartDateEstimated == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "startdateestimated":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.StartDateEstimated >= value0 && item.StartDateEstimated <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            DateTime value;
                            if (DateTime.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.StartDateEstimated == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "startdate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.StartDate >= value0 && item.StartDate <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            DateTime value;
                            if (DateTime.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.StartDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "enddateestimated":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.EndDateEstimated >= value0 && item.EndDateEstimated <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            DateTime value;
                            if (DateTime.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.EndDateEstimated == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "collectionenddate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Collections
                                        where item.EndDate >= value0 && item.EndDate <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            DateTime value;
                            if (DateTime.TryParse(qf.Value, out value))
                            {
                                inter = from item in Collections
                                        where item.EndDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "datedescription":
                        inter = from item in Collections
                                where this.IsStringValid(item.DateDescription) && item.DateDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "datacollectionagenttype":
                        inter = from item in Collections
                                where this.IsStringValid(item.DataCollectionAgentType) && item.DataCollectionAgentType.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "datacollectionagentprimary":
                        inter = from item in Collections
                                where this.IsStringValid(item.DataCollectionAgentPrimary) && item.DataCollectionAgentPrimary.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "datacollectionagentnonprimary":
                        inter = from item in Collections
                                where this.IsStringValid(item.DataCollectionAgentNonPrimary) && item.DataCollectionAgentNonPrimary.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "confidentialitylaw":
                        inter = from item in Collections
                                where this.IsStringValid(item.ConfidentialityLaw) && item.ConfidentialityLaw.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "voluntaryconfidentialitystatement":
                        inter = from item in Collections
                                where this.IsStringValid(item.VoluntaryConfidentialityStatement) && item.VoluntaryConfidentialityStatement.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "voluntaryconfidentialitystatementrespondent":
                        inter = from item in Collections
                                where this.IsStringValid(item.VoluntaryConfidentialityStatementRespondent) && item.VoluntaryConfidentialityStatementRespondent.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "experimentdescription":
                        inter = from item in Collections
                                where this.IsStringValid(item.ExperimentDescription) && item.ExperimentDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "experimentresults":
                        inter = from item in Collections
                                where this.IsStringValid(item.ExperimentResults) && item.ExperimentResults.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "respondents":
                        inter = from r in Context.TblRespondent
                                where r.CollectionId.HasValue && ((this.IsStringValid(r.Type) && r.Type.ToLowerInvariant().Contains(qf.Value)) || (this.IsStringValid(r.TypeDetail) && r.TypeDetail.ToLowerInvariant().Contains(qf.Value)))
                                let c = r.Collection
                                join d in Context.TblDetail on c.DetailId.Value equals d.Id
                                let fu = d.FollowUp
                                let s = fu.Study
                                where this.IsStringValid(c.Name)
                                select new CollectionStubViewModel()
                                {
                                    Id = c.Id,
                                    Name = this.StringFormatter(c.Name),
                                    Study = new StudyStubViewModel()
                                    {
                                        Id = c.DetailId.Value,
                                        Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                        Series = new SeriesStubViewModel()
                                        {
                                            Id = s.ProgramId,
                                            Name = this.StringFormatter(s.Program.Name)
                                        }
                                    }
                                };
                        break;

                    case "packages":
                        inter = from p in Context.TblPackage
                                where (this.IsStringValid(p.Title) && p.Title.ToLowerInvariant().Contains(qf.Value))
                                   || (this.IsStringValid(p.ICRIdMain) && this.IsStringValid(p.ICRIdVersion) && $"ICRAS {p.ICRIdMain}.{p.ICRIdVersion}".ToLowerInvariant().Contains(qf.Value))
                                   || (this.IsStringValid(p.EDICSNumber) && $"EDICS {p.EDICSNumber}".ToLowerInvariant().Contains(qf.Value))
                                   || (this.IsStringValid(p.ICRReferenceNumber) && p.ICRReferenceNumber.ToLowerInvariant().Contains(qf.Value))
                                   || (this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) && ($"{p.OfficeOMBNumber}-{p.StudyOMBNumber} v." + (this.IsStringValid(p.VersionOMBNumber) ? p.VersionOMBNumber : "NEW")).ToLowerInvariant().Contains(qf.Value))
                                join l in Context.TblLink on p.Id equals l.PackageId
                                let c = l.Collection
                                join d in Context.TblDetail on c.DetailId.Value equals d.Id
                                let fu = d.FollowUp
                                let s = fu.Study
                                where this.IsStringValid(c.Name)
                                select new CollectionStubViewModel()
                                {
                                    Id = c.Id,
                                    Name = this.StringFormatter(c.Name),
                                    Study = new StudyStubViewModel()
                                    {
                                        Id = c.DetailId.Value,
                                        Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                                        Series = new SeriesStubViewModel()
                                        {
                                            Id = s.ProgramId,
                                            Name = this.StringFormatter(s.Program.Name)
                                        }
                                    }
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
                            results = results.Intersect(inter.ToList(), new CollectionStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new CollectionStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new CollectionStubViewModelComparer());
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

            var collections = Collections.ToList();
            if (!collections.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in collections
                                         where (item.Study != default(StudyStubViewModel) && !string.IsNullOrWhiteSpace(item.Study.Name) && item.Study.Name.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Type) && item.Type.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.CostYears) && item.CostYears.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.CostDescription) && item.CostDescription.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DateDescription) && item.DateDescription.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataCollectionAgentType) && item.DataCollectionAgentType.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataCollectionAgentPrimary) && item.DataCollectionAgentPrimary.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataCollectionAgentNonPrimary) && item.DataCollectionAgentNonPrimary.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ConfidentialityLaw) && item.ConfidentialityLaw.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatement) && item.VoluntaryConfidentialityStatement.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatementRespondent) && item.VoluntaryConfidentialityStatementRespondent.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ExperimentDescription) && item.ExperimentDescription.Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ExperimentResults) && item.ExperimentResults.Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new CollectionStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }
    }
}