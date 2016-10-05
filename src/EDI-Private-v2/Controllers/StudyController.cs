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
    public sealed class StudyController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<StudyViewModel> Studies;
        private readonly Func<int, IQueryable<CollectionStubViewModel>> CollectionStubGenerator;
        private readonly Func<int, IQueryable<ContactViewModel>> ContactGenerator;
        private readonly Func<int, IQueryable<FileStubViewModel>> FileStubGenerator;
        private readonly Func<int, StudyStubViewModel> PreviousStudyStubGenerator;

        public StudyController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            Studies = from d in Context.TblDetail
                      where d.Releasable == true
                      let fu = d.FollowUp
                      let s = fu.Study
                      where this.IsStringValid(s.Name) || this.IsStringValid(fu.Name)
                      select new StudyViewModel()
                      {
                          Id = d.Id,
                          Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(s.Name),
                          Symbol = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Abbreviation) : this.StringFormatter(s.Abbreviation),
                          OldName1 = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName1) : default(string),
                          OldName1Symbol = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName1Abbreviation) : default(string),
                          OldName1Duration = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName1Duration) : default(string),
                          OldName2 = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName2) : default(string),
                          OldName2Symbol = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName2Abbreviation) : default(string),
                          OldName2Duration = !this.IsStringValid(fu.Name) ? this.StringFormatter(s.OldName2Duration) : default(string),
                          Series = !this.IsStringValid(s.Program.Name) ? default(SeriesStubViewModel) : new SeriesStubViewModel()
                          {
                              Id = s.Program.Id,
                              Name = this.StringFormatter(s.Program.Name)
                          },
                          Investigator = this.StringFormatter(d.Steward),
                          BureauCode = this.StringFormatter(d.BureauCode),
                          ProgramCode = this.StringFormatter(d.ProgramCode),
                          Website = this.IsStringValid(s.Website) && Uri.IsWellFormedUriString(s.Website, UriKind.Absolute) ? new Uri(s.Website) : default(Uri),
                          Keywords = this.StringFormatter(d.Keywords),
                          Description = this.IsStringValid(fu.Abstract) ? this.StringFormatter(fu.Abstract) : this.StringFormatter(s.Abstract),
                          AuthorizingLaw = this.StringFormatter(d.AuthorizingLaw),
                          TotalCost = this.IntegerFormatter(d.TotalCost.Value),
                          TotalCostYears = this.StringFormatter(d.FullScaleYears),
                          TotalCostDescription = this.StringFormatter(d.TotalCostDetail),
                          Universe = this.BooleanFormatter(d.UniverseSurvey),
                          Sample = this.BooleanFormatter(d.SampleSurvey),
                          Longitudinal = this.BooleanFormatter(d.Longitudinal),
                          CrossSectional = this.BooleanFormatter(d.CrossSectional),
                          ProgramMonitoring = this.BooleanFormatter(d.ProgramMonitoring),
                          GranteeReporting = this.BooleanFormatter(d.GranteeReports),
                          Voluntary = this.BooleanFormatter(d.Voluntary),
                          Mandatory = this.BooleanFormatter(d.Mandatory),
                          RequiredForBenefits = this.BooleanFormatter(d.RequiredForBenefits),
                          RequirementDescription = this.StringFormatter(d.RequirementDetail),
                          RequirementReason = this.StringFormatter(d.RequirementReason),
                          SORN = this.StringFormatter(d.SORN),
                          SORNUrl = this.IsStringValid(d.SORNUrl) && Uri.IsWellFormedUriString(d.SORNUrl, UriKind.Absolute) ? new Uri(d.SORNUrl) : default(Uri),
                          ConfidentialityRestrictions = this.StringFormatter(d.ConfidentialityRestrictions),
                          PII_DI = this.BooleanFormatter(d.PII_DI),
                          PIA = this.BooleanFormatter(d.PIA),
                          DataCouldBePublic = this.BooleanFormatter(d.CouldBePublic),
                          PublicationStatisticsType = this.StringFormatter(d.PublicationTypeStats),
                          PublicationStatisticsUrl = this.IsStringValid(d.PublicationUrlStats) && Uri.IsWellFormedUriString(d.PublicationUrlStats, UriKind.Absolute) ? new Uri(d.PublicationUrlStats) : default(Uri),
                          PublicationStatisticsDate = this.DateTimeFormatter(d.PublicationDateStats),
                          PublicationDataUrl = this.IsStringValid(d.PublicationUrlData) && Uri.IsWellFormedUriString(d.PublicationUrlData, UriKind.Absolute) ? new Uri(d.PublicationUrlData) : default(Uri),
                          PublicationDataDate = this.DateTimeFormatter(d.PublicationDateData),
                          PublicationRestrictedUseDataDate = this.DateTimeFormatter(d.RestrictedUseDate),
                          SubjectPopulation = string.Join("; ", new[]
                              {
                                  d.Students > 0 ? "Students" : string.Empty,
                                  d.Staff > 0 ? "Staff" : string.Empty,
                                  d.Institutions > 0 ? "Institutions" : string.Empty,
                                  d.Programs > 0 ? "Programs" : string.Empty,
                                  d.Age0_2 > 0 ? "Age 0–2" : string.Empty,
                                  d.Age3_5 > 0 ? "Age 3–5" : string.Empty,
                                  d.Age6_21 > 0 ? "Age 6–21" : string.Empty,
                                  d.AgeOlderThan21 > 0 ? "Age Older Than 21" : string.Empty,
                                  d.AgeNa > 0 ? "Age Not Applicable" : string.Empty,
                                  d.PreK > 0 ? "Pre-Kindergarten" : string.Empty,
                                  d.ElementarySchool > 0 ? "Elementary School" : string.Empty,
                                  d.MiddleSchool > 0 ? "Middle School" : string.Empty,
                                  d.HighSchool > 0 ? "High School" : string.Empty,
                                  d.Postsecondary > 0 ? "Postsecondary Education" : string.Empty,
                                  d.Graduate > 0 ? "Graduate Education" : string.Empty,
                                  d.ContinuedTechnicalEd > 0 ? "Continued/Technical Education" : string.Empty,
                                  d.GeneralAdult > 0 ? "General Adult Population" : string.Empty,
                                  d.EducationLevelNa > 0 ? "Education Level Not Applicable" : string.Empty,
                                  this.StringFormatter(d.OtherSubject),
                                  this.StringFormatter(d.OtherPopulation)
                              }
                              .Where(str => !string.IsNullOrWhiteSpace(str))),
                          SubjectPopulationDescription = this.StringFormatter(d.SubjectPopulationDetail),
                          DataLevelsAvailable = string.Join("; ", new[]
                              {
                                  d.IndividualData == true ? "Individual" : string.Empty,
                                  d.ClassroomData == true ? "Classroom" : string.Empty,
                                  d.GradeLevelData == true ? "Grade Level" : string.Empty,
                                  d.SchoolInstitutionData == true ? "School/Institution" : string.Empty,
                                  d.LEAData == true ? "Local education agency (LEA)" : string.Empty,
                                  d.StateData == true ? "State" : string.Empty,
                                  d.RegionData == true ? "Region" : string.Empty,
                                  d.NationalData == true ? "United States" : string.Empty
                              }
                              .Where(str => !string.IsNullOrWhiteSpace(str))),
                          DataLevelPublic = this.StringFormatter(d.PublicAccessLevel),
                          DataLevelDescription = this.StringFormatter(d.DataLevelDetail)
                      };
            PreviousStudyStubGenerator = id =>
                (from d in Context.TblDetail
                 where d.Id == id && d.FollowUp.PrecedingId.HasValue
                 join fu in Context.TblFollowUp on d.FollowUp.PrecedingId.Value equals fu.Id
                 select new StudyStubViewModel()
                 {
                     Id = fu.Details.FirstOrDefault().Id,
                     Name = this.IsStringValid(fu.Name) ? this.StringFormatter(fu.Name) : this.StringFormatter(fu.Study.Name)
                 })
                .FirstOrDefault();
            ContactGenerator = id =>
                from d in Context.TblDetail
                where d.Id == id
                let p = d.FollowUp.Study.Program
                let contactIds = new Dictionary<int, int?>()
                {
                    { 1, p.PrimaryContactId },
                    { 2, p.SecondContactId },
                    { 3, p.ThirdContactId },
                    { 4, p.SupervisorId }
                }
                where contactIds.Any(cid => cid.Value.HasValue)
                from contactId in contactIds.Where(cid => cid.Value.HasValue)
                join c in Context.TblContact on contactId.Value.Value equals c.Id
                where this.IsStringValid(c.FirstName) && this.IsStringValid(c.LastName)
                select new ContactViewModel()
                {
                    Id = c.Id,
                    SortOrder = contactId.Key,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    EmailAddress = c.Email,
                    TelephoneNumber = c.Phone
                };
            CollectionStubGenerator = id =>
                from d in Context.TblDetail
                where d.Id == id
                join c in Context.TblCollection on d.Id equals c.DetailId.Value
                where this.IsStringValid(c.Name)
                select new CollectionStubViewModel()
                {
                    Id = c.Id,
                    Name = this.StringFormatter(c.Name),
                    Study = new StudyStubViewModel()
                    {
                        Id = d.Id,
                        Name = this.IsStringValid(d.FollowUp.Name) ? this.StringFormatter(d.FollowUp.Name) : this.StringFormatter(d.FollowUp.Study.Name),
                        Series = new SeriesStubViewModel()
                        {
                            Id = d.FollowUp.Study.ProgramId,
                            Name = this.StringFormatter(d.FollowUp.Study.Program.Name)
                        }
                    }
                };
            FileStubGenerator = id =>
            {
                var ret =
                    from d in Context.TblDetail
                    where d.Id == id
                    join sd in Context.TblSDLink on d.FollowUp.StudyId equals sd.StudyId
                    join t in Context.TblDataset on sd.DatasetId equals t.Id
                    join df in Context.TblDFLink on sd.Id equals df.SDLinkId
                    join f in Context.TblFile on df.FileId equals f.Id
                    where this.IsStringValid(f.Name)
                    select new FileStubViewModel()
                    {
                        Id = df.Id,
                        Name = this.StringFormatter(f.Name),
                        Format = this.StringFormatter(f.Format),
                        Restriction =
                            t.Restriction == 0 ? "Public"
                            : (t.Restriction == 1 ? "Restricted Public"
                            : "Private")
                    };
                if (ret.Any())
                {
                    return ret.Select(item =>
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

            var result = Studies.FirstOrDefault(item => item.Id == id.Value);
            if (result == default(StudyViewModel))
            {
                return NotFound();
            }

            result.PreviousStudy = PreviousStudyStubGenerator(result.Id);
            result.Contacts = ContactGenerator(result.Id).ToList();
            result.Collections = CollectionStubGenerator(result.Id).ToList();
            result.Files = FileStubGenerator(result.Id).ToList();

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
            if (!queryFrames.Any() || !Studies.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<StudyStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<StudyStubViewModel> inter;
                bool flag;
                switch (qf.Key)
                {
                    case "name":
                        inter = from item in Studies
                                where this.IsStringValid(item.Name) && item.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "symbol":
                        inter = from item in Studies
                                where this.IsStringValid(item.Symbol) && item.Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName1) && item.OldName1.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1symbol":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName1Symbol) && item.OldName1Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname1duration":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName1Duration) && item.OldName1Duration.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName2) && item.OldName2.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2symbol":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName2Symbol) && item.OldName2Symbol.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "oldname2duration":
                        inter = from item in Studies
                                where this.IsStringValid(item.OldName2Duration) && item.OldName2Duration.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "series":
                        inter = from item in Studies
                                where item.Series != default(SeriesStubViewModel) && this.IsStringValid(item.Series.Name) && item.Series.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "investigator":
                        inter = from item in Studies
                                where this.IsStringValid(item.Investigator) && item.Investigator.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "bureaucode":
                        inter = from item in Studies
                                where this.IsStringValid(item.BureauCode) && item.BureauCode.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "programcode":
                        inter = from item in Studies
                                where this.IsStringValid(item.ProgramCode) && item.ProgramCode.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "website":
                        inter = from item in Studies
                                where item.Website != default(Uri) && this.IsStringValid(item.Website.ToString()) && item.Website.ToString().ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "keywords":
                        inter = from item in Studies
                                where this.IsStringValid(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "description":
                        inter = from item in Studies
                                where this.IsStringValid(item.Description) && item.Description.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "authorizinglaw":
                        inter = from item in Studies
                                where this.IsStringValid(item.AuthorizingLaw) && item.AuthorizingLaw.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "totalcost":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Studies
                                        where item.TotalCost >= value0 && item.TotalCost <= value1
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
                                inter = from item in Studies
                                        where item.TotalCost == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "totalcostyears":
                        inter = from item in Studies
                                where this.IsStringValid(item.TotalCostYears) && item.TotalCostYears.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "totalcostdescription":
                        inter = from item in Studies
                                where this.IsStringValid(item.TotalCostDescription) && item.TotalCostDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "universe":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.Universe == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "sample":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.Sample == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "longitudinal":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.Longitudinal == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "crosssectional":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.CrossSectional == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "programmonitoring":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.ProgramMonitoring == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "granteereporting":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.GranteeReporting == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "voluntary":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.Voluntary == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "mandatory":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.Mandatory == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "requiredforbenefits":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.RequiredForBenefits == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "requirementdescription":
                        inter = from item in Studies
                                where this.IsStringValid(item.RequirementDescription) && item.RequirementDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "requirementreason":
                        inter = from item in Studies
                                where this.IsStringValid(item.RequirementReason) && item.RequirementReason.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "sorn":
                        inter = from item in Studies
                                where this.IsStringValid(item.SORN) && item.SORN.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "sornurl":
                        inter = from item in Studies
                                where item.SORNUrl != default(Uri) && this.IsStringValid(item.SORNUrl.ToString()) && item.SORNUrl.ToString().ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "confidentialityrestrictions":
                        inter = from item in Studies
                                where this.IsStringValid(item.ConfidentialityRestrictions) && item.ConfidentialityRestrictions.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "pii_di":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.PII_DI == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "pia":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.PIA == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "datacouldbepublic":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Studies
                                    where item.DataCouldBePublic == flag
                                    select item.ToStub();
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "publicationstatisticstype":
                        inter = from item in Studies
                                where this.IsStringValid(item.PublicationStatisticsType) && item.PublicationStatisticsType.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "publicationstatisticsurl":
                        inter = from item in Studies
                                where item.PublicationStatisticsUrl != default(Uri) && this.IsStringValid(item.PublicationStatisticsUrl.ToString()) && item.PublicationStatisticsUrl.ToString().ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "publicationstatisticsdate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Studies
                                        where item.PublicationStatisticsDate >= value0 && item.PublicationStatisticsDate <= value1
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
                                inter = from item in Studies
                                        where item.PublicationStatisticsDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "publicationdataurl":
                        inter = from item in Studies
                                where item.PublicationDataUrl != default(Uri) && this.IsStringValid(item.PublicationDataUrl.ToString()) && item.PublicationDataUrl.ToString().ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "publicationdatadate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Studies
                                        where item.PublicationDataDate >= value0 && item.PublicationDataDate <= value1
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
                                inter = from item in Studies
                                        where item.PublicationDataDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "publicationrestrictedusedatadate":
                        if (qf.Value.Contains("~"))
                        {
                            DateTime value0, value1;
                            var values = qf.Value.Split('~');
                            if (DateTime.TryParse(values[0], out value0) && DateTime.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Studies
                                        where item.PublicationRestrictedUseDataDate >= value0 && item.PublicationRestrictedUseDataDate <= value1
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
                                inter = from item in Studies
                                        where item.PublicationRestrictedUseDataDate == value
                                        select item.ToStub();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "subjectpopulation":
                        inter = from item in Studies
                                where this.IsStringValid(item.SubjectPopulation) && item.SubjectPopulation.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "subjectpopulationdescription":
                        inter = from item in Studies
                                where this.IsStringValid(item.SubjectPopulationDescription) && item.SubjectPopulationDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "datalevelsavailable":
                        inter = from item in Studies
                                where this.IsStringValid(item.DataLevelsAvailable) && item.DataLevelsAvailable.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "datalevelpublic":
                        inter = from item in Studies
                                where this.IsStringValid(item.DataLevelPublic) && item.DataLevelPublic.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "dataleveldescription":
                        inter = from item in Studies
                                where this.IsStringValid(item.DataLevelDescription) && item.DataLevelDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "previousstudy":
                        inter = from item in Studies
                                where item.PreviousStudy != default(StudyStubViewModel) && this.IsStringValid(item.PreviousStudy.Name) && item.PreviousStudy.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub();
                        break;

                    case "contacts":
                        inter = from d in Context.TblDetail
                                where d.Releasable == true
                                let fu = d.FollowUp
                                let s = fu.Study
                                let contactIds = new Dictionary<int, int?>()
                                {
                                    { 1, s.Program.PrimaryContactId },
                                    { 2, s.Program.SecondContactId },
                                    { 3, s.Program.ThirdContactId },
                                    { 4, s.Program.SupervisorId }
                                }
                                where contactIds.Any(cid => cid.Value.HasValue)
                                from contactId in contactIds.Where(cid => cid.Value.HasValue)
                                join c in Context.TblContact on contactId.Value.Value equals c.Id
                                where this.IsStringValid($"{c.FirstName} {c.LastName}")
                                   ? this.StringFormatter($"{c.FirstName} {c.LastName}").ToLowerInvariant().Contains(qf.Value)
                                   : false
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
                        break;

                    case "collections":
                        inter = from d in Context.TblDetail
                                where d.Releasable == true
                                join c in Context.TblCollection on d.Id equals c.DetailId.Value
                                where this.IsStringValid(c.Name) && c.Name.ToLowerInvariant().Contains(qf.Value)
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
                        break;

                    case "files":
                        inter = from d in Context.TblDetail
                                let fu = d.FollowUp
                                let s = fu.Study
                                where this.IsStringValid(s.Name) || this.IsStringValid(fu.Name)
                                join sd in Context.TblSDLink on s.Id equals sd.StudyId
                                join df in Context.TblDFLink on sd.Id equals df.SDLinkId
                                join f in Context.TblFile on df.FileId equals f.Id
                                where this.IsStringValid(f.Name) && f.Name.ToLowerInvariant().Contains(qf.Value)
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
                            results = results.Intersect(inter.ToList(), new StudyStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new StudyStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new StudyStubViewModelComparer());
                        }
                        break;
                }
            }

            ViewBag.queryItems = queryItems;

            return View("Search", results);
        }

        // GET: /<controller>/<action>/query
        [HttpGet("[action]/{query}", Order = 2)]
        public IActionResult SearchFull(string query = null)
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

            var studies = Studies.ToList();
            if (!studies.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in studies
                                         where (!string.IsNullOrWhiteSpace(item.Name) && item.Name.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Symbol) && item.Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1) && item.OldName1.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1Symbol) && item.OldName1Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName1Duration) && item.OldName1Duration.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2) && item.OldName2.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2Symbol) && item.OldName2Symbol.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.OldName2Duration) && item.OldName2Duration.ToLowerInvariant().Contains(term))
                                             || (item.Series != default(SeriesStubViewModel) && !string.IsNullOrWhiteSpace(item.Series.Name) && item.Series.Name.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Investigator) && item.Investigator.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.BureauCode) && item.BureauCode.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ProgramCode) && item.ProgramCode.ToLowerInvariant().Contains(term))
                                             || (item.Website != default(Uri) && !string.IsNullOrWhiteSpace(item.Website.ToString()) && item.Website.ToString().ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Description) && item.Description.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.AuthorizingLaw) && item.AuthorizingLaw.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.TotalCostYears) && item.TotalCostYears.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.TotalCostDescription) && item.TotalCostDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.RequirementDescription) && item.RequirementDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.RequirementReason) && item.RequirementReason.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.SORN) && item.SORN.ToLowerInvariant().Contains(term))
                                             || (item.SORNUrl != default(Uri) && !string.IsNullOrWhiteSpace(item.SORNUrl.ToString()) && item.SORNUrl.ToString().ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ConfidentialityRestrictions) && item.ConfidentialityRestrictions.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PublicationStatisticsType) && item.PublicationStatisticsType.ToLowerInvariant().Contains(term))
                                             || (item.PublicationStatisticsUrl != default(Uri) && !string.IsNullOrWhiteSpace(item.PublicationStatisticsUrl.ToString()) && item.PublicationStatisticsUrl.ToString().ToLowerInvariant().Contains(term))
                                             || (item.PublicationDataUrl != default(Uri) && !string.IsNullOrWhiteSpace(item.PublicationDataUrl.ToString()) && item.PublicationDataUrl.ToString().ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.SubjectPopulation) && item.SubjectPopulation.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.SubjectPopulationDescription) && item.SubjectPopulationDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataLevelsAvailable) && item.DataLevelsAvailable.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataLevelPublic) && item.DataLevelPublic.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.DataLevelDescription) && item.DataLevelDescription.ToLowerInvariant().Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new StudyStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }

        // GET: /<controller>/<action>/query
        [HttpGet("[action]/{query}", Order = 3)]
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

            var studies = Studies.ToList();
            if (!studies.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in studies
                                         where (!string.IsNullOrWhiteSpace(item.Name) && item.Name.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Description) && item.Description.ToLowerInvariant().Contains(term))
                                         select item.ToStub())
                .GroupBy(item => item, new StudyStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }
    }
}