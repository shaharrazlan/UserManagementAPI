```md
# 🚀 User Management API 🔐

A secure and scalable User Authentication API built with **ASP.NET Core, MongoDB, and JWT Authentication.**

---

## 🌟 **Features**
✅ User Registration & Login  
✅ Secure JWT Authentication  
✅ Bcrypt Password Hashing  
✅ MongoDB Database Integration  
✅ Role-Based Access Control (Admin/User)  
✅ CORS Support for Frontend Apps  

---

## 🛠 **Tech Stack**
- **Backend:** ASP.NET Core, C#
- **Database:** MongoDB
- **Authentication:** JWT (JSON Web Token)
- **Security:** Bcrypt Password Hashing
- **Frontend Support:** React, TypeScript
---

## 🚀 **Getting Started**
### 🔹 **Clone the Repository**
```sh
git clone https://github.com/shaharrazlan/UserManagementAPI.git
cd UserManagementAPI
```

### 🔹 **Configure Environment Variables**
Create a `.env` file in the root directory:
```env
MONGO_CONNECTION_STRING=your-mongodb-connection-string
MONGO_DATABASE_NAME=your-database-name
JWT_SECRET=your-secret-key
CLIENT_APP_URL=http://localhost:3000
```

### 🔹 **Run the Project**
```sh
dotnet run
```

---

## 📌 **API Endpoints**
### 🔐 **Authentication**
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Login and receive a JWT token |

### 🛠 **User Management**
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET`  | `/api/user/profile` | Get authenticated user profile |
| `POST` | `/api/user/update` | Update user details |

💡 **To access protected routes**, include the JWT token in the `Authorization` header:
```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

---

## 🎨 **Project Structure**
```sh
📦 UserManagementAPI
 ┣ 📂 Controllers         # API controllers
 ┣ 📂 Services            # Business logic
 ┣ 📂 Repositories        # Database operations
 ┣ 📂 Models              # MongoDB data models
 ┣ 📂 DTOs                # Data transfer objects
 ┣ 📜 Program.cs          # App configuration
 ┣ 📜 README.md           # Project documentation
```

---

## 🛡 **Security Best Practices**
🔹 **Use strong JWT secrets** (`JWT_SECRET`)  
🔹 **Store JWTs in HTTP-only cookies (instead of localStorage)**  
🔹 **Implement refresh tokens for better security**  
🔹 **Enable HTTPS in production**  

---
