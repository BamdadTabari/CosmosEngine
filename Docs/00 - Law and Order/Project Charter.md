# Cosmos Engine
# Project Charter

---

# Vision

Cosmos Engine is a long-term scientific software project dedicated to understanding the universe through simulation.

The project began as a way to learn physics by implementing scientific concepts in software. Over time, its vision expanded into building a flexible simulation engine capable of modeling known physical laws and, in the future, experimenting with alternative physical models and hypothetical universes.

The ultimate goal is not merely to write software, but to build a continuously growing knowledge base alongside a simulation engine.

---

# Mission

The mission of Cosmos Engine is to:

- Learn Physics through implementation.
- Learn Mathematics through simulation.
- Study Astronomy and Cosmology.
- Build a reusable scientific simulation engine.
- Document all acquired knowledge in both Persian and English.
- Create an educational resource that can benefit other learners.

---

# Core Philosophy

Knowledge First.

Understanding always comes before implementation.

Every feature follows this workflow:

Question

↓

Understanding

↓

Documentation

↓

Implementation

↓

Verification

Programming is considered the final step of learning rather than the first.

---

# Documentation Philosophy

Documentation is a first-class citizen of the project.

Every scientific concept must be:

1. Understood.
2. Documented in Persian.
3. Documented in English.
4. Implemented.

Documentation is not optional.

It is part of the development process.

---

# Documentation Structure

Docs/

Knowledge/
- fa/
- en/

Project/
- Architecture
- Physics
- Mathematics
- OrbitalMechanics
- Research
- Roadmap

Knowledge chapters follow a common template:

- Title
- Goal
- Motivation
- The Question
- Intuition
- Explanation
- Scientific View
- Mathematics
- Cosmos Engine
- Summary
- Further Reading

Every chapter also contains:

- Document Version
- Last Updated
- Status
- Reviewed

---

# Scientific Principles

Cosmos Engine models scientific knowledge rather than reality itself.

Every implementation should be based on accepted scientific models.

Scientific models are continuously open to refinement as new knowledge is acquired.

---

# Development Rules

Before writing new code:

- Understand the scientific concept.
- Update the documentation.
- Verify the understanding.
- Only then implement the feature.

Scientific correctness has higher priority than implementation speed.

---

# Architecture

The project is organized into three primary layers:

Cosmos.Domain

Contains entities, value objects, structs and domain models.

Cosmos.Engine

Contains physics models, integrators, calculators, services and simulation logic.

Cosmos.Desktop

Contains rendering, input, camera, loaders and presentation.

---

# Long-Term Vision

The project evolves through multiple stages:

Stage 1

Reproduce known physical laws.

Stage 2

Improve scientific accuracy.

Stage 3

Model orbital mechanics and space missions.

Stage 4

Provide a platform for scientific experimentation.

Stage 5

Support defining and simulating alternative physical laws and hypothetical universes.

---

# Quality Standard

The objective is not to produce the largest amount of code.

The objective is to produce software whose scientific foundations are fully understood and thoroughly documented.

---

Version: 1.0

Status: Active

Last Updated: 2026-07-10