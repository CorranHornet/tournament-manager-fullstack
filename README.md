# Tournament Manager System

[![.NET Core Build and Test](https://github.com/CorranHornet/tournament-manager-fullstack/actions/workflows/dotnet.yml/badge.svg)](https://github.com/CorranHornet/tournament-manager-fullstack/actions/workflows/dotnet.yml)

## Project Overview
A fullstack tournament management system designed with a focus on modern architecture, testability, and scalability. This project demonstrates high-quality code standards by implementing Clean Architecture and effective design patterns.

## Tech Stack
### Backend
- **Framework**: .NET 9 (ASP.NET Core Web API)
- **Architecture**: Clean Architecture
- **Data Access**: Entity Framework Core
- **Design Patterns**: Generic Repository Pattern, Dependency Injection

### Frontend
- **Framework**: React (Vite)
- **Language**: JavaScript/TypeScript

## Key Features
*   **Generic Repository**: Implemented for centralized and reusable data access logic.
*   **Fullstack Integration**: Seamless communication between the React frontend and the backend API.
*   **Automated Pipeline**: Every push is automatically verified via GitHub Actions to ensure code integrity.

## Getting Started
To run this project locally, follow these steps:

### Prerequisites
- [.NET SDK 9.0+](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/)

### Installation
1. Clone the repository: 
   `git clone https://github.com/CorranHornet/tournament-manager-fullstack.git`
2. **Backend**:
```bash
   cd tournament-manager
   dotnet restore
   dotnet run
3.  **Frontend**:
   cd tournament-frontend
   npm install
   npm run dev