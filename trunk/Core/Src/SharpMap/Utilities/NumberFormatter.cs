namespace Topology.Utilities
{
    using System;
    using System.Globalization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class NumberFormatter
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly NumberFormatter formatter = new NumberFormatter();
        private NumberFormatInfo nfi = new NumberFormatInfo();

        /// <summary>
        /// 
        /// </summary>
        private NumberFormatter()
        {
            this.nfi.NumberDecimalSeparator = ".";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NumberFormatInfo GetNfi()
        {
            return formatter.nfi;
        }
    }
}

