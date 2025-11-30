# CLAUDE.md - AI Assistant Guide for The Invisible Hand

## Project Overview

**The Invisible Hand** is an economics role-playing game designed to teach basic economics concepts like supply and demand in an engaging, interactive way. The project is based on a role-playing game framework for non-formal education in market economics.

### Project State
- **Status**: Early development stage
- **Platform**: iOS/macOS (Swift/Xcode)
- **Initial Commit**: 9754e54
- **Current Branch**: `claude/claude-md-mimckix6yob8cdug-01KRzGKrWXwUAsw8TZaJ4LqL`

## Repository Structure

```
the-invisible-hand/
├── README.md           # Project description
├── .gitignore         # Xcode/Swift gitignore configuration
└── CLAUDE.md          # This file (AI assistant guide)
```

**Note**: Source code directories are not yet created. When implementing features, follow the conventions below.

## Recommended Project Structure

When creating the initial project structure, follow iOS/Swift best practices:

```
the-invisible-hand/
├── TheInvisibleHand/           # Main application target
│   ├── App/                    # App lifecycle and configuration
│   ├── Models/                 # Data models and game logic
│   │   ├── Economy/           # Economic simulation models
│   │   ├── Player/            # Player and character models
│   │   └── Market/            # Market and trading models
│   ├── Views/                 # SwiftUI views
│   │   ├── Game/              # Main game interface
│   │   ├── Market/            # Market/trading interface
│   │   └── Education/         # Educational content screens
│   ├── ViewModels/            # View models (MVVM pattern)
│   ├── Services/              # Business logic and services
│   │   ├── GameEngine/       # Core game mechanics
│   │   └── EconomySimulator/ # Economic simulation engine
│   ├── Resources/             # Assets, localizations
│   └── Utilities/             # Helper functions and extensions
├── TheInvisibleHandTests/     # Unit tests
├── TheInvisibleHandUITests/   # UI tests
└── TheInvisibleHand.xcodeproj # Xcode project file
```

## Development Conventions

### Swift Code Style

1. **Naming Conventions**
   - Use `PascalCase` for types (classes, structs, enums, protocols)
   - Use `camelCase` for functions, variables, and constants
   - Use descriptive names that clearly indicate purpose
   - Prefix protocol names with describing their capability (e.g., `Tradeable`, `Marketable`)

2. **Architecture Pattern**
   - Use **MVVM** (Model-View-ViewModel) for UI layer
   - Separate business logic into dedicated Service classes
   - Keep Views lightweight and declarative (SwiftUI)
   - Use Combine framework for reactive programming where appropriate

3. **Code Organization**
   - One type per file (exceptions for small, closely-related types)
   - Group related functionality using `// MARK: - Section Name`
   - Keep files under 300 lines when possible
   - Use extensions to organize protocol conformances

4. **Error Handling**
   - Use Swift's native error handling (`throws`, `try`, `catch`)
   - Define custom error enums for domain-specific errors
   - Provide meaningful error messages for debugging and user feedback

5. **Documentation**
   - Use `///` for public APIs and complex logic
   - Include parameter descriptions and return values
   - Document any assumptions or preconditions
   - Add `// MARK:` comments to organize code sections

### Game Design Principles

When implementing game features:

1. **Educational Focus**
   - Every game mechanic should teach or reinforce economic concepts
   - Provide clear feedback on economic decisions
   - Include explanations of supply/demand dynamics
   - Make economic cause-and-effect relationships visible

2. **Core Economic Concepts to Model**
   - Supply and demand equilibrium
   - Price elasticity
   - Market competition
   - Resource scarcity
   - Opportunity cost
   - Profit maximization
   - Market efficiency

3. **Game Mechanics**
   - Role-playing elements (player as merchant, producer, or consumer)
   - Trading system with dynamic pricing
   - Resource management
   - Market simulation with NPCs
   - Decision-making with economic consequences
   - Progress tracking and learning objectives

### Testing Requirements

1. **Unit Tests**
   - Test all business logic and game mechanics
   - Test economic simulation calculations
   - Aim for >80% code coverage on critical paths
   - Use meaningful test names that describe the scenario

2. **UI Tests**
   - Test critical user flows (trading, market navigation)
   - Verify educational content is accessible
   - Test game state persistence

3. **Test Organization**
   - Mirror the main app structure in test targets
   - Use `Given-When-Then` pattern for test organization
   - Group related tests using `// MARK:`

## Development Workflow

### Branching Strategy

- **Main Branch**: Not yet established (currently on feature branch)
- **Feature Branches**: Use format `claude/[description]-[session-id]`
- All development should be done on designated feature branches
- Push to remote using: `git push -u origin <branch-name>`

### Commit Guidelines

1. **Commit Messages**
   - Use clear, descriptive commit messages
   - Start with a verb (Add, Update, Fix, Refactor, etc.)
   - Keep subject line under 72 characters
   - Include detailed description for complex changes

2. **Commit Frequency**
   - Commit logical units of work
   - Don't commit broken code
   - Commit after completing each feature/fix

### Git Operations

