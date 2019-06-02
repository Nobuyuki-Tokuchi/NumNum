using System;
using System.Collections.Generic;
using System.Text;

namespace NumNum.Translator
{
    class NumInterpreter : NumTranslator
    {
        private readonly Stack<uint> stack;
        private readonly Dictionary<uint, uint> variables;
        private readonly List<InterpreterOperator> tokens;
        private readonly Stack<InterpreterOperator> controlStack;

        public NumInterpreter() : base()
        {
            this.stack = new Stack<uint>();
            this.variables = new Dictionary<uint, uint>();
            this.tokens = new List<InterpreterOperator>();
            this.controlStack = new Stack<InterpreterOperator>();
        }

        protected override void Preprocess()
        {
        }

        protected override void Postprocess()
        {
            int length = tokens.Count;

            this.controlStack.Clear();

            for (int i = 0; i < length; i++)
            {
                InterpreterOperator op = this.tokens[i];

                switch (op.Operator)
                {
                    case NumOperator.COUNT:
                        this.stack.Push((uint)this.stack.Count);
                        break;
                    case NumOperator.POP:
                        if(this.stack.Count < 1)
                        {
                            throw new ApplicationException("empty stack");
                        }
                        else
                        {
                            this.stack.Pop();
                        }
                        break;
                    case NumOperator.COPY:
                        if(this.stack.TryPeek(out uint result))
                        {
                            this.stack.Push(result);
                        }
                        else
                        {
                            throw new ApplicationException("empty stack");
                        }
                        break;
                    case NumOperator.SWAP:
                        if(this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(top);
                            this.stack.Push(next);
                        }
                        break;
                    case NumOperator.OVER:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Peek();

                            this.stack.Push(top);
                            this.stack.Push(next);
                        }
                        break;
                    case NumOperator.INCREMENT:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            this.stack.Push(top + 1);
                        }
                        break;
                    case NumOperator.DECREMENT:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            this.stack.Push(top - 1);
                        }
                        break;
                    case NumOperator.NOT:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            this.stack.Push(~top);
                        }
                        break;
                    case NumOperator.ROTATE_DOWN:
                        if (this.stack.Count < 3)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();
                            uint bottom = this.stack.Pop();

