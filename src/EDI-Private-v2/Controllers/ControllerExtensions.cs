using System;
using System.Collections.Generic;
using System.Linq;
using EDIPrivate.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDIPrivate.Controllers
{
    internal static class ControllerExtensions
    {
        internal static readonly IEnumerable<DateTime> DateTimeEscapes = new[] { new DateTime(1000, 01, 01), new DateTime(8888, 12, 31), new DateTime(9999, 12, 31) };
        internal static readonly IEnumerable<double> DoubleEscapes = new[] { 0.0, -0.01, -0.08, -0.09 };
        internal static readonly IEnumerable<int> IntegerEscapes = new[] { 0, -1, -8, -9 };
        internal static readonly IEnumerable<string> StringEscapes = new[] { "None", "TBD", "Unavailable", "NA" };

        internal static string NilQuery(this Controller controller) => "cmp=nil&";

        internal static bool IsStringValid(this Controller controller, string str) =>
            !string.IsNullOrWhiteSpace(str) && !StringEscapes.Contains(str);

        internal static bool BooleanFormatter(this Controller controller, bool? b) =>
            b.HasValue ? b.Value : false;

        internal static DateTime? DateTimeFormatter(this Controller controller, DateTime? date) =>
            date.HasValue && !DateTimeEscapes.Contains(date.Value) ? date : null;

        internal static double? DoubleFormatter(this Controller controller, double? x) =>
            x.HasValue && !DoubleEscapes.Contains(x.Value) ? x : null;

        internal static int? IntegerFormatter(this Controller controller, int? x) =>
            x.HasValue && !IntegerEscapes.Contains(x.Value) ? x : null;

        internal static string StringFormatter(this Controller controller, string str) =>
            controller.IsStringValid(str) ? str : null;

        internal static IEnumerable<QueryFrame> YieldQueryFrames(this Controller controller, IEnumerable<string> queryItems)
        {
            for (int i = 0; i < queryItems.Count(); i += 4)
            {
                yield return new QueryFrame(
                    key: queryItems.ElementAt(i + 2),
                    value: queryItems.ElementAt(i + 3),
                    op: queryItems.ElementAt(i + 1));
            }
        }
    }
}