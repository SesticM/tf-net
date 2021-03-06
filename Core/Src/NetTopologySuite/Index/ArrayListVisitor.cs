using System;
using System.Collections;
using System.Text;

namespace Topology.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class ArrayListVisitor : IItemVisitor
    {
        private ArrayList items = new ArrayList();
        
        /// <summary>
        /// 
        /// </summary>
        public ArrayListVisitor() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void VisitItem(object item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public ArrayList Items
        {
            get
            {
                return items;
            }
        }
    }
}
