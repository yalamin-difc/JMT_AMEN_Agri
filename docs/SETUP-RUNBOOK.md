# SETUP-RUNBOOK.md — Running the store locally

> Goal: a new person can go from a clean machine to a running store by following this top to
> bottom. Fill in the `[CONFIRM: ...]` placeholders during Phase 0 as you actually run it,
> then delete this note.

---

## 1. Prerequisites

Install these first:

- **.NET SDK 10** — the exact version is pinned in `global.json`. Verify with:
  ```
  dotnet --version
  ```
  It should report a 10.x version. `[CONFIRM: exact version observed]`
- **SQL Server** — local instance or a container. Recommended for consistency:
  ```
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_strong_password1" \
    -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
  ```
  `[CONFIRM: which approach the team standardised on]`
- **Git**, and access to the JMT fork on GitHub.
- (For the storefront's client assets, if needed) **Node.js** — `[CONFIRM: version, or "not required"]`

---

## 2. Get the code

```
git clone https://github.com/<JMT-ORG>/eShopOnWeb.git
cd eShopOnWeb
```

`[CONFIRM: actual repo URL]`

---

## 3. Configure secrets (never commit these)

Use .NET user secrets, not appsettings, for the database connection and any keys:

```
dotnet user-secrets set "ConnectionStrings:CatalogConnection" "Server=localhost,1433;Database=Microsoft.eShopOnWeb.CatalogDb;User Id=sa;Password=Your_strong_password1;TrustServerCertificate=True;" --project src/Web
dotnet user-secrets set "ConnectionStrings:IdentityConnection" "Server=localhost,1433;Database=Microsoft.eShopOnWeb.Identity;User Id=sa;Password=Your_strong_password1;TrustServerCertificate=True;" --project src/Web
```

`[CONFIRM: exact connection string names the app expects — check appsettings.json keys]`

---

## 4. Create / migrate the database

```
dotnet ef database update -c CatalogContext -p src/Infrastructure -s src/Web
dotnet ef database update -c AppIdentityDbContext -p src/Infrastructure -s src/Web
```

`[CONFIRM: actual context names and whether seed runs automatically on first launch]`

---

## 5. Run the storefront

```
dotnet run --project src/Web
```

Then open the URL it prints (typically `https://localhost:5001`).
You should see the catalog with seed products. `[CONFIRM: actual local URL]`

---

## 6. Run the admin area (needs PublicApi too)

The Blazor admin needs the API running **at the same time**. In a second terminal:

```
dotnet run --project src/PublicApi
```

Then browse to the `/admin` path on the storefront. `[CONFIRM: exact admin URL and default login]`

> This dual-project requirement is the #1 local-setup gotcha. If admin looks broken, the API
> is probably not running.

---

## 7. Smoke test (QA owns this)

Confirm the happy path works end to end:

1. Browse the catalog
2. Filter by category / brand
3. Open a product detail page
4. Add to basket
5. Go to checkout

`[CONFIRM: any step that behaves unexpectedly — log it]`

---

## Troubleshooting

- **Wrong SDK version:** install the version in `global.json`. `[CONFIRM: install command used]`
- **DB connection fails:** check the container is running and the password matches the secret.
- **Admin is blank/broken:** make sure `PublicApi` is running.
- `[CONFIRM: add any machine-specific gotchas the team hit]`
