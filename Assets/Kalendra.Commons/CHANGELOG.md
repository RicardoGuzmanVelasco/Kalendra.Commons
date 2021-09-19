# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
- Package layout complete creation.
- Architecture assembly to be full or half deprecated in the future.

## [0.1.0] - 2021-09-19

Changes make API incompatible. May require assembly references changes. 

### Added
- RandomService provides different collection interfaces to dispatch to.

### Changed
- Services interfaces are no longer in Architecture assembly.
  - They belongs to Domain from now on.
  - Some implementations, as possible, belonging to domain as well.

## [0.0.3] - 2021-09-18
### Added
- Package layout basic creation.

## [0.0.2] - 2021-5-2
### Removed
- FluentAssertions as dependency.
  - Removed because of dependency error when importing this package through git.

## [0.0.1] - 2021-5-1
### Added
- Architecture conventions.
- Package layout with Tests.
- Builders layout.
