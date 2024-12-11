using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityProjectFirst
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            //Каждый раз чистим БД
            using (var db = new Hotel())
            {
                db.Database.EnsureDeleted();
            }

            Guest guest = new Guest("Гулецкий Прохор Олегович", "iriwisiri@gmail.com", "+375296552450");

            Room room = new Room("Люкс 1", 5, 100);

            Hotel.AddReservation(guest, room, new DateTime(2023, 12, 1), new DateTime(2023, 12, 10));

        }
    }
}