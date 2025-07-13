# 🛍️ ShopTestEventSourcingCqrsKafka

## 📚 Описание проекта

Обучающий проект для обучения архитектуре микросервисов  с использованием следующих паттернов и технологий:

- **CQRS (Command Query Responsibility Segregation)** — разделение запросов и команд.
- **Event Sourcing** — хранение состояния через события.
- **Apache Kafka** — взаимодействие между сервисами через брокер событий.
- **MediatR** — реализация паттерна CQRS.
- **ASP.NET Core** — построение микросервисов.
- **Docker Compose** — для запуска Kafka + Zookeeper.

---

## 🧠 Теория

### 📌 CQRS

Паттерн CQRS разделяет операции чтения и записи:
- **Command** (Запись) — изменяет состояние, не возвращает данные (`BuyItemCommand`)
- **Query** (Чтение) — только возвращает данные, не изменяет состояние

Это упрощает масштабирование и разделение ответственности.

---

### 📌 Event Sourcing

Вместо хранения текущего состояния, система сохраняет **последовательность событий**, описывающих изменения.  
Например:
1. `TransactionStarted`
2. `StockReserved`
3. `PurchaseCompleted`

Таким образом, можно:
- восстановить любое состояние
- легко отлаживать
- вести аудит изменений

---

### 📌 Kafka

Kafka — брокер сообщений. Вместо вызова HTTP напрямую:
- Сервис **публикует событие** (publish)
- Другой сервис **слушает топик** (subscribe) и реагирует

Kafka даёт:
- Надёжную и асинхронную коммуникацию
- Устойчивость к сбоям (всё пишется в лог)
- Возможность масштабировать обработчики

---

## 🧱 Структура проекта

```
ShopTestEventSourcingCqrsKafka/
├── docker-compose.yml        # Kafka + Zookeeper
├── README.md                 # Этот файл
│
├── Common/                   # Общие интерфейсы и утилиты
│   ├── IKafkaProducer.cs
│   ├── IKafkaConsumer.cs
│   ├── IEventStore.cs
│   ├── KafkaProducer.cs
│   ├── KafkaConsumer.cs
│   └── FileEventStore.cs
│
├── ShopService/              # Покупки
│   ├── Program.cs
│   ├── Controllers/BuyController.cs
│   ├── Commands/BuyItemCommand.cs
│   ├── Commands/BuyItemHandler.cs
│   └── Kafka/KafkaTransactionListener.cs
│
├── StockService/             # Резервирование товара
│   ├── Program.cs
│   └── Kafka/TransactionListener.cs
```

---

## 🔄 Как работает проект

### 1. Клиент отправляет команду `BuyItemCommand` в ShopService
- Через `MediatR`, команда попадает в `BuyItemHandler`
- Сохраняется событие `TransactionStarted` в EventStore
- Отправляется сообщение в Kafka-топик `transactions`

### 2. StockService слушает Kafka
- Получает событие `TransactionStarted`
- Проверяет товар в наличии (эмуляция)
- Публикует событие `StockReserved` в Kafka

### 3. ShopService слушает `stock-reserved`
- Сохраняет событие `StockReserved` в EventStore
- (можно дополнить завершением транзакции)

---

## 🚀 Запуск

### Шаг 1: Поднять Kafka

```bash
docker-compose up -d
```

Проверь, что сервисы работают:

```bash
docker ps
```

---

### Шаг 2: Запустить сервисы

```bash
cd ShopService
dotnet run

cd ../StockService
dotnet run
```

---

### Шаг 3: Отправить команду покупки

```http
POST http://localhost:5000/buy
Content-Type: application/json

{
  "userId": "e.g. 11111111-1111-1111-1111-111111111111",
  "itemId": "e.g. 22222222-2222-2222-2222-222222222222"
}
```

---

### Шаг 4: Проверить EventStore

Нужно проверить папку `events/`, в ней будут файлы `*.json` с сохранёнными событиями по транзакциям.

---

## ✅ Возможные доработки
- Создание БД
- Заполнение товаров
- Полноценный ввод механизма транзакции и ее отката (`TransactionCompleted`)
- Компенсации при ошибках
- Query часть для CQRS (например, история заказов)
- UI или Swagger

---

## 🧠 Вывод

Паттерны **CQRS + Event Sourcing + Kafka**:
- Надёжны и масштабируемы
- Упрощают отладку и восстановление
- Разделяют ответственность и повышают отказоустойчивость

---

