using Cosmos.Desktop;
using Cosmos.Desktop.Input;
using Cosmos.Desktop.Loaders;
using Cosmos.Desktop.Rendering;
using Cosmos.Desktop.Styles;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Calculators;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Integrators;
using Cosmos.Engine.Maneuvers;
using Cosmos.Engine.Services;
using Cosmos.Engine.Simulation;
using Cosmos.Engine.Tracking;
using Raylib_cs;
using static Raylib_cs.Raylib;

// TODO:
// HI 
// Im not insane
// all of these will be refactored

const int width = 1080; //1920;
const int height = 920; //1080;


IIntegrator integrator =
    new SemiImplicitEulerIntegrator();

IPhysicsModel physics =
    new NewtonianPhysicsModel(
        integrator);

var orbitalTracker =
    new OrbitalTracker();

var state =
    new SimulationState();

var inputHandler =
    new InputHandler();

var styleLoader =
    new StyleLoader();

var loader =
    new UniverseLoader();

var executionSystem =
    new ManeuverExecutionSystem();

var simulationRunner =
    new SimulationRunner(
        physics,
        orbitalTracker,
        executionSystem,
        fixedDeltaTime: 0.001);

var universe =
    loader.Load(
        "DataFiles/solar-system.json");

var styleConfig =
    styleLoader.Load(
        "DataFiles/body-styles.json");



var hudRenderer =
    new HudRenderer();

Dictionary<Guid, Queue<Vector3D>>
    Trails = [];


SetConfigFlags(
    ConfigFlags.Msaa4xHint |
    ConfigFlags.VSyncHint |
    ConfigFlags.HighDpiWindow |
    ConfigFlags.ResizableWindow);

InitWindow(
    width,
    height,
    "Cosmos Engine");

var renderer =
    new UniverseRenderer(
        new PlanetStyleProvider(
            styleConfig));

SetTraceLogLevel(TraceLogLevel.None);


SetTargetFPS(60);
//SetTargetFPS(144);
DisableCursor();

var sun =
universe
    .FindBody("Sun");

var earth = universe.FindBody("Earth");

var orbitCalculator =
    new CircularOrbitCalculator();

var shipPosition =
    earth.Position +
    new Vector3D(
        15,
        0,
        0);

var radius =
    (shipPosition - sun.Position)
    .Magnitude();

var orbitalVelocity =
    orbitCalculator.Calculate(
        sun.Mass.Value,
        radius);

var tangent =
    new Vector3D(
        0,
        1,
        0);

var ship =
    new Spacecraft(
        shipPosition,

        tangent *
        orbitalVelocity,

        new Mass(0.01),

        "Explorer-1");

universe.AddBody(ship);

state.Camera.Target = sun;

var explorer =
    universe.FindBody(
        "Explorer-1");

state.ControlledBody =
    explorer;

while (!WindowShouldClose())
{
    inputHandler.Handle(
        state,
        universe);

    // Translate UI controls into simulation-clock settings.
    simulationRunner.IsPaused =
        state.Paused;

    simulationRunner.TimeScale =
        state.SimulationSpeed;

    // Rendering reports elapsed real time;
    // the runner converts it into deterministic simulation steps.
    simulationRunner.Update(
        universe,
        GetFrameTime(),
        state.BurnTarget,
        state.CurrentPlan,
        state.BurnState);

    BeginDrawing();

    renderer.Render(
        universe,
        state.Camera,
        Trails);

    hudRenderer.Render(
        universe,
        state.Camera,
        state.SimulationSpeed,
        orbitalTracker,
        state);

    EndDrawing();


}
CloseWindow();