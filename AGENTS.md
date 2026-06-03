# AGENTS.md — Rules for AI coding agents working on this repository

> This file tells AI coding agents (Cursor, Claude, Copilot, etc.) how to behave in this
> repository. **Read this file fully before making any change.** These rules are not
> suggestions — they exist because this project is maintained by a very small team
> (one engineering lead + one QA) and cannot absorb large, unreviewed, or risky changes.

---

## 1. What this project is

This is the e-commerce store for **JMT International Free Zone Company**, a Dubai (UAE)
trading company that imports Ethiopian-origin agricultural products — green coffee, sesame,
pulses/beans, and spices — and sells them to buyers in Dubai.

The codebase is a **fork of NimblePros/eShopOnWeb**, an ASP.NET Core 10 reference store built
with Clean Architecture. We are **extending** it, not rewriting it.

We sell to **two kinds of buyer** from **one catalog**:
- **Retail** — individuals buying small bags (250 g / 500 g / 1 kg). Instant card checkout.
- **Wholesale** — cafés, roasters, hotels, traders buying sacks/cartons by weight. Request-a-quote.

---

## 2. The golden rules (never break these)

1. **Do not rewrite or re-architect.** Keep eShopOnWeb's existing project structure
   (Web, PublicApi, BlazorAdmin, BlazorShared, ApplicationCore, Infrastructure) exactly as-is.
   Do **not** introduce microservices, message brokers, new architectural "modules", Docker
   orchestration, or a different ORM. If you think a refactor is needed, **stop and propose it
   in a comment — do not perform it.**

2. **Make small, scoped changes.** One field, one page, or one feature per change. Never
   produce a diff that touches dozens of files at once. If a task is large, break it into steps
   and do the first step only.

3. **All changes are additive and reversible.** Especially database changes — new columns must
   be **nullable or defaulted**. Never drop or rename existing columns, tables, or public APIs
   in a single change.

4. **Never touch the RED ZONE without explicit human direction** (see section 5).

5. **Never commit secrets.** API keys, connection strings, gateway credentials, and messaging
   provider tokens go in environment variables / .NET user secrets — **never** in source code,
   `appsettings.json`, or this repo. If you need a secret, add a **placeholder** and a note.

6. **Write or update tests for every behavioural change.** If you add a feature, add a test.
   If the lead cannot see a test proving it works, the change does not merge.

7. **Currency is AED only.** All prices, all display, everywhere. Do not add other currencies.

---

## 3. How to work

- Branch from `main`. Never push directly to `main` (it is protected).
- Keep each pull request focused on one task. Small PRs get reviewed; large PRs get rejected.
- Run `dotnet build` and `dotnet test` locally (or rely on CI) **before** marking a PR ready.
- Explain your change in the PR description in plain language: what, why, and how to test it.
- If you are unsure, **ask in a comment instead of guessing.** A question is cheap; a wrong
  guess in payment or auth code is expensive.

---

## 4. Tech stack facts (do not deviate)

- **.NET / ASP.NET Core 10** — the version is pinned in `global.json`. Do not change it.
- **Entity Framework Core** for data access, with the repository + specification pattern
  (`Ardalis.Specification`) that eShopOnWeb already uses. Reuse it; do not invent a new
  data-access approach.
- **Razor Pages** for the storefront, **Blazor** for the admin area — same as upstream.
- **SQL Server** database (one database). No second datastore, no Redis, unless the lead
  explicitly asks.
- Existing abstractions to **reuse, not replace**: `IEmailSender` for email, the
  repository interfaces for data, and the payment abstraction for checkout.

---

## 5. AI involvement zones — know which zone your task is in

### GREEN — you may drive (still reviewed, but low risk)
- Branding, copy, About/company content
- Seed data (categories, products, origins, descriptions)
- Adding simple, additive product fields and showing them on a page
- Writing unit/integration tests
- Documentation

### YELLOW — you may draft, the lead reviews carefully
- New Razor Pages or Blazor components
- EF Core migrations (must be additive)
- The wholesale "Request a Quote" flow
- The messaging integration (`IMessageSender`: SMS / WhatsApp)
- Admin screens

### RED — you may ASSIST ONLY; the human owns it line by line. Do NOT complete autonomously.
- **Payment / checkout** code and the payment gateway integration (PayTabs / Telr)
- **Authentication, authorization, identity, passwords, tokens**
- **Anything that calculates money** — prices, totals, tax, discounts, tiered pricing
- **Security-sensitive** configuration (CORS, cookies, secrets handling)

If a task is RED, produce a draft or explanation, clearly say "this is RED-zone, human must
verify", and **do not** mark it as finished.

---

## 6. Domain vocabulary (use these terms correctly)

- **Catalog item** — a single product (e.g. "Yirgacheffe green coffee, washed, Grade 1").
- **Origin / region** — Ethiopian source: Yirgacheffe, Sidamo, Guji, Limu, Harrar, etc.
- **Grade** — quality grade (Grade 1, Grade 2, Specialty).
- **Process** — coffee processing method: Washed, Natural, Honey.
- **Crop year** — harvest year, e.g. "2025/26".
- **Unit of measure** — how it's sold: 250 g bag (retail), kg / quintal / sack / ton (wholesale).
- **MOQ** — minimum order quantity, used for wholesale.
- **Retail buyer** vs **wholesale buyer** — the two customer types; same catalog, different pricing/flow.

---

## 7. What to do when you finish a task

1. Confirm the build passes and tests are green.
2. Confirm you did not add secrets, did not break existing data, and stayed in your zone.
3. Write a short PR description (what / why / how to test).
4. Hand off to the engineering lead for review. **Do not merge your own work.**

---

## 8. Things that are explicitly OUT OF SCOPE for this application

- **Paying Ethiopian exporters / suppliers.** That is JMT's treasury/banking function and lives
  entirely outside this app. Never build supplier-payment features.
- **Customs / import logistics.** Handled by JMT's freight forwarder, not the website.
- **Multi-currency.** AED only.
- **Microservices, Kubernetes, message brokers.** Not now. Not unless the lead explicitly asks.
