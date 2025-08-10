# ForecastEngine API

Современный и отказоустойчивый RESTful API сервис для получения метеорологических данных, построенный на стеке .NET 9. Проект демонстрирует применение лучших практик backend-разработки, включая чистую архитектуру, безопасность и оптимизацию производительности.

---

## 🚀 Ключевые возможности

*   **Аутентификация и авторизация:** Безопасный доступ к API на основе JWT-токенов.
*   **Работа с данными:** Система регистрации и управления профилями пользователей.
*   **Пользовательские подписки:** Возможность для пользователей сохранять "избранные" города для быстрого доступа к погоде.
*   **Интеграция с внешним API:** Получение актуальных погодных данных от OpenWeatherMap.
*   **Оптимизация:** Кэширование запросов в памяти для снижения задержки и нагрузки на внешний сервис.
*   **Надежность:** Централизованная обработка ошибок и продвинутая валидация входящих данных.
*   **Контейнеризация:** Готовность к развертыванию с помощью Docker.

---

## 🛠️ Стек технологий

*   **Фреймворк:** .NET 9 / ASP.NET Core
*   **Язык:** C#
*   **База данных:** PostgreSQL
*   **ORM:** Entity Framework Core
*   **Аутентификация:** JWT (JSON Web Tokens)
*   **Валидация:** FluentValidation
*   **Логирование:** Serilog
*   **Кэширование:** IMemoryCache
*   **Контейнеризация:** Docker
*   **Архитектурные паттерны:**
    *   Dependency Injection (DI)
    *   Repository Pattern
    *   Options Pattern
    *   Middleware для обработки ошибок

---

## ⚙️ API Эндпоинты

### Auth Controller (`/api/auth`)
*   `POST /register` - Регистрация нового пользователя.
*   `POST /login` - Вход в систему, получение JWT-токена.

### Weather Controller (`/api/weather`)
*   `GET /current` - (🔒 Требуется авторизация) Получение текущей погоды для указанного города.
*   `GET /favorites` - (🔒 Требуется авторизация) Получение погоды для всех городов, добавленных в избранное.

### Favorites Controller (`/api/favorites`)
*   `GET /` - (🔒 Требуется авторизация) Получить список всех избранных городов пользователя.
*   `POST /` - (🔒 Требуется авторизация) Добавить город в избранное.
*   `DELETE /` - (🔒 Требуется авторизация) Удалить город из избранного.

---

## 🏁 Начало работы

### Необходимые условия
*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [PostgreSQL](https://www.postgresql.org/download/)
*   [Docker](https://www.docker.com/products/docker-desktop/) (Опционально, для запуска в контейнере)

### Установка и запуск

1.  **Клонируйте репозиторий:**
    ```bash
    git clone https://github.com/TvoyLogin/forecast-engine-api.git
    cd forecast-engine-api
    ```

2.  **Настройте конфигурацию:**
    *   В файле `WeatherApi/appsettings.Development.json` укажите свои данные для подключения к PostgreSQL и ключи API. Используйте следующий шаблон:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Port=5432;Database=forecast_db;Username=postgres;Password=ВАШ_ПАРОЛЬ"
      },
      "JwtSettings": {
        "SecretKey": "ВАШ_ДЛИННЫЙ_И_НАДЕЖНЫЙ_СЕКРЕТНЫЙ_КЛЮЧ"
      },
      "WeatherApiSettings": {
        "ApiKey": "ВАШ_КЛЮЧ_ОТ_OPENWEATHERMAP_API"
      }
    }
    ```

3.  **Примените миграции базы данных:**
    *   Откройте проект в Visual Studio или используйте .NET CLI и выполните команду в папке проекта:
    ```bash
    dotnet ef database update
    ```

4.  **Запустите приложение:**
    ```bash
    dotnet run
    ```
    API будет доступно по адресу `https://localhost:7123` (или другому порту, указанному в `launchSettings.json`), а документация Swagger UI - по адресу `https://localhost:7123/swagger`.
