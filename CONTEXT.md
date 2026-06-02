# CONTEXT.md — Project orientation

This document orients any new person — or AI agent — to what this project is, who it serves,
and how the pieces fit. Read this first, then read `AGENTS.md` for the working rules.

---

## The business

**JMT International Free Zone Company** is a UAE (Dubai) trading company. It imports
Ethiopian-origin agricultural products and sells them to buyers in Dubai through this online store.

- **Products:** green coffee, sesame, pulses/beans, spices.
- **Customers:** in Dubai. Two types — **retail** (individuals, small bags) and **wholesale**
  (cafés, roasters, hotels, traders buying in bulk).
- **Currency:** AED only.

### Two separate money flows (important)

| Flow | Who pays whom | Lives in this app? |
|------|----------------|--------------------|
| Customer checkout | Dubai buyer pays JMT (AED) | **Yes** |
| Supplier payment | JMT pays Ethiopian exporters (USD) | **No** — treasury/banking, outside the app |

The application **only** handles customer checkout. Paying suppliers in Ethiopia is a finance
function and must never be built into the website.

---

## The application

A fork of **NimblePros/eShopOnWeb** — an ASP.NET Core 10 reference e-commerce app built with
Clean Architecture and a monolithic (single deployable) model. We extend it; we do not rewrite it.

### Projects (unchanged from upstream)

| Project | Role |
|---------|------|
| `Web` | Razor Pages storefront (what customers see) |
| `PublicApi` | Web API used by the admin app |
| `BlazorAdmin` | Blazor admin portal (manage catalog, orders) — needs `PublicApi` running |
| `BlazorShared` | Shared models between admin and API |
| `ApplicationCore` | Domain entities, interfaces, specifications (the heart of the app) |
| `Infrastructure` | EF Core, data access, email, external services |

> Note: the admin area depends on `PublicApi`. To see admin working locally, run **both**
> `Web` and `PublicApi`. See `SETUP-RUNBOOK.md`.

### Key technologies

- ASP.NET Core 10 (version pinned in `global.json`)
- Entity Framework Core + `Ardalis.Specification` (repository/specification pattern)
- SQL Server (one database)
- ASP.NET Identity for accounts

---

## What we are building (in order)

1. **Phase 0 — Foundation:** CI pipeline, branch protection, these guardrail docs, a smoke test.
2. **Phase A — Retail launch:** JMT branding, Ethiopian agri content (AED), additive product
   fields, instant card checkout (PayTabs/Telr), email + SMS order notifications.
3. **Phase B — Wholesale:** customer types, gated bulk pricing, minimum order quantities,
   request-a-quote flow, "quote ready" messages.
4. **Phase C — Hardening:** backups, HTTPS, domain, VAT config, Apple Pay/BNPL, runbook, soft launch.

Retail launches **before** wholesale is built. Never build both at once.

---

## External services (set up accounts early — approvals take time)

| Service | Purpose | Notes |
|---------|---------|-------|
| PayTabs **or** Telr | Card checkout in AED | UAE merchant approval can take days–weeks |
| Twilio **or** Unifonic | SMS + WhatsApp messages | Single provider for both channels |
| WhatsApp Business (via provider/Meta) | WhatsApp notifications | Needs business verification + template approval (lead time) — not on launch critical path |

---

## Who does what

- **Engineering Lead** — reviews every change, owns RED-zone work (payments, auth, money math),
  approves all merges.
- **QA** — owns the smoke test and regression checks; verifies migrations and notifications.
- **AI agents (Cursor, etc.)** — drive GREEN-zone work, draft YELLOW-zone work, assist only on
  RED-zone work. Bound by `AGENTS.md`.

---

## Where to find things

- Working rules for humans and AI: `AGENTS.md`, `.cursorrules`
- How to run locally: `docs/SETUP-RUNBOOK.md`
- Confirmed versions: `docs/ENVIRONMENT.md`
- How migrations work here: `docs/MIGRATIONS.md`
- The full plan this is based on: the JMT Implementation Plan document.
