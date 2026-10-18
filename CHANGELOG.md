# Changelog

## Unreleased

- Restore animal icons beside names in the Animals tab using RimWorld's built-in label-column rendering.

## 1.7.0 — Unofficial release candidate, 2026-10-16

- Contain reflected NullReferenceException failures in milk/wool Active getters so they do not abort the table.
- Retry after one second; warn once per component with animal ID and race. Recovery resumes display; unrelated exceptions still propagate.
- Use weak-key diagnostic state; leave existing component caches unchanged.
- Use WorldRendererUtility.WorldRendered for newer game references.
- Add a startup marker for the patched build.
- Preserve assembly identity AnimalTab 8.0.0.0; start this fork's release numbering at 1.7.0.
- Add portable build, regression tests, Windows CI, and release packaging.
- Preserve the original Fluffy.AnimalTab package ID for dependency checks and load-order rules, require Harmony, and document replacing the original installation.
- Retain licenses/credits and remove upstream publishing IDs and update metadata.

Limitation: the invalid map/holder state itself has not been repaired. A game update occurred between the failing and successful local tests. The standalone fork still requires an in-game smoke test.
