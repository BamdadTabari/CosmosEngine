# 12 - Orbital Reference Bodies

**Version:** 1.0  
**Last Updated:** 2026-08-15  
**Status:** Draft  
**Reviewed:** Pending implementation verification  

---

## Goal

Understand how orbital motion must be defined relative to another body, and clearly distinguish between:

- the maneuvering body;
- the central body;
- the coordinate origin;
- the camera target.

This distinction is necessary before implementing reliable orbital maneuvers in Cosmos Engine.

---

## Motivation

In orbital mechanics, position alone is not enough.

A spacecraft does not simply have an "orbital radius" because it has a position in the simulation.

An orbit is defined relative to another body.

For example, if Explorer-1 is orbiting the Sun, its orbital radius is determined by the distance between Explorer-1 and the Sun.

It is not necessarily the distance between Explorer-1 and the global coordinate origin.

This difference may appear unimportant when the Sun is initially placed at:

```text
(0, 0, 0)

but Cosmos Engine allows bodies to move dynamically under gravity.

Therefore, the Sun is not guaranteed to remain exactly at the global origin.

A physically meaningful orbital model must describe relationships between bodies rather than depending on accidental coordinate placement.

The Question

Suppose Explorer-1 is orbiting the Sun.

How should its orbital radius be calculated?

Which body should receive a maneuver burn?

What role should the Sun have?

And should changing the camera target have any effect on the physics?

Intuition

Imagine the Sun and Explorer-1 both moving through the simulation.

Suppose their global positions are:

Explorer-1 = (150, 20, 0)


Sun = (2, 1, 0)

Explorer-1's global position is:

(150, 20, 0)

but its position relative to the Sun is:

(150, 20, 0) - (2, 1, 0)

which gives:

(148, 19, 0)

That relative vector is what matters for the Sun-centered orbit.

The important question is therefore not:

Where is the spacecraft in the universe?

but:

Where is the spacecraft relative to the body it is orbiting?

This is a fundamental idea in orbital mechanics.

Explanation

Several concepts in Cosmos Engine may appear similar because they all refer to bodies in the simulation.

Scientifically, however, they have very different meanings.

Maneuvering Body

The maneuvering body is the object whose motion is intentionally changed.

For the current experimental Hohmann transfer:

Maneuvering Body = Explorer-1

When a maneuver applies a change in velocity:

Δv

that velocity change must be applied to Explorer-1.

The maneuvering body is the spacecraft performing the maneuver.

It is not automatically the camera target.

It is not automatically the central body.

Central Body

The central body is the body relative to which an orbit is being described.

For the current Hohmann-transfer experiment:

Central Body = Sun

The Sun provides the main gravitational reference for the transfer.

The spacecraft's orbital radius is measured relative to the Sun.

The central body's mass is also used when calculating the gravitational parameter:

μ = GM

For the current experiment, the Hohmann transfer is therefore approximately heliocentric.

Camera Target

The camera target belongs to the presentation layer of the application.

Its responsibility is to determine what the user is currently observing.

For example, the camera may be focused on:

Earth

while Explorer-1 continues to orbit the Sun.

Changing the camera target must not change the physical meaning of the maneuver.

The camera target must not determine:

which body receives Δv;
which body defines the orbit;
which mass is used for orbital calculations;
which body acts as the central gravitational reference.

Rendering state and physical state must remain conceptually independent.

Coordinate Origin

The coordinate origin is simply the point:

(0, 0, 0)

in the simulation's global coordinate system.

A body may happen to be located at this point.

For example, the Sun may initially be created at:

Sun.Position = (0, 0, 0)

but this does not mean:

Sun == coordinate origin

These are different concepts.

The coordinate origin belongs to the coordinate system.

The Sun is a physical body.

If the Sun moves because of gravitational interaction with other bodies, the global origin does not move with it.

Therefore, physical calculations should not silently assume that the Sun remains at the origin.

Scientific View

The current experimental Hohmann transfer in Cosmos Engine uses a simplified orbital model.

The intended interpretation is:

Maneuvering Body : Explorer-1
Central Body     : Sun
Reference Frame  : Sun-centered / heliocentric
Initial Orbit    : Circular approximation
Target Orbit     : Circular approximation
Burn Model       : Instantaneous Δv impulses
Orbit Plane      : XY plane

This is an approximation.

It is useful for learning and for developing the orbital-mechanics subsystem, but it must not yet be described as a general-purpose mission-planning model.

Two-Body Approximation

The Hohmann transfer equations assume that orbital motion is dominated by a single central gravitational body.

Conceptually:

      Explorer-1
          ●
          |
          |
          |
          ☀
         Sun

The spacecraft is treated as orbiting the Sun.

Other gravitational bodies may exist in the simulation, but the analytical Hohmann calculation currently does not model their perturbations.

Instantaneous Burns

The current maneuver model treats burns as instantaneous changes in velocity.

Conceptually:

v_before
    +
   Δv
    =
v_after

No burn duration is currently modeled.

This means the maneuver behaves like an impulse rather than a realistic rocket engine operating over time.

Current Limitations

The current Hohmann-transfer model does not yet include:

finite-duration burns;
fuel consumption;
changing spacecraft mass;
thrust-to-mass relationships;
arbitrary orbital planes;
inclination changes;
automatic dominant-body selection;
perturbations from multiple gravitating bodies;
general N-body mission planning;
Lambert targeting;
patched-conic interplanetary trajectories.

These are future subjects.

They should not be added before the current model is understood and verified.

Mathematics
Relative Position

Let:

x
s
	​


represent the global position of the spacecraft.

Let:

x
c
	​


represent the global position of the central body.

The spacecraft's position relative to the central body is:

r
=
x
s
	​

−
x
c
	​


where:

r

is the orbital relative-position vector.

Orbital Radius

The orbital radius is the magnitude of the relative-position vector:

r=∣
r
∣

Therefore:

r=∣
x
s
	​

−
x
c
	​

∣

For Explorer-1 orbiting the Sun:

r=∣
x
Explorer−1
	​

−
x
Sun
	​

∣

This is the physically meaningful orbital radius.

Why Position.Magnitude() Is Not Always Enough

Suppose we calculate:

spacecraft.Position.Magnitude()

Mathematically, this gives:

∣
x
spacecraft
	​

∣

which is the spacecraft's distance from the global origin.

This only equals the orbital radius when the central body is exactly at the origin:

x
centralBody
	​

=0

In the general case:

∣
x
spacecraft
	​

∣

=∣
x
spacecraft
	​

−
x
centralBody
	​

∣

Therefore, a robust orbital model should use relative positions.

Gravitational Parameter

Orbital mechanics commonly combines the gravitational constant and central-body mass into the gravitational parameter:

μ=GM

where:

G is the gravitational constant;
M is the mass of the central body.

Cosmos Engine currently uses normalized simulation units rather than SI units.

The current gravitational constant is:

G = 100

The current Sun mass is approximately:

M_sun = 100000

Therefore:

μ=GM
sun
	​

μ=100×100000
μ=10,000,000

This matches the current value used by the experimental Hohmann-transfer calculator.

However, this value must not be interpreted as a universal constant.

It is derived from the current central-body assumption:

Central Body = Sun

If another body becomes the central body, its mass must determine the appropriate value of μ.

Relative Velocity

For the current Hohmann maneuver model, the prograde burn direction
must therefore be derived from the spacecraft velocity relative to
the central body rather than from its global velocity.

The same idea also applies to velocity.

If both bodies are moving, the spacecraft's orbital velocity relative to the central body is:

v
rel
	​

=
v
spacecraft
	​

−
v
centralBody
	​


This is important because orbital mechanics is based on relative motion.

Using only the spacecraft's global velocity may become incorrect when the central body itself has significant motion.

This chapter does not yet change the implementation to use relative velocity, but the distinction should remain explicit for future maneuver work.

Tangential Direction

For a planar circular orbit, the spacecraft's velocity is approximately perpendicular to its radial vector.

If:

r

is the radial direction, then the prograde direction is approximately tangent to the orbit.

In the current simplified XY-plane model, if the normalized radial direction is:

r
^
=(r
x
	​

,r
y
	​

,0)

one possible tangent direction is:

t
^
=(−r
y
	​

,r
x
	​

,0)

This corresponds to a 90-degree rotation in the XY plane.

However, that tangent must be derived from the spacecraft's position relative to the central body:

r
=
x
spacecraft
	​

−
x
centralBody
	​


not from the spacecraft's position relative to the global origin.

Cosmos Engine

The current Hohmann-transfer experiment should conceptually model the following relationship:

                 Maneuver
                    │
                    ▼
               Explorer-1
                    ●
                    │
                    │ orbital relationship
                    │
                    ▼
                   Sun
                    ☀

Explorer-1 is the maneuvering body.

The Sun is the central body.

The camera exists outside this physical relationship.

Conceptually:

Camera
  │
  └── observes any selected body


Explorer-1
  │
  ├── receives Δv
  │
  └── orbits relative to Sun


Sun
  │
  ├── provides central mass
  │
  └── defines the orbital reference
Intended Responsibilities
Explorer-1

Explorer-1 should:

contain the spacecraft state;
provide its current position;
provide its current velocity;
receive maneuver Δv.
Sun

The Sun should:

act as the current Hohmann central body;
provide the central mass;
define the relative orbital position;
define the current heliocentric reference.
Camera.Target

Camera.Target should:

determine which body is visually observed;
affect rendering behavior only.

It should not decide which body participates in an orbital maneuver.

Current Conceptual Problem

The current Desktop experiment uses the camera target while preparing the temporary Hohmann transfer.

This creates an accidental connection between rendering state and physical behavior.

Conceptually, code shaped like:

var target =
    state.Camera.Target;

cannot safely determine the maneuvering body.

If the user changes the camera from Explorer-1 to Earth, the physical maneuver should not suddenly belong to Earth.

The maneuvering body must instead be selected from simulation state explicitly.

For the current experiment, that body is:

state.ControlledBody

when that body represents Explorer-1.

Orbital Radius in Cosmos Engine

The intended orbital-radius calculation is conceptually:

var relativePosition =
    spacecraft.Position -
    centralBody.Position;


var orbitalRadius =
    relativePosition.Magnitude();

This expresses the actual physical relationship.

By contrast:

spacecraft.Position.Magnitude();

implicitly measures distance from the global origin.

That hidden assumption should not define orbital mechanics.

Hohmann Transfer Contract

Before changing implementation code, the experimental Hohmann transfer should have the following explicit contract:

Maneuvering Body
    Explorer-1


Central Body
    Sun


Reference Frame
    Sun-centered / heliocentric approximation


Initial Orbit
    Circular approximation


Target Orbit
    Circular approximation


Burn Model
    Two instantaneous Δv impulses


Orbital Plane
    XY plane


Gravitational Parameter
    μ = G × M_sun

This contract defines what the current implementation is intended to represent.

Anything beyond this contract should be treated as future work.

Common Misconceptions
"The camera target is the maneuver target."

No.

The camera target is a presentation concept.

The maneuvering body is a physical simulation concept.

Changing one must not silently change the other.

"The central body is always at the origin."

No.

A central body may initially be placed at the origin, but it may move during the simulation.

The global coordinate origin and the physical central body are separate concepts.

"Position.Magnitude() always gives orbital radius."

No.

It gives distance from the global origin.

Orbital radius must generally be calculated relative to the central body:

r=∣
x
spacecraft
	​

−
x
centralBody
	​

∣
"The target orbit is the body receiving the maneuver."

No.

An orbit is not the object being accelerated.

The spacecraft performs the maneuver.

The target orbit describes the desired trajectory after the maneuver.

"μ = 10,000,000 is a universal Cosmos Engine constant."

No.

The value currently comes from:

μ=GM

using the current normalized gravitational constant and Sun mass.

A different central body would produce a different gravitational parameter.

"A Hohmann transfer works for every orbital situation."

No.

The classical Hohmann transfer assumes a highly simplified situation involving two coplanar circular orbits around the same dominant central body.

It is not a general solution for arbitrary trajectories.

Summary

Orbital mechanics is fundamentally relational.

A spacecraft's orbital state is meaningful relative to another body.

For the current Cosmos Engine Hohmann-transfer experiment:

Maneuvering Body = Explorer-1


Central Body = Sun


Camera Target = presentation only

The orbital relative-position vector is:

r
=
x
spacecraft
	​

−
x
centralBody
	​


and the orbital radius is:

r=∣
r
∣

The gravitational parameter is:

μ=GM

For the current normalized Sun model:

μ=10,000,000

The coordinate origin must not be confused with the central body.

The camera target must not be confused with the maneuvering body.

Keeping these concepts separate is necessary before Cosmos Engine can safely implement more advanced orbital maneuvers.

Further Reading
Two-body orbital mechanics
Relative position and relative velocity
Reference frames
Heliocentric reference frames
Gravitational parameter μ
Circular orbital velocity
Hohmann transfer
Impulsive orbital maneuvers
Orbital energy
Orbital angular momentum
Patched-conic approximation


این یکی دیگه نسخه‌ی کامل فایل انگلیسیه و مستقیم می‌تونی بذاری توی `Docs/Knowledge/en/12 - Orbital Reference Bodies.md`. 

TRANSLATE with
x
  English
Arabic	Hebrew	Polish
Bulgarian	Hindi	Portuguese
Catalan	Hmong Daw	Romanian
Chinese Simplified	Hungarian	Russian
Chinese Tradi# 12 - Orbital Reference Bodies

**Version:** 1.0  
**Last Updated:** 2026-08-15  
**Status:** Draft  
**Reviewed:** Pending implementation verification  

---

## Goal

Understand how orbital motion must be defined relative to another body, and clearly distinguish between:

- the maneuvering body;
- the central body;
- the coordinate origin;
- the camera target.

This distinction is necessary before implementing reliable orbital maneuvers in Cosmos Engine.

---

## Motivation

In orbital mechanics, position alone is not enough.

A spacecraft does not simply have an "orbital radius" because it has a position in the simulation.

An orbit is defined relative to another body.

For example, if Explorer-1 is orbiting the Sun, its orbital radius is determined by the distance between Explorer-1 and the Sun.

It is not necessarily the distance between Explorer-1 and the global coordinate origin.

This difference may appear unimportant when the Sun is initially placed at:

    (0, 0, 0)

but Cosmos Engine allows bodies to move dynamically under gravity.

Therefore, the Sun is not guaranteed to remain exactly at the global origin.

A physically meaningful orbital model must describe relationships between bodies rather than depending on accidental coordinate placement.

---

## The Question

Suppose Explorer-1 is orbiting the Sun.

How should its orbital radius be calculated?

Which body should receive a maneuver burn?

What role should the Sun have?

And should changing the camera target have any effect on the physics?

---

## Intuition

Imagine the Sun and Explorer-1 both moving through the simulation.

Suppose their global positions are:

    Explorer-1 = (150, 20, 0)
    Sun        = (2, 1, 0)

Explorer-1's global position is:

    (150, 20, 0)

but its position relative to the Sun is:

    (150, 20, 0) - (2, 1, 0)

which gives:

    (148, 19, 0)

That relative vector is what matters for the Sun-centered orbit.

The important question is therefore not:

> Where is the spacecraft in the universe?

but:

> Where is the spacecraft relative to the body it is orbiting?

This is a fundamental idea in orbital mechanics.

---

## Explanation

Several concepts in Cosmos Engine may appear similar because they all refer to bodies in the simulation.

Scientifically, however, they have very different meanings.

### Maneuvering Body

The maneuvering body is the object whose motion is intentionally changed.

For the current experimental Hohmann transfer:

    Maneuvering Body = Explorer-1

When a maneuver applies a change in velocity:

    Δv

that velocity change must be applied to Explorer-1.

The maneuvering body is the spacecraft performing the maneuver.

It is not automatically the camera target.

It is not automatically the central body.

### Central Body

The central body is the body relative to which an orbit is being described.

For the current Hohmann-transfer experiment:

    Central Body = Sun

The Sun provides the main gravitational reference for the transfer.

The spacecraft's orbital radius is measured relative to the Sun.

The central body's mass is also used when calculating the gravitational parameter:

    μ = GM

For the current experiment, the Hohmann transfer is therefore approximately heliocentric.

### Camera Target

The camera target belongs to the presentation layer of the application.

Its responsibility is to determine what the user is currently observing.

For example, the camera may be focused on:

    Earth

while Explorer-1 continues to orbit the Sun.

Changing the camera target must not change the physical meaning of the maneuver.

The camera target must not determine:

- which body receives Δv;
- which body defines the orbit;
- which mass is used for orbital calculations;
- which body acts as the central gravitational reference.

Rendering state and physical state must remain conceptually independent.

### Coordinate Origin

The coordinate origin is simply the point:

    (0, 0, 0)

in the simulation's global coordinate system.

A body may happen to be located at this point.

For example, the Sun may initially be created at:

    Sun.Position = (0, 0, 0)

but this does not mean:

    Sun == coordinate origin

These are different concepts.

The coordinate origin belongs to the coordinate system.

The Sun is a physical body.

If the Sun moves because of gravitational interaction with other bodies, the global origin does not move with it.

Therefore, physical calculations should not silently assume that the Sun remains at the origin.

---

## Scientific View

The current experimental Hohmann transfer in Cosmos Engine uses a simplified orbital model.

The intended interpretation is:

    Maneuvering Body : Explorer-1
    Central Body     : Sun
    Reference Frame  : Sun-centered / heliocentric
    Initial Orbit    : Circular approximation
    Target Orbit     : Circular approximation
    Burn Model       : Instantaneous Δv impulses
    Orbit Plane      : XY plane

This is an approximation.

It is useful for learning and for developing the orbital-mechanics subsystem, but it must not yet be described as a general-purpose mission-planning model.

### Two-Body Approximation

The Hohmann transfer equations assume that orbital motion is dominated by a single central gravitational body.

Conceptually:

          Explorer-1
              ●
              |
              |
              |
              ☀
             Sun

The spacecraft is treated as orbiting the Sun.

Other gravitational bodies may exist in the simulation, but the analytical Hohmann calculation currently does not model their perturbations.

### Instantaneous Burns

The current maneuver model treats burns as instantaneous changes in velocity.

Conceptually:

    v_before
        +
       Δv
        =
    v_after

No burn duration is currently modeled.

This means the maneuver behaves like an impulse rather than a realistic rocket engine operating over time.

### Current Limitations

The current Hohmann-transfer model does not yet include:

- finite-duration burns;
- fuel consumption;
- changing spacecraft mass;
- thrust-to-mass relationships;
- arbitrary orbital planes;
- inclination changes;
- automatic dominant-body selection;
- perturbations from multiple gravitating bodies;
- general N-body mission planning;
- Lambert targeting;
- patched-conic interplanetary trajectories.

These are future subjects.

They should not be added before the current model is understood and verified.

---

## Mathematics

### Relative Position

Let:

\[
\vec{x}_s
\]

represent the global position of the spacecraft.

Let:

\[
\vec{x}_c
\]

represent the global position of the central body.

The spacecraft's position relative to the central body is:

\[
\vec{r}
=
\vec{x}_s
-
\vec{x}_c
\]

where:

\[
\vec{r}
\]

is the orbital relative-position vector.

### Orbital Radius

The orbital radius is the magnitude of the relative-position vector:

\[
r = |\vec{r}|
\]

Therefore:

\[
r
=
|\vec{x}_s - \vec{x}_c|
\]

For Explorer-1 orbiting the Sun:

\[
r
=
|\vec{x}_{Explorer-1}
-
\vec{x}_{Sun}|
\]

This is the physically meaningful orbital radius.

### Why `Position.Magnitude()` Is Not Always Enough

Suppose we calculate:

    spacecraft.Position.Magnitude()

Mathematically, this gives:

\[
|\vec{x}_{spacecraft}|
\]

which is the spacecraft's distance from the global origin.

This only equals the orbital radius when the central body is exactly at the origin:

\[
\vec{x}_{centralBody} = 0
\]

In the general case:

\[
|\vec{x}_{spacecraft}|
\neq
|\vec{x}_{spacecraft}
-
\vec{x}_{centralBody}|
\]

Therefore, a robust orbital model should use relative positions.

### Gravitational Parameter

Orbital mechanics commonly combines the gravitational constant and central-body mass into the gravitational parameter:

\[
\mu = GM
\]

where:

- \(G\) is the gravitational constant;
- \(M\) is the mass of the central body.

Cosmos Engine currently uses normalized simulation units rather than SI units.

The current gravitational constant is:

    G = 100

The current Sun mass is approximately:

    M_sun = 100000

Therefore:

\[
\mu
=
G M_{sun}
\]

\[
\mu
=
100 \times 100000
\]

\[
\mu
=
10,000,000
\]

This matches the current value used by the experimental Hohmann-transfer calculator.

However, this value must not be interpreted as a universal constant.

It is derived from the current central-body assumption:

    Central Body = Sun

If another body becomes the central body, its mass must determine the appropriate value of \(\mu\).

### Relative Velocity

The same idea also applies to velocity.

If both bodies are moving, the spacecraft's orbital velocity relative to the central body is:

\[
\vec{v}_{rel}
=
\vec{v}_{spacecraft}
-
\vec{v}_{centralBody}
\]

This is important because orbital mechanics is based on relative motion.

Using only the spacecraft's global velocity may become incorrect when the central body itself has significant motion.

This chapter does not yet change the implementation to use relative velocity, but the distinction should remain explicit for future maneuver work.

### Tangential Direction

For a planar circular orbit, the spacecraft's velocity is approximately perpendicular to its radial vector.

If:

\[
\vec{r}
\]

is the radial direction, then the prograde direction is approximately tangent to the orbit.

In the current simplified XY-plane model, if the normalized radial direction is:

\[
\hat{r}
=
(r_x, r_y, 0)
\]

one possible tangent direction is:

\[
\hat{t}
=
(-r_y, r_x, 0)
\]

This corresponds to a 90-degree rotation in the XY plane.

However, that tangent must be derived from the spacecraft's position relative to the central body:

\[
\vec{r}
=
\vec{x}_{spacecraft}
-
\vec{x}_{centralBody}
\]

not from the spacecraft's position relative to the global origin.

---

## Cosmos Engine

The current Hohmann-transfer experiment should conceptually model the following relationship:

                     Maneuver
                        |
                        v
                   Explorer-1
                        ●
                        |
                        | orbital relationship
                        |
                        v
                       Sun
                        ☀

Explorer-1 is the maneuvering body.

The Sun is the central body.

The camera exists outside this physical relationship.

Conceptually:

    Camera
      |
      └── observes any selected body

    Explorer-1
      |
      ├── receives Δv
      |
      └── orbits relative to Sun

    Sun
      |
      ├── provides central mass
      |
      └── defines the orbital reference

### Intended Responsibilities

#### Explorer-1

Explorer-1 should:

- contain the spacecraft state;
- provide its current position;
- provide its current velocity;
- receive maneuver Δv.

#### Sun

The Sun should:

- act as the current Hohmann central body;
- provide the central mass;
- define the relative orbital position;
- define the current heliocentric reference.

#### Camera.Target

`Camera.Target` should:

- determine which body is visually observed;
- affect rendering behavior only.

It should not decide which body participates in an orbital maneuver.

### Current Conceptual Problem

The current Desktop experiment uses the camera target while preparing the temporary Hohmann transfer.

This creates an accidental connection between rendering state and physical behavior.

Conceptually, code shaped like:

    var target =
        state.Camera.Target;

cannot safely determine the maneuvering body.

If the user changes the camera from Explorer-1 to Earth, the physical maneuver should not suddenly belong to Earth.

The maneuvering body must instead be selected from simulation state explicitly.

For the current experiment, that body is:

    state.ControlledBody

when that body represents Explorer-1.

### Orbital Radius in Cosmos Engine

The intended orbital-radius calculation is conceptually:

    var relativePosition =
        spacecraft.Position -
        centralBody.Position;

    var orbitalRadius =
        relativePosition.Magnitude();

This expresses the actual physical relationship.

By contrast:

    spacecraft.Position.Magnitude();

implicitly measures distance from the global origin.

That hidden assumption should not define orbital mechanics.

### Hohmann Transfer Contract

Before changing implementation code, the experimental Hohmann transfer should have the following explicit contract:

    Maneuvering Body
        Explorer-1

    Central Body
        Sun

    Reference Frame
        Sun-centered / heliocentric approximation

    Initial Orbit
        Circular approximation

    Target Orbit
        Circular approximation

    Burn Model
        Two instantaneous Δv impulses

    Orbital Plane
        XY plane

    Gravitational Parameter
        μ = G × M_sun

This contract defines what the current implementation is intended to represent.

Anything beyond this contract should be treated as future work.

---

## Common Misconceptions

### "The camera target is the maneuver target."

No.

The camera target is a presentation concept.

The maneuvering body is a physical simulation concept.

Changing one must not silently change the other.

### "The central body is always at the origin."

No.

A central body may initially be placed at the origin, but it may move during the simulation.

The global coordinate origin and the physical central body are separate concepts.

### "`Position.Magnitude()` always gives orbital radius."

No.

It gives distance from the global origin.

Orbital radius must generally be calculated relative to the central body:

\[
r
=
|\vec{x}_{spacecraft}
-
\vec{x}_{centralBody}|
\]

### "The target orbit is the body receiving the maneuver."

No.

An orbit is not the object being accelerated.

The spacecraft performs the maneuver.

The target orbit describes the desired trajectory after the maneuver.

### "`μ = 10,000,000` is a universal Cosmos Engine constant."

No.

The value currently comes from:

\[
\mu = GM
\]

using the current normalized gravitational constant and Sun mass.

A different central body would produce a different gravitational parameter.

### "A Hohmann transfer works for every orbital situation."

No.

The classical Hohmann transfer assumes a highly simplified situation involving two coplanar circular orbits around the same dominant central body.

It is not a general solution for arbitrary trajectories.

---

## Summary

Orbital mechanics is fundamentally relational.

A spacecraft's orbital state is meaningful relative to another body.

For the current Cosmos Engine Hohmann-transfer experiment:

    Maneuvering Body = Explorer-1
    Central Body     = Sun
    Camera Target    = presentation only

The orbital relative-position vector is:

\[
\vec{r}
=
\vec{x}_{spacecraft}
-
\vec{x}_{centralBody}
\]

and the orbital radius is:

\[
r = |\vec{r}|
\]

The gravitational parameter is:

\[
\mu = GM
\]

For the current normalized Sun model:

\[
\mu = 10,000,000
\]

The coordinate origin must not be confused with the central body.

The camera target must not be confused with the maneuvering body.

Keeping these concepts separate is necessary before Cosmos Engine can safely implement more advanced orbital maneuvers.

---

## Further Reading

- Two-body orbital mechanics
- Relative position and relative velocity
- Reference frames
- Heliocentric reference frames
- Gravitational parameter \(\mu\)
- Circular orbital velocity
- Hohmann transfer
- Impulsive orbital maneuvers
- Orbital energy
- Orbital angular momentum
- Patched-conic approximationtional	Indonesian	Slovak
Czech	Italian	Slovenian
Danish	Japanese	Spanish
Dutch	Klingon	Swedish
English	Korean	Thai
Estonian	Latvian	Turkish
Finnish	Lithuanian	Ukrainian
French	Malay	Urdu
German	Maltese	Vietnamese
Greek	Norwegian	Welsh
Haitian Creole	Persian	
TRANSLATE with
COPY THE URL BELOW
Back
EMBED THE SNIPPET BELOW IN YOUR SITE
Enable collaborative features and customize widget: Bing Webmaster Portal
