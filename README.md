# JMT starter files

Drop these into the root of your forked eShopOnWeb repository. They set up the guardrails,
CI, and documentation described in the JMT Implementation Plan, so the team and AI agents start
on the right footing.

## Where each file goes

```
<repo root>/
├── AGENTS.md                          ← AI agent rules (read first)
├── .cursorrules                       ← condensed rules for Cursor
├── CONTEXT.md                         ← project orientation
├── global.json                        ← pins the .NET SDK version
├── .env.example                       ← placeholder config (NO real secrets)
├── .github/
│   ├── workflows/
│   │   └── ci.yml                     ← build + test on every PR
│   └── pull_request_template.md       ← enforces the checklist on every PR
└── docs/
    ├── SETUP-RUNBOOK.md               ← how to run locally (fill in as you go)
    ├── ENVIRONMENT.md                 ← confirmed versions + account status
    ├── MIGRATIONS.md                  ← safe database-change rules
    ├── SMOKE-TEST.md                  ← QA regression baseline
    └── adr/
        └── ADR-0001-build-on-eshoponweb.md
```

## First steps (Phase 0)

1. Commit these files on a branch and open a PR (this also tests branch protection).
2. In GitHub repo settings, protect `main`: require a pull request and require the **CI** check to pass.
3. Add a repository secret named `TEST_DB_PASSWORD` (a strong password) so CI's SQL Server starts.
4. Adjust `global.json` to the exact .NET 10 SDK version your team installs.
5. Work through `docs/SETUP-RUNBOOK.md`, filling in every `[CONFIRM: ...]` placeholder.
6. Fill in `docs/ENVIRONMENT.md` and start the external-account applications (payment, messaging).

## Important

- Never commit real secrets. `.env.example` holds placeholders only.
- These files encode the rules in `AGENTS.md`. If you change how the team works, update `AGENTS.md`
  and `.cursorrules` together so humans and AI stay in sync.
