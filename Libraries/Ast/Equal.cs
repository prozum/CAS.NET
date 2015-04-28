﻿using System;

namespace Ast
{
    public class Equal : Operator
    {
        public Equal() : base("=", 0) { }
        public Equal(Expression left, Expression right) : base(left, right, "=", 0) { }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Equal(Left.Reduce(this), Right.Reduce(this));
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Equal(Left.Expand(), Right.Expand());
        }

        public override Expression Clone()
        {
            return new Equal(Left.Clone(), Right.Clone());
        }

        public override Expression CurrectOperator()
        {
            return new Equal(Left.CurrectOperator(), Right.CurrectOperator());
        }
    }
}

