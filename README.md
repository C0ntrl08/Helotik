# Helotik

Helotik is a .NET 10 solution containing a .NET MAUI cross-platform application and a service/repository backend. It demonstrates a layered architecture for managing domain entities (for example, job applications) with separation between UI, business logic, and data access.

## Key points
- Target framework: .NET 10
- UI: .NET MAUI (cross-platform mobile/desktop)
- Architecture: Presentation (MAUI) → Services → Repositories → Persistence
- Example service: Juribi/Services/JobApplicationRepository.cs — repository pattern for application data

## Who this is for
- Hiring managers: demonstrates experience with modern .NET, MAUI, and layered architecture.
- Developers: shows repository-driven data access, separation of concerns, and readiness for unit testing and extension.

## Run (developer)
1. Prerequisites:
   - Visual Studio 2026 with the .NET MAUI workload
   - .NET 10 SDK
2. Open the solution `Helotik.slnx` in Visual Studio 2026.
3. Select a MAUI startup target (Windows, Android emulator, iOS simulator) and press Run (F5).
4. CLI build: `dotnet build Helotik.slnx`

## Development notes
- Keep business rules in the Services layer; the UI should remain thin and focus on presentation.
- Use repository abstractions for persistence to keep services testable (use mocks or in-memory stores for unit tests).

## License
MIT License
