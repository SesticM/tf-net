using System;

namespace Topology.Graph.Petri
{
	/// <summary>
	/// A directed edge of a net which may connect a <see cref="Place"/>
	/// to a <see cref="Transition"/> or a <see cref="Transition"/> to
	/// a <see cref="Place"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Usually represented by an arrow.
	/// </para>
	/// </remarks>
	public interface IArc<Token>  : IEdge<IPetriVertex>
	{
		/// <summary>
        /// Gets or sets a value indicating if the <see cref="Arc"/>
		/// instance is a <strong>input arc.</strong>
		/// </summary>
		/// <remarks>
		/// <para>
		/// An arc that leads from an input <see cref="Place"/> to a
		/// <see cref="Transition"/> is called an <em>Input Arc</em> of
		/// the transition.
		/// </para>
		/// </remarks>
		bool IsInputArc {get;}

		/// <summary>
		/// Gets or sets the <see cref="Place"/> instance attached to the
		/// <see cref="Arc"/>.
		/// </summary>
		/// <value>
		/// The <see cref="Place"/> attached to the <see cref="Arc"/>.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		IPlace<Token> Place {get;}

		/// <summary>
		/// Gets or sets the <see cref="Transition"/> instance attached to the
		/// <see cref="Arc"/>.
		/// </summary>
		/// <value>
		/// The <see cref="Transition"/> attached to the <see cref="Arc"/>.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		ITransition<Token> Transition{get;}

		/// <summary>
		/// Gets or sets the arc annotation.
		/// </summary>
		/// <value>
		/// The <see cref="IExpression"/> annotation instance.
		/// </value>
		/// <remarks>
		/// <para>
		/// An expression that may involve constans, variables and operators
		/// used to annotate the arc. The expression evaluates over the type
		/// of the arc's associated place.
		/// </para>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference (Nothing in Visual Basic).
		/// </exception>
		IExpression<Token> Annotation {get;set;}
	}
}
