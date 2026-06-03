# ENVIRONMENT.md — Confirmed versions & accounts

> Fill in the `[CONFIRM]` cells during Phase 0 so the whole team (and AI agents) work against
> the same known environment. Update whenever something changes.

## Toolchain

| Item | Expected | Confirmed value | Confirmed by | Date |
|------|----------|-----------------|--------------|------|
| .NET SDK | 10.x (see `global.json`) | `[CONFIRM]` | | |
| .NET runtime | 10.x | `[CONFIRM]` | | |
| Database engine | SQL Server 2022 | `[CONFIRM]` | | |
| Node.js (if needed) | `[CONFIRM or "n/a"]` | `[CONFIRM]` | | |
| OS used by team | mixed | `[CONFIRM]` | | |

## External service accounts (status)

| Service | Purpose | Account opened? | Approved? | Owner | Notes |
|---------|---------|-----------------|-----------|-------|-------|
| PayTabs / Telr | Card checkout (AED) | `[ ]` | `[ ]` | Lead | Approval can take days–weeks |
| Twilio / Unifonic | SMS + WhatsApp | `[ ]` | `[ ]` | Lead | One provider, both channels |
| WhatsApp Business | WhatsApp messages | `[ ]` | `[ ]` | Lead | Needs Meta verification + template approval |
| Domain registrar | Production domain | `[ ]` | n/a | Lead | |
| Hosting | Run the app in production | `[ ]` | n/a | Lead | e.g. Azure App Service |

## Secrets locations (never in the repo)

| Secret | Where it lives | Notes |
|--------|----------------|-------|
| DB connection string | .NET user secrets (local), env vars / key vault (prod) | |
| Payment gateway keys | env vars / key vault | RED zone |
| Messaging provider token | env vars / key vault | |
| `[CONFIRM others]` | | |
