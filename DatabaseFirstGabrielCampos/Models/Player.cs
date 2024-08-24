using System;
using System.Collections.Generic;

namespace DatabaseFirstGabrielCampos.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string PlayerName { get; set; } = null!;

    public int? TeamId { get; set; }

    public string? Position { get; set; }

    public virtual Team? Team { get; set; }
}