                            this.stack.Push(next);
                            this.stack.Push(top);
                            this.stack.Push(bottom);
                        }
                        break;
                    case NumOperator.ROTATE_UP:
                        if (this.stack.Count < 3)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();
                            uint bottom = this.stack.Pop();

                            this.stack.Push(top);
                            this.stack.Push(bottom);
                            this.stack.Push(next);
                        }
                        break;
                    case NumOperator.AND:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next & top);
                        }
                        break;
                    case NumOperator.OR:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next | top);
                        }
                        break;
                    case NumOperator.XOR:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next ^ top);
                        }
                        break;
                    case NumOperator.SHIFT_RIGHT:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next >> (int)top);
                        }
                        break;
                    case NumOperator.SHIFT_LEFT:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next << (int)top);
                        }
                        break;
                    case NumOperator.ADD:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next + top);
                        }
                        break;
                    case NumOperator.SUBTRACT:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next - top);
                        }
                        break;
                    case NumOperator.MULTIPLY:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next * top);
                        }
                        break;
                    case NumOperator.DIVIDE:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next / top);
                        }
                        break;
                    case NumOperator.REMAINDER:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next % top);
                        }
                        break;
                    case NumOperator.GREATER_THAN:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next > top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.LESS_THAN:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next < top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.EQUAL:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next == top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.NOT_EQUAL:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next != top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.TEST:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Peek();

                            this.stack.Push(top);
                            this.stack.Push(next == top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.GREATER_THAN_OR_EQUAL:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next >= top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.LESS_THAN_OR_EQUAL:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Pop();

                            this.stack.Push(next <= top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.EQUAL_ZERO:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            this.stack.Push(top == 0 ? 1U : 0U);
                        }
                        break;
                    case NumOperator.NOT_EQUAL_ZERO:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            this.stack.Push(top != 0 ? 1U : 0U);
                        }
                        break;
                    case NumOperator.TEST_NOT:
                        if (this.stack.Count < 2)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();
                            uint next = this.stack.Peek();

                            this.stack.Push(top);
                            this.stack.Push(next != top ? 1U : 0U);
                        }
                        break;
                    case NumOperator.IF_NOT_ZERO:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Peek();

                            if(top == 0)
                            {
                                i = op.JumpPosition;
                            }
                        }
                        break;
                    case NumOperator.IF_ZERO:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Peek();

                            if (top != 0)
                            {
                                i = op.JumpPosition;
                            }
                        }
                        break;
                    case NumOperator.END_IF:
                        break;
                    case NumOperator.WHILE_NOT_ZERO:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Peek();

                            if (top == 0)
                            {
                                i = op.JumpPosition;
                            }
                        }
                        break;
                    case NumOperator.END_WHILE:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Peek();

                            if (top != 0)
                            {
                                i = op.JumpPosition;
                            }
                        }
                        break;
                    case NumOperator.CONSTANT_8_BIT:
                    case NumOperator.CONSTANT_16_BIT:
                    case NumOperator.CONSTANT_32_BIT:
                        this.stack.Push(op.Value);
                        break;
                    case NumOperator.INPUT_CHARACTER:
                        this.stack.Push((uint)Console.Read());
                        break;
                    case NumOperator.OUTPUT_CHARACTER:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            Console.Write(char.ConvertFromUtf32((int)top));
                        }
                        break;
                    case NumOperator.STORE_VARIABLE:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            this.variables[op.Value] = this.stack.Pop();
                        }
                        break;
                    case NumOperator.COPY_VAIRABLE:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            this.variables[op.Value] = this.stack.Peek();
                        }
                        break;
                    case NumOperator.LOAD_VARIABLE:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            this.stack.Push(this.variables[op.Value]);
                        }
                        break;
                    case NumOperator.INPUT_INTEGER:
                        string str = Console.ReadLine();
                        if (uint.TryParse(str, out uint value)) {
                            this.stack.Push(value);
                        }
                        else
                        {
                            throw new ApplicationException($"invalid value: \"{str}\"");
                        }
                        break;
                    case NumOperator.OUTPUT_INTEGER:
                        if (this.stack.Count < 1)
                        {
                            throw new ApplicationException("insufficient in stack");
                        }
                        else
                        {
                            uint top = this.stack.Pop();

                            Console.Write(top);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void Count()
        {
            AddToken(NumOperator.COUNT);
        }

        protected override void Pop()
        {
            AddToken(NumOperator.POP);
        }

        protected override void Copy()
        {
            AddToken(NumOperator.COPY);
        }

        protected override void Swap()
        {
            AddToken(NumOperator.SWAP);
        }

        protected override void Over()
        {
            AddToken(NumOperator.OVER);
        }

        protected override void Increment()
        {
            AddToken(NumOperator.INCREMENT);
        }

        protected override void Decrement()
        {
            AddToken(NumOperator.DECREMENT);
        }

        protected override void Not()
        {
            AddToken(NumOperator.NOT);
        }

        protected override void RotateDown()
        {
            AddToken(NumOperator.ROTATE_DOWN);
        }

        protected override void RotateUp()
        {
            AddToken(NumOperator.ROTATE_UP);
        }

        protected override void And()
        {
            AddToken(NumOperator.AND);
        }

        protected override void Or()
        {
            AddToken(NumOperator.OR);
        }

        protected override void Xor()
        {
            AddToken(NumOperator.XOR);
        }

        protected override void ShiftRight()
        {
            AddToken(NumOperator.SHIFT_RIGHT);
        }

        protected override void ShiftLeft()
        {
            AddToken(NumOperator.SHIFT_LEFT);
        }

        protected override void Add()
        {
            AddToken(NumOperator.ADD);
        }

        protected override void Subtract()
        {
            AddToken(NumOperator.SUBTRACT);
        }

        protected override void Multiply()
        {
            AddToken(NumOperator.MULTIPLY);
        }

        protected override void Divide()
        {
            AddToken(NumOperator.DIVIDE);
        }

        protected override void Remainder()
        {
            AddToken(NumOperator.REMAINDER);
        }

        protected override void GreaterThan()
        {
            AddToken(NumOperator.GREATER_THAN);
        }

        protected override void LessThan()
        {
            AddToken(NumOperator.LESS_THAN);
        }

        protected override void Equal()
        {
            AddToken(NumOperator.EQUAL);
        }

        protected override void NotEqual()
        {
            AddToken(NumOperator.NOT_EQUAL);
        }

        protected override void Test()
        {
            AddToken(NumOperator.TEST);
        }

        protected override void GreaterThanOrEqual()
        {
            AddToken(NumOperator.GREATER_THAN_OR_EQUAL);
        }

        protected override void LessThanOrEqual()
        {
            AddToken(NumOperator.LESS_THAN_OR_EQUAL);
        }

        protected override void EqualZero()
        {
            AddToken(NumOperator.EQUAL_ZERO);
        }

        protected override void NotEqualZero()
        {
            AddToken(NumOperator.NOT_EQUAL_ZERO);
        }

        protected override void TestNot()
        {
            AddToken(NumOperator.TEST_NOT);
        }

        protected override void IfNotZero()
        {
            var op = new InterpreterOperator
            {
                Operator = NumOperator.IF_NOT_ZERO,
            };

            this.controlStack.Push(op);
            AddToken(op);
        }

        protected override void IfZero()
        {
            var op = new InterpreterOperator
            {
                Operator = NumOperator.IF_NOT_ZERO,
            };

            this.controlStack.Push(op);
            AddToken(op);
        }

        protected override void EndIf()
        {
            bool success = this.controlStack.TryPop(out InterpreterOperator ifop);

            if (success && (ifop.Operator == NumOperator.IF_NOT_ZERO || ifop.Operator == NumOperator.IF_ZERO))
            {
                ifop.JumpPosition = this.tokens.Count;
                AddToken(NumOperator.END_IF);
            }
            else
            {
                throw new ApplicationException($"invalid operator: {NumOperator.END_IF}");
            }
        }

        protected override void WhileNotZero()
        {
            var op = new InterpreterOperator
            {
                Operator = NumOperator.WHILE_NOT_ZERO,
            };

            // EndWhileで使用するために自分の位置を設定しておく
            op.JumpPosition = this.tokens.Count;

            this.controlStack.Push(op);
            AddToken(op);
        }

        protected override void EndWhile()
        {
            bool success = this.controlStack.TryPop(out InterpreterOperator whileop);

            if (success && whileop.Operator == NumOperator.WHILE_NOT_ZERO)
            {
                // WhileNotZeroにてWhileNotZeroの位置を設定しているので取得する
                int pos = whileop.JumpPosition;

                whileop.JumpPosition = this.tokens.Count;
                AddToken(new InterpreterOperator
                {
                    Operator = NumOperator.END_WHILE,
                    JumpPosition = pos,
                });
            }
            else
            {
                throw new ApplicationException($"invalid operator: {NumOperator.END_WHILE}");
            }
        }

        protected override void Constant8Bit(uint value)
        {
            AddToken(NumOperator.CONSTANT_8_BIT, value);
        }

        protected override void Constant16Bit(uint value)
        {
            AddToken(NumOperator.CONSTANT_16_BIT, value);
        }

        protected override void Constant32Bit(uint value)
        {
            AddToken(NumOperator.CONSTANT_32_BIT, value);
        }

        protected override void InputCharacter()
        {
            AddToken(NumOperator.INPUT_CHARACTER);
        }

        protected override void OutputCharacter()
        {
            AddToken(NumOperator.OUTPUT_CHARACTER);
        }

        protected override void StoreVariable(uint value)
        {
            AddToken(NumOperator.STORE_VARIABLE, value);
        }

        protected override void CopyVariable(uint value)
        {
            AddToken(NumOperator.COPY_VAIRABLE, value);
        }

        protected override void LoadVariable(uint value)
        {
            AddToken(NumOperator.LOAD_VARIABLE, value);
        }

        protected override void InputInteger()
        {
            AddToken(NumOperator.INPUT_INTEGER);
        }

        protected override void OutputInteger()
        {
            AddToken(NumOperator.OUTPUT_INTEGER);
        }

        private void AddToken(NumOperator op, uint value = 0)
        {
            AddToken(new InterpreterOperator
            {
                Operator = op,
                Value = value,
            });
        }

        private void AddToken(InterpreterOperator iop)
        {
            this.tokens.Add(iop);
        }
    }

    class InterpreterOperator : NumToken
    {
        public int JumpPosition { get; set; }
    }
}
