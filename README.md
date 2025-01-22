
# Electricity Company Outages Project

## Running the Project

### Prerequisites

1. **.NET 8 SDK or later**
2. **SQL Server 14** instance with the necessary stored procedures and database schema.
3. **Node.js** (for front-end dependencies if needed).
4. **A modern browser** for accessing the web portal.

### Steps to Run

#### 1. Database Setup:
   - Ensure the database is properly configured with the required staging tables, fact tables, and stored procedures.
   - Back up the schema of the database before starting.
   - Update the connection strings in the application settings files for each microservice.

#### 2. Run the API Microservice:
   - Navigate to the API project directory.
   - Execute the provided command or executable to start the application.
   - Access the API at **[http://localhost:5000](http://localhost:5000)**. Keep the API running to handle requests from the console application and web portal.

#### 3. Run the Console Application:
   - Navigate to the console application directory.
   - Execute the provided command or executable to generate test data and call the APIs.
   - This will:
     - Populate the staging tables with realistic test data.
     - Trigger the APIs for transferring outage details from staging tables to fact tables.

#### 4. Run the Web Portal:
   - Navigate to the web portal project directory.
   - Execute the provided command or executable to start the application.
   - Access the portal at **[http://localhost:4234](http://localhost:4234)**.
   - Log in using the following credentials:
     - **Username**: admin
     - **Password**: admin

#### 5. Using the Web Portal:
   - Access the portal at **[http://localhost:4234](http://localhost:4234)**.
   - Utilize the provided pages to search, manage, and add outages:
     - **Search for Outages Page**: Search for outages using various criteria and close one or all outages from the search results.
     - **Ignored Outages Page**: View and delete ignored outages.
     - **Add New Outage & Network Hierarchy Search Page**:
       - Add new outages.
       - Search the hierarchy of the electricity company’s network elements.
       - Retrieve all outages for a specific network element and delete them as needed.

#### 6. Stop the API and Console Applications (Optional):
   - Once the data is generated and the APIs have been called, you can stop running the API and console applications if no further data updates are required.

---

## Overview

The Electricity Company Outages project consists of three microservices, all designed and implemented using a three-tier architecture. This project provides robust solutions for managing, monitoring, and manipulating outage data within an electricity company’s infrastructure. The microservices are designed with scalability, maintainability, and performance in mind, leveraging modern technologies such as EF Core, dependency injection, concurrency, and parallelism.

### Microservices

#### 1. API Microservice

**Description**:

This microservice is an API application that exposes two endpoints:

- **API for transferring outage details from staging tables to fact tables**: This API calls a stored procedure to perform the transfer while ensuring data integrity.
- **API for another specific stored procedure**: Provides additional functionality related to outage data.

**Features**:

- Implements rate limiting to control the number of API requests.
- Utilizes compression to optimize data transfer.
- Employs EF Core for database interactions.
- Incorporates dependency injection for modularity and maintainability.

**Port**: Runs on **[http://localhost:5000](http://localhost:5000)**.

#### 2. Console Application Microservice

**Description**:

A console application designed for generating test data in the staging table and automating the process of calling the APIs provided by the API microservice.

**Features**:

- Generates realistic test data for the staging tables.
- Automatically triggers the APIs for data transfer operations.
- Implements concurrency and parallelism to efficiently handle large datasets.
- Utilizes EF Core for database operations.
- Integrates dependency injection to promote cleaner and more testable code.

#### 3. Web Portal Microservice

**Description**:

A responsive web portal built using ASP.NET MVC, Bootstrap, and JavaScript to provide a user-friendly interface for managing outages.

**Pages and Features**:

- **Login Page**: Provides secure authentication for users. Use `admin` as the username and `admin` as the password.
- **Search for Outages Page**: Allows users to search for outages using different criteria. Includes the ability to close one or all outages from the search results.
- **Ignored Outages Page**: Displays a list of ignored outages and allows users to delete them.
- **Add New Outage & Network Hierarchy Search Page**: Includes a form to add a new outage and a search feature to explore the hierarchy of the electricity company’s network elements.

**Technologies**:

- Bootstrap for responsive design.
- JavaScript for dynamic interactions.
- EF Core for data access.
- Dependency injection for clean architecture.

**Port**: Runs on **[http://localhost:4234](http://localhost:4234)**.

---

## Key Features Across Microservices

1. **EF Core**: Used for database operations in all microservices, ensuring robust and efficient data handling.
2. **Dependency Injection**: Ensures modularity and simplifies testing.
3. **Concurrency and Parallelism**: Improves performance and optimizes resource usage.
4. **Rate Limiting and Compression**: Enhances API performance and reliability.
5. **Bootstrap and JavaScript**: Delivers a modern and responsive user interface for the web portal.

---

## Architecture

The project is built on a three-tier architecture:

1. **Presentation Layer**: Web portal providing a user-friendly interface.
2. **Application Layer**: Handles business logic in APIs and the console application.
3. **Data Layer**: Manages data storage and retrieval using EF Core and stored procedures.

---


