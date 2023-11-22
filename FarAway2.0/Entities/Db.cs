using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FarAway2._0.Entities
{
    public partial class Db : DbContext
    {
        public virtual DbSet<AdditionalServicesForRent> AdditionalServicesForRent { get; set; } = null!;
        public virtual DbSet<BranchCharacteristics> BranchCharacteristics { get; set; } = null!;
        public virtual DbSet<Branches> Branches { get; set; } = null!;
        public virtual DbSet<FrequencyOfServices> FrequencyOfServices { get; set; } = null!;
        public virtual DbSet<HistoryOfFreezing> HistoryOfFreezing { get; set; } = null!;
        public virtual DbSet<ListOfActions> ListOfActions { get; set; } = null!;
        public virtual DbSet<ListOfAdditionalServices> ListOfAdditionalServices { get; set; } = null!;
        public virtual DbSet<ParkingSpaceRental> ParkingSpaceRental { get; set; } = null!;
        public virtual DbSet<ParkingSpotStatuses> ParkingSpotStatuses { get; set; } = null!;
        public virtual DbSet<ParkingSpots> ParkingSpots { get; set; } = null!;
        public virtual DbSet<RentalStatuses> RentalStatuses { get; set; } = null!;
        public virtual DbSet<Roles> Roles { get; set; } = null!;
        public virtual DbSet<ServiceProviders> ServiceProviders { get; set; } = null!;
        public virtual DbSet<TypeOfRentByDuration> TypeOfRentByDuration { get; set; } = null!;
        public virtual DbSet<TypesOfCarExchangeSystem> TypesOfCarExchangeSystem { get; set; } = null!;
        public virtual DbSet<TypesOfParking> TypesOfParking { get; set; } = null!;
        public virtual DbSet<Users> Users { get; set; } = null!;

        public Db()
        {
        }

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer("Data Source=1DESKTOP-4QRPJV8\\SQLEXPRESS;" +
                "Initial Catalog=FarAway;Integrated Security=True;Multiple Active Result Sets=True;" +
                "TrustServerCertificate=True;");
                // DESKTOP-2NAFSVT\SQLEXPRESS;
                // DESKTOP-4QRPJV8\SQLEXPRESS
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalServicesForRent>(entity =>
            {
                entity.HasKey(e => new { e.idRent, e.idService });

                entity.HasOne(d => d.FrequencyOfServicePerformanceInDaysNavigation)
                    .WithMany(p => p.AdditionalServicesForRent)
                    .HasForeignKey(d => d.FrequencyOfServicePerformanceInDays)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionalServicesForRent_FrequencyOfServices");

                entity.HasOne(d => d.idRentNavigation)
                    .WithMany(p => p.AdditionalServicesForRent)
                    .HasForeignKey(d => d.idRent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionalServicesForRent_ParkingSpaceRental");

                entity.HasOne(d => d.idServiceNavigation)
                    .WithMany(p => p.AdditionalServicesForRent)
                    .HasForeignKey(d => d.idService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionalServicesForRent_ListOfAdditionalServices");

                entity.HasOne(d => d.idServiceProvidersNavigation)
                    .WithMany(p => p.AdditionalServicesForRent)
                    .HasForeignKey(d => d.idServiceProviders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdditionalServicesForRent_ServiceProviders");
            });

            modelBuilder.Entity<BranchCharacteristics>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.TheCostOfAParkingSpacePerDay).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.idNavigation)
                    .WithOne(p => p.BranchCharacteristics)
                    .HasForeignKey<BranchCharacteristics>(d => d.id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BranchCharacteristics_Branches");
            });

            modelBuilder.Entity<Branches>(entity =>
            {
                entity.HasOne(d => d.idTypeOfCarExchangeSystemNavigation)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.idTypeOfCarExchangeSystem)
                    .HasConstraintName("FK_Branches_TypesOfCarExchangeSystem");

                entity.HasOne(d => d.idTypeOfParkingNavigation)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.idTypeOfParking)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Branches_TypesOfParking");
            });

            modelBuilder.Entity<FrequencyOfServices>(entity =>
            {
                entity.Property(e => e.FrequencyName).HasMaxLength(50);
            });

            modelBuilder.Entity<HistoryOfFreezing>(entity =>
            {
                entity.HasKey(e => new { e.idRental, e.FreezingNumber });

                entity.Property(e => e.DateOfAction).HasColumnType("date");

                entity.HasOne(d => d.idActionNavigation)
                    .WithMany(p => p.HistoryOfFreezing)
                    .HasForeignKey(d => d.idAction)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoryOfFreezing_ListOfActions");

                entity.HasOne(d => d.idRentalNavigation)
                    .WithMany(p => p.HistoryOfFreezing)
                    .HasForeignKey(d => d.idRental)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HistoryOfFreezing_ParkingSpaceRental");
            });

            modelBuilder.Entity<ListOfActions>(entity =>
            {
                entity.Property(e => e.ActionName).HasMaxLength(10);
            });

            modelBuilder.Entity<ListOfAdditionalServices>(entity =>
            {
                entity.Property(e => e.SevicePrice).HasColumnType("money");
            });

            modelBuilder.Entity<ParkingSpaceRental>(entity =>
            {
                entity.Property(e => e.RentEndDate).HasColumnType("date");

                entity.Property(e => e.RentalStartDate).HasColumnType("date");

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.idParkingSpotNavigation)
                    .WithMany(p => p.ParkingSpaceRental)
                    .HasForeignKey(d => d.idParkingSpot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpaceRental_ParkingSpots");

                entity.HasOne(d => d.idRentalStatusNavigation)
                    .WithMany(p => p.ParkingSpaceRental)
                    .HasForeignKey(d => d.idRentalStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpaceRental_RentalStatuses");

                entity.HasOne(d => d.idTypeOfRentByDurationNavigation)
                    .WithMany(p => p.ParkingSpaceRental)
                    .HasForeignKey(d => d.idTypeOfRentByDuration)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpaceRental_TypeOfRentByDuration");

                entity.HasOne(d => d.idUserNavigation)
                    .WithMany(p => p.ParkingSpaceRental)
                    .HasForeignKey(d => d.idUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpaceRental_Users");
            });

            modelBuilder.Entity<ParkingSpotStatuses>(entity =>
            {
                entity.Property(e => e.StatusName).HasMaxLength(30);
            });

            modelBuilder.Entity<ParkingSpots>(entity =>
            {
                entity.HasOne(d => d.idBranchNavigation)
                    .WithMany(p => p.ParkingSpots)
                    .HasForeignKey(d => d.idBranch)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpots_Branches");

                entity.HasOne(d => d.idParkingSpotStatusNavigation)
                    .WithMany(p => p.ParkingSpots)
                    .HasForeignKey(d => d.idParkingSpotStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpots_ParkingSpotStatuses");
            });

            modelBuilder.Entity<RentalStatuses>(entity =>
            {
                entity.Property(e => e.StatusName).HasMaxLength(20);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<ServiceProviders>(entity =>
            {
                entity.Property(e => e.ITIN).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(11);
            });

            modelBuilder.Entity<TypeOfRentByDuration>(entity =>
            {
                entity.Property(e => e.PriceCoefficient).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.TypeName).HasMaxLength(40);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Login).HasMaxLength(30);

                entity.Property(e => e.Name).HasMaxLength(30);

                entity.Property(e => e.Patronymic).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(11);

                entity.Property(e => e.Surname).HasMaxLength(40);

                entity.HasOne(d => d.idRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.idRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
