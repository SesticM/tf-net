using System;
using System.Collections.Generic;

namespace Topology.Graph.Petri
{
    [Serializable]
    public sealed class AllwaysTrueConditionExpression<Token> : IConditionExpression<Token>
    {
		public bool IsEnabled(IList<Token> tokens)
		{
			return true;
		}
	}
}
