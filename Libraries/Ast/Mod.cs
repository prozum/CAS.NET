﻿using System;

namespace Ast
{
    public class Mod : BinaryOperator
    {
        public override string Identifier { get { return "%"; } }
        public override int Priority { get{ return 40; } }

        public Mod() { }
        public Mod(Expression left, Expression right) : base(left, right) { }

        internal override Expression Evaluate(Expression caller)
        {
            return Left % Right;
        }

        protected override Expression ExpandHelper(Expression left, Expression right)
        {
            return new Mod(left, right);
        }

        protected override Expression ReduceHelper(Expression left, Expression right)
        {
            return new Mod(left, right);
        }
    }
}

