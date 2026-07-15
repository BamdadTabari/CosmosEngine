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


// note: remember add comment for every conceptal part of code
// example: what is Semi-Implicit Euler?


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



Observation

🟡 O-014

Title

Integrator contains simulation safety limits.

Description

Velocity clamping is mixed with numerical integration.

Status

Pending

Action

Review after Simulation Stability chapter.


🟡 O-015

Title

Position clamping exists inside Integrator.

Status

Pending

Action

Review after World Boundary design.


❓ Q-003

Who owns the current acceleration state?

PhysicsModel

Integrator

Body

Review after PhysicsModel.



🟡 O-016

Title

Possible Engine pipeline identified.

Pipeline

PhysicsModel

↓

Integrator

↓

Tracking

Status

Needs validation after reviewing Tracking and Services.



🟡 O-017

Title

OrbitalTracker assumes the Sun is always the central body.

Status

Pending

Action

Review after Reference Frames / Orbital Systems.


🟢 Architectural Pattern P-001

Tracking Components maintain historical simulation state without participating in physics calculations.


❓ Q-004

Who owns the simulation update pipeline and invokes OrbitalTracker.Update()?

Review after Services.


✅ C-001

Physics Model فقط مسئول قوانین فیزیک است.

نه حل عددی.

نه زمان.

نه رندر.


✅ C-002

Engine Pipeline تقریباً تأیید شد.

Universe

↓

PhysicsModel

↓

Acceleration

↓

Integrator

↓

Updated Bodies

قبلاً فقط حدس زده بودیم.

الان تقریباً تأیید شده.


✅ C-003

شتاب‌ها قبل از هرگونه Integration محاسبه می‌شوند.

این باعث می‌شود

تمام اجسام

جهان اولیه را ببینند.

این یک طراحی علمی صحیح است.


🟡 O-018
Title

PhysicsModel owns temporary simulation state.

Description
Dictionary<Guid, Vector3D> accelerations

داخل PhysicsModel نگهداری می‌شود.

این State مربوط به Domain نیست.

مربوط به Runtime Simulation است.

فعلاً مشخص نیست این بهترین محل نگهداری آن است یا خیر.

Status

Pending


🟡 O-019
Title

NewtonianPhysicsModel both evaluates physics and orchestrates integration.

Description

در حال حاضر این کلاس دو مسئولیت دارد:

محاسبهٔ شتاب‌ها
هماهنگ‌کردن Integrator

از دید معماری ممکن است این نقش «هماهنگ‌کنندهٔ مرحلهٔ فیزیک» باشد، نه صرفاً «مدل فیزیکی». برای قضاوت نهایی باید ابتدا Pipeline کامل Engine و Services بررسی شود.

Status

Pending


🟡 O-020
Title

Physical constant G remains locally hardcoded.

این همان Observation قبلی را تقویت می‌کند.

الان می‌بینیم

PhysicsModel هم

همان

const double G = 100;

را دارد.

پس O-011 کاملاً معتبرتر شد.


🟡 O-021
Title

NewtonianPhysicsModel mutates Universe indirectly through Integrator.

قبلاً گفته بودیم:

PhysicsModel mutates Universe

الان دقیق‌ترش این است:

خود PhysicsModel مستقیماً Body را تغییر نمی‌دهد.

بلکه

Integrator

این کار را انجام می‌دهد.

یعنی Mutation

از داخل PhysicsModel

Delegate می‌شود.

این Observation نسبت به O-005 دقیق‌تر است.


❓Q-005

آیا PhysicsModel باید فقط شتاب‌ها را تولید کند،

یا اجرای Integrator نیز جزو مسئولیت آن است؟

فعلاً پاسخ نمی‌دهیم.

بعد از دیدن Pipeline.


❓Q-006

آیا Simulation Pipeline در لایهٔ بالاتر (مثلاً SimulationService یا Engine) باید مدیریت شود و PhysicsModel صرفاً یک تابع خالص برای محاسبهٔ نیروها/شتاب‌ها باشد؟

