# Cosmos Engine 🚀

<img width="1281" height="718" alt="Cosmos Engine 3D simulation" src="https://github.com/user-attachments/assets/e666bb4b-2f72-4da0-931d-6067666da88e" />

**Cosmos Engine** is an open-source C#/.NET project for learning orbital mechanics, numerical simulation, and computational physics through implementation.

The project is not intended to be only a visual space simulator. Its primary purpose is to understand the mathematics and physics behind each feature before turning that knowledge into documented, tested software.

> Knowledge First → Question → Understanding → Documentation → Implementation → Verification

## Current Capabilities

### Physics and Numerical Simulation

- Newtonian N-body gravitational interaction
- Plummer gravitational softening for close encounters
- Semi-implicit Euler integration
- Deterministic fixed-timestep simulation
- Time scaling and pause control
- Shared-state, two-phase acceleration calculation
- Normalized simulation-unit system

### Orbital Mechanics

- Circular-orbit calculations
- Escape-velocity calculations
- Orbital-energy calculations
- Orbital statistics and tracking
- Hohmann-transfer planning
- Maneuver execution experiments

### Scientific Verification

The test suite verifies both software behavior and scientific expectations:

- Fixed-timestep accumulation
- Pause and time-scale behavior
- Invalid and non-finite numerical inputs
- Close-range gravitational behavior
- Orbital-scale accuracy after softening
- Bounded Earth-like orbital motion
- Controlled orbital-energy drift
- Linear-momentum conservation

### 3D Visualization

- Raylib-cs 3D rendering
- Planetary bodies and spacecraft
- Orbital trails
- Camera orbit, zoom, and target tracking
- Simulation-speed controls
- HUD and body information
- Configurable body styles and solar-system data

## Project Structure

```text
Cosmos.Domain
    Entities, value objects, vectors, and domain concepts

Cosmos.Engine
    Physics models, integrators, orbital calculations,
    simulation timing, maneuvers, and tracking

Cosmos.Desktop
    Raylib-based rendering, input, loaders, camera, and HUD

Cosmos.Engine.Tests
    Unit, characterization, numerical, and scientific tests

Docs
    Physics, mathematics, assumptions, and design documentation
```

## Scientific Assumptions

The current simulation is deliberately limited and transparent about its assumptions:

- Gravity is Newtonian and instantaneous.
- Bodies are currently modeled primarily as point masses.
- The simulation uses normalized internal units rather than SI units.
- The gravitational constant is calibrated to the current scaled dataset.
- Plummer softening is a numerical approximation, not a collision model.
- Relativistic effects are not currently modeled.
- Numerical integration introduces approximation error that must be measured.

These limitations are documented instead of being hidden behind arbitrary numerical clamps.

## Knowledge-First Development

Scientific changes follow this workflow:

1. Define the question.
2. Study the mathematical and physical concept.
3. State assumptions and units.
4. Document the understanding.
5. Implement the smallest meaningful change.
6. Verify it with focused automated tests.
7. Record limitations and future questions.

This approach makes Cosmos Engine both a software project and a structured path for learning physics through code.

## Technology

- C#
- .NET 10
- xUnit
- Raylib-cs
- JSON-based simulation data
- Git and GitHub

## Current Direction

The current focus is stabilizing the scientific core of the C# implementation:

- Improving numerical diagnostics
- Measuring conservation properties
- Refining orbital verification
- Documenting the normalized unit system
- Preparing for explicit body radii and collision modeling
- Expanding orbital-mechanics foundations

Large architectural rewrites and the planned Python version remain intentionally parked until the scientific C# core is better understood and verified.

## Status

**Active learning and development project**

Cosmos Engine is evolving through small, documented, and testable steps rather than feature-first development.
