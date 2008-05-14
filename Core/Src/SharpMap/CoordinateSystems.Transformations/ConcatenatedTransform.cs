namespace Topology.CoordinateSystems.Transformations
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    internal class ConcatenatedTransform : MathTransform
    {
        private List<ICoordinateTransformation> _CoordinateTransformationList;
        /// <summary>
        /// 
        /// </summary>
        protected IMathTransform _inverse;

        /// <summary>
        /// 
        /// </summary>
        public ConcatenatedTransform() : this(new List<ICoordinateTransformation>())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformlist"></param>
        public ConcatenatedTransform(List<ICoordinateTransformation> transformlist)
        {
            this._CoordinateTransformationList = transformlist;
        }

        /// <summary>
        /// Returns the inverse of this conversion.
        /// </summary>
        /// <returns>IMathTransform that is the reverse of the current conversion.</returns>
        public override IMathTransform Inverse()
        {
            if (this._inverse == null)
            {
                this._inverse = new ConcatenatedTransform(this._CoordinateTransformationList);
                this._inverse.Invert();
            }
            return this._inverse;
        }

        /// <summary>
        /// Reverses the transformation
        /// </summary>
        public override void Invert()
        {
            this._CoordinateTransformationList.Reverse();
            foreach (ICoordinateTransformation transformation in this._CoordinateTransformationList)
            {
                transformation.MathTransform.Invert();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override double[] Transform(double[] point)
        {
            double[] numArray = (double[]) point.Clone();
            foreach (ICoordinateTransformation transformation in this._CoordinateTransformationList)
            {
                numArray = transformation.MathTransform.Transform(numArray);
            }
            return numArray;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public override List<double[]> TransformList(List<double[]> points)
        {
            List<double[]> list = new List<double[]>(points.Count);
            foreach (ICoordinateTransformation transformation in this._CoordinateTransformationList)
            {
                list = transformation.MathTransform.TransformList(list);
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ICoordinateTransformation> CoordinateTransformationList
        {
            get
            {
                return this._CoordinateTransformationList;
            }
            set
            {
                this._CoordinateTransformationList = value;
                this._inverse = null;
            }
        }

        /// <summary>
        /// Gets a Well-Known text representation of this object.
        /// </summary>
        /// <value></value>
        public override string WKT
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        /// <summary>
        /// Gets an XML representation of this object.
        /// </summary>
        /// <value></value>
        public override string XML
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}

