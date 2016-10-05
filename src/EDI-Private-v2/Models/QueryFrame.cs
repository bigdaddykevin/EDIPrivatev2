using System;
using System.Linq;

namespace EDIPrivate.Models
{
    internal enum BooleanOperator
    {
        Nil,
        And,
        Or,
        Not
    };

    internal sealed class QueryFrame
    {
        internal string Key { get; }
        internal string Value { get; }
        internal BooleanOperator Operator { get; }

        internal QueryFrame(string key, string value, string op)
        {
            if (new[] { key, value, op }.Any(str => string.IsNullOrWhiteSpace(str)))
            {
                throw new ArgumentNullException();
            }

            BooleanOperator parseVal;
            if (!Enum.TryParse(op, true, out parseVal))
            {
                parseVal = BooleanOperator.Nil;
            }

            Key = key;
            Value = value;
            Operator = parseVal;
        }
    }
}