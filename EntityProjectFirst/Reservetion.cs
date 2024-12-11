using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityProjectFirst
{
    public class Reservation
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public int GuestId { get; set; }
        public Guest Guest { get; set; } = null!;
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public bool IsCancelled { get; set; } = false;

        public Reservation(int RoomId, int GuestId, DateTime CheckInDate, DateTime CheckOutDate, bool IsCancelled = false) 
        {
            this.RoomId = RoomId;
            this.GuestId = GuestId;
            this.CheckInDate = CheckInDate;
            this.CheckOutDate = CheckOutDate;
        }
    }
}
