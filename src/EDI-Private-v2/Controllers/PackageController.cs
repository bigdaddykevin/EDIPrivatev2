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
    public sealed class PackageController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<PackageViewModel> Packages;
        private readonly Func<int, IQueryable<CollectionStubViewModel>> CollectionStubGenerator;

        public PackageController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            Packages = from p in Context.TblPackage
                       where new[] { this.IsStringValid(p.ICRIdMain), this.IsStringValid(p.EDICSNumber), this.IsStringValid(p.ICRReferenceNumber), this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) }.Any(b => b)
                       select new PackageViewModel()
                       {
                           Id = p.Id,
                           ICRAS = this.IsStringValid(p.ICRIdMain) && this.IsStringValid(p.ICRIdVersion) ? $"{p.ICRIdMain}.{p.ICRIdVersion}" : null,
                           EDICS = this.StringFormatter(p.EDICSNumber),
                           ICRReferenceNumber = this.StringFormatter(p.ICRReferenceNumber),
                           OMBControlNumber = this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) ? $"{p.OfficeOMBNumber}-{p.StudyOMBNumber} v." + (this.IsStringValid(p.VersionOMBNumber) ? p.VersionOMBNumber : "NEW") : string.Empty,
                           CFDA = this.StringFormatter(p.CFDANumber),
                           Title = this.StringFormatter(p.Title),
                           Keywords = this.StringFormatter(p.Keywords),
                           Abstract = this.StringFormatter(p.Abstract),
                           IssueDate = this.DateTimeFormatter(p.DateNoticeIssued),
                           ExpirationDate = this.DateTimeFormatter(p.Expiration),
                           NoticeType = this.StringFormatter(p.TypeOfNotice),
                           TermsOfClearance = this.StringFormatter(p.TOC),
                           NumberRespondents = this.IntegerFormatter(p.NumberRespondents),
                           NumberResponses = this.IntegerFormatter(p.NumberResponses),
                           PercentCollectedElectronically = this.DoubleFormatter(p.PercentCollectedElectronically),
                           BurdenTotal = this.IntegerFormatter(p.BurdenHoursTotal),
                           BurdenChange = this.IntegerFormatter(p.BurdenHoursChange),
                           BurdenAdjustment = this.IntegerFormatter(p.BurdenHoursAdjustment),
                           BurdenExplanation = this.StringFormatter(p.ChangeExplanation),
                           PublicComment = this.StringFormatter(p.PublicCommentDocument),
                           PublicCommentResponse = this.StringFormatter(p.PublicResponseDocument),
                           OMBPassback = this.StringFormatter(p.OMBResponseDocument),
                           AuthorizingLawCited = this.StringFormatter(p.AuthorizingLawCited),
                           AuthorizingLawText = this.StringFormatter(p.AuthorizingLawText),
                           ContractorConfidentialityFormLocation = this.StringFormatter(p.ContractorConfidentialityForm),
                       };
            CollectionStubGenerator = id =>
                from l in Context.TblLink
                where l.PackageId == id
                join c in Context.TblCollection on l.CollectionId equals c.Id
                join d in Context.TblDetail on c.DetailId.Value equals d.Id
                let fu = d.FollowUp
                let s = fu.Study
                where d.Releasable == true && this.IsStringValid(c.Name)
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
        }

        // GET: /<controller>/id?
        [HttpGet("{id:int?}", Order = 0)]
        public IActionResult Index(int? id = null)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var result = Packages.FirstOrDefault(item => item.Id == id.Value);
            if (result == default(PackageViewModel))
            {
                return NotFound();
            }

            result.Collections = CollectionStubGenerator(result.Id).ToList();

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
            if (!queryFrames.Any() || !Packages.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<PackageStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<PackageStubViewModel> inter;
                switch (qf.Key)
                {
                    case "icras":
                        inter = from item in Packages
                                where this.IsStringValid(item.ICRAS) && item.ICRAS.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "edics":
                        inter = from item in Packages
                                where this.IsStringValid(item.EDICS) && item.EDICS.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "icrreferencenumber":
                        inter = from item in Packages
                                where this.IsStringValid(item.ICRReferenceNumber) && item.ICRReferenceNumber.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "ombcontrolnumber":
                        inter = from item in Packages
                                where this.IsStringValid(item.OMBControlNumber) && item.OMBControlNumber.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "cfda":
                        inter = from item in Packages
                                where this.IsStringValid(item.CFDA) && item.CFDA.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "title":
                        inter = from item in Packages
                                where this.IsStringValid(item.Title) && item.Title.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "keywords":
                        inter = from item in Packages
                                where this.IsStringValid(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "abstract":
                        inter = from item in Packages
                                where this.IsStringValid(item.Abstract) && item.Abstract.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "issuedate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.IssueDate >= value0 && item.IssueDate <= value1
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
                                inter = from item in Packages
                                        where item.IssueDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "expirationdate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.ExpirationDate >= value0 && item.ExpirationDate <= value1
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
                                inter = from item in Packages
                                        where item.ExpirationDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "noticetype":
                        inter = from item in Packages
                                where this.IsStringValid(item.NoticeType) && item.NoticeType.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "termsofclearance":
                        inter = from item in Packages
                                where this.IsStringValid(item.TermsOfClearance) && item.TermsOfClearance.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "numberrespondents":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.NumberRespondents >= value0 && item.NumberRespondents <= value1
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
                                inter = from item in Packages
                                        where item.NumberRespondents == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "numberresponses":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.NumberResponses >= value0 && item.NumberResponses <= value1
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
                                inter = from item in Packages
                                        where item.NumberResponses == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "percentcollectedelectronically":
                        if (qf.Value.Contains("~"))
                        {
                            double value0, value1;
                            var values = qf.Value.Split('~');
                            if (double.TryParse(values[0], out value0) && double.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.PercentCollectedElectronically >= value0 && item.PercentCollectedElectronically <= value1
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            double value;
                            if (double.TryParse(qf.Value, out value))
                            {
                                inter = from item in Packages
                                        where item.PercentCollectedElectronically == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdentotal":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.BurdenTotal >= value0 && item.BurdenTotal <= value1
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
                                inter = from item in Packages
                                        where item.BurdenTotal == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenchange":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.BurdenChange >= value0 && item.BurdenChange <= value1
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
                                inter = from item in Packages
                                        where item.BurdenChange == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenadjustment":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Packages
                                        where item.BurdenAdjustment >= value0 && item.BurdenAdjustment <= value1
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
                                inter = from item in Packages
                                        where item.BurdenAdjustment == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenexplanation":
                        inter = from item in Packages
                                where this.IsStringValid(item.BurdenExplanation) && item.BurdenExplanation.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "publiccomment":
                        inter = from item in Packages
                                where this.IsStringValid(item.PublicComment) && item.PublicComment.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "publiccommentresponse":
                        inter = from item in Packages
                                where this.IsStringValid(item.PublicCommentResponse) && item.PublicCommentResponse.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "ombpassback":
                        inter = from item in Packages
                                where this.IsStringValid(item.OMBPassback) && item.OMBPassback.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "authorizinglawcited":
                        inter = from item in Packages
                                where this.IsStringValid(item.AuthorizingLawCited) && item.AuthorizingLawCited.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "authorizinglawtext":
                        inter = from item in Packages
                                where this.IsStringValid(item.AuthorizingLawText) && item.AuthorizingLawText.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "contractorconfidentialityformlocation":
                        inter = from item in Packages
                                where this.IsStringValid(item.ContractorConfidentialityFormLocation) && item.ContractorConfidentialityFormLocation.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "collections":
                        inter = from l in Context.TblLink
                                let c = l.Collection
                                where this.IsStringValid(c.Name) && c.Name.ToLowerInvariant().Contains(qf.Value)
                                join p in Context.TblPackage on l.PackageId equals p.Id
                                select new PackageStubViewModel()
                                {
                                    Id = p.Id,
                                    ReferenceNumber = this.IsStringValid(p.ICRIdMain) && this.IsStringValid(p.ICRIdVersion) ? $"ICRAS {p.ICRIdMain}.{p.ICRIdVersion}"
                                        : (this.IsStringValid(p.EDICSNumber) ? $"EDICS {p.EDICSNumber}"
                                        : (this.IsStringValid(p.ICRReferenceNumber) ? p.ICRReferenceNumber
                                        : (this.IsStringValid(p.OfficeOMBNumber) && this.IsStringValid(p.StudyOMBNumber) ? $"{p.OfficeOMBNumber}-{p.StudyOMBNumber} v." + (this.IsStringValid(p.VersionOMBNumber) ? p.VersionOMBNumber : "NEW")
                                        : string.Empty)))
                                };
                        if (!inter.Any())
                        {
                            inter = inter.Select(item =>
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
                            results = results.Intersect(inter.ToList(), new PackageStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new PackageStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new PackageStubViewModelComparer());
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

            var packages = Packages.ToList();
            if (!packages.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in packages
                                         where (!string.IsNullOrWhiteSpace(item.ICRAS) && item.ICRAS.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.EDICS) && item.EDICS.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ICRReferenceNumber) && item.ICRReferenceNumber.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OMBControlNumber) && item.OMBControlNumber.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.CFDA) && item.CFDA.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Title) && item.Title.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Abstract) && item.Abstract.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.NoticeType) && item.NoticeType.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.TermsOfClearance) && item.TermsOfClearance.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.BurdenExplanation) && item.BurdenExplanation.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PublicComment) && item.PublicComment.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PublicCommentResponse) && item.PublicCommentResponse.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OMBPassback) && item.OMBPassback.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.AuthorizingLawCited) && item.AuthorizingLawCited.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.AuthorizingLawText) && item.AuthorizingLawText.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ContractorConfidentialityFormLocation) && item.ContractorConfidentialityFormLocation.ToLowerInvariant().Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new PackageStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }
    }
}