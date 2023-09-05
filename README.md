# Web API для магазина одежды

## Архитектура

DDD + CQRS

## Стек технологий 

- [ASP.NET Core 7](https://dotnet.microsoft.com/en-us/apps/aspnet)
- [Entity Framework](https://learn.microsoft.com/en-us/aspnet/entity-framework)
- [PostgreSQL](https://www.postgresql.org/)
- [Mediator](https://github.com/martinothamar/Mediator)
- [Mapperly](https://github.com/riok/mapperly)
- [Fluent Validation](https://docs.fluentvalidation.net/en/latest/)

## Функционал 

### Аутентификация 
- ✔️ JwtBearer

### Структура ролей
- ✔️ Администратор
- ✔️ Менеджер
- ✔️ Пользователь

### Личный кабинет пользователя 
- ✔️ Профиль
- ✔️ История заказов

### Категории
- ✔️ CRUD-операции

### Товары
- ✔️ CRUD-операции
- ✔️ Создание отзывов

### Каталог
- ✔️ Получение всех товаров
- ✔️ Филтр по категориям
- ✔️ Вывод top-new - 3 последних добавленных товара
- ✔️ «**Хиты продаж**» - получение товаров, купленных наибольшее кол-во раз