using System;
using System.Collections.Generic;

namespace ProjectLibrary.Models;

public partial class Film
{
    public int FilmId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string AgeRestriction { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
    