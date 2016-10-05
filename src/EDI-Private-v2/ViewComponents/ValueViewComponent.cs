using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIPrivate.Models;
using EDIPrivate.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EDIPrivate.ViewComponents
{
    [Route("[controller]")]
    [ResponseCache(Duration = 300)]
    public sealed class ValueViewComponent : ViewComponent
    {
        private readonly DataContext Context;

        public ValueViewComponent(DataContext context)
        {
            if (context == default(DataContext))
            {
                throw new ArgumentNullException();
            }

            Context = context;
        }

        public IEnumerable<ValueViewModel> GetValue(Guid id)
        {
            Predicate<string> isStringValid = str => !string.IsNullOrWhiteSpace(str) && !new[] { "None", "TBD", "Unavailable", "NA", "<null>" }.Contains(str);
            Func<string, string> stringFormatter = str => isStringValid(str) ? str : null;

            return (from fe in Context.TblFELink
                    where fe.Id == id
                    join ev in Context.TblEVLink on fe.Id equals ev.FELinkId
                    join v in Context.TblValue on ev.ValueId equals v.Id
                    where isStringValid(v.Option) || isStringValid(v.Label)
                    select new ValueViewModel()
                    {
                        Option = isStringValid(v.Option) ? stringFormatter(v.Option) : string.Empty,
                        Label = isStringValid(v.Label) ? stringFormatter(v.Label) : string.Empty
                    })
                   .ToList();
        }

        public Task<IEnumerable<ValueViewModel>> GetValueAsync(Guid id) =>
            Task.FromResult(GetValue(id));

        public async Task<IViewComponentResult> InvokeAsync(Guid? id = null)
        {
            if (!id.HasValue)
            {
                throw new ArgumentNullException();
            }

            var results = await GetValueAsync(id.Value);
            if (!results.Any())
            {
                return View("EmptyResult");
            }

            return View("Default", results);
        }
    }
}