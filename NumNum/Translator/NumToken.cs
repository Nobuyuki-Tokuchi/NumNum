using System;
using System.Collections.Generic;
using System.Text;

namespace NumNum.Translator
{
    class NumToken
    {
        public NumOperator Operator { get; set; }
        public uint Value { get; set; }

        public override string ToString()
        {
            return $"Token(Operator: {Operator.ToString()}, Value: {Value})";
        }
    }
}
