﻿using System;
using System.Collections.Generic;

namespace Ast
{
    public class Evaluator : Scope
    {
        private Parser _parser = new Parser();

        public Evaluator () : this(null) {}
        public Evaluator (string parseString)
        {
            if (parseString != null)
                Parse(parseString);
            SetVar("deg", new Boolean(true));
            SetVar("debug", new Boolean(true));
            SetVar("pi", new Irrational((decimal)Math.PI));
        }

        public void Parse(string parseString)
        {
            statements.Clear();
            _parser.Parse(parseString, this);
        }

    }
}