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
    public sealed class RespondentController : Controller
    {
        private readonly DataContext Context;
        private readonly IQueryable<RespondentViewModel> Respondents;

        public RespondentController(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
            var validActivities = from d in Context.TblDetail
                                  where d.Releasable == true
                                  join c in Context.TblCollection on d.Id equals c.DetailId.Value
                                  where this.IsStringValid(c.Name)
                                  select new
                                  {
                                      Id = c.Id,
                                      Name = c.Name
                                  };
            Respondents = from r in Context.TblRespondent
                          where this.IsStringValid(r.Type) && r.CollectionId.HasValue && validActivities.ToList().Any(va => va.Id == r.CollectionId.Value)
                          join d in Context.TblDetail on r.Collection.DetailId.Value equals d.Id
                          let fu = d.FollowUp
                          let s = fu.Study
                          select new RespondentViewModel()
                          {
                              Id = r.Id,
                              Collection = new CollectionStubViewModel()
                              {
                                  Id = r.CollectionId.Value,
                                  Name = this.StringFormatter(validActivities.FirstOrDefault(va => va.Id == r.CollectionId).Name),
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
                              },
                              Type = this.StringFormatter(r.Type),
                              Description = this.StringFormatter(r.TypeDetail),
                              Keywords = this.StringFormatter(r.Topics),
                              Voluntary = this.BooleanFormatter(r.Voluntary),
                              Mandatory = this.BooleanFormatter(r.Mandatory),
                              RequiredForBenefits = this.BooleanFormatter(r.RequiredForBenefits),
                              RequirementDescription = this.StringFormatter(r.RequiredResponseDetail),
                              RequirementReason = this.StringFormatter(r.ResponseRequirementReason),
                              PopulationSizeEstimated = this.IntegerFormatter(r.ExpectedSampleSize),
                              PopulationSize = this.IntegerFormatter(r.ActualSampleSize),
                              PopulationSizeDescription = this.StringFormatter(r.ActualSampleSizeDetail),
                              ResponseSizeEstimated = this.IntegerFormatter(r.RespondentSizeEstimate),
                              ResponseSize = this.IntegerFormatter(r.RespondentSizeActual),
                              ResponseRateEstimated = this.DoubleFormatter(r.ResponseRateEstimated),
                              ResponseRate = this.DoubleFormatter(r.ResponseRateActual),
                              ResponseRateDescription = this.StringFormatter(r.ResponseRateActualDetail),
                              Burden = this.IntegerFormatter(r.BurdenTimeTotal),
                              BurdenPerRespondent = this.IntegerFormatter(r.BurdenTimePerRespondent),
                              BurdenPerRespondentSurvey = this.IntegerFormatter(r.SurveyBurdenPerRespondent),
                              BurdenPerRespondentAssessment = this.IntegerFormatter(r.AssessmentBurdenPerRespondent),
                              ConsentExplicit = this.BooleanFormatter(r.ExplicitConsent),
                              ConsentImplicit = this.BooleanFormatter(r.ImplicitConsent),
                              ConsentNotApplicable = this.BooleanFormatter(r.ConsentNotApplicable) || this.BooleanFormatter(r.ParticipatoryConsent),
                              ConsentForm = this.StringFormatter(r.ConsentForm),
                              Population = string.Join("; ", new[]
                                  {
                                      r.Age0_2 == true ? "Age 0–2" : string.Empty,
                                      r.Age3_5 == true ? "Age 3–5" : string.Empty,
                                      r.Age6_21 == true ? "Age 6–21" : string.Empty,
                                      r.AgeOlderThan21 == true ? "Age Older Than 21" : string.Empty,
                                      r.AgeNa == true ? "Age Not Applicable" : string.Empty,
                                      r.PreK == true ? "Pre-Kindergarten" : string.Empty,
                                      r.ElementarySchool == true ? "Elementary School" : string.Empty,
                                      r.MiddleSchool == true ? "Middle School" : string.Empty,
                                      r.HighSchool == true ? "High School" : string.Empty,
                                      r.Postsecondary == true ? "Postsecondary Education" : string.Empty,
                                      r.Graduate == true ? "Graduate Education" : string.Empty,
                                      r.ContinuedTechnicalEd == true ? "Continued/Technical Education" : string.Empty,
                                      r.AdultEducation == true ? "General Adult Population" : string.Empty,
                                      r.EducationLevelNa == true ? "Education Level Not Applicable" : string.Empty,
                                      this.StringFormatter(r.OtherPopulation)
                                  }
                                  .Where(str => !string.IsNullOrWhiteSpace(str))),
                              PopulationDescription = this.StringFormatter(r.PopulationDetail),
                              ResponseType = string.Join("; ", new[]
                                  {
                                      r.AdministrativeRecords == true ? "Administrative Records" : string.Empty,
                                      r.AddressUpdate == true ? "Address Update" : string.Empty,
                                      r.ListData == true ? "List Data" : string.Empty,
                                      r.Recruitment == true ? "Recruitment" : string.Empty,
                                      r.CoordinationHelp == true ? "Coordination Assistance" : string.Empty,
                                      r.Screener == true ? "Screener" : string.Empty,
                                      r.Assessment == true ? "Assessment" : string.Empty,
                                      r.Survey == true ? "Survey" : string.Empty,
                                      r.AbbreviatedSurvey == true ? "Abbreviated Survey" : string.Empty,
                                      r.ProgramReporting == true ? "Program Reporting" : string.Empty,
                                      r.EDFacts == true ? "EDFacts" : string.Empty,
                                      r.CPS == true ? "Central Processing System (CPS)" : string.Empty,
                                      r.NSLDS == true ? "National Student Loan Data System (NSLDS)" : string.Empty,
                                      this.StringFormatter(r.OtherResponseType)
                                  }
                                  .Where(str => !string.IsNullOrWhiteSpace(str))),
                              ResponseTypeDescription = this.StringFormatter(r.ResponseTypeDetail),
                              ResponseMode = string.Join("; ", new[]
                                  {
                                      r.Paper > 0 ? "Paper" : string.Empty,
                                      r.PhoneNotCATI > 0 ? "Telephone" : string.Empty,
                                      r.CATI > 0 ? "Computer-assisted telephone interview (CATI)" : string.Empty,
                                      r.Web > 0 ? "Internet" : string.Empty,
                                      r.Email > 0 ? "Email" : string.Empty,
                                      r.F2FNotCAPI > 0 ? "Personal interview" : string.Empty,
                                      r.CAPI > 0 ? "Computer-assisted personal interview (CAPI)" : string.Empty,
                                      r.Spreadsheet > 0 ? "Spreadsheet" : string.Empty,
                                      r.PRS > 0 ? "Personnel Response System (PRS)" : string.Empty,
                                      this.StringFormatter(r.OtherCollectionMode)
                                  }
                                  .Where(str => !string.IsNullOrWhiteSpace(str))),
                              ResponseModeDescription = this.StringFormatter(r.CollectionModeDetail),
                              AdditionalLanguageInstrument = this.StringFormatter(r.OtherLanguages),
                              AdditionalLanguageInterpreter = this.StringFormatter(r.Interpreters),
                              IncentiveCashValue = this.StringFormatter(r.CashRangeAmount),
                              IncentiveCashDescription = this.StringFormatter(r.CashDetail),
                              IncentiveNonCashValue = this.StringFormatter(r.NonCashRangeAmount),
                              IncentiveNonCashDescription = this.StringFormatter(r.NonCashDetail),
                              IncentiveJustification = this.StringFormatter(r.JustificationForIncentive),
                              ConfidentialityLaw = this.StringFormatter(r.ConfidentialityLawCited),
                              VoluntaryConfidentialityStatementInstrument = this.StringFormatter(r.VCLanguageIC),
                              VoluntaryConfidentialityStatementContactMaterial = this.StringFormatter(r.VCLanguageLetters),
                              VoluntaryConfidentialityStatementFaq = this.StringFormatter(r.VCLanguageFaq),
                              VoluntaryConfidentialityStatementBrochure = this.StringFormatter(r.VCLanguageBrochure),
                              FollowUpInformedConsentStatement = this.StringFormatter(r.FICLanguage),
                              FollowUpInformedConsentLocation = this.StringFormatter(r.FICLanguageLocations),
                              PaperworkReductionActStatement = this.StringFormatter(r.PRAStatement),
                              PaperworkReductionActLocation = this.StringFormatter(r.PRAStatementLocations)
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

            var result = Respondents.FirstOrDefault(item => item.Id == id.Value);
            if (result == default(RespondentViewModel))
            {
                return NotFound();
            }

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
            if (!queryFrames.Any() || !Respondents.Any())
            {
                return BadRequest();
            }

            var results = Enumerable.Empty<RespondentStubViewModel>();
            foreach (var qf in queryFrames)
            {
                IQueryable<RespondentStubViewModel> inter;
                bool flag;
                switch (qf.Key)
                {
                    case "collection":
                        inter = from item in Respondents
                                where item.Collection != default(CollectionStubViewModel) && this.IsStringValid(item.Collection.Name) && item.Collection.Name.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "type":
                        inter = from item in Respondents
                                where this.IsStringValid(item.Type) && item.Type.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "description":
                        inter = from item in Respondents
                                where this.IsStringValid(item.Description) && item.Description.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "keywords":
                        inter = from item in Respondents
                                where this.IsStringValid(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "voluntary":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.Voluntary == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "mandatory":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.Mandatory == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "requiredforbenefits":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.RequiredForBenefits == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "requirementdescription":
                        inter = from item in Respondents
                                where !string.IsNullOrWhiteSpace(item.RequirementDescription) && item.RequirementDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "requirementreason":
                        inter = from item in Respondents
                                where this.IsStringValid(item.RequirementReason) && item.RequirementReason.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "populationsizeestimated":
                        if (qf.Value.Contains("|"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('|');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.PopulationSizeEstimated >= value0 && item.PopulationSizeEstimated <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.PopulationSizeEstimated == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "populationsize":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.PopulationSize >= value0 && item.PopulationSize <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.PopulationSize == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "populationsizedescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.PopulationSizeDescription) && item.PopulationSizeDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "responsesizeestimated":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.ResponseSizeEstimated >= value0 && item.ResponseSizeEstimated <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.ResponseSizeEstimated == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "responsesize":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.ResponseSize >= value0 && item.ResponseSize <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.ResponseSize == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "responserateestimated":
                        if (qf.Value.Contains("~"))
                        {
                            double value0, value1;
                            var values = qf.Value.Split('~');
                            if (double.TryParse(values[0], out value0) && double.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.ResponseRateEstimated >= value0 && item.ResponseRateEstimated <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.ResponseRateEstimated == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "responserate":
                        if (qf.Value.Contains("~"))
                        {
                            double value0, value1;
                            var values = qf.Value.Split('~');
                            if (double.TryParse(values[0], out value0) && double.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.ResponseRate >= value0 && item.ResponseRate <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.ResponseRate == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "responseratedescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ResponseRateDescription) && item.ResponseRateDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "burden":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.Burden >= value0 && item.Burden <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.Burden == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenperrespondent":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.BurdenPerRespondent >= value0 && item.BurdenPerRespondent <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.BurdenPerRespondent == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenperrespondentsurvey":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.BurdenPerRespondentSurvey >= value0 && item.BurdenPerRespondentSurvey <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.BurdenPerRespondentSurvey == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "burdenperrespondentassessment":
                        if (qf.Value.Contains("~"))
                        {
                            int value0, value1;
                            var values = qf.Value.Split('~');
                            if (int.TryParse(values[0], out value0) && int.TryParse(values[1], out value1) && value0 <= value1)
                            {
                                inter = from item in Respondents
                                        where item.BurdenPerRespondentAssessment >= value0 && item.BurdenPerRespondentAssessment <= value1
                                        select item.ToStub(true);
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
                                inter = from item in Respondents
                                        where item.BurdenPerRespondentAssessment == value
                                        select item.ToStub(true);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        break;

                    case "consentexplicit":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.ConsentExplicit == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "consentimplicit":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.ConsentImplicit == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "consentnotapplicable":
                        if (bool.TryParse(qf.Value, out flag))
                        {
                            inter = from item in Respondents
                                    where item.ConsentNotApplicable == flag
                                    select item.ToStub(true);
                        }
                        else
                        {
                            continue;
                        }
                        break;

                    case "consentform":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ConsentForm) && item.ConsentForm.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "population":
                        inter = from item in Respondents
                                where this.IsStringValid(item.Population) && item.Population.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "populationdescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.PopulationDescription) && item.PopulationDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "responsetype":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ResponseType) && item.ResponseType.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "responsetypdescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ResponseTypeDescription) && item.ResponseTypeDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "responsemode":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ResponseMode) && item.ResponseMode.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "responsemodedescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ResponseModeDescription) && item.ResponseModeDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "additionallanguageinstrument":
                        inter = from item in Respondents
                                where this.IsStringValid(item.AdditionalLanguageInstrument) && item.AdditionalLanguageInstrument.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "additionallanguageinterpreter":
                        inter = from item in Respondents
                                where this.IsStringValid(item.AdditionalLanguageInterpreter) && item.AdditionalLanguageInterpreter.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "incentivecashvalue":
                        inter = from item in Respondents
                                where this.IsStringValid(item.IncentiveCashValue) && item.IncentiveCashValue.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "incentivecashdescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.IncentiveCashDescription) && item.IncentiveCashDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "incentivenoncashvalue":
                        inter = from item in Respondents
                                where this.IsStringValid(item.IncentiveNonCashValue) && item.IncentiveNonCashValue.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "incentivenoncashdescription":
                        inter = from item in Respondents
                                where this.IsStringValid(item.IncentiveNonCashDescription) && item.IncentiveNonCashDescription.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "incentivejustification":
                        inter = from item in Respondents
                                where this.IsStringValid(item.IncentiveJustification) && item.IncentiveJustification.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "confidentialitylaw":
                        inter = from item in Respondents
                                where this.IsStringValid(item.ConfidentialityLaw) && item.ConfidentialityLaw.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "voluntaryconfidentialitystatementinstrument":
                        inter = from item in Respondents
                                where this.IsStringValid(item.VoluntaryConfidentialityStatementInstrument) && item.VoluntaryConfidentialityStatementInstrument.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "voluntaryconfidentialitystatementcontactmaterial":
                        inter = from item in Respondents
                                where this.IsStringValid(item.VoluntaryConfidentialityStatementContactMaterial) && item.VoluntaryConfidentialityStatementContactMaterial.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "voluntaryconfidentialitystatementfaq":
                        inter = from item in Respondents
                                where this.IsStringValid(item.VoluntaryConfidentialityStatementFaq) && item.VoluntaryConfidentialityStatementFaq.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "voluntaryconfidentialitystatementbrochure":
                        inter = from item in Respondents
                                where this.IsStringValid(item.VoluntaryConfidentialityStatementBrochure) && item.VoluntaryConfidentialityStatementBrochure.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "followupinformedconsentstatement":
                        inter = from item in Respondents
                                where this.IsStringValid(item.FollowUpInformedConsentStatement) && item.FollowUpInformedConsentStatement.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "followupinformedconsentlocation":
                        inter = from item in Respondents
                                where this.IsStringValid(item.FollowUpInformedConsentLocation) && item.FollowUpInformedConsentLocation.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "paperworkreductionactstatement":
                        inter = from item in Respondents
                                where this.IsStringValid(item.PaperworkReductionActStatement) && item.PaperworkReductionActStatement.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
                        break;

                    case "paperworkreductionactlocation":
                        inter = from item in Respondents
                                where this.IsStringValid(item.PaperworkReductionActLocation) && item.PaperworkReductionActLocation.ToLowerInvariant().Contains(qf.Value)
                                select item.ToStub(true);
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
                            results = results.Intersect(inter.ToList(), new RespondentStubViewModelComparer());
                        }
                        break;

                    case BooleanOperator.Or:
                        if (results.Any())
                        {
                            results = results.Union(inter.ToList(), new RespondentStubViewModelComparer());
                        }
                        else
                        {
                            results = inter.ToList();
                        }
                        break;

                    case BooleanOperator.Not:
                        if (results.Any())
                        {
                            results = results.Except(inter.ToList(), new RespondentStubViewModelComparer());
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

            IEnumerable<string> terms = query.ToLowerInvariant()
                                        .Split(' ');
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

            var respondents = Respondents.ToList();
            if (!respondents.Any())
            {
                return BadRequest();
            }

            var results =
                terms.SelectMany(term => from item in respondents
                                         where (item.Collection != default(CollectionStubViewModel) && !string.IsNullOrWhiteSpace(item.Collection.Name) && item.Collection.Name.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Type) && item.Type.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Description) && item.Description.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Keywords) && item.Keywords.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.RequirementDescription) && item.RequirementDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.RequirementReason) && item.RequirementReason.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PopulationSizeDescription) && item.PopulationSizeDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ResponseRateDescription) && item.ResponseRateDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ConsentForm) && item.ConsentForm.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.Population) && item.Population.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PopulationDescription) && item.PopulationDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ResponseType) && item.ResponseType.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ResponseTypeDescription) && item.ResponseTypeDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ResponseMode) && item.ResponseMode.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ResponseModeDescription) && item.ResponseModeDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.AdditionalLanguageInstrument) && item.AdditionalLanguageInstrument.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.AdditionalLanguageInterpreter) && item.AdditionalLanguageInterpreter.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.IncentiveCashValue) && item.IncentiveCashValue.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.IncentiveCashDescription) && item.IncentiveCashDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.IncentiveNonCashValue) && item.IncentiveNonCashValue.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.IncentiveNonCashDescription) && item.IncentiveNonCashDescription.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.IncentiveJustification) && item.IncentiveJustification.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.ConfidentialityLaw) && item.ConfidentialityLaw.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatementInstrument) && item.VoluntaryConfidentialityStatementInstrument.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatementContactMaterial) && item.VoluntaryConfidentialityStatementContactMaterial.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatementFaq) && item.VoluntaryConfidentialityStatementFaq.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.VoluntaryConfidentialityStatementBrochure) && item.VoluntaryConfidentialityStatementBrochure.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.FollowUpInformedConsentStatement) && item.FollowUpInformedConsentStatement.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.FollowUpInformedConsentLocation) && item.FollowUpInformedConsentLocation.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PaperworkReductionActStatement) && item.PaperworkReductionActStatement.ToLowerInvariant().Contains(term))
                                             || (!string.IsNullOrWhiteSpace(item.PaperworkReductionActLocation) && item.PaperworkReductionActLocation.ToLowerInvariant().Contains(term))
                                         select item.ToStub(true))
                .GroupBy(item => item, new RespondentStubViewModelComparer())
                .OrderBy(g => g.Count())
                .Select(g => g.Key);

            ViewBag.queryItems = string.Join("; ", terms);

            return View("SearchLimited", results);
        }
    }
}