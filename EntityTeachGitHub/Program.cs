using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Infrastructure; //Для управлением базы данных

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}

//DbContext - контекст данных для взаимодействия с бд
public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>(); //набор хранимых данных в БД

    //Database.EnsureCreated() гарантирует создание БД, Async асинхронная версия
    //Если БД создалось - true, если нет - false

    //Database.EnsureDeleted гарантирует удаление БД 
    public ApplicationContext() //Благодаря этого у нас будет всегда чистая БД при запуске
    {
        Database.EnsureDeleted(); // гарантируем, что бд удалена
        Database.EnsureCreated(); // гарантируем, что бд будет создана
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Параметры подключения
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HelloApp;Trusted_Connection=True;");
    }
}

class Programm
{
    public static void Main()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            //CanConnect - доступна ли БД
            bool isAvalaible = db.Database.CanConnect();
            // bool isAvalaible2 = await db.Database.CanConnectAsync();
            if (isAvalaible) Console.WriteLine("База данных доступна");
            else Console.WriteLine("База данных не доступна");

            // создаем два объекта User
            User tom = new User { Name = "Tom", Age = 33 };
            User alice = new User { Name = "Alice", Age = 26 };

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


    }
}
