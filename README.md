# GRID SCADA System – NST Course Project

This is a GRID SCADA (Supervisory Control and Data Acquisition) system developed for the Napredne softverske tehnologije course. The project features a layered architecture with both simulation capabilities.

The system is built using:
- .NET for backend services
- React for the frontend
- SQLite for persistence

## System Overview

The SCADA system consists of the following key components:

- **SCADA Core** – Core processing engine for simulation, tag scanning, and real-time I/O communication
- **Database Manager** – Frontend interface for managing tags and user accounts
- **Trending Application** – Displays real-time tag values
- **Alarms** – SignalR component that displays real-time alarm notifications for critical values
- **Report Manager** – Provides report generation based on tag and alarm data

## Getting Started

### Prerequisites

- .NET 8 SDK or newer
- Node.js with npm
- SQLite

### Backend Setup

```bash
cd Backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend Setup

```bash
cd frontend
npm install
npm start
```

## Configuration Files
- `scadaConfig.xml`: Stores tag and scanning configuration
