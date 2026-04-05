# 🎮 Turn-Based Game Backend

A modular and scalable backend system built with ASP.NET Core for turn-based multiplayer games.

This project is designed to handle core gameplay mechanics like **game sessions, player actions, and state management**, while keeping the architecture clean and extensible.

---

## ✨ Features

- 🎲 Turn-based game logic handling
- 🧩 Modular architecture for easy extension
- 🔐 JWT-based authentication
- ⚡ Real-time updates (WebSockets / SignalR ready)
- 💾 Database integration with Entity Framework Core
- 📄 Swagger API documentation
- 🧱 Clean separation of concerns (Controllers, Services, Models)

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core Web API
- **Database:** PostgreSQL / MongoDB
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Tokens
- **Real-time:** SignalR / WebSockets
- **API Docs:** Swagger

---

## 📂 Project Structure

```
/Controllers     → API endpoints  
/Services        → Business logic  
/Models          → Domain models  
/DTOs            → Request/response objects  
/Data            → Database context  
/Middleware      → Custom middleware  
```

---

## ⚙️ Getting Started

### Prerequisites

- .NET SDK 10
- PostgreSQL

---

### Installation

```bash
git clone https://github.com/tarangkuhikar/PlaySync.git
cd <repo-name>
dotnet restore
```

---

### Run the Project

```bash
dotnet run
```

---

### Swagger

```
http://localhost:5208/swagger/index.html
```

---

## 🎮 Core Concepts

### Game Flow

1. Create a game session
2. Players join the game
3. Turns are processed sequentially
4. Game state updates after each move

---

### Turn Handling

- Ensures only the **active player** can make a move
- Validates game rules before updating state
- Maintains consistency across all players

---

## 🧠 Design Goals

- Write **clean, maintainable backend code**
- Keep logic **modular and reusable**
- Make it easy to plug in **different game types**
- Ensure **data consistency** in multiplayer scenarios

---

## 🚧 Current Status

- Core game flow implemented
- Authentication system in progress
- Real-time updates being integrated

---

## 📈 Future Improvements

- Redis caching for performance
- Matchmaking system
- Spectator mode
- Docker support
- CI/CD pipeline

---

## 🐛 Challenges & Learnings

- Managing consistent game state across multiple players
- Designing flexible turn-based logic
- Handling edge cases in player actions
- Structuring a modular backend architecture

---

## 🤝 Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

---

## 📬 Contact

- GitHub: https://github.com/tarangkuhikar

---

⭐ If you find this useful, consider starring the repo!
