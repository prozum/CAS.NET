﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class Simplify : SysFunc
    {
        public Simplify(List<Expression> args, Scope scope)
            : base("simplify", args, scope)
        {
            validArgs = new List<ArgKind>()
                {
                    ArgKind.Expression
                };
        }

        public override Expression Evaluate()
        {
            if (!isArgsValid())
                return new ArgError(this);

            return args[0].Simplify();
        }

        public override Expression Clone()
        {
            throw new NotImplementedException();
        }
    }
}

