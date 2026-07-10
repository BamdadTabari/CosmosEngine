# 08 — Coordinate Systems

---

# Title

Coordinate Systems

---

# Goal

To understand coordinate systems, how they define the position of objects and vectors, and why they are essential in physics and Cosmos Engine.

---

# Motivation

In the previous chapter, we learned that vectors have both magnitude and direction.

However, consider the vector

```
(3, 5, -2)
```

A natural question arises:

Relative to what?

Without a common reference, these numbers have no physical meaning.

Coordinate systems provide that common reference.

---

# The Question

What is a coordinate system?

Why is it necessary for describing positions?

How do vectors become meaningful within a coordinate system?

---

# Intuition

Imagine your friend tells you:

"I'm standing at point (10, 8)."

Your first question would probably be:

"Relative to where?"

Without an agreed reference point, those numbers are meaningless.

If everyone agrees that the lower-left corner of a map is (0,0), the location immediately becomes meaningful.

A coordinate system provides this shared reference.

---

# Explanation

A coordinate system is a framework used to describe the position of points, objects, and vectors relative to a defined origin.

Every coordinate system includes:

- An origin
- One or more axes
- A positive direction for each axis
- A unit of measurement

In two-dimensional space, the axes are typically:

- X
- Y

In three-dimensional space:

- X
- Y
- Z

---

# Scientific View

In physics, positions are never absolute.

Every position is expressed relative to a coordinate system.

When we specify the position of an object, we are always giving coordinates with respect to a chosen reference.

This idea forms one of the foundations of classical mechanics and orbital mechanics.

---

# Mathematics

A point in two-dimensional space is represented as

```
P = (x, y)
```

In three-dimensional space

```
P = (x, y, z)
```

Examples:

```
Earth = (149,600,000 km, 0, 0)
```

```
Spacecraft = (1200, 350, -80)
```

These values are meaningful only when the coordinate system is defined.

---

# Cosmos Engine

Every object in Cosmos Engine has a position.

That position is represented using a three-dimensional vector.

Examples include:

- Position
- Velocity
- Acceleration

All are expressed within a coordinate system.

The `Vector3D` structure stores coordinates in three-dimensional space.

In future versions, Cosmos Engine will support multiple coordinate systems, including:

- World Coordinate System
- Local Coordinate System
- Orbital Coordinate System

---

# Common Misconceptions

### 1. Coordinates are the location itself.

Incorrect.

Coordinates are only a way to describe a location.

---

### 2. The X, Y, and Z axes are always fixed.

Incorrect.

Different problems may use different coordinate systems.

---

### 3. Every software system uses the same coordinate system.

Incorrect.

Different engines and applications may adopt different axis conventions.

---

### 4. Coordinates have meaning without a reference frame.

Incorrect.

Coordinates are meaningful only when the coordinate system has been defined.

---

# Summary

A coordinate system provides the framework for describing positions and directions in space.

Without a coordinate system, vectors and positions have no precise meaning.

All spatial calculations in Cosmos Engine rely on coordinate systems.

---

# Further Reading

- Scalars
- Vectors
- Space
- Cartesian Coordinate System
- Reference Frames

---

Document Version: 1.0

Last Updated: 2026-07-10

Status: Completed

Reviewed: Yes