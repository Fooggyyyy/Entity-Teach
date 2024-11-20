using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging; //Для управлением базы данных


//CRUD операции - create, read, update, delete
public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Position { get; set; }
}

//DbContext - контекст данных для взаимодействия с бд
public class ApplicationContext : DbContext
{
    readonly StreamWriter logStream = new StreamWriter("C:\\Users\\user\\source\\repos\\EntityTeach\\mylog.txt", false);

    private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=HelloApp;Trusted_Connection=True;";
    public DbSet<User> Users => Set<User>(); //набор хранимых данных в БД

    //Database.EnsureCreated() гарантирует создание БД, Async асинхронная версия
    //Если БД создалось - true, если нет - false

    //Database.EnsureDeleted гарантирует удаление БД 
    public ApplicationContext(string _connectionstring) //Благодаря этого у нас будет всегда чистая БД при запуске
    {
        //Database.EnsureDeleted(); // гарантируем, что бд удалена
        Database.EnsureCreated(); // гарантируем, что бд будет создана
        this._connectionString = _connectionstring;
    }

    //Метод LogTo - метод для логирования
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Параметры подключения
    {
        optionsBuilder.UseSqlServer(_connectionString);
        optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Debug); //Логирование в файл
    }

    public override void Dispose() //Для утилизации файлового потока для логирования
    {
        base.Dispose();
        logStream.Dispose();
    }

    //Настройки логгирования:
    //Trace - более детализированные сообщения
    //Debug - для полезной информации при разработке и отладке приложения
    //Information - краткий уровень информации для отслеживания потока выполнения
    //Warning - запись неожиданных событий, которые не были предусмотрены.
    //Error - запись ошибок
    //Critical - запись критических ошибок

    //Сообщения с лога делятся на 3 вида:
    //SqlServerEventId: описывает сообщения, специфические для провайдера для MS SQL Server
    //CoreEventId: описывает сообщения, общие для всех провайдеров Entity Framework Core
    //RelationalEventId: описывает сообщения, общие для всех провайдеров для реляционных баз данных
    //optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });

    //Категории сообщений:
    //Database.Command: категория для выполняемых команд, позволяет получить выполняемый код SQL
    //Database.Connection : категория для операций подключения к БД
    //Database.Transaction : категория для транзакций с бд
    //Migration: категория для миграций
    //Model: категория для действий, совершаемых при привязке модели
    //Query: категория для запросов за исключением тех, что генерируют исполняемый код SQL
    //Scaffolding: категория для действий, выполняемых в поцессе обратного инжиниринга(то есть когда по базе данных генерируются классы и класс контекста)
    //Update: категория для сообщений вызова DbContext.SaveChanges()
    //Infrastructure: категория для всех остальных сообщений
    //optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
}

class Programm
{
    public static void Main()
    {
        using (ApplicationContext db = new ApplicationContext("Server=(localdb)\\MSSQLLocalDB;Database=HelloApp;Trusted_Connection=True;"))
        {
            //db.Database.Migrate(); - при измении применть для миграции

            //CanConnect - доступна ли БД
            bool isAvalaible = db.Database.CanConnect();
            // bool isAvalaible2 = await db.Database.CanConnectAsync();
            if (isAvalaible) Console.WriteLine("База данных доступна");
            else { Console.WriteLine("База данных не доступна"); return; }

            // создаем два объекта User
            User tom = new User { Name = "Tom", Age = 33, Position = "Рабочий"};
            User alice = new User { Name = "Alice", Age = 26, Position = "Рабочий" };

            // добавляем их в бд
            db.Users.Add(tom);
            db.Users.Add(alice);
            db.SaveChanges();
            Console.WriteLine("Объекты успешно сохранены");

            // получаем объекты из бд и выводим на консоль
            var users = db.Users.ToList();
            Console.WriteLine("Список объектов:");
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }

        // Редактирование
        using (ApplicationContext db = new ApplicationContext("Server=(localdb)\\MSSQLLocalDB;Database=HelloApp;Trusted_Connection=True;"))
        {
            // получаем первый объект
            User? user = db.Users.FirstOrDefault();
            if (user != null)
            {
                user.Name = "Bob";
                user.Age = 44;
                //обновляем объект
                //db.Users.Update(user);
                db.SaveChanges();
            }
            // выводим данные после обновления
            Console.WriteLine("\nДанные после редактирования:");
            var users = db.Users.ToList();
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }

        // Удаление
        using (ApplicationContext db = new ApplicationContext("Server=(localdb)\\MSSQLLocalDB;Database=HelloApp;Trusted_Connection=True;"))
        {
            // получаем первый объект
            User? user = db.Users.FirstOrDefault();
            if (user != null)
            {
                //удаляем объект
                db.Users.Remove(user);
                db.SaveChanges();
            }
            // выводим данные после обновления
            Console.WriteLine("\nДанные после удаления:");
            var users = db.Users.ToList();
            foreach (User u in users)
            {
                Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
            }
        }

    }
}
