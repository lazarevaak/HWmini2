##HW2_MINI
Это учебное веб-приложение, разработанное для автоматизации бизнес-процессов Московского зоопарка. В рамках архитектурного подхода **Domain-Driven Design (DDD)** и принципов **Clean Architecture**, реализованы основные use case'ы по управлению животными, вольерами и расписанием кормлений.

---

## ✅ Реализованный функционал

| Use Case                                     | Класс / Модуль                                |
|---------------------------------------------|-----------------------------------------------|
| ➕ Добавление животного                      | `AnimalsController.AddAnimal`, `Animal`       |
| ❌ Удаление животного                        | `AnimalsController.DeleteAnimal`, `IAnimalRepository` |
| ➕ Добавление вольера                        | `EnclosuresController.AddEnclosure`, `Enclosure` |
| ❌ Удаление вольера                          | `EnclosuresController.DeleteEnclosure`        |
| 🔄 Перемещение животного                    | `AnimalTransferService`, `Animal.MoveTo`      |
| 📅 Просмотр расписания кормлений            | `FeedingController.GetAll`, `FeedingSchedule` |
| ➕ Добавление кормления                      | `FeedingController.ScheduleFeeding`, `FeedingSchedule` |
| ✅ Отметить кормление как выполненное        | `FeedingController.MarkFeedingCompleted`      |
| 📊 Получение статистики по зоопарку         | `ZooStatisticsService`, `ZooStatisticsDto`    |

Дополнительно:

- Вспомогательные DTO: `TransferAnimalRequest`, `FeedingRequest`, `ZooStatisticsDto`.
- In-Memory реализация репозиториев для тестирования (`ZooInfrastructure.Repositories`).
- Swagger-документация и интерактивное API.

---

## 🧠 Применение Domain-Driven Design (DDD)

| Концепция DDD                        | Реализация                                        |
|-------------------------------------|---------------------------------------------------|
| 🧩 **Entity**                        | `Animal`, `Enclosure`, `FeedingSchedule`          |
| 🔁 **Value Object**                  | `Food`, `Gender`                                  |
| ⚡ **Domain Events**                 | `AnimalMovedEvent`, `FeedingTimeEvent`, `EnclosureCleanedEvent` |
| 🧠 **Rich Domain Model (поведение)** | Методы в `Animal`, `Enclosure`, `FeedingSchedule` |
| ❌ **Исключения**                    | `Excep : Exception` — доменные ошибки             |

---

## 🏛️ Структура Clean Architecture

- **Domain Layer (ядро)**  
  `ZooDomain/Entities` — содержит бизнес-сущности и поведение  
  `ZooDomain/Enum`, `ZooDomain/Actions` — перечисления и события  

- **Application Layer**  
  `ZooApplication/Services` — сервисы: `AnimalTransferService`, `FeedingOrganizationService`, `ZooStatisticsService`  
  `ZooApplication/Interfaces` — интерфейсы репозиториев и сервисов  
  `ZooApplication/DTOs` — входные и выходные структуры  

- **Infrastructure Layer**  
  `ZooInfrastructure/Repositories` — InMemory-реализация репозиториев для тестирования  
  `ZooInfrastructure` не зависит от Web или UI  

- **Presentation Layer (Web API)**  
  `ZooPresentation/Controllers` — REST-контроллеры  
  `Program.cs` — настройка сервисов, маршрутов, Swagger  

---

## 🧪 Покрытие Unit-тестами

Проект покрыт **чистыми юнит-тестами** с использованием xUnit:

- `AnimalTransferServiceTests`
- `FeedingOrganizationServiceTests`
- `ZooStatisticsServiceTests`
- `AnimalTests`, `EnclosureTests`, `FeedingScheduleTests`

Покрытие: **>80% бизнес-логики**, без использования Moq или сторонних фреймворков.

---

## 🚀 Быстрый старт

```bash
git clone https://github.com/yourusername/zoo-management-system.git
cd ZooPresentation
dotnet run --urls http://localhost:5000# HW mini 2
## Лазарева Александра Константиновна
