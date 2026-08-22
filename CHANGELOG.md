English | [한국어](CHANGELOG.ko.md)

# Changelog

All notable changes to this project are documented here. Format loosely follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [1.0.0.2] - 2026-08-22

### Fixed
- Sync hit-testing could miss a genuinely-matching descendant when an intermediate ancestor's bounding rect was stale/clipped (e.g. Chrome's GPU-compositor surface pane). Recursion now walks every child unconditionally and keeps the smallest-area match instead of gating on each ancestor's rect.

### Added
- **Control View** toggle for the UIA tree — RawView (default) shows every UIA peer including Legacy IAccessible bridge duplicates; ControlView filters to `IsControlElement` only, reducing duplicate/noisy nodes for Sync and tree browsing.
- **Selector2** in the Property panel — AND-combines every available identifying property (AutomationId/Name/ClassName/ControlType), for cases where a single property alone (e.g. a reused AutomationId) is ambiguous.
- **KeyPath** — sibling-index path from the tab root to the selected element (e.g. `0/2/1`), as a last-resort fallback identifier.
- **TypePath** — ControlType path from the tab root to the selected element (e.g. `.\pane\pane\...\combo box`).

### Performance
- KeyPath/TypePath are computed from the already-loaded tree structure (`TreeNode.Parent`/`.Index`) instead of issuing fresh UIA calls, avoiding a per-selection round-trip cost that was noticeable on deep trees.

## [1.0.0.1] - 2026-08-15 – 2026-08-18

### Added
- Initial public release on GitHub.
- Window state control (Show/Hide/Enable/Disable/Move/Resize/Always On Top for Win32; Minimize/Maximize/Restore/Close/Move/Resize/Rotate for UIA elements).
- System Diagnostics dialog (system-wide GDI/USER object ranking, full desktop Z-order stack).
- A/B Property Snapshot Compare, with diff export.
- Message Log ↔ Rect/DPI correlation view.
- Sync (hover-tracking) mode for both the Win32 and UIA hierarchy trees.
- Application icon.

### Changed
- Documentation updates; source cleanup pass.
