using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WeddingPlanner.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId {get;set;}

        public int UserId {get;set;}

        public int WeddingId {get;set;}

        public User User {get;set;}

        public Wedding Wedding {get;set;}




    }
}   