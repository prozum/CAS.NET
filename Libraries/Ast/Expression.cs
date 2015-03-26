﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ast
{
	public abstract class Expression 
	{
        public Dictionary<string, Expression> definitions;
		public Operator parent;
		public abstract Expression Evaluate ();

		//public abstract string ToString ();
		//public abstract bool Contains (Expression a);

        #region Add
        public static Expression operator +(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Sub
        public static Expression operator -(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Mul
        public static Expression operator *(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Div
        public static Expression operator /(Expression a, Expression b)
        {
            throw new NotImplementedException();
        }
        #endregion
	}
}