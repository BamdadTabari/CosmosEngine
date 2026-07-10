# 07 — Vectors

---

# Title

Vectors

---

# Goal

To understand the concept of vectors, distinguish them from scalars, and recognize their importance in physics and Cosmos Engine.

---

# Motivation

In the previous chapter, we learned that many physical quantities require only a magnitude.

However, many phenomena in nature cannot be fully described by magnitude alone.

For example, knowing that a spacecraft is traveling at 7,500 meters per second is not enough.

We must also know the direction of its motion.

Vectors were introduced to describe quantities that require both magnitude and direction.

---

# The Question

What is a vector?

Why do some physical quantities require both magnitude and direction?

How do vectors differ from scalars?

---

# Intuition

Imagine two cars moving at the same speed of 100 km/h.

If one is traveling north and the other south, their motions are completely different despite having the same speed.

Magnitude alone is insufficient.

Direction matters.

Quantities that require both magnitude and direction are called vectors.

---

# Explanation

A vector is a quantity with two essential properties:

- Magnitude
- Direction

Both are required to completely describe the quantity.

Examples include:

- Position
- Displacement
- Velocity
- Acceleration
- Force
- Momentum

---

# Scientific View

Vectors are the primary language of motion in physics.

Most laws of classical mechanics are expressed using vectors.

Examples include:

- Newton's Second Law
- Velocity
- Acceleration
- Gravitational force

Without vectors, describing motion in two- or three-dimensional space would not be possible.

---

# Mathematics

A vector in two-dimensional space is commonly written as:

```
(x, y)
```

In three-dimensional space:

```
(x, y, z)
```

The magnitude of a 2D vector is

```
|v| = √(x² + y²)
```

The magnitude of a 3D vector is

```
|v| = √(x² + y² + z²)
```

Example:

```
v = (3, 4)
```

Its magnitude is

```
|v| = 5
```

---

# Cosmos Engine

Vectors are fundamental throughout Cosmos Engine.

They are used to represent:

- Planet positions
- Object velocities
- Gravitational acceleration
- Forces
- Camera direction
- Spacecraft trajectories
- Orbital maneuvers
- Navigation systems

The `Vector3D` structure is one of the core components of the engine, and many physics calculations will rely on it.

---

# Common Misconceptions

### 1. Every vector is just an arrow.

Incorrect.

An arrow is only a graphical representation of a vector.

A vector itself is a mathematical and physical concept.

---

### 2. Two vectors with the same magnitude are always equal.

Incorrect.

If their directions differ, they are different vectors.

---

### 3. Vectors are used only in physics.

Incorrect.

Vectors are widely used in mathematics, computer graphics, robotics, game development, machine learning, and many other fields.

---

### 4. A vector always represents motion.

Incorrect.

Many vectors describe quantities unrelated to motion.

For example, force is a vector even though it is not itself a motion.

---

# Summary

A vector is a quantity that has both magnitude and direction.

Vectors form the foundation of mechanics, motion, forces, and scientific simulations.

Nearly every spatial calculation in Cosmos Engine will be based on vectors.

---

# Further Reading

- Scalars
- Coordinate Systems
- Linear Algebra
- Classical Mechanics
- Vector Algebra

---

Document Version: 1.0

Last Updated: 2026-07-10

Status: Completed

Reviewed: Yes