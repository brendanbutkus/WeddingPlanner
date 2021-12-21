using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
    [Key]
    public int WeddingId {get;set;}

    [Required]
    [MinLength(1)]
    public string WedderOne {get;set;}

    [Required]
    [MinLength(1)]
    public string WedderTwo {get;set;}

    [Required]
    public string WeddingAddress {get;set;}
    
    public DateTime Date {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    public List <Reservation> GuestList {get;set;}
    
    public int UserId {get;set;}

    public User Creator {get;set;}
    // do we need this id because the user had created it?


    }

}