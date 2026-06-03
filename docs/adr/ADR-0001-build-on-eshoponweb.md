# ADR-0001 — Build on eShopOnWeb as a single deployable app

- **Status:** Accepted
- **Date:** `[fill in]`
- **Deciders:** Engineering Lead

## Context

JMT needs an online store quickly, maintained by a very small team (one engineering lead, one QA,
and AI coding agents). We forked NimblePros/eShopOnWeb, an ASP.NET Core 10 reference store built
with Clean Architecture and a single-process (monolithic) deployment model.

We considered: (a) building from scratch, (b) using a SaaS platform (Shopify/WooCommerce),
(c) extending eShopOnWeb as-is.

## Decision

We extend eShopOnWeb as a single deployable application. We keep its existing project structure
(Web, PublicApi, BlazorAdmin, BlazorShared, ApplicationCore, Infrastructure) and its patterns
(EF Core, repository + specification, IEmailSender). We do **not** introduce microservices,
message brokers, or new architectural modules.

## Consequences

- **Positive:** fast start on a proven codebase; one thing to deploy and reason about; matches the
  team's size; AI agents can make small, contained changes.
- **Negative:** we inherit eShopOnWeb's conventions and any limitations; some agri/wholesale
  features must be built rather than configured.
- **Revisit when:** a concrete, measured operational signal proves the monolith is the bottleneck
  (not before). Until then, simplicity wins.

---

<!--
  Copy this template for new decisions: ADR-0002, ADR-0003, ...
  Likely next ADRs for JMT:
   - ADR-0002: Payment gateway choice (PayTabs vs Telr)
   - ADR-0003: Messaging provider choice (Twilio vs Unifonic) and SMS-first, WhatsApp-later
   - ADR-0004: Retail-first, wholesale-second sequencing
   - ADR-0005: AED-only currency
-->

## ADR template

```
# ADR-XXXX — <short title>

- Status: Proposed | Accepted | Superseded by ADR-YYYY
- Date:
- Deciders:

## Context
<what problem / forces are at play>

## Decision
<what we decided>

## Consequences
<positive, negative, and when to revisit>
```
