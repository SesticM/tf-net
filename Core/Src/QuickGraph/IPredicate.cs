using System;

namespace Topology.Graph
{
    public interface IPredicate<T>
    {
        bool Test(T t);
    }
}
