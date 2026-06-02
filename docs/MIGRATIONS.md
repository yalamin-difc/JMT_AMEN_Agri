# MIGRATIONS.md — Database changes, done safely

> The team is small and cannot recover easily from data loss. Every database change follows
> these rules. This is YELLOW-zone work: AI may draft a migration, but the engineering lead
> reviews the generated SQL before it runs anywhere real.

---

## The rules

1. **Additive only.** Add new columns/tables. Do **not** drop or rename existing columns or
   tables in a normal change.
2. **New columns are nullable or have a default.** This guarantees existing rows stay valid and
   the migration can apply to a populated database.
3. **Test on both an empty and a populated database** before merging.
4. **Every migration must be reversible.** Confirm the `Down` step is sensible.
5. **Seed data must be idempotent** — running it twice must not create duplicates.

---

## Add a migration

After changing an entity and its EF configuration:

```
dotnet ef migrations add <DescriptiveName> -c CatalogContext -p src/Infrastructure -s src/Web
```

`[CONFIRM: the actual context name and project paths for this repo]`

Use clear names: `AddAgriProductFields`, `AddMinimumOrderQuantity`, etc.

---

## Apply a migration (local)

```
dotnet ef database update -c CatalogContext -p src/Infrastructure -s src/Web
```

---

## Verify before merge (QA checklist)

- [ ] Migration applies cleanly to a **fresh/empty** database
- [ ] Migration applies cleanly to a **populated** database (with existing seed data) — no data loss
- [ ] Existing seed data still loads
- [ ] New columns are nullable or defaulted
- [ ] `Down` migration is present and sensible (reversible)
- [ ] Seed is idempotent (re-running does not duplicate)
- [ ] Smoke test still passes

---

## Roll back

To revert to the migration *before* the last one:

```
dotnet ef database update <PreviousMigrationName> -c CatalogContext -p src/Infrastructure -s src/Web
```

To remove the last (not-yet-applied) migration from the code:

```
dotnet ef migrations remove -c CatalogContext -p src/Infrastructure -s src/Web
```

---

## Example: the first agri-product migration (Phase A)

Adding the fields from the implementation plan (`UnitOfMeasure`, `OriginRegion`, `Grade`,
`ProcessingMethod`, `CropYear`, `MoisturePercent`, `MinimumOrderQuantity`) — all nullable or
defaulted so existing catalog rows remain valid:

```
dotnet ef migrations add AddAgriProductFields -c CatalogContext -p src/Infrastructure -s src/Web
dotnet ef database update -c CatalogContext -p src/Infrastructure -s src/Web
```

Then QA runs the verify checklist above against a populated database.
