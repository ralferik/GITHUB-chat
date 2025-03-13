# iti0301-2025

# 🏦 Princess Sofia's Casino - Client-Server Communication

This project implements a **TCP-based client-server communication** system where multiple clients can connect to a server, exchange messages, and receive responses. The server handles multiple clients concurrently, logs their IP addresses and ports, and supports message broadcasting.

---

## 🚀 Features (currently operating)

- **Multi-client support**: Multiple clients can connect to the server at the same time.
- **Server-side logging**: Logs each connected client's IP and port.
- **Client-server messaging**: Clients receive a welcome message and can send messages to the server.
- **Message broadcasting**: The server echoes messages back to other clients.
- **Graceful disconnection handling**: When a client disconnects, the server removes it from the active list.
- **Multi-player functionality**: Our game can support at least 2 players.

---

## 📥 Prerequisites

Ensure you have the following installed before running the application:

- **.NET SDK 6.0 or later** (for running the server)  
  Download: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
- **Unity (2020+ recommended)** (for running the client)  
  Download: [https://unity.com/download](https://unity.com/download)

---
## Technologies

- Unity 6 framework
- C#
- Blender

---

## How to play

- You can use the arrows on your keyboard to move forward and backward, left and right in the room.
- There are currently no 3D character animations, meaning body parts cannot be moved yet, and the character cannot turn around.

---

## 🖥 Running the Server

### **1️⃣ Open the terminal (Command Prompt, PowerShell, or Terminal)**

If you're using **Windows**, open PowerShell.  
If you're using **Linux/macOS**, open a terminal window.

### **2️⃣ Navigate to the Server Directory**

Move into the server folder:

```sh
cd iti0301-2025/Server

### **3️⃣ Build the Server
Before running, compile the server code:
dotnet build

### **4️⃣ Run the Server
After building, start the server:
dotnet run

### **5️⃣ Expected Server Output
nginx
Copy
Edit
Server is running on localhost:8080
Client connected. IP: 127.0.0.1, Port: 56789
Received from client (127.0.0.1:56789): Hello Server!
Client disconnected. IP: 127.0.0.1, Port: 56789



```
