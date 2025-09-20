# PiikkiTracker

> **Note**: This project is still in MVP stage.

A credit tracking system for student organizations built with ASP.NET Core 9.0 and Blazor Server. The system allows board members to log completed jobs and earn credits that can be used to purchase products, featuring a leaderboard to encourage participation.

## Introduction

PiikkiTracker is designed to motivate student organization members by tracking their contributions and rewarding them with credits. Members can log completed jobs, earn credits, and use those credits to purchase products or services. The system includes a competitive leaderboard that ranks the most active members, encouraging continuous participation and helping distribute the workload among organizers.

## Features

- **Job Management**: Create and manage tasks that earn credits when completed
- **Product Catalog**: Set up products that can be purchased with earned credits
- **Credit System**: Automatic calculation and transfer of credits based on job completion
- **Leaderboard**: Competitive ranking system for the most active members
- **User Administration**: Comprehensive management of users, jobs, and products
- **Transaction History**: Complete audit trail of all credit movements
- **Data Visualization**: Charts and analytics for tracking system usage and engagement

## Technology Stack

- **Framework**: ASP.NET Core 9.0 with Blazor Server Components
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **UI**: Interactive Server Components with Bootstrap

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd PiikkiTracker
   ```

2. Configure the database connection in `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TabTracker;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Build and run the application:
   ```bash
   dotnet build PiikkiTracker.sln
   dotnet run
   ```

### Development

For development with hot reload:
```bash
dotnet watch run --project PiikkiTracker.sln
```

## Development Commands

### Build and Run
```bash
# Build the solution
dotnet build PiikkiTracker.sln

# Run the application
dotnet run

# Run with hot reload (recommended for development)
dotnet watch run --project PiikkiTracker.sln
```

### Database Operations
```bash
# Add a new migration
dotnet ef migrations add MigrationName

# Update database with latest migrations
dotnet ef database update

# Drop database (development only)
dotnet ef database drop
```

### Publishing
```bash
# Publish for deployment
dotnet publish PiikkiTracker.sln
```

## Architecture

### Core Models

- **ApplicationUser**: Extended Identity user with balance tracking and navigation properties
- **Job**: Tasks that can be completed for credits (Payment amount)
- **Product**: Items purchasable with credits (Price)
- **UserJob**: Junction entity tracking user completion of jobs
- **UserProduct**: Junction entity tracking user purchases
- **Transaction**: Records all credit movements (both earning and spending)

### Project Structure

```
PiikkiTracker/
├── Components/
│   ├── Account/          # Identity-related components
│   ├── Layout/           # Layout components (NavBar, MainLayout, etc.)
│   ├── Pages/            # Page components organized by feature
│   │   ├── Charts/       # Data visualization components
│   │   ├── Job_Pages/    # Job management pages
│   │   ├── Product_Pages/ # Product management pages
│   │   ├── Transaction_Pages/ # Transaction management
│   │   ├── User_Job_Pages/ # User job tracking
│   │   └── User_Product_Pages/ # User purchase tracking
│   └── Shared/           # Reusable components
├── Data/                 # Entity models and DbContext
├── Repository/           # Data access layer with Repository pattern
└── wwwroot/             # Static assets
```

### Repository Pattern

The application uses the Repository pattern with dependency injection:
- Interface definitions in `Repository/IRepository/`
- Implementations in `Repository/`
- All repositories are registered as scoped services

## Vision

The goal is to continuously encourage board members and officers to volunteer their time for jobs that benefit the organization and help distribute the workload among event organizers. By gamifying contributions through credits and leaderboards, we aim to create a more engaged and collaborative environment.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes following existing code conventions
4. Add/update tests if necessary
5. Submit a pull request
