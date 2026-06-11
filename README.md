# Budget Tracker

A self-hosted, multi-user budget tracker. A household runs one backend; each member keeps their own wallets and records income and expenses against them. Planned clients: CLI, Telegram bot, and web/app — all talking to the same server and synchronizing data.

> **Status: early development.** The domain core (the `Wallet` aggregate, value objects, validation, optimistic concurrency) and its unit tests are in place. There is no persistence, API, or working client yet — the CLI is a stub. See the [roadmap](ROADMAP.md).

## Why

A learning project that is also meant to be used. The goal is a small, correct financial domain built deliberately with Clean Architecture, DDD, and CQRS, then grown into a real tool (Telegram quick-entry, receipt upload, LLM-assisted categorization) without over-engineering.

## Design

- **Clean Architecture** — dependencies point inward. `Domain` depends on nothing; `Application` depends on `Domain`; `Shared` is a leaf. Infrastructure and clients sit at the edges.
- **DDD** — `Wallet` is the aggregate root and owns its invariants: currency consistency, sufficient funds, and atomic balance/transaction/version updates. Other aggregates are referenced by id.
- **Lightweight CQRS** — separate command and query handlers with read-shaped DTOs, over one model and one store. Heavy CQRS (separate read store, event sourcing) is intentionally avoided: balances need strong consistency and the scale does not justify it.
- **Railway-oriented errors** — domain operations return `Result` / `Result<T>` instead of throwing for expected failures.
- **Immutable value objects** — `Money`, `Balance` (factory-validated, private constructors).
- **Optimistic concurrency** — each aggregate carries a `Version`; conflicts surface as `ConcurrencyConflictException`.

## Project layout

| Project | Responsibility |
| --- | --- |
| `BudgetTracker.Domain` | Aggregates, entities, value objects, domain errors |
| `BudgetTracker.Application` | Commands, queries, handlers, ports (`IWalletRepository`) |
| `BudgetTracker.Shared` | Cross-cutting primitives (`Result`, `Money`, `Error`, `Currency`) |
| `BudgetTracker.CLI` | Entry point (stub for now) |
| `BudgetTracker.Domain.Tests` | Domain unit tests (xUnit) |

## Build & test

Requires the .NET 10 SDK.

```bash
dotnet build
dotnet test
```

## Roadmap

See [ROADMAP.md](ROADMAP.md). Work is tracked as GitHub issues grouped into milestones M1–M7.
