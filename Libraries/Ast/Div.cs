﻿using System;

namespace Ast
{
    public class Div : Operator, IInvertable
    {
        public Div() : base("/", 50) { }
        public Div(Expression left, Expression right) : base(left, right, "/", 50) { }

        protected override Expression Evaluate(Expression caller)
        {
            return Left / Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            if (left is Operator && (left as Operator).priority < priority)
            {
                if (left is Add)
                {
                    return new Add(new Div((left as Operator).Left, right).Reduce(), new Div((left as Operator).Right, right).Reduce());
                }
                else if (left is Sub)
                {
                    return new Sub(new Div((left as Operator).Left, right).Reduce(), new Div((left as Operator).Right, right).Reduce());
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else if (right is Operator && (right as Operator).priority < priority)
            {
                if (right is Add)
                {
                    return new Add(new Div((right as Operator).Left, left).Reduce(), new Div((right as Operator).Right, Left).Reduce());
                }
                else if (right is Sub)
                {
                    return new Sub(new Div((right as Operator).Left, left).Reduce(), new Div((right as Operator).Right, Left).Reduce());
                }
                else
                {
                    return new Div(left.Expand(), right.Expand());
                }
            }
            else
            {
                return new Div(left.Expand(), right.Expand());
            }
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            if (right is Number && right.CompareTo(Constant.One))
            {
                return left;
            }
            else if (left is Number && right is Number)
            {
                return left / right;
            }
            else if (left is Div)
            {
                return new Div((left as Div).Left, new Mul((left as Div).Right, right));
            }
            else if (right is Div)
            {
                return new Div(new Mul(left, (right as Div).Right), (right as Div).Left);
            }
            else if ((left is Exp && right is Exp) && (left as Exp).Left.CompareTo((right as Exp).Left))
            {
                return new Exp((left as Exp).Left, new Sub((left as Exp).Right, (right as Exp).Right));
            }
            else if (left is Variable && right is Variable && CompareVariables(left as Variable, right as Variable))
            {
                return VariableOperation(left as Variable, right as Variable);
            }

            return new Div(left, right);
        }

        private bool CompareVariables(Variable left, Variable right)
        {
            return left.identifier == right.identifier && left.GetType() == right.GetType();
        }

        private Expression VariableOperation(Variable left, Variable right)
        {
            Expression res;

            if (((left.exponent < right.exponent) as Boolean).value)
            {
                var symbol = right.Clone();

                (symbol as Variable).exponent = (right.exponent - left.exponent) as Number;
                res = new Div(left.prefix, symbol);
            }
            else if (((left.exponent > right.exponent) as Boolean).value)
            {
                var symbol = right.Clone();

                (symbol as Variable).exponent = (left.exponent - right.exponent) as Number;
                res = new Div(symbol, right.prefix);
            }
            else
            {
                res = left.prefix / right.prefix;
            }

            return res;
        }

        public override Expression Clone()
        {
            return new Div(Left.Clone(), Right.Clone());
        }

        public Expression Inverted(Expression other)
        {
            return new Mul(other, Right);
        }

        public override Expression CurrectOperator()
        {
            return new Div(Left.CurrectOperator(), Right.CurrectOperator());
        }

        public bool IsNegative()
        {
            return DefaultIsNegative();
        }

        public Expression ToNegative()
        {
            throw new NotImplementedException();
        }
    }
}

