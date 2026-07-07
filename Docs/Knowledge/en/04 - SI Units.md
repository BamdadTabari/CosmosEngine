# SI Units

---

## Goal

The goal of this chapter is to introduce the International System of Units (SI) and explain why standardized units are essential for physics and simulation.

By the end of this chapter, the reader should understand what SI is, why it exists, and why Cosmos Engine relies on it.

---

## Motivation

Imagine two scientists discussing the distance between Earth and the Moon.

One says:

"The distance is 384,400."

The other asks:

"384,400 what? Meters? Kilometers? Miles?"

A number alone has no physical meaning.

Every physical quantity must include both a numerical value and a unit.

---

## The Question

What is a unit?

Why does science require a universal system of measurement?

---

## Intuition

Suppose you measure the height of a table.

You might say:

1 meter.

Or:

100 centimeters.

Or:

1000 millimeters.

The number changes, but the table does not.

A physical quantity is defined by both its numerical value and its unit.

---

## Explanation

SI stands for the **International System of Units**.

It is the globally accepted standard system used throughout science and engineering.

Its purpose is to ensure that measurements are consistent, reproducible, and universally understood.

Without standard units, scientific communication would be unreliable.

---

## Scientific View

The SI system defines seven base units:

- meter (m) — length
- kilogram (kg) — mass
- second (s) — time
- ampere (A) — electric current
- kelvin (K) — thermodynamic temperature
- mole (mol) — amount of substance
- candela (cd) — luminous intensity

Most physical quantities are derived from combinations of these base units.

Examples include:

- Velocity → meters per second (m/s)
- Acceleration → meters per second squared (m/s²)
- Force → newton (N)
- Energy → joule (J)

---

## Mathematics

Pure mathematics works with abstract numbers.

Physics works with quantities.

A number without a unit is usually meaningless in a physical context.

For this reason, dimensional consistency is one of the most important checks in scientific calculations.

---

## Cosmos Engine

All internal calculations in Cosmos Engine use SI units.

Examples include:

- Mass → kilograms (kg)
- Distance → meters (m)
- Time → seconds (s)
- Velocity → meters per second (m/s)
- Acceleration → meters per second squared (m/s²)

Using a single unit system keeps the physics engine consistent and minimizes conversion errors.

If alternative units are displayed in the future, conversions should occur only in the presentation layer—not inside the physics engine.

---

## Summary

Numbers alone are not enough in physics.

Every physical quantity requires both a value and a unit.

Cosmos Engine adopts the SI system to ensure consistency, accuracy, and reliability throughout the simulation.

---

## Further Reading

Chapter 05 — Physical Quantities

---

Document Version: 1.0

Last Updated: 2026/07/07

Status: Stable

Reviewed: ✅