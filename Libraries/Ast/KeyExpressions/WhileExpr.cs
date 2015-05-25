﻿using System;

namespace Ast
{
    public class WhileExpr : Expression
    {
        public Expression Condition;
        public Expression Expression;

        readonly int MaxIterations = 10000;

        public WhileExpr(Scope scope)
        {
            CurScope = scope;
        }

        public override Expression Evaluate()
        {
            int i = 0;

            while (i++ < MaxIterations)
            {
                var res = Condition.ReduceEvaluate();

                if (CurScope.Error)
                    return Constant.Null;

                if (res is Boolean)
                {
                    if (!(res as Boolean).@bool)
                        break;
                }

                Expression.Evaluate();

                if (CurScope.Error)
                    return Constant.Null;
            }

            if (i > MaxIterations)
                CurScope.Errors.Add(new ErrorData("while: Overflow!"));

            return Constant.Null;

        }

        public override string ToString()
        {
            return Expression.ToString();
        }
    }
}

