# 12 - اجسام مرجع در مکانیک مداری

Version: 1.1
Last Updated: 2026-08-19
Status: Draft
Reviewed: Pending implementation verification

## Goal

درک تفاوت میان جسم مانوردِهنده، جسم مرکزی، مبدأ مختصات
و هدف دوربین در یک شبیه‌سازی مداری.

## Motivation

یک مدار همیشه نسبت به یک مرجع تعریف می‌شود.

اینکه یک جسم در مختصات جهانی کجا قرار دارد، به تنهایی
شعاع مدار آن را مشخص نمی‌کند.

## The Question

اگر Explorer-1 به دور خورشید حرکت کند،
شعاع مدار آن چگونه تعریف می‌شود؟

## Intuition

موقعیت خود فضاپیما کافی نیست.

آنچه اهمیت دارد فاصله فضاپیما از جسمی است که
مدار نسبت به آن تعریف شده است.

## Scientific View

در مدل فعلی انتقال هوهمان Cosmos Engine:

- Maneuvering Body: Explorer-1
- Central Body: Sun
- Reference Frame: Sun-centered
- Camera Target: فاقد نقش فیزیکی در مانور

هدف دوربین فقط یک مفهوم مربوط به نمایش است و نباید
روی محاسبات فیزیکی تأثیر بگذارد.

## Mathematics

بردار مکان نسبی:

r⃗ = x⃗_spacecraft - x⃗_centralBody

شعاع مدار:

r = |r⃗|

پارامتر گرانشی:

μ = GM

در مدل فعلی:

G = 100
M_sun = 100000

بنابراین:

μ = 10,000,000


### سرعت نسبی

برای تحلیل حرکت مداری، سرعت فضاپیما نیز باید نسبت به جسم مرکزی تعریف شود.

v⃗_rel = v⃗_spacecraft - v⃗_centralBody

اگر Sun و Explorer-1 هر دو دارای یک سرعت انتقالی مشترک باشند،
آن سرعت مشترک نباید جهت یا اندازه مانور مداری را تغییر دهد.

در مدل فعلی Hohmann، جهت prograde باید از سرعت نسبی فضاپیما
نسبت به جسم مرکزی تعیین شود، نه از سرعت آن در مختصات جهانی.

این مدل همچنان فرض می‌کند burn در نقطه‌ای انجام می‌شود که
سرعت مداری تقریباً مماس بر مدار است.


## Cosmos Engine

Hohmann Transfer آزمایشی فعلی باید به صورت صریح
Explorer-1 را به عنوان جسم مانوردِهنده و Sun را
به عنوان جسم مرکزی در نظر بگیرد.

Camera.Target نباید تعیین کند که چه جسمی burn دریافت می‌کند.

همچنین محاسبه شعاع مدار نباید به این فرض پنهان وابسته باشد
که Sun همیشه در مبدأ مختصات قرار دارد.

## Common Misconceptions

- Camera Target همان Central Body نیست.
- Position.Magnitude() همیشه شعاع مدار نیست.
- جسم مرکزی الزاماً در origin قرار ندارد.
- Hohmann Transfer فعلی یک مدل ساده‌شده و impulsive است.

## Summary

مدار یک رابطه میان اجسام است، نه صرفاً یک مختصات جهانی.

در Cosmos Engine باید Maneuvering Body،
Central Body و Camera Target مفاهیمی مستقل باقی بمانند.

## Further Reading

- Two-body orbital mechanics
- Reference frames
- Hohmann transfer
- Gravitational parameter μ