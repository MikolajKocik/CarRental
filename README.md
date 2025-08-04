# CarRental (project done during studies)

CarRental-MVC is a comprehensive web application built using the **MVC (Model-View-Controller)** architecture to manage car rentals efficiently, clearly, and scalably. The application allows customers to browse available car models and book vehicles, while administrators have full control over the vehicle fleet and reservations.

---

## Project Description

CarRental-MVC leverages modern software architectural principles including **CQRS**, **DDD (Domain-Driven Design)**, and **Clean Architecture**, while adhering to **SOLID** principles. The solution is organized into clear layers:

- **Presentation Layer** – ASP.NET Core MVC (Views and Controllers)
- **Application Layer** – Business logic implementation using CQRS with MediatR
- **Domain Layer** – Domain entities and models (DDD)
- **Infrastructure Layer** – Data access with Entity Framework Core and other dependencies

---

## Key Features

### 1. Vehicle Fleet Management
- **Add New Vehicles**: Add details such as vehicle make, model, and an **image path** -> IFormFile
- **Edit Vehicle Details**: Update existing vehicle information.
- **Remove Vehicles**: Delete vehicles from the fleet.
- **Availability Management**: Automatically update the availability status of vehicles sharing the same make and model.
- **Fleet Overview**: Check the number of vehicles by make and model.

### 2. Car Reservations
- **Availability Checks**: Verify if a vehicle is available before booking.
- **Booking Process**: Reserve vehicles seamlessly.
- **Email Notifications**: Send booking confirmation emails via SMTP.
  - **Important**: The email provided by the user **must be a valid email address** to ensure that the confirmation message is successfully delivered.

### 3. Admin Panel
- **Reservation Overview**: Monitor all reservations in real time.
- **Fleet and Reservation Management**: Manage vehicles and customer bookings with ease.
- **Real-Time Data**: Access up-to-date counts of vehicles by make or model.

---

## Technologies and Tools

- **ASP.NET Core MVC** – Framework for the application's structure and user interface.
- **Entity Framework Core** – Manages database operations.
- **Bootstrap** – Provides styling and responsive design.
- **MediatR** – Implements **CQRS** by decoupling request/response handling.
- **FluentValidation.AspNetCore** – Validates user inputs and ensures business rules are followed.
- **AutoMapper** – Simplifies mapping between domain objects and DTOs.
- **SMTP Protocol** – Used to send booking confirmation emails.
- **dotnet user-secrets** – Securely stores sensitive configuration data (e.g., SMTP credentials).
- **MS SQL** - SQL database type.
---

## Libraries and Packages

### MediatR
- **Purpose**: Implements **CQRS** by decoupling the handling of requests and responses.
- **Installation**:
```bash
  dotnet add package MediatR
  dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```
### FluentValidation.AspNetCore
- **Purpose**: Provides robust validation for user inputs, ensuring business rules are enforced.
- **Installation**:
```bash
  dotnet add package FluentValidation.AspNetCore
```
### AutoMapper
- **Purpose**: Simplifies the mapping between domain objects and Data Transfer Objects (DTOs).
- **Installation**:
```bash
  dotnet add package AutoMapper
  dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
```

## SMTP Protocol Configuration
CarRental-MVC uses the SMTP protocol to send booking confirmation emails. Important configuration details:

- **SMTP Password**: Use the following password for the Google microapp that enables sending emails via SMTP:
```bash
  nixj qaux dxrn whei
```
- **Securely Storing SMTP Credentials**:
1. Open a terminal in your project directory.
2. Use dotnet user-secrets to store your SMTP credentials securely. For example:
```bash
  dotnet user-secrets set "EmailSettings:SenderPassword" "nixj qaux dxrn whei" --project "Path\To\Your\Project.csproj"
```
3.Verify the stored secret:
```bash
  dotnet user-secrets list --project "Path\To\Your\Project.csproj"
```
- **User Email Validity**: Ensure that the email provided by the user is valid. Without a valid email, the booking confirmation will not be delivered.

## Project Structure Overview
```bash
  CarRental-MVC/
├── Presentation/         # ASP.NET Core MVC (Views, Controllers)
├── Application/          # Business logic and CQRS implementations
├── Domain/               # Domain entities and business models
└── Infrastructure/       # Data access, SMTP configuration, etc.
```
## Diagram ERD

![image](https://github.com/user-attachments/assets/8334842b-89b1-429f-a0a1-24c7ba597152)

Information: admin account -> L: admin@example.com / P: Admin123! // for tests
