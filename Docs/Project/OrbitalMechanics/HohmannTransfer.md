# Hohmann Transfer

## Overview

A Hohmann Transfer is the most fuel-efficient transfer between two circular orbits.

It uses an intermediate elliptical orbit.

---

## Concept

Initial Orbit
    ↓

Transfer Ellipse
    ↓

Target Orbit

---

## Burn 1

The spacecraft performs a prograde burn.

This increases velocity and places the spacecraft onto the transfer ellipse.

---

## Burn 2

At the destination orbit, another burn circularizes the orbit.

---

## Delta-V

The total mission cost is:

Total Delta-V = DeltaV1 + DeltaV2

Lower Delta-V generally means lower fuel consumption.

---

## Applications

- Orbit raising
- Orbit lowering
- Earth-to-Mars transfers
- Satellite deployment

---

## Limitations

Hohmann transfers are fuel efficient but not time efficient.

Faster transfers usually require more Delta-V.

---

## Cosmos Engine

Current implementation:

- Circular start orbit
- Circular target orbit
- Single central body
- Two impulse burns

Future improvements:

- Bi-elliptic transfers
- Plane changes
- Lambert solver
- Interplanetary mission planner