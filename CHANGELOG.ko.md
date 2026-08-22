[English](CHANGELOG.md) | 한국어

# Changelog

이 프로젝트의 주요 변경 사항을 기록합니다. 형식은 [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)를 느슨하게 따릅니다.

## [1.0.0.2] - 2026-08-22

### Fixed
- Sync 기능 시 Hittest bounding rect가 포함되어 있지 않을 때 실제로 매치하는 하위 요소를 찾지 못하는 버그 수정. 이제 모든 자식으로 무조건 Hittest확인, 그중 가장 작은 면적의 결과를 선택합니다.

### Added
- **Control View** 토글(UIA 트리) — RawView(기본값)는 Legacy IAccessible 브릿지 중복 PEER를 포함한 모든 UIA PEER를 보여주고, ControlView는 `IsControlElement`만 필터링해 Sync/Tree 탐색 시 중복·노이즈 노드를 줄입니다.
목록 아이템 최적화
- Property 패널에 **Selector2** 추가 — 사용 가능한 식별 속성(AutomationId/Name/ClassName/ControlType)을 전부 AND 조합. 속성 하나(예: 재사용되는 AutomationId)만으로는 애매한 경우를 위한 것입니다.
- **KeyPath** — 탭 루트부터 선택 요소까지의 형제 인덱스 경로(예: `0/2/1`), 최후의 폴백 식별자로 사용합니다.
- **TypePath** — 탭 루트부터 선택 요소까지의 ControlType 경로(예: `.\pane\pane\...\combo box`).

### Performance
- KeyPath/TypePath는 별도 UIA 호출 없이 이미 로드된 트리 구조(`TreeNode.Parent`/`.Index`)에서 계산

## [1.0.0.1] - 2026-08-15 – 2026-08-18

### Added
- GitHub 최초 공개.
- 창 상태 제어(Win32는 Show/Hide/Enable/Disable/Move/Resize/Always On Top, UIA 요소는 Minimize/Maximize/Restore/Close/Move/Resize/Rotate).
- System Diagnostics 대화상자(시스템 전체 GDI/USER 오브젝트 랭킹, 데스크톱 전체 Z-order 스택).
- A/B 속성 스냅샷 비교(Snapshot Compare), diff export 포함.
- Message Log ↔ Rect/DPI 상관관계 뷰.
- Win32/UIA 계층 트리 양쪽에 Sync(호버 추적) 모드 추가.
- 애플리케이션 아이콘 적용.

### Changed
- 문서 업데이트, 소스 정리.
