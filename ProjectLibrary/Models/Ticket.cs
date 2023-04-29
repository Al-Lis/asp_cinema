using System;
using System.Collections.Generic;

namespace ProjectLibrary.Models;

public partial class Ticket
{
    public int TicketsId { get; set; }

    public int SessionId { get; set; }

    public int RowNumber { get; set; }

    public int SeatNumber { get; set; }

    public int UserId { get; set; }

    public bool? CheckControl { get; set; }

    public virtual Session Session { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
