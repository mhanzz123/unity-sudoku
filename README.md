# Unity Sudoku Game 🎮

A complete and feature-rich Sudoku game made in Unity using C#. Designed with gameplay, data persistence, and user-friendly features in mind. Implements algorithms like backtracking, and includes user login, stats tracking, and UI interactions.

---

## ✅ Features

- 🎲 **Dynamic Sudoku Board Generator**
  - Uses backtracking and random insertion to generate solvable boards
  - 3 difficulty levels: Easy, Medium, Hard

- 🧠 **Full Game Logic**
  - Cell validation, color-coded feedback for correct/wrong answers
  - Timer and score tracking

- 🔐 **User System**
  - Register & login with password validation
  - Password recovery using a secure backup code

- 📊 **Game History Tracker**
  - Saves up to 24 previous games with time, difficulty, mistakes

- 🔢 **Advanced Player Experience**
  - Candidate numbers (small notes)
  - Interactive number panel
  - In-game feedback with green/red highlights

---

## 🛠️ Tech Stack

- Unity Engine (C#)
- Unity UI System
- PlayerPrefs for local data
- JSON serialization
- Basic data structures and algorithms (backtracking, constraint solving)

---

## 📂 Code Structure Highlights

- `Game.cs` — core gameplay, board generation, win/loss logic
- `SudokuGenerator.cs` — board generator using backtracking
- `AuthManager.cs` — user account management and recovery
- `GameHistoryManager.cs` — stat tracking UI
- `SudokuObject.cs` — Sudoku logic layer
- `FieldPrefabObject.cs` / `ControlPrefabObject.cs` — visual representation and interaction

---

## 🧑 Author

Built by **Matouš Gaja (Matthew)**  
This is one of my first serious Unity projects — combining design, logic, user systems, and persistence.

---

## 🧪 Screenshots
(Include images of the login screen, game board, win screen, and history panel here.)

---

## 📝 License

This is a personal project for learning and portfolio purposes. Contact me if you'd like to use it, contribute, or collaborate.

