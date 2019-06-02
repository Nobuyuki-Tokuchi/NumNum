using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NumNum.Translator
{
    class NumToLua : NumTranslator
    {
        const string INDENT = "  ";
        private readonly StringBuilder indent;
        private StreamWriter writer;

        public NumToLua() : base()
        {
            this.indent = new StringBuilder();
        }

        protected override void Preprocess()
        {
            this.writer = new StreamWriter(this.OutputStream);

            this.writer.WriteLine("local stack = {}");
            this.writer.WriteLine("local variables = {}");
            this.writer.WriteLine("local temporary = nil");
        }
        protected override void Postprocess()
        {
            this.writer.Close();
        }

        protected override void Count()
        {
            this.writer.WriteLine("{0}table.insert(stack, #stack)", this.indent);
        }

        protected override void Pop()
        {
            this.writer.WriteLine("{0}table.remove(stack)", this.indent);
        }

        protected override void Copy()
        {
            this.writer.WriteLine("{0}table.insert(stack, stack[#stack])", this.indent);
        }

        protected override void Swap()
        {
            this.writer.WriteLine("{0}stack[#stack - 1], stack[#stack] = stack[#stack], stack[#stack - 1]", this.indent);
        }

        protected override void Over()
        {
            this.writer.WriteLine("{0}table.insert(stack, stack[#stack - 1])", this.indent);
        }

        protected override void Increment()
        {
            this.writer.WriteLine("{0}stack[#stack] = (stack[#stack] + 1) & 0xffffffff", this.indent);
        }

        protected override void Decrement()
        {
            this.writer.WriteLine("{0}stack[#stack] = (stack[#stack] - 1) & 0xffffffff", this.indent);
        }

        protected override void Not()
        {
            this.writer.WriteLine("{0}stack[#stack] = ~stack[#stack] & 0xffffffff", this.indent);
        }

        protected override void RotateDown()
        {
            this.writer.WriteLine("{0}stack[#stack - 2], stack[#stack - 1], stack[#stack] =  stack[#stack - 1], stack[#stack], stack[#stack - 2]", this.indent);
        }

        protected override void RotateUp()
        {
            this.writer.WriteLine("{0}stack[#stack - 2], stack[#stack - 1], stack[#stack] =  stack[#stack], stack[#stack - 2], stack[#stack - 1]", this.indent);
        }

        protected override void And()
        {
            BiOperator("&");
        }

        protected override void Or()
        {
            BiOperator("|");
        }

        protected override void Xor()
        {
            BiOperator("~");
        }

        protected override void ShiftRight()
        {
            BiOperator(">>");
        }

        protected override void ShiftLeft()
        {
            BiOperator("<<");
        }

        protected override void Add()
        {
            BiOperator("+");
        }

        protected override void Subtract()
        {
            BiOperator("-");
        }

        protected override void Multiply()
        {
            BiOperator("*");
        }

        protected override void Divide()
        {
            BiOperator("//");
        }

        protected override void Remainder()
        {
            BiOperator("%");
        }

        private void BiOperator(string op)
        {
            this.writer.WriteLine("{0}temporary = table.remove(stack)", this.indent);
            this.writer.WriteLine("{0}stack[#stack] = (stack[#stack] {1} temporary) & 0xffffffff", this.indent, op);
        }

        protected override void GreaterThan()
        {
            CompareOperator(">");
        }

        protected override void LessThan()
        {
            CompareOperator("<");
        }

        protected override void Equal()
        {
            CompareOperator("==");
        }

        protected override void NotEqual()
        {
            CompareOperator("~=");
        }

        protected override void Test()
        {
            TestOperator("==");
        }

        protected override void GreaterThanOrEqual()
        {
            CompareOperator(">=");
        }

        protected override void LessThanOrEqual()
        {
            CompareOperator("<=");
        }

        protected override void EqualZero()
        {
            CheckZero("==");
        }

        protected override void NotEqualZero()
        {
            CheckZero("~=");
        }

        protected override void TestNot()
        {
            TestOperator("~=");
        }

        private void CompareOperator(string op)
        {
            this.writer.WriteLine("{0}temporary = table.remove(stack)", this.indent);
            this.writer.WriteLine("{0}if stack[#stack] {1} temporary then stack[#stack] = 1 else stack[#stack] = 0 end", this.indent, op);
        }

        private void TestOperator(string op)
        {
            this.writer.WriteLine("{0}if stack[#stack - 1] {1} stack[#stack] then temporary = 1 else temporary = 0 end", this.indent, op);
            this.writer.WriteLine("{0}table.insert(stack, temporary)", this.indent);
        }

        private void CheckZero(string op)
        {
            this.writer.WriteLine("{0}if stack[#stack] {1} 0 then stack[#stack] = 1 else stack[#stack] = 0 end", this.indent, op);
        }

        protected override void IfNotZero()
        {
            this.writer.WriteLine("{0}if stack[#stack] ~= 0 then", this.indent);
            this.indent.Append(INDENT);
        }

        protected override void IfZero()
        {
            this.writer.WriteLine("{0}if stack[#stack] == 0 then", this.indent);
        }

        protected override void EndIf()
        {
            this.indent.Remove(0, 2);
            this.writer.WriteLine("{0}end", this.indent);
        }

        protected override void WhileNotZero()
        {
            this.writer.WriteLine("{0}while stack[#stack] ~= 0 do", this.indent);
            this.indent.Append(INDENT);
        }

        protected override void EndWhile()
        {
            this.indent.Remove(0, 2);
            this.writer.WriteLine("{0}end", this.indent);
        }

        protected override void Constant8Bit(uint value)
        {
            SetValue(value);
        }

        protected override void Constant16Bit(uint value)
        {
            SetValue(value);
        }

        protected override void Constant32Bit(uint value)
        {
            SetValue(value);
        }

        private void SetValue(uint value)
        {
            this.writer.WriteLine("{0}table.insert(stack, {1})", this.indent, value);
        }

        protected override void InputCharacter()
        {
            this.writer.WriteLine("{0}table.insert(stack, string.byte(io.read(1)))", this.indent);
        }

        protected override void OutputCharacter()
        {
            this.writer.WriteLine("{0}io.write(string.char(table.remove(stack)))", this.indent);
        }

        protected override void StoreVariable(uint value)
        {
            this.writer.WriteLine("{0}variables[{1}] = table.remove(stack)", this.indent);
        }

        protected override void CopyVariable(uint value)
        {
            this.writer.WriteLine("{0}variables[{1}] = stack[#stack]", this.indent);
        }

        protected override void LoadVariable(uint value)
        {
            this.writer.WriteLine("{0}table.insert(stack, variables[{1}])", this.indent);
        }

        protected override void InputInteger()
        {
            this.writer.WriteLine("{0}temporary = io.read([=[n]=])", this.indent);
            this.writer.WriteLine("{0}if temporary == nil then", this.indent);
            this.writer.WriteLine("{0}{1}error([=[invalid number]=])", this.indent, INDENT);
            this.writer.WriteLine("{0}end", this.indent);
            this.writer.WriteLine("{0}table.insert(stack, temporary)", this.indent);
        }

        protected override void OutputInteger()
        {
            this.writer.WriteLine("{0}io.write(table.remove(stack))", this.indent);
        }
    }
}
