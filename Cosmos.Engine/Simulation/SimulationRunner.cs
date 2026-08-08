using Cosmos.Domain.Entities;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Models;
using Cosmos.Engine.Tracking;
using System.Text;

namespace Cosmos.Engine.Simulation;

public sealed class SimulationRunner
{
    private readonly IPhysicsModel _physicsModel;
    private readonly OrbitalTracker _orbitalTracker;
    private readonly ManeuverExecutionSystem _maneuverExecutionSystem;

    private double _accumulator;
    private double _timeScale = 1;

    public double FixedDeltaTime { get; }
    public double SimulationTime { get; private set; }
    public double TimeScale
    {
        get => _timeScale;

        set
        {
            if (value < 0 ||
                double.IsNaN(value) ||
                double.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "TimeScale must be a finite, non-negative number.");
            }

            _timeScale = value;
        }
    }
    public bool IsPaused { get; set; }

    public SimulationRunner(
        IPhysicsModel physicsModel,
        OrbitalTracker orbitalTracker,
        ManeuverExecutionSystem maneuverExecutionSystem,
        double fixedDeltaTime)
    {
        if (fixedDeltaTime <= 0 ||
            double.IsNaN(fixedDeltaTime) ||
            double.IsInfinity(fixedDeltaTime))
        {
            throw new ArgumentOutOfRangeException(
                nameof(fixedDeltaTime));
        }

        _physicsModel = physicsModel;
        _orbitalTracker = orbitalTracker;
        _maneuverExecutionSystem = maneuverExecutionSystem;

        FixedDeltaTime = fixedDeltaTime;
    }

    public void Update(
     Universe universe,
     double realDeltaTime,
     Body? burnTarget,
     ManeuverPlan? currentPlan,
     BurnExecutionState burnState)
    {
        if (IsPaused)
        {
            return;
        }

        if (realDeltaTime < 0 ||
            double.IsNaN(realDeltaTime) ||
            double.IsInfinity(realDeltaTime))
        {
            throw new ArgumentOutOfRangeException(
                nameof(realDeltaTime));
        }


        // _accumulator یعنی زمان جمع‌شده‌ای که هنوز به یک گام کامل شبیه‌سازی تبدیل نشده است.
        //مثلاً اگر:
        //FixedDeltaTime = 0.001;
        //realDeltaTime = 0.0069;
        //        Runner شش گام کامل اجرا می‌کند:
        //6×0.001 = 0.006
        //و باقی‌مانده را نگه می‌دارد:
        //        Accumulator = 0.0009
        // -----------          -----------          -----------
        // Convert elapsed real time into elapsed simulation time.
        // Rendering speed must never determine the laws of the universe.
        _accumulator += realDeltaTime * TimeScale;

        // Advance the universe through deterministic, fixed-size steps.
        // Every subsystem experiences exactly the same amount of time.
        while (_accumulator >= FixedDeltaTime)
        {
            _physicsModel.Step(
                universe,
                FixedDeltaTime);

            if (burnTarget is not null &&
                currentPlan is not null)
            {
                _maneuverExecutionSystem.Update(
                    burnTarget,
                    currentPlan,
                    FixedDeltaTime,
                    burnState);
            }

            _orbitalTracker.Update(
                universe.Bodies);

            SimulationTime += FixedDeltaTime;
            _accumulator -= FixedDeltaTime;
        }
    }
}