﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FarAwayClient.Models;

public partial class RentalStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; }

    public virtual ICollection<ParkingSpaceRental> ParkingSpaceRentals { get; set; } = new List<ParkingSpaceRental>();
}