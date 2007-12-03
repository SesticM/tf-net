namespace Topology.Converters.WellKnownText
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    /// <summary>
    /// Converts spatial reference IDs to a Well-Known Text representation.
    /// </summary>
    public class SpatialReference
    {
        /// <summary>
        /// Converts a Spatial Reference ID to a Well-known Text representation
        /// </summary>
        /// <param name="srid">Spatial Reference ID</param>
        /// <returns>Well-known text</returns>
        public static string SridToWkt(int srid)
        {
            XmlDocument document = new XmlDocument();
            string filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + @"\SpatialRefSys.xml";
            document.Load(filename);
            XmlNode node = document.DocumentElement.SelectSingleNode("/SpatialReference/ReferenceSystem[SRID='" + srid.ToString() + "']");
            if (node != null)
            {
                return node.LastChild.InnerText;
            }
            return "";
        }
    }
}

