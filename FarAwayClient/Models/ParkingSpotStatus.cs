﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FarAwayClient.Models;

public partial class ParkingSpotStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; }

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();
}