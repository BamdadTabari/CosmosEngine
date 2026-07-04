using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Domain.Navigation
{
    public interface INavigationTarget
    {
        string Name { get; }

        Vector3D Position { get; }

        Vector3D Velocity { get; }

        ReferenceContext ReferenceContext { get; }
    }
}
