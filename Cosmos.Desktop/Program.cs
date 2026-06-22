using Cosmos.Desktop;
using Cosmos.Domain.Entities;
using Cosmos.Domain.Structs;
using Cosmos.Domain.ValueObjects;
using Cosmos.Engine.Contracts;
using Cosmos.Engine.Services;
using Raylib_cs;
using static Raylib_cs.Raylib;


IPhysicsModel physics =
    new NewtonianPhysicsModel();

var state =
    new SimulationState();

var inputHandler =
    new InputHandler();


var renderer =
    new UniverseRenderer();

//var hudRenderer =
//    new HudRenderer();

var universe = new Universe();

var giant = new Body(
    new Vector3D(0, 0, 0),
    new Vector3D(0, 0, 0),
    new Mass(1000000),
    "Sun");

var alpha = new Body(
    new Vector3D(90, 0, 0),
    new Vector3D(0, 330, 0),
    new Mass(200),
    "Alpha");

var alphaMoon = new Body(
    new Vector3D(102, 0, 0),
    new Vector3D(0, 395, 0),
    new Mass(156),
    "AlphaMoon");

var beta = new Body(
    new Vector3D(150, 0, 60),
    new Vector3D(0, 255, 0),
    new Mass(150),
    "Beta");

var betaMoon1 = new Body(
    new Vector3D(164, 0, 60),
    new Vector3D(0, 305, 0),
    new Mass(49),
    "BetaMoon1");

var betaMoon2 = new Body(
    new Vector3D(170, 0, 60),
    new Vector3D(0, 315, 0),
    new Mass(456),
    "BetaMoon2");

var gamma = new Body(
    new Vector3D(240, 0, -80),
    new Vector3D(0, 205, 0),
    new Mass(569),
    "Gamma");

var delta = new Body(
    new Vector3D(340, 0, 100),
    new Vector3D(0, 175, 0),
    new Mass(250),
    "Delta");

var deltaMoon = new Body(
    new Vector3D(356, 0, 100),
    new Vector3D(0, 220, 0),
    new Mass(666),
    "DeltaMoon");

var epsilon = new Body(
    new Vector3D(470, 0, -140),
    new Vector3D(0, 145, 0),
    new Mass(500),
    "Epsilon");

var epsilonMoon1 = new Body(
    new Vector3D(486, 0, -140),
    new Vector3D(0, 180, 0),
    new Mass(1356),
    "EpsilonMoon1");

var epsilonMoon2 = new Body(
    new Vector3D(493, 0, -140),
    new Vector3D(0, 188, 0),
    new Mass(114),
    "EpsilonMoon2");

var epsilonMoon3 = new Body(
    new Vector3D(501, 0, -140),
    new Vector3D(0, 196, 0),
    new Mass(113),
    "EpsilonMoon3");


universe.AddBody(giant);

universe.AddBody(alpha);
universe.AddBody(alphaMoon);

universe.AddBody(beta);
universe.AddBody(betaMoon1);
universe.AddBody(betaMoon2);

universe.AddBody(gamma);

universe.AddBody(delta);
universe.AddBody(deltaMoon);

universe.AddBody(epsilon);
universe.AddBody(epsilonMoon1);
universe.AddBody(epsilonMoon2);
universe.AddBody(epsilonMoon3);

Dictionary<Guid, Queue<Vector3D>>
    Trails = [];

InitWindow(1280, 720, "Cosmos Engine");

SetTargetFPS(60);


state.Camera.Target = giant;

state.Camera.Distance = 800;

state.Camera.AngleX = 0.8;

state.Camera.AngleY = 0.35;



while (!WindowShouldClose())
{

    for (int i = 0; i < 100; i++)
    {
        physics.Step(universe, 0.001);
    }

    inputHandler.Handle(
    state,
    giant,
    alpha,
    beta,
    gamma,
    delta,
    epsilon);

    renderer.Render(
    universe,
    state.Camera,
    Trails);

    //hudRenderer.Render(
    //state.Camera,
    //state.SimulationSpeed,
    //state.Paused);

}
CloseWindow();