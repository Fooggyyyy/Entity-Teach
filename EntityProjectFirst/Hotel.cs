using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProjectFirst
{
    public class Hotel : DbContext
    {
        private string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Hotel;Trusted_Connection=True;";

        public DbSet<Guest> Guest => Set<Guest>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<Room> Rooms => Set<Room>();

        public Hotel(string _connectionstring = "Server=(localdb)\\MSSQLLocalDB;Database=Hotel;Trusted_Connection=True;")
        {
            Database.EnsureCreated();
            this._connectionString = _connectionstring;
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(_connectionString);

        public static void AddReservation(Guest guest, Room room, DateTime In, DateTime Out)
        {
            if(room.Capacity - 1 > 0)
            {
                if (In < Out)
                {
                    guest.Reservations.Add(new Reservation(room.Id, guest.Id, In, Out));
                    room.Reservations.Add(new Reservation(room.Id, guest.Id, In, Out));
                    using (var db = new Hotel())
                    {
                        db.Reservations.Add(new Reservation(room.Id, guest.Id, In, Out));
                        db.SaveChanges();
  
                    }
                    --room.Capacity;
                    Console.WriteLine("Резервация прошла успешно!!!");
                }
                else
                    throw new Exception("Дата приезда должна быть раньше, чем дата отъезда");
            }
            else
            {
                throw new Exception("В комнате закончились места");
            }
        }

        public ICollection<Guest> GetAllGuest()
        {
            using(var db = new Hotel())
            {
                return db.Guest.ToList();
            }
        }

        public ICollection<Room> GetAllRoom()
        {
            using (var db = new Hotel())
            {
                return db.Rooms.ToList();
            }
        }

        public ICollection<Reservation> GetAllResevation()
        {
            using (var db = new Hotel())
            {
                return db.Reservations.ToList();    
            }
        }
    }
}
