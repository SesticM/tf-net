using System;
using System.Collections.Generic;

namespace Topology.Graph.Petri
{
	public sealed class IdentityExpression<Token>  :IExpression<Token>
	{
		public IList<Token> Eval(IList<Token> marking)
		{
			return marking;
		}
	}
}
