using System;
using System.Collections.Generic;

namespace ProjectLibrary.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public long PhoneNumb { get; set; }

    public DateTime? DateBirthday { get; set; }

    public string Password { get; set; } = null!;

    public int AdminCheck { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
