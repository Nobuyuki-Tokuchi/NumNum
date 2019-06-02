using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NumNum.Translator
{
    public abstract class NumTranslator
    {
        private readonly List<NumToken> tokens;

        public Stream OutputStream { get; set; }
        public bool ShowTokens { get; set; }

        protected NumTranslator()
        {
            tokens = new List<NumToken>();
        }

        public void Compile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                Compile(reader);
            }
        }

        public void Compile(FileStream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                Compile(reader);
            }
        }

        public void CompileCode(string code)
        {
            using(var reader = new StringReader(code))
            {
                Compile(reader);
            }
        }

        public void Compile(TextReader reader)
        {
            Read(reader);
            if(this.ShowTokens)
            {
                Console.WriteLine("[{0}]", string.Join(",", this.tokens));
            }
            Translate();
        }

        private void Read(TextReader reader)
        {
            StringBuilder builder = new StringBuilder();
            int ch;
            int count = -1;
            NumToken token = null;

            while ((ch = reader.Read()) != -1)
            {
                string str = char.ConvertFromUtf32(ch);

                if (uint.TryParse(str, out uint result))
                {
                    builder.Append(result);

                    if(count > 1)
                    {
                        count--;
                    }
                    else if (count == 1)
                    {
                        if(token == null)
                        {
                            throw new InvalidOperationException("Not found token");
                        }
                        string valueString = builder.ToString();

                        for (int i = 0; i < valueString.Length; i++)
                        {
                            token.Value = token.Value * 4 + (uint)(valueString[i] - '0');
                        }

                        count = -1;
                        builder.Clear();
                    }
                    else
                    {
                        if (result >= 0 && result <= 4)
                        {
                            token = new NumToken
                            {
                                Operator = (NumOperator)uint.Parse(builder.ToString()),
                            };

                            this.tokens.Add(token);

                            switch (token.Operator)
                            {
                                case NumOperator.CONSTANT_8_BIT:
                                    count = 4;
                                    break;
                                case NumOperator.CONSTANT_16_BIT:
                                case NumOperator.STORE_VARIABLE:
                                case NumOperator.COPY_VAIRABLE:
                                case NumOperator.LOAD_VARIABLE:
                                    count = 8;
                                    break;
                                case NumOperator.CONSTANT_32_BIT:
                                    count = 16;
                                    break;
                                default:
                                    break;
                            }

                            builder.Clear();
                        }
                    }
                }
            }
        }

        private void Translate()
        {
            Preprocess();
            foreach (var token in this.tokens)
            {
                switch (token.Operator)
                {
                    case NumOperator.COUNT:
                        Count();
                        break;
                    case NumOperator.POP:
                        Pop();
                        break;
                    case NumOperator.COPY:
                        Copy();
                        break;
                    case NumOperator.SWAP:
                        Swap();
                        break;
                    case NumOperator.OVER:
                        Over();
                        break;
                    case NumOperator.INCREMENT:
                        Increment();
                        break;
                    case NumOperator.DECREMENT:
                        Decrement();
                        break;
                    case NumOperator.NOT:
                        Not();
                        break;
                    case NumOperator.ROTATE_DOWN:
                        RotateDown();
                        break;
                    case NumOperator.ROTATE_UP:
                        RotateUp();
                        break;
                    case NumOperator.AND:
                        And();
                        break;
                    case NumOperator.OR:
                        Or();
                        break;
                    case NumOperator.XOR:
                        Xor();
                        break;
                    case NumOperator.SHIFT_RIGHT:
                        ShiftRight();
                        break;
                    case NumOperator.SHIFT_LEFT:
                        ShiftLeft();
                        break;
                    case NumOperator.ADD:
                        Add();
                        break;
                    case NumOperator.SUBTRACT:
                        Subtract();
                        break;
                    case NumOperator.MULTIPLY:
                        Multiply();
                        break;
                    case NumOperator.DIVIDE:
                        Divide();
                        break;
                    case NumOperator.REMAINDER:
                        Remainder();
                        break;
                    case NumOperator.GREATER_THAN:
                        GreaterThan();
                        break;
                    case NumOperator.LESS_THAN:
                        LessThan();
                        break;
                    case NumOperator.EQUAL:
                        Equal();
                        break;
                    case NumOperator.NOT_EQUAL:
                        NotEqual();
                        break;
                    case NumOperator.TEST:
                        Test();
                        break;
                    case NumOperator.GREATER_THAN_OR_EQUAL:
                        GreaterThanOrEqual();
                        break;
                    case NumOperator.LESS_THAN_OR_EQUAL:
                        LessThanOrEqual();
                        break;
                    case NumOperator.EQUAL_ZERO:
                        EqualZero();
                        break;
                    case NumOperator.NOT_EQUAL_ZERO:
                        NotEqualZero();
                        break;
                    case NumOperator.TEST_NOT:
                        TestNot();
                        break;
                    case NumOperator.IF_NOT_ZERO:
                        IfNotZero();
                        break;
                    case NumOperator.IF_ZERO:
                        IfZero();
                        break;
                    case NumOperator.END_IF:
                        EndIf();
                        break;
                    case NumOperator.WHILE_NOT_ZERO:
                        WhileNotZero();
                        break;
                    case NumOperator.END_WHILE:
                        EndWhile();
                        break;
                    case NumOperator.BREAK:
                        Break();
                        break;
                    case NumOperator.CONSTANT_8_BIT:
                        Constant8Bit(token.Value);
                        break;
                    case NumOperator.CONSTANT_16_BIT:
                        Constant16Bit(token.Value);
                        break;
                    case NumOperator.CONSTANT_32_BIT:
                        Constant32Bit(token.Value);
                        break;
                    case NumOperator.INPUT_CHARACTER:
                        InputCharacter();
                        break;
                    case NumOperator.OUTPUT_CHARACTER:
                        OutputCharacter();
                        break;
                    case NumOperator.STORE_VARIABLE:
                        StoreVariable(token.Value);
                        break;
                    case NumOperator.COPY_VAIRABLE:
                        CopyVariable(token.Value);
                        break;
                    case NumOperator.LOAD_VARIABLE:
                        LoadVariable(token.Value);
                        break;
                    case NumOperator.INPUT_INTEGER:
                        InputInteger();
                        break;
                    case NumOperator.OUTPUT_INTEGER:
                        OutputInteger();
                        break;
                }
            }

            Postprocess();
        }

        protected abstract void Preprocess();
        protected abstract void Postprocess();

        protected abstract void Count();
        protected abstract void Pop();
        protected abstract void Copy();
        protected abstract void Swap();
        protected abstract void Over();
        protected abstract void Increment();
        protected abstract void Decrement();
        protected abstract void Not();
        protected abstract void RotateDown();
        protected abstract void RotateUp();
        protected abstract void And();
        protected abstract void Or();
        protected abstract void Xor();
        protected abstract void ShiftRight();
        protected abstract void ShiftLeft();
        protected abstract void Add();
        protected abstract void Subtract();
        protected abstract void Multiply();
        protected abstract void Divide();
        protected abstract void Remainder();
        protected abstract void GreaterThan();
        protected abstract void LessThan();
        protected abstract void Equal();
        protected abstract void NotEqual();
        protected abstract void Test();
        protected abstract void GreaterThanOrEqual();
        protected abstract void LessThanOrEqual();
        protected abstract void EqualZero();
        protected abstract void NotEqualZero();
        protected abstract void TestNot();
        protected abstract void IfNotZero();
        protected abstract void IfZero();
        protected abstract void EndIf();
        protected abstract void WhileNotZero();
        protected abstract void EndWhile();
        protected abstract void Break();
        protected abstract void Constant8Bit(uint value);
        protected abstract void Constant16Bit(uint value);
        protected abstract void Constant32Bit(uint value);
        protected abstract void InputCharacter();
        protected abstract void OutputCharacter();
        protected abstract void StoreVariable(uint value);
        protected abstract void CopyVariable(uint value);
        protected abstract void LoadVariable(uint value);
        protected abstract void InputInteger();
        protected abstract void OutputInteger();
    }
}
