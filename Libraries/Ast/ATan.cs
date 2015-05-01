﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class ATan : SysFunc, IInvertable
    {
        public ATan() : this(null, null) { }
        public ATan(List<Expression> args, Scope scope)
            : base("atan", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        protected override Expression Evaluate(Expression caller)
        {
            if (!isArgsValid())
                return new ArgError(this);

            var res = args[0].Evaluate();

            var degrees = (Boolean)scope.GetVar("deg");
            if (degrees == null)
                degrees = new Boolean(false);

            if (res is Real)
            {
                return ReturnValue(new Irrational(Math.Atan((double) ((degrees ? Constant.DegToRad.Value  : 1) * (res as Real).Value) ))).Evaluate();
            }

            return new Error(this, "Could not take ATan of: " + args[0]);
        }

        internal override Expression Reduce(Expression caller)
        {
            return ReduceHelper<ATan>();
        }

        public override Expression Clone()
        {
            return MakeClone<ATan>();
        }

        public Expression Inverted(Expression other)
        {
            List<Expression> newArgs = new List<Expression>();
            newArgs.Add(other);
            return new Tan(newArgs, scope);
        }
    }
}

