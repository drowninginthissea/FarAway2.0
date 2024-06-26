﻿namespace FarAway2._0.Tools.Collections
{
    public enum TableNames
    {
        [TableType(TableTypes.JournalTables)]
        AdditionalServicesForRent,
        [TableType(TableTypes.JournalTables)]
        BranchCharacteristics,
        [TableType(TableTypes.JournalTables)]
        Branches,
        [TableType(TableTypes.ReferenceTables)]
        FrequencyOfServices,
        [TableType(TableTypes.ReferenceTables)]
        ListOfAdditionalServices,
        [TableType(TableTypes.JournalTables)]
        ParkingSpaceRental,
        [TableType(TableTypes.JournalTables)]
        ParkingSpots,
        [TableType(TableTypes.ReferenceTables)]
        ParkingSpotStatuses,
        [TableType(TableTypes.ReferenceTables)]
        RentalStatuses,
        [TableType(TableTypes.ReferenceTables)]
        Roles,
        [TableType(TableTypes.ReferenceTables)]
        ServiceProviders,
        [TableType(TableTypes.ReferenceTables)]
        TypeOfRentByDuration,
        [TableType(TableTypes.ReferenceTables)]
        TypesOfCarExchangeSystem,
        [TableType(TableTypes.ReferenceTables)]
        TypesOfParking,
        [TableType(TableTypes.JournalTables)]
        Users,
        [TableType(TableTypes.Null)]
        Null
    }
}
