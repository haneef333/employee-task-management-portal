# Employee Task Management Portal

A web-based internal tool for managing and tracking employee tasks across teams. Built with ASP.NET Core 8 MVC as part of my portfolio for a Custom Software Engineer role.

## Features

- **Role-based access:** Manager and Employee roles with different permissions
- **Task management:** Create, assign, edit, and delete tasks (Manager only)
- **Task tracking:** Employees view and track their assigned tasks
- **Status & priority:** Tasks have status (To Do, In Progress, Done) and priority (Low, Medium, High)
- **Filtering:** Filter tasks by status and priority
- **Dashboard:** Task count summary by status for quick overview
- **Authentication:** Secure login and registration using ASP.NET Core Identity

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core 8 (MVC) |
| Language | C# |
| ORM | Entity Framework Core |
| Database | SQLite |
| Frontend | Razor Views + Bootstrap 5 |
| Auth | ASP.NET Core Identity |
| Testing | xUnit + Moq |
| Version Control | Git + GitHub |

## Architecture

- **MVC Pattern** — clear separation between Models, Views, and Controllers
- **Repository Pattern** — data access is decoupled from business logic via `ITaskRepository`
- **Dependency Injection** — repositories and services injected via constructor injection
- **Code-First Migrations** — database schema managed through EF Core migrations

## Getting Started

### Prerequisites
- .NET 8 SDK or higher
- Git

### Setup

1. Clone the repository