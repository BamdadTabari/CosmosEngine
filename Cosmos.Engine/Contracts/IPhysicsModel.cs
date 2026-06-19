using Cosmos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Engine.Contracts
{
    public interface IPhysicsModel
    {
        void Step(
            Universe universe,
            double deltaTime);
    }
}