فعلاً ثبت می‌شود.



VIP NOTE: به موقع ریفکتور و تصمیم گیری به یاد داشته باش که تمام فرمول های فیزیک به کار رفته شده و مفاهیم فیزیکی و عددی به کار رفته شده را به شکل کامنت در جای مناسب بزاریم
VIP NOTE: به یاد بیار موقع کدنویسی و ریفکتور که من میخوام ثابت های فیزیک قابل تغییر باشند از داخل شبیه ساز. مثلا یه تنظیمات برای شبیه ساز بذاریم که بتونیم مثلا مقادی ری همچون G رو خودمون حین اجرا تغییر بدیم و یک دکمه ی reset هم داشته باشه

✅ C-004

Subsystem مستقلی برای Maneuverها شکل گرفته است.

✅ C-005

Planning از Execution جدا شده.

این دقیقاً چیزی است که در نرم‌افزارهای واقعی Mission Design هم می‌بینیم.

✅ C-006

Thruster از Maneuver مستقل است.

این هم طراحی خوبی است.

چون بعداً می‌توانی

Ion Thruster

Chemical Engine

RCS

Nuclear

را اضافه کنی.


🟡 O-022
Title

Duplicate maneuver representations.

دو رکورد داریم.

HohmannTransfer

و

ManeuverPlan

که تقریباً دقیقاً یک داده را نگه می‌دارند.

Δv1

Δv2

TransferTime

TotalDeltaV

فعلاً معلوم نیست چرا هر دو وجود دارند.

ممکن است بعداً یکی نتیجه‌ی محاسبه و دیگری یک Plan عمومی باشد، اما در وضعیت فعلی تقریباً هم‌پوشانی دارند.

Status

Pending


🟡 O-023
Title

Planner constructs calculator internally.

همان الگویی که قبلاً دیده بودیم.

new HohmannTransferCalculator()

داخل Planner.

این همان Observation O-013 را تقویت می‌کند.



🟡 O-024
Title

Execution logic assumes exactly two burns.

این قسمت

BurnStep 0

BurnStep 1

BurnStep 2

BurnStep 3

کاملاً Hohmann-specific است.

در حالی که اسم کلاس

ManeuverExecutionSystem

خیلی عمومی است.

یعنی اسم می‌گوید:

تمام Maneuverها.

کد می‌گوید:

فقط Hohmann.

Pending.


🟡 O-025
Title

Transfer midpoint approximated as 50% of transfer time.

TransferTime * 0.5

در انتقال هوهمان ایده‌ی کلی این است که Burn دوم در رسیدن به آپوآپسیس مدار انتقال انجام شود. اگر از ابتدا TransferTime را به عنوان زمان کل مسیر انتقال (که برابر با نصف دوره‌ی مدار انتقال است) محاسبه کرده باشی، Burn دوم باید در پایان همین مدت انجام شود، نه در نصف آن.

بنابراین استفاده از 0.5 این سؤال را ایجاد می‌کند که تعریف TransferTime در Calculator دقیقاً چیست. ممکن است صرفاً برای Demo بوده باشد، اما از نظر علمی نیاز به بازبینی دارد.

Status

Pending


🟡 O-026
Title

ManeuverExecutionSystem computes burn direction itself.

داخل

ApplyBurn()

خود کلاس

radial

↓

tangent

↓

velocity

را محاسبه می‌کند.

در حالی که

ThrusterSystem

هم داریم.

ممکن است این دو بعداً همپوشانی پیدا کنند.

Pending.


🟡 O-027
Title

Current burn direction assumes circular equatorial orbit.

این قسمت

var tangent =
new Vector3D(
-radial.Y,
radial.X,
0)

تنها در یک حالت درست است:

مدار دوبعدی
صفحه XY
مدار تقریباً دایره‌ای

برای مدارهای سه‌بعدی یا بیضوی، جهت مماس به این سادگی به دست نمی‌آید.

