using System;

namespace Topology.Graph
{
    /// <summary>
    /// Specialized exception to report unconnected vertices
    /// </summary>
    [Serializable]
    public sealed class VertexNotConnectedException : ApplicationException
    {
        /// <summary>
        /// 
        /// </summary>
        public VertexNotConnectedException() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public VertexNotConnectedException(string message) : base( message ) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public VertexNotConnectedException(string message, System.Exception inner) : base( message, inner ) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public VertexNotConnectedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base( info, context ) { }
    }
}