- **Push**: Always use `git push -u origin <branch-name>`
- **Retry Logic**: If network failures occur, retry up to 4 times with exponential backoff (2s, 4s, 8s, 16s)
- **Branch Naming**: Must start with `claude/` and end with matching session ID

## Key Technical Decisions

### Technology Stack

1. **Language**: Swift 5.x+
2. **UI Framework**: SwiftUI (recommended for modern iOS development)
3. **Minimum iOS Version**: To be determined (recommend iOS 15+)
4. **Dependencies**: To be determined based on features
   - Consider Combine for reactive programming
   - Consider GameplayKit for game mechanics
   - Avoid unnecessary third-party dependencies

### Architecture Decisions

1. **State Management**
   - Use `@State`, `@Binding`, `@ObservedObject` for SwiftUI
   - Consider single source of truth pattern
   - Implement proper data flow (unidirectional when possible)

2. **Persistence**
   - To be determined: UserDefaults, CoreData, or SwiftData
   - Save game progress automatically
   - Support multiple save slots if appropriate

3. **Economic Simulation**
   - Implement realistic but simplified economic models
   - Use configurable parameters for game balance
   - Ensure deterministic behavior for testing

## AI Assistant Guidelines

### When Adding Features

1. **Read First**: Always read existing code before making changes
2. **Consistency**: Match existing code style and patterns
3. **Documentation**: Document all public APIs and complex logic
4. **Testing**: Write tests for new functionality
5. **Simplicity**: Avoid over-engineering; implement what's needed
6. **Educational Value**: Ensure features support learning objectives

### Code Review Checklist

Before committing code, verify:
- [ ] Code follows Swift style conventions
- [ ] No compiler warnings
- [ ] Tests are passing
- [ ] Documentation is up to date
- [ ] No hardcoded values that should be configurable
- [ ] Economic mechanics are educationally sound
- [ ] UI is accessible and user-friendly
- [ ] No security vulnerabilities introduced

### Common Patterns to Use

1. **Dependency Injection**: Inject dependencies via initializers
2. **Protocol-Oriented Programming**: Use protocols for abstraction
3. **Value Types**: Prefer structs over classes when possible
4. **Immutability**: Use `let` over `var` when possible
5. **Guard Statements**: Use early returns for validation
6. **Optional Handling**: Use optional chaining and nil coalescing

### Common Patterns to Avoid

1. **Massive View Controllers**: Keep view logic minimal
2. **Singletons**: Avoid except for true global state
3. **Force Unwrapping**: Minimize use of `!` operator
4. **Stringly-Typed Code**: Use enums instead of string constants
5. **Premature Optimization**: Focus on clarity first

## Economic Game Design Notes

### Learning Objectives

Players should understand:
1. How supply and demand determine prices
2. The role of competition in markets
3. How scarcity affects value
4. Trade-offs and opportunity costs
5. Price signals and market information

### Gameplay Loop (Recommended)

1. **Setup**: Player chooses role and starting resources
2. **Trading**: Buy and sell goods in dynamic market
3. **Decisions**: Make economic choices with clear trade-offs
4. **Feedback**: See immediate market reactions
5. **Learning**: Understand why outcomes occurred
6. **Progression**: Unlock new scenarios and complexity

### Difficulty Progression

- Start with simple supply/demand scenarios
- Gradually introduce market dynamics
- Add competition and external events
- Include complex multi-market scenarios
- Challenge players with realistic economic dilemmas

## Resources and References

### Swift and iOS Development
- [Swift.org Documentation](https://docs.swift.org)
- [Apple Human Interface Guidelines](https://developer.apple.com/design/human-interface-guidelines/)
- [SwiftUI Tutorials](https://developer.apple.com/tutorials/swiftui)

### Economics Education
- Research non-formal education methodologies
- Study existing economics teaching games
- Review supply/demand simulation models

### Game Design
- Focus on fun and engagement
- Balance realism with playability
- Iterate based on playtesting feedback

## Getting Started for AI Assistants

When working on this project:

1. **First Time Setup**
   - Verify Xcode is installed and configured
   - Create initial Xcode project structure if not exists
   - Set up git hooks if beneficial

2. **Before Each Session**
   - Check current branch and git status
   - Review recent commits and changes
   - Understand current development goals

3. **During Development**
   - Use TodoWrite to track multi-step tasks
   - Commit logical units of work
   - Write tests alongside features
   - Document as you code

4. **Before Pushing**
   - Run all tests
   - Verify code compiles without warnings
   - Review changes for quality
   - Ensure commit messages are clear

## Project Vision

The Invisible Hand aims to make economics education accessible and engaging through interactive gameplay. The game should:
- Be fun and replayable
- Teach core economic concepts intuitively
- Provide immediate feedback on decisions
- Scale from beginner to advanced topics
- Support various learning styles
- Be suitable for non-formal education settings

## Contact and Contribution

- Repository owner: grahamom
- Current development branch: `claude/claude-md-mimckix6yob8cdug-01KRzGKrWXwUAsw8TZaJ4LqL`
- For questions or issues, create GitHub issues

---

**Last Updated**: 2025-11-30
**Document Version**: 1.0
**Project Status**: Initial Development
