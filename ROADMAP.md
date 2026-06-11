# Roadmap

Phased plan, tracked as GitHub milestones. Each milestone is a prerequisite for the next: nothing is built before the domain runs, and identity is laid into the model before clients consume it. Shared wallets are deliberately deferred to the end.

## M1 — Runnable core
Make the domain execute end-to-end and clean up the foundations.
- Remove dead `IBudgetRepository` interface
- Reject null in `Result<T>.Success`
- Make transaction date deterministic (inject the clock)
- Add in-memory `IWalletRepository`
- CLI: run the core flow end-to-end

## M2 — Tenant seam (stubbed)
Wire multi-tenant ownership through the app, with a stubbed current user. No real auth yet.
- Introduce `ICurrentUser` port with a stub
- Add `Wallet.OwnerId` and scope access by current user (NotFound on mismatch)

## M3 — Server & persistence
Stand up the single backend and a real store; enable multi-client sync.
- Expose the application over an HTTP API
- EF Core persistence and real repository
- Read-side query port with projections (learning)

## M4 — Authentication & users
Replace the stub with real identity.
- `User` aggregate and Telegram identity binding (one-time code)
- Replace stub `ICurrentUser` with real authentication

## M5 — Telegram bot
First real multi-client.
- Quick entry and balance listing
- Receipt upload

## M6 — AI assistance
LLM behind ports; domain stays clean.
- Category suggestion via LLM
- Spending insights and advice

## M7 — Shared wallets (later)
Household/shared wallets.
- Membership and transaction authorship (`CreatedBy`); access limited to members

## Deliberately out of scope (for now)
- Separate auth microservice / gRPC auth service — auth is a module inside the single backend, not a networked service.
- Heavy CQRS (separate read store, event sourcing) — incompatible with strong-consistency balances at this scale.
- A separate `Household` aggregate — a member set on the wallet covers the need until org-level features exist.
