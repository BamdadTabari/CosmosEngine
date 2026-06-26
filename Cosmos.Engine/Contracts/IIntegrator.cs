using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Contracts
{
    public interface IIntegrator
    {
        void Integrate(
            Body body,
            Vector3D acceleration,
            double deltaTime);
    }
}
