# SMOKE-TEST.md — Manual regression baseline (QA)

> A short, fixed list of steps that must always work. Run it before every release and after any
> significant change. It is the behavioural baseline for the store. Mark PASS/FAIL and note
> anything surprising. As features ship (instant checkout, messaging, wholesale), add their happy
> paths here.

Build/version under test: `[fill in]`   Date: `[fill in]`   Tester: `[fill in]`

---

## Core retail path (must always pass)

| # | Step | Expected result | Pass? |
|---|------|-----------------|-------|
| 1 | Open the storefront home/catalog | Catalog loads with products and images | `[ ]` |
| 2 | Filter by category (e.g. Coffee) | Only matching products show | `[ ]` |
| 3 | Filter by origin/brand | Only matching products show | `[ ]` |
| 4 | Search for a product name | Relevant results appear | `[ ]` |
| 5 | Open a product detail page | Name, AED price, description, agri fields show | `[ ]` |
| 6 | Add an item to the basket | Basket count/total updates | `[ ]` |
| 7 | Change quantity in the basket | Total recalculates correctly (AED) | `[ ]` |
| 8 | Proceed to checkout | Checkout page loads; prompts login if required | `[ ]` |
| 9 | Register / log in | Account works; basket is preserved | `[ ]` |
| 10 | Place an order (or invoice fallback) | Order is created; confirmation shown | `[ ]` |
| 11 | View order in account history | The order appears with correct items/total | `[ ]` |

## Admin path

| # | Step | Expected result | Pass? |
|---|------|-----------------|-------|
| A1 | Log into the admin area (with PublicApi running) | Admin loads | `[ ]` |
| A2 | List catalog items | Products appear | `[ ]` |
| A3 | Edit a product (e.g. price, origin) | Change saves and shows on storefront | `[ ]` |

## Notifications (add when Phase A4 ships)

| # | Step | Expected result | Pass? |
|---|------|-----------------|-------|
| N1 | Place an order | Confirmation email received | `[ ]` |
| N2 | Place an order | Confirmation SMS received | `[ ]` |
| N3 | (When live) Payment completes | Payment-received message received | `[ ]` |

## Wholesale (add when Phase B ships)

| # | Step | Expected result | Pass? |
|---|------|-----------------|-------|
| W1 | Log in as a wholesale buyer | Bulk/tier pricing is visible | `[ ]` |
| W2 | Try below the MOQ | Blocked with a clear message | `[ ]` |
| W3 | Submit a quote request | Request is recorded; sales is notified | `[ ]` |
| W4 | Sales responds to the quote | Buyer gets a "quote ready" message | `[ ]` |

---

## Notes / defects found

`[Log anything that failed or behaved unexpectedly, with steps to reproduce.]`
