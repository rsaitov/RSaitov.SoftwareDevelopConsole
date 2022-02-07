# Расчет зарплаты ООО “Разработка софта”
Моя версия реализации задания, опубликованного на youtube-канале Степана Берегового: https://www.youtube.com/c/SBeregovoyRU/

Описание задания:
https://docs.google.com/document/d/1kZz1ozAwNTVkIxWoyPYI_zTw6mos3CI03MyXnCxNbeM/edit#heading=h.jolz5iaffihg

## Реализация ##

**Проект Data**
- слой работы с БД IRepository (реализация TextFileDB)
- объекты хранения данных (Person, Worker, TimeRecord)

**Проект Domain**
- объекты с поведением (сотрудники, таблицы учета рабочего времени, отчеты)
- слой бизнес-логики IService

Объекты реализующие IWorker - виды сотрудников по ролям:
- менеджер (Manager)
- сотрудник на зарплате (Employee)
- фрилансер (Freelancer)

## Что можно улучшить ##

- [СДЕЛАНО с использованием Autofac] IoC-контейнер для глобального управления зависимостями (https://github.com/thangchung/awesome-dotnet-core#ioc). **Не совсем понимаю необходимость использования IoC-контейнера в консольном приложении. Контейнер резолвится для входного класса приложения, в конструкторе интерфейсам присваиваются зарегистрированные классы. Дальше этот входной класс создает экзмепляры сервисов, передавая зарегистрированные классы в их конструкторы. Что мешает в коде входного класса создать экземпляр под нужный интерфейс? Кода будет столько же. Возможно стоит создать фабрику сервисов, которая будет принимать зарегистрированные классы. Тогда хоть как-то можно упростить разработку и сократить код.** https://github.com/rsaitov/RSaitov.SoftwareDevelopConsole/pull/18#discussion_r795976995 
- [СДЕЛАНО Persistence/entity/ResponseObject.cs] custom-response object (CRO) для упаковки результата выполнения команды, текста ошибки, возвращаемого объекта. Сейчас методы возвращают просто false. Поднимать исключения для информирования вверх по стеку об ошибке "дорого". CRO может включать разнообразные данные - успешность выполнения метода, сообщение, возвращаемые объекты итп. Минус - необходимость явного приведения при передаче возвращаемого объекта.
- устранить избыточную вычитку из БД. Производить фильтрацию на уровне запросов к БД, а не на уровне IService. Пример: GetTimeRecords(IWorker worker) выбирает из БД все рабочие часы сотрудников с той же ролью. Можно добавить аналогичный метод в IRepository.

## Что можно добавить ##

- [СДЕЛАНО с использованием log4Net] логирование для хранения истории выполнения команд. (https://github.com/thangchung/awesome-dotnet-core#logging) -  
- локализация (https://github.com/thangchung/awesome-dotnet-core#internationalization)