این احتمالاً یک ساده‌سازی آگاهانه برای نسخه‌ی فعلی است.

Pending.



🟡 O-028
Title

ThrusterSystem mixes impulsive burns and continuous thrust.

داخل یک کلاس

هم داریم

ProgradeBurn()

که یک Impulse آنی است،

و هم

ApplyProgradeThrust()

که Continuous Thrust را مدل می‌کند.

این دو از نظر فیزیکی دو مفهوم متفاوت هستند، هرچند هر دو به سامانهٔ پیشران مربوط‌اند.

Pending.


❓Q-007

آیا ManeuverPlan قرار است یک Plan عمومی برای همهٔ انواع مانور باشد یا صرفاً نمایش دیگری از HohmannTransfer است؟

❓Q-008

آیا ThrusterSystem باید تنها مسئول تغییر Velocity باشد و ManeuverExecutionSystem صرفاً زمان‌بندی و ترتیب اجرای Burnها را مدیریت کند؟

❓Q-009

تعریف دقیق TransferTime در HohmannTransferCalculator چیست؟ آیا زمان رسیدن به Burn دوم است یا زمان کامل مسیر انتقال؟ پاسخ این سؤال تعیین می‌کند که شرط TransferTime * 0.5 از نظر علمی درست است یا خیر.


VIP NOTE: به موقع ریفکتور و تصمیم گیری به یاد داشته باش که تمام فرمول های فیزیک به کار رفته شده و مفاهیم فیزیکی و عددی به کار رفته شده را به شکل کامنت در جای مناسب بزاریم
VIP NOTE: به یاد بیار موقع کدنویسی و ریفکتور که من میخوام ثابت های فیزیک قابل تغییر باشند از داخل شبیه ساز. مثلا یه تنظیمات برای شبیه ساز بذاریم که بتونیم مثلا مقادی ری همچون G رو خودمون حین اجرا تغییر بدیم و یک دکمه ی reset هم داشته باشه


VIP NOTE: به موقع ریفکتور و تصمیم گیری به یاد داشته باش که تمام فرمول های فیزیک به کار رفته شده و مفاهیم فیزیکی و عددی به کار رفته شده را به شکل کامنت در جای مناسب بزاریم
VIP NOTE: به یاد بیار موقع کدنویسی و ریفکتور که من میخوام ثابت های فیزیک قابل تغییر باشند از داخل شبیه ساز. مثلا یه تنظیمات برای شبیه ساز بذاریم که بتونیم مثلا مقادی ری همچون G رو خودمون حین اجرا تغییر بدیم و یک دکمه ی reset هم داشته باشه


VIP NOTE: به موقع ریفکتور و تصمیم گیری به یاد داشته باش که تمام فرمول های فیزیک به کار رفته شده و مفاهیم فیزیکی و عددی به کار رفته شده را به شکل کامنت در جای مناسب بزاریم
VIP NOTE: به یاد بیار موقع کدنویسی و ریفکتور که من میخوام ثابت های فیزیک قابل تغییر باشند از داخل شبیه ساز. مثلا یه تنظیمات برای شبیه ساز بذاریم که بتونیم مثلا مقادی ری همچون G رو خودمون حین اجرا تغییر بدیم و یک دکمه ی reset هم داشته باشه


VIP NOTE: به موقع ریفکتور و تصمیم گیری به یاد داشته باش که تمام فرمول های فیزیک به کار رفته شده و مفاهیم فیزیکی و عددی به کار رفته شده را به شکل کامنت در جای مناسب بزاریم
VIP NOTE: به یاد بیار موقع کدنویسی و ریفکتور که من میخوام ثابت های فیزیک قابل تغییر باشند از داخل شبیه ساز. مثلا یه تنظیمات برای شبیه ساز بذاریم که بتونیم مثلا مقادی ری همچون G رو خودمون حین اجرا تغییر بدیم و یک دکمه ی reset هم داشته باشه


---
Version: 1.0

Last Updated: 2026-07-14