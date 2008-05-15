﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Topology.Geometries
{
    public interface ILineString : ICurve
    {
        IPoint GetPointN(int n);

        ICoordinate GetCoordinateN(int n);

        ILineString Reverse();
    }
}
