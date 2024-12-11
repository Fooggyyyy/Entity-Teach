using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProjectFirst
{
    public class Room
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!; 
        public int Capacity { get; set; } 
        public int PricePerNight { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public Room(string Name, int Capacity, int PricePerNight)
        {
            this.Name = Name;
            this.Capacity = Capacity;
            this.PricePerNight = PricePerNight;
            AddRoom();
        }

        private void AddRoom()
        {
            using(var db = new Hotel())
            {
                db.Rooms.Add(this);
                db.SaveChanges();
                Console.WriteLine("Мы добавили комнату с следующими параметрами:");
                View();
            }
        }

        private void View()
        {
            Console.WriteLine($"Имя: {this.Name}, Вместимость: {this.Capacity}, Цена за ночь: {this.PricePerNight}");
        }

        public void Remove()
        {
            using( var db = new Hotel())
            {
                db.Rooms.Remove(this);
                db.SaveChanges();
            }
        }
    }
}
