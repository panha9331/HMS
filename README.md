# HMS
# 🏨 Hotel Management System

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

A comprehensive, modern Hotel Management System designed to streamline front-desk operations, automate room bookings, manage guest records, track billing, and provide real-time reporting for hotel staff and administrators.

---

## 📑 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Architecture & Modules](#-architecture--modules)
- [Getting Started](#-getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Configuration & Environment Variables](#-configuration--environment-variables)
- [API Endpoints](#-api-endpoints)
- [Screenshots](#-screenshots)
- [Contributing](#-contributing)
- [License](#-license)
- [Contact](#-contact)

---

## ✨ Features

### 🛏️️ Room & Inventory Management
* **Room Categories:** Create, edit, and categorize rooms (Single, Double, Suite, Deluxe, etc.).
* **Status Tracking:** Real-time visibility of room availability (Available, Occupied, Reserved, Maintenance, Cleaning).
* **Amenity Mapping:** Assign amenities and pricing tiers to specific room types.

### 📅 Booking & Reservation System
* **Seamless Reservations:** Create walk-in or online bookings with date range pickers.
* **Check-In / Check-Out:** Fast automated check-in and check-out workflows.
* **Overbooking Protection:** Real-time conflict detection during reservation scheduling.

### 👤 Guest Management
* **Guest Profiles:** Maintain detailed history, contact info, and special requests/preferences.
* **Identification Tracking:** Securely attach ID proofs (Passport, Driver's License) to guest profiles.

### 💳 Billing & Invoicing
* **Automated Folio Generation:** Sum room charges, add-on services (room service, laundry, spa), and taxes automatically.
* **Multiple Payment Gateways:** Support for Credit/Debit Cards, Cash, Bank Transfer, and online gateways (Stripe/PayPal).
* **PDF Invoicing:** Export clean, branded PDF invoices for guests upon checkout.

### 📊 Dashboard & Analytics
* **Occupancy Rates:** Visual breakdown of current occupancy and monthly statistics.
* **Revenue Metrics:** Track daily, weekly, and monthly revenue performance.
* **Quick Stats:** Overview of pending check-ins, check-outs, and available rooms for the day.

### 🔐 User Roles & Security
* **Role-Based Access Control (RBAC):** Admin, Front Desk Staff, Housekeeping, and Accountant access levels.
* **Authentication:** Secure login with JWT / Session-based authentication and password hashing.

---

## 🛠️ Tech Stack

*(Update this section according to your project's stack)*

* **Frontend:** React.js / Next.js / HTML5 + Tailwind CSS
* **Backend:** Node.js (Express) / Python (Django/FastAPI) / Java (Spring Boot)
* **Database:** PostgreSQL / MongoDB / MySQL
* **Authentication:** JWT (JSON Web Tokens) / OAuth 2.0
* **Deployment:** Docker, AWS / Vercel / Heroku

---

## 🏗️ Architecture & Modules

```text
       ┌────────────────────────┐
       │   Frontend Web App     │
       └───────────┬────────────┘
                   │ REST API / GraphQL
       ┌───────────▼────────────┐
       │     Backend Server     │
       └─────┬────────────┬─────┘
             │            │
  ┌──────────▼───┐    ┌───▼────────────┐
  │  PostgreSQL  │    │ Payment Gateway│
  │   Database   │    │ (Stripe/PayPal)│
  └──────────────┘    └────────────────┘
```

---

## 🚀 Getting Started

Follow these steps to set up and run the project locally.

### Prerequisites

Ensure you have the following installed on your machine:

* [Node.js](https://nodejs.org/) (v16.x or later) / [Python](https://www.python.org/) (v3.9+)
* [Git](https://git-scm.com/)
* [PostgreSQL](https://www.postgresql.org/) or [MongoDB](https://www.mongodb.com/)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/hotel-management-system.git
   cd hotel-management-system
   ```

2. **Install Backend Dependencies:**
   ```bash
   cd backend
   npm install   # or pip install -r requirements.txt
   ```

3. **Install Frontend Dependencies:**
   ```bash
   cd ../frontend
   npm install
   ```

4. **Set up Environment Variables:**
   Create a `.env` file in the root of the backend directory (refer to `.env.example`).

5. **Run Database Migrations:**
   ```bash
   npm run db:migrate   # or python manage.py migrate
   ```

6. **Start the Development Servers:**
   * **Backend:** `npm run dev` (Runs on `http://localhost:5000`)
   * **Frontend:** `npm start` (Runs on `http://localhost:3000`)

---

## ⚙️ Configuration & Environment Variables

Sample `.env` file for the backend server:

```env
PORT=5000
NODE_ENV=development
DATABASE_URL=postgres://user:password@localhost:5432/hotel_db
JWT_SECRET=your_jwt_secret_key_here
STRIPE_SECRET_KEY=sk_test_123456789
```

---

## 📋 API Endpoints

| Method | Endpoint | Description | Access |
| :--- | :--- | :--- | :--- |
| **POST** | `/api/auth/login` | Authenticate user & get token | Public |
| **GET** | `/api/rooms` | Fetch list of all rooms | Staff, Admin |
| **POST** | `/api/rooms` | Create a new room entry | Admin |
| **POST** | `/api/bookings` | Create a new reservation | Staff, Admin |
| **GET** | `/api/bookings/:id` | Fetch specific booking details | Staff, Admin |
| **POST** | `/api/billing/checkout` | Process room check-out & bill | Staff, Admin |

---

## 🖼️️ Screenshots

*(Add screenshots of your project here to make the README visually engaging!)*

| Dashboard Overview | Room Management |
| :---: | :---: |
| ![Dashboard](https://via.placeholder.com/400x220?text=Dashboard+UI) | ![Rooms](https://via.placeholder.com/400x220?text=Room+Management+UI) |

---

## 🤝 Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📜 License

Distributed under the MIT License. See `LICENSE` for more information.

---

## 📧 Contact

**Moeurn Panha / ISA**
* Email: moeurn.panha.dev@gmail.com
* GitHub: [@panha9331](https://github.com/panha9331)
