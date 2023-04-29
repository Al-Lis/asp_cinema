using System;
using System.Collections.Generic;

namespace ProjectLibrary.Models;

public partial class Session
{
    public int SessionId { get; set; }

    public int FilmId { get; set; }

    public decimal Price { get; set; }

    public DateTime DateTime { get; set; }

    public int NumberZal { get; set; }

    public virtual Film Film { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
