# Cosmos Engine
# Engineering Journal

---

# Project

Cosmos Engine

---

# Current Development Method

Knowledge Driven Development (KDD)

Development Cycle

Scientific Understanding

↓

Documentation

↓

Scientific Source Review

↓

Discussion

↓

Redesign (if needed)

↓

Implementation

↓

Verification

↓

Documentation Update

---

# Current Session

Completed

- ✅ Chapter 06 — Scalars
- ✅ Chapter 07 — Vectors
- ✅ Chapter 08 — Coordinate Systems
- ✅ Chapter 09 — Space
- ✅ Chapter 10 — Time
- ✅ Chapter 11 — Foundations Review

Project Status updated.

Development workflow redesigned.

Scientific Source Review adopted as the next project phase.

---

# Current Focus

Synchronize scientific knowledge with the existing source code.

The objective is to ensure that every implemented feature is scientifically understood before further development continues.

---

# Parking Lot

Ideas intentionally postponed until the appropriate time.

- Architecture Decision Records (ADR)
- Knowledge Reference IDs (KN-xxxx)
- Documentation ↔ Source Code cross references
- Automatic documentation generation
- Knowledge graph between chapters
- Alternative physics framework
- Plugin-based physics models

---

# Next Session

Scientific Source Review

Starting Point

Cosmos.Domain

↓

Entities

↓

Body.cs

Rules

- Do not modify the code immediately.
- Analyze before redesigning.
- Every property must have scientific justification.
- Ask the four scientific questions for every class.

---

# Four Scientific Questions

For every class:

1. What scientific concept does this represent?

2. Which physical laws justify its existence?

3. Is anything scientifically missing?

4. Would a physicist agree with this model?

---

# Long-Term Objective

Cosmos Engine is not only a simulation engine.

It is a scientific software project whose implementation must always remain synchronized with scientific understanding.

Knowledge comes before implementation.

Implementation validates knowledge.

---

# Observations


Observation-001

Position has no explicit Reference Frame.

Scientific Impact

Position is meaningful only relative to a reference frame.

Decision

Pending


Observation-002

Acceleration is currently modeled as a Body property.

Scientific Observation

Acceleration is not an intrinsic property of a body.

Decision

Pending


O-003

Title:
Premature Domain Concepts

Description:
Several domain concepts such as ReferenceFrame,
ReferenceContext and Navigation were introduced before
their underlying scientific concepts were fully studied.

Status:
Pending

Action:
Revisit after completing Mechanics and Kinematics chapters.


🟡 Observation O-004

Title

Acceleration is represented as Vector3D.

Description

Current implementation treats acceleration as a mathematical vector rather than an explicit physical quantity.

Status

Pending

Action

Revisit after the Mechanics chapter.


❓ Question Q-001

Who owns force accumulation?

PhysicsModel?
Integrator?
Another component?

Review after PhysicsModel.


🟡 Observation O-005

Title

PhysicsModel mutates Universe in-place.

Status

Pending

Action

Review after Services layer to confirm if mutable state is the intended architecture.



🟡 O-006

Title

BurnStep is represented as int.

Status

Pending

Action

Review after Maneuver chapter.



🟡 O-007

Title

EnergyStatistics has implicit units.

Status

Pending

Action

Review during Physical Quantities / Unit System.



🟡 O-008

Title

ParentBody is stored as string.

Status

Pending

Action

Review after Navigation architecture.


🟡 O-009

Title

Engine Models are primarily runtime snapshots and execution state rather than domain objects.

Status

Confirmed as architectural pattern (needs validation after reviewing remaining Engine models).




Observation بزرگ O-010

تا قبل از دیدن این پوشه، فکر می‌کردم Calculators یک پوشه است.

الان می‌گم:

نه.

این پوشه در واقع چهار فصل مختلف را قاطی کرده.

یعنی از نظر مفهوم، این‌ها کنار هم نیستند.

من اینطوری فصل‌بندی‌شون می‌کنم:

Chapter 3

3.1 Fundamental Physics

    CircularOrbitCalculator

    EscapeVelocityCalculator

    OrbitalEnergyCalculator

↓

3.2 Orbital Mechanics

    HohmannTransferCalculator

↓

3.3 Orbital Analysis

    OrbitalStatisticsCalculator

↓

3.4 Sphere of Influence

    SphereOfInfluenceCalculator

    SphereOfInfluenceStatisticsCalculator

یعنی اگر کتاب بود، من اصلاً این هفت فایل را پشت سر هم نمی‌گذاشتم.

یه Observation دیگه

تقریباً همه فایل‌ها دارند یک کار انجام می‌دهند.

Input

↓

Math

↓

Output

هیچ State ندارند.

هیچ Cache ندارند.

هیچ Dependency عجیبی ندارند.

یعنی واقعاً Calculator هستند.

این خیلی خوبه.

✅ Confirmed

ولی...

یه چیزی اینجا داد زد.

G
private const double G = 100;

اینجا هست.

بعد

اینجا هم هست.

private const double G = 100;

بعد

اینجا

private const double Mu = 10_000_000;

😂😂

اینجا یه داستان خوابیده.

این مشکل الان نیست.

ولی قطعاً باید ثبت بشه.

چون

الان

سه مفهوم مختلف داریم:

G

μ

Mass

که هنوز Relationshipشان مشخص نشده.

از دید فیزیک

μ = GM

یعنی

μ

یک Constant جهانی نیست.

وابسته به جرم جسم مرکزی است.

در حالی که اینجا

const double Mu

داریم.

این احتمالاً فقط برای Demo بوده.

ولی باید بعداً برگردیم.


🟡 O-011

Title

Physical constants are duplicated and partially hardcoded.

Description

G and μ appear as local constants in multiple calculators. Their physical relationship has not yet been unified.

Status

Pending

Action

Review after Unit System and Physical Constants chapter.



🟡 O-012

Title

Vector helper methods are used inconsistently across calculators.

Status

Pending

Action

Review after Vector chapter.


❓ Q-002

Should OrbitalStatisticsCalculator depend on OrbitalTracker, or should tracking data be provided externally?

Review after Tracking chapter.

🟡 O-013

Title

Calculator creates another calculator internally instead of receiving it as a dependency.

Status

Pending

Action

Review after Services/Composition Root.



---
Version: 1.0

Last Updated: 2026-07-11