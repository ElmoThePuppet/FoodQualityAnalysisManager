# Food Quality Analysis Manager

## Overview
The **Food Quality Analysis Manager** is a distributed application designed to process and analyze food batch data. It consists of two main services:

1. **QualityManager** – Responsible for receiving food batch submissions and publishing analysis requests.
2. **AnalysisEngine** – Listens for analysis requests, processes them, and returns analysis results.

The system utilizes **RabbitMQ** as the message broker for asynchronous communication and **SQL Server** as the database for storing food batch and analysis results.

---

## **Technology Stack**
- **.NET 8** (Minimal API)
- **MassTransit** (for messaging abstraction)
- **RabbitMQ** (message broker)
- **SQL Server** (database)
- **Docker** (for containerization)
- **Entity Framework Core** (ORM)
- **XUnit & FluentAssertions** (testing)

---

## **Project Structure**
```plaintext
FoodQualityAnalysisSolution/
│── QualityManager/          # Handles food batch submissions and publishes analysis requests
│── AnalysisEngine/          # Consumes requests, processes them, and publishes results
│── Contracts/               # Shared message contracts between services
│── docker-compose.yml       # Docker configuration for services and RabbitMQ
│── README.md                # Project documentation
```

---

## **How to Run the Project**
### **Prerequisites**
Ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [RabbitMQ](https://www.rabbitmq.com/)

### **Running with Docker Compose**
To spin up all services, run:
```sh
docker-compose up --build
```
This will start **QualityManager**, **AnalysisEngine**, **SQL Server**, and **RabbitMQ** in containers.

### **Running Locally**
1. **Start RabbitMQ** (if not using Docker)
   ```sh
   rabbitmq-server
   ```
2. **Run SQL Server** (ensure it’s available locally or in a container)
3. **Start QualityManager**
   ```sh
   cd QualityManager
   dotnet run
   ```
4. **Start AnalysisEngine**
   ```sh
   cd AnalysisEngine
   dotnet run
   ```

---

## **Key Architectural Choices**
### **SQL Server vs PostgreSQL**
I chose **SQL Server** because:
- I am familiar with the technology.
- No major performance constraints that would necessitate PostgreSQL.
- Seamless integration with Entity Framework Core.

### **RabbitMQ vs Azure Service Bus**
I opted for **RabbitMQ** because:
- It provides full control over message flow.
- Open-source and lightweight compared to Azure Service Bus.
- Works well for local development without requiring cloud setup.
- Further implementation through MassTransit library encapsulates configuration.

However, in a production cloud-based environment, **Azure Service Bus** could be considered for managed infrastructure and built-in retry mechanisms.

### **Minimal API vs Traditional MVC**
I used **Minimal API** because:
- **Performance**: Faster and more lightweight than MVC.
- **Simplicity**: Easier to set up for a microservices-based architecture.
- **No unnecessary boilerplate**: Keeps the API concise and focused.

### **CQRS & DDD Considerations**
This project does not implement **CQRS (Command Query Responsibility Segregation)** or **Domain-Driven Design (DDD)** because:

- **CQRS**:
  - The current system's complexity does not require separate command and query models.
  - A traditional CRUD approach is sufficient for managing food batches and analysis results.
  - Implementing CQRS would introduce additional overhead without significant benefits at this stage.
  - **Future Consideration**: If the system scales, CQRS could improve performance by optimizing read and write operations separately.

- **DDD**:
  - The project follows a layered architecture but does not enforce strict DDD principles.
  - Given the relatively simple domain, aggregates, value objects, and bounded contexts would add unnecessary complexity.
  - **Future Consideration**: If the domain logic becomes more intricate (e.g., extensive validation rules, multiple microservices), transitioning to a richer DDD approach might be beneficial.


---

## **Potential improvements**
- **Authentication & Authorization** can be implemented for access control (e.g., JWT, OAuth2.0).
- **Error Handling & Logging** can be enhanced through more robust exception handling.
- **Message Resiliency** could be improved through dead letter queues or retries.

---

## **Contributing**
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-branch`)
3. Commit changes (`git commit -m "Description"`)
4. Push to GitHub (`git push origin feature-branch`)
5. Open a Pull Request

---

