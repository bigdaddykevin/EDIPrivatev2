using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDIPrivate.Controllers
{
    [Route("[controller]")]
    public sealed class SearchController : Controller
    {
        #region keys

        private static readonly SelectList BinaryOperatorKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "AND", Value = "AND" },
                new SelectListItem() { Text = "OR", Value = "OR" },
                new SelectListItem() { Text = "NOT", Value = "NOT" }
            },
            "Value",
            "Text");

        private static readonly SelectList SeriesKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Name", Value = "name" },
                new SelectListItem() { Text = "Symbol", Value = "symbol" },
                new SelectListItem() { Text = "Old Name 1", Value = "oldname1" },
                new SelectListItem() { Text = "Old Name 1 Symbol", Value = "oldname1symbol" },
                new SelectListItem() { Text = "Old Name 1 Duration", Value = "oldname1duration" },
                new SelectListItem() { Text = "Old Name 2", Value = "oldname2" },
                new SelectListItem() { Text = "Old Name 2 Symbol", Value = "oldname2symbol" },
                new SelectListItem() { Text = "Old Name 2 Duration", Value = "oldname2duration" },
                new SelectListItem() { Text = "Description", Value = "description" },
                new SelectListItem() { Text = "Study Names", Value = "studies" }
            },
            "Value",
            "Text");

        private static readonly SelectList StudyKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Name", Value = "name" },
                new SelectListItem() { Text = "Symbol", Value = "symbol" },
                new SelectListItem() { Text = "Old Name 1", Value = "oldname1" },
                new SelectListItem() { Text = "Old Name 1 Symbol", Value = "oldname1symbol" },
                new SelectListItem() { Text = "Old Name 1 Duration", Value = "oldname1duration" },
                new SelectListItem() { Text = "Old Name 2", Value = "oldname2" },
                new SelectListItem() { Text = "Old Name 2 Symbol", Value = "oldname2symbol" },
                new SelectListItem() { Text = "Old Name 2 Duration", Value = "oldname2duration" },
                new SelectListItem() { Text = "Parent Series Name", Value = "series" },
                new SelectListItem() { Text = "Investigator", Value = "investigator" },
                new SelectListItem() { Text = "Bureau Code", Value = "bureaucode" },
                new SelectListItem() { Text = "Program Code", Value = "programcode" },
                new SelectListItem() { Text = "Website", Value = "website" },
                new SelectListItem() { Text = "Keywords", Value = "keywords" },
                new SelectListItem() { Text = "Description", Value = "description" },
                new SelectListItem() { Text = "Authorizing Law", Value = "authorizinglaw" },
                new SelectListItem() { Text = "Total Cost", Value = "totalcost" },
                new SelectListItem() { Text = "Total Cost Years", Value = "totalcostyears" },
                new SelectListItem() { Text = "Total Cost Description", Value = "totalcostdescription" },
                new SelectListItem() { Text = "Universe Collection", Value = "universe" },
                new SelectListItem() { Text = "Sample Collection", Value = "sample" },
                new SelectListItem() { Text = "Longitudinal Collection", Value = "longitudinal" },
                new SelectListItem() { Text = "Cross-Sectional Collection", Value = "crosssectional" },
                new SelectListItem() { Text = "Program Monitoring Collection", Value = "programmonitoring" },
                new SelectListItem() { Text = "Grantee Reporting Collection", Value = "granteereporting" },
                new SelectListItem() { Text = "Voluntary Collection", Value = "voluntary" },
                new SelectListItem() { Text = "Mandatory Collection", Value = "mandatory" },
                new SelectListItem() { Text = "Required for Benefits Collection", Value = "requiredforbenefits" },
                new SelectListItem() { Text = "Collection Requirement Description", Value = "requirementdescription" },
                new SelectListItem() { Text = "Collection Requirement Reason", Value = "requirementreason" },
                new SelectListItem() { Text = "SORN", Value = "sorn" },
                new SelectListItem() { Text = "SORN URL", Value = "sornurl" },
                new SelectListItem() { Text = "Confidentiality Restrictions", Value = "confidentialityrestrictions" },
                new SelectListItem() { Text = "PII-DI", Value = "pii_di" },
                new SelectListItem() { Text = "PIA", Value = "pia" },
                new SelectListItem() { Text = "Data Could Be Public", Value = "datacouldbepublic" },
                new SelectListItem() { Text = "Statistics Publication Type", Value = "publicationstatisticstype" },
                new SelectListItem() { Text = "Statistics Publication URL", Value = "publicationstatisticsurl" },
                new SelectListItem() { Text = "Statistics Publication Date", Value = "publicationstatisticsdate" },
                new SelectListItem() { Text = "Data Publication URL", Value = "publicationdataurl" },
                new SelectListItem() { Text = "Data Publication Date", Value = "publicationdatadate" },
                new SelectListItem() { Text = "Restricted-Use Data Publication Date", Value = "publicationrestrictedusedatadate" },
                new SelectListItem() { Text = "Subject Population", Value = "subjectpopulation" },
                new SelectListItem() { Text = "Subject Population Description", Value = "subjectpopulationdescription" },
                new SelectListItem() { Text = "Data Levels Available", Value = "datalevelsavailable" },
                new SelectListItem() { Text = "Public Data Level", Value = "datalevelpublic" },
                new SelectListItem() { Text = "Data Level Description", Value = "dataleveldescription" },
                new SelectListItem() { Text = "Previous Study Name", Value = "previousstudy" },
                new SelectListItem() { Text = "Contact Names", Value = "contacts" },
                new SelectListItem() { Text = "Collection Names", Value = "collections" },
                new SelectListItem() { Text = "File Names", Value = "files" }
            },
            "Value",
            "Text");

        private static readonly SelectList CollectionKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Name", Value = "name" },
                new SelectListItem() { Text = "Parent Study Name", Value = "study" },
                new SelectListItem() { Text = "Collection Type", Value = "type" },
                new SelectListItem() { Text = "Cost", Value = "cost" },
                new SelectListItem() { Text = "Cost Years", Value = "costyears" },
                new SelectListItem() { Text = "Cost Description", Value = "costdescription" },
                new SelectListItem() { Text = "Recruitment Estimated Start Date", Value = "recruitmentstartdateestimated" },
                new SelectListItem() { Text = "Collection Estimated Start Date", Value = "startdateestimated" },
                new SelectListItem() { Text = "Collection Start Date", Value = "startdate" },
                new SelectListItem() { Text = "Collection Estimated End Date", Value = "enddateestimated" },
                new SelectListItem() { Text = "Collection End Date", Value = "enddate" },
                new SelectListItem() { Text = "Date Description", Value = "datedescription" },
                new SelectListItem() { Text = "Data Collection Agent Type", Value = "datacollectionagenttype" },
                new SelectListItem() { Text = "Primary Data Collection Agent", Value = "datacollectionagentprimary" },
                new SelectListItem() { Text = "Non-Primary Data Collection Agent", Value = "datacollectionagentnonprimary" },
                new SelectListItem() { Text = "Confidentiality Law", Value = "confidentialitylaw" },
                new SelectListItem() { Text = "Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatement" },
                new SelectListItem() { Text = "Respondent Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatementrespondent" },
                new SelectListItem() { Text = "Experiment Description", Value = "experimentdescription" },
                new SelectListItem() { Text = "Experiment Results", Value = "experimentresults" },
                new SelectListItem() { Text = "Respondent Names", Value = "respondents" },
                new SelectListItem() { Text = "Package Titles", Value = "packages" }
            },
            "Value",
            "Text");

        private static readonly SelectList PackageKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Title", Value = "title" },
                new SelectListItem() { Text = "ICRAS", Value = "icras" },
                new SelectListItem() { Text = "EDICS", Value = "edics" },
                new SelectListItem() { Text = "ICR Reference Number", Value = "icrreferencenumber" },
                new SelectListItem() { Text = "OMB Control Number", Value = "ombcontrolnumber" },
                new SelectListItem() { Text = "CFDA", Value = "cfda" },
                new SelectListItem() { Text = "Keywords", Value = "keywords" },
                new SelectListItem() { Text = "Abstract", Value = "abstract" },
                new SelectListItem() { Text = "Issue Date", Value = "issuedate" },
                new SelectListItem() { Text = "Expiration Date", Value = "expirationdate" },
                new SelectListItem() { Text = "Notice Type", Value = "noticetype" },
                new SelectListItem() { Text = "Terms of Clearance", Value = "termsofclearance" },
                new SelectListItem() { Text = "Number of Respondents", Value = "numberrespondents" },
                new SelectListItem() { Text = "Number of Responses", Value = "numberresponses" },
                new SelectListItem() { Text = "Percent Collected Electronically", Value = "percentcollectedelectronically" },
                new SelectListItem() { Text = "Total Burden", Value = "burdentotal" },
                new SelectListItem() { Text = "Burden Change", Value = "burdenchange" },
                new SelectListItem() { Text = "Burden Adjustment", Value = "burdenadjustment" },
                new SelectListItem() { Text = "Burden Change Explanation", Value = "burdenexplanation" },
                new SelectListItem() { Text = "Public Comment Document", Value = "publiccomment" },
                new SelectListItem() { Text = "Public Comment Response Document", Value = "publiccommentresponse" },
                new SelectListItem() { Text = "OMB Passback Document", Value = "ombpassback" },
                new SelectListItem() { Text = "Authorizing Law Cited", Value = "authorizinglawcited" },
                new SelectListItem() { Text = "Authorizing Law Text", Value = "authorizinglawtext" },
                new SelectListItem() { Text = "Contractor Confidentiality Form Location", Value = "contractorconfidentialityformlocation" },
                new SelectListItem() { Text = "Study Names", Value = "studies" }
            },
            "Value",
            "Text");

        private static readonly SelectList FileKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Name", Value = "name" },
                new SelectListItem() { Text = "Format", Value = "format" },
                new SelectListItem() { Text = "Dataset", Value = "dataset" },
                new SelectListItem() { Text = "Restriction", Value = "restriction" },
                new SelectListItem() { Text = "Location", Value = "location" },
                new SelectListItem() { Text = "Location Description", Value = "locationdescription" },
                new SelectListItem() { Text = "Study Names", Value = "studies" },
                new SelectListItem() { Text = "Element Names & Labels", Value = "elements" }
            },
            "Value",
            "Text");

        private static readonly SelectList RespondentKeys = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Parent Collection Name", Value = "collection" },
                new SelectListItem() { Text = "Type", Value = "type" },
                new SelectListItem() { Text = "Description", Value = "description" },
                new SelectListItem() { Text = "Keywords", Value = "keywords" },
                new SelectListItem() { Text = "Voluntary Response", Value = "voluntary" },
                new SelectListItem() { Text = "Mandatory Response", Value = "mandatory" },
                new SelectListItem() { Text = "Required for Benefits Response", Value = "requiredforbenefits" },
                new SelectListItem() { Text = "Response Requirement Description", Value = "requirementdescription" },
                new SelectListItem() { Text = "Response Requirement Reason", Value = "requirementreason" },
                new SelectListItem() { Text = "Estimated Population Size", Value = "populationsizeestimated" },
                new SelectListItem() { Text = "Population Size", Value = "populationsize" },
                new SelectListItem() { Text = "Population Size Description", Value = "populationsizedescription" },
                new SelectListItem() { Text = "Estimated Response Size", Value = "responsesizeestimated" },
                new SelectListItem() { Text = "Response Size", Value = "responsesize" },
                new SelectListItem() { Text = "Estimated Respones Rate", Value = "responserateestimated" },
                new SelectListItem() { Text = "Response Rate", Value = "responserate" },
                new SelectListItem() { Text = "Response Rate Description", Value = "responseratedescription" },
                new SelectListItem() { Text = "Burden", Value = "burden" },
                new SelectListItem() { Text = "Burden Per Respondent", Value = "burdenperrespondent" },
                new SelectListItem() { Text = "Survey Burden Per Respondent", Value = "burdenperrespondentsurvey" },
                new SelectListItem() { Text = "Assessment Burden Per Respondent", Value = "burdenperrespondentassessment" },
                new SelectListItem() { Text = "Explicit Consent", Value = "consentexplicit" },
                new SelectListItem() { Text = "Implicit Consent", Value = "consentimplicit" },
                new SelectListItem() { Text = "Consent Not Applicable", Value = "consentnotapplicable" },
                new SelectListItem() { Text = "Consent Form", Value = "consentform" },
                new SelectListItem() { Text = "Population", Value = "population" },
                new SelectListItem() { Text = "Population Description", Value = "populationdescription" },
                new SelectListItem() { Text = "Response Type", Value = "responsetype" },
                new SelectListItem() { Text = "Response Type Description", Value = "responsetypdescription" },
                new SelectListItem() { Text = "Response Mode", Value = "responsemode" },
                new SelectListItem() { Text = "Response Mode Description", Value = "responsemodedescription" },
                new SelectListItem() { Text = "Instrument Additional Language", Value = "additionallanguageinstrument" },
                new SelectListItem() { Text = "Interpreter Additional Language", Value = "additionallanguageinterpreter" },
                new SelectListItem() { Text = "Cash Incentive Value", Value = "incentivecashvalue" },
                new SelectListItem() { Text = "Cash Incentive Description", Value = "incentivecashdescription" },
                new SelectListItem() { Text = "Non-Cash Incentive Value", Value = "incentivenoncashvalue" },
                new SelectListItem() { Text = "Non-Cash Incentive Description", Value = "incentivenoncashdescription" },
                new SelectListItem() { Text = "Incentive Justification", Value = "incentivejustification" },
                new SelectListItem() { Text = "Confidentiality Law", Value = "confidentialitylaw" },
                new SelectListItem() { Text = "Instrument Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatementinstrument" },
                new SelectListItem() { Text = "Contact Material Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatementcontactmaterial" },
                new SelectListItem() { Text = "FAQ Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatementfaq" },
                new SelectListItem() { Text = "Brochure Voluntary & Confidentiality Statement", Value = "voluntaryconfidentialitystatementbrochure" },
                new SelectListItem() { Text = "Follow-Up Informed Consent Statement", Value = "followupinformedconsentstatement" },
                new SelectListItem() { Text = "Follow-Up Informed Consent Statement Location", Value = "followupinformedconsentlocation" },
                new SelectListItem() { Text = "Paperwork Reduction Act Statement", Value = "paperworkreductionactstatement" },
                new SelectListItem() { Text = "Paperwork Reduction Act Statement Location", Value = "paperworkreductionactlocation" }
            },
            "Value",
            "Text");

        #endregion keys

        // GET: /<controller>/
        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            ViewData["cmp"] = BinaryOperatorKeys;
            ViewData["seriesKeys"] = SeriesKeys;
            ViewData["studyKeys"] = StudyKeys;
            ViewData["collectionKeys"] = CollectionKeys;
            ViewData["respondentKeys"] = RespondentKeys;
            ViewData["packageKeys"] = PackageKeys;
            ViewData["fileKeys"] = FileKeys;

            return View();
        }
    }
}