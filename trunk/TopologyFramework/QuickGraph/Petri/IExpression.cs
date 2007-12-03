using System;
using System.Collections.Generic;

namespace Topology.Graph.Petri
{
	public interface IExpression<Token>
	{
		IList<Token> Eval(IList<Token> marking);
	}
}
