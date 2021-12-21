using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class User
    {
    [Key]
    public int UserId {get;set;}

    [Required]
    public string FirstName {get;set;}

    [Required]
    public string LastName {get;set;}

    [Required]
    [EmailAddress]
    public string Email {get;set;}

    [Required]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password")]

    public string Confirm {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    // public int WeddingId {get;set;}
    // do we need the id on both sides for a many to many?

    public List <Reservation> WeddingList {get;set;}


    }

}