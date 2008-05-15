using System;
using System.Collections.Generic;

namespace Topology.Graph.Petri
{
	public interface IConditionExpression<Token>
	{
		bool IsEnabled(IList<Token> tokens);
	}
}
