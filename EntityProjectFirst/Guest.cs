using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProjectFirst
{
    public class Guest
    {
        public int Id { get; set; }
        public string? FullName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        
        public Guest(string? FullName, string? email, string? phoneNumber)
        {
            this.FullName = FullName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            if (!CheckInformation())
            {
                throw new Exception("Не корректно введены данные");
            }
            AddGuest();
        }

        private void AddGuest()
        {
            using(var db = new Hotel())
            {
                db.Guest.Add(this);   
                db.SaveChanges();
                Console.WriteLine("Мы добавили гостя с следующими данными: ");
                View();
            }
        }

        private void View()
        {
            Console.WriteLine($"Имя: {this.FullName}, Email: {this.Email}, Phone Number: {this.PhoneNumber}");
        }

        private bool CheckInformation()
        {
            //9 - Минимальный по длине номер
            //15 - Максимальный по длине номер
            //this.FullName.Split(' ').Length != 3 - Значит пользователь ввел не полное ФИО
            if (!this.PhoneNumber.Contains("+") || this.PhoneNumber.Length < 9 || this.PhoneNumber.Length > 15
                || !this.Email.Contains("@") || !this.Email.Contains(".") || this.FullName.Split(' ').Length != 3)
                return false;
            return true;
        }

        public void Remove()
        {
            using (var db = new Hotel())
            {
                db.Guest.Remove(this); 
            }
        }
    }
}
