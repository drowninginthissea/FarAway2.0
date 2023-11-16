using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FarAway2._0.Entities
{
    public partial class Db : DbContext
    {
        public virtual DbSet<AdditionalServicesForRent> AdditionalServicesForRent { get; set; }
        public virtual DbSet<BranchCharacteristics> BranchCharacteristics { get; set; }
        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<FrequencyOfServices> FrequencyOfServices { get; set; }
        public virtual DbSet<ListOfAdditionalServices> ListOfAdditionalServices { get; set; }
        public virtual DbSet<ParkingSpaceRental> ParkingSpaceRental { get; set; }
        public virtual DbSet<ParkingSpotStatuses> ParkingSpotStatuses { get; set; }
        public virtual DbSet<ParkingSpots> ParkingSpots { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<ServiceProviders> ServiceProviders { get; set; }
        public virtual DbSet<TypeOfRentByDuration> TypeOfRentByDuration { get; set; }
        public virtual DbSet<TypesOfCarExchangeSystem> TypesOfCarExchangeSystem { get; set; }
        public virtual DbSet<TypesOfParking> TypesOfParking { get; set; }
        public virtual DbSet<Users> Users { get; set; }

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
                optionsBuilder.UseSqlServer("Data Source=HOKAGE\\SQLEXPRESS;Initial Catalog=FarAway;" +
                    "Integrated Security=True;Trust Server Certificate=True;Command Timeout=300;" +
                    "MultipleActiveResultSets=True;");
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

                entity.Property(e => e.TheCostOfAParkingSpacePerDay).HasColumnType("money");

                entity.HasOne(d => d.idNavigation)
                    .WithOne(p => p.BranchCharacteristics)
                    .HasForeignKey<BranchCharacteristics>(d => d.id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BranchCharacteristics_Branches");
            });

            modelBuilder.Entity<Branches>(entity =>
            {
                entity.Property(e => e.Address).IsRequired();

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
                entity.Property(e => e.FrequencyName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ListOfAdditionalServices>(entity =>
            {
                entity.Property(e => e.SeviceDescription).IsRequired();

                entity.Property(e => e.SeviceName).IsRequired();

                entity.Property(e => e.SevicePrice).HasColumnType("money");
            });

            modelBuilder.Entity<ParkingSpaceRental>(entity =>
            {
                entity.Property(e => e.RentEndDate).HasColumnType("date");

                entity.Property(e => e.RentalStartDate).HasColumnType("date");

                entity.HasOne(d => d.idParkingSpotNavigation)
                    .WithMany(p => p.ParkingSpaceRental)
                    .HasForeignKey(d => d.idParkingSpot)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ParkingSpaceRental_ParkingSpots");

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
                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasMaxLength(30);
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

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ServiceProviders>(entity =>
            {
                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.ITIN)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<TypeOfRentByDuration>(entity =>
            {
                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<TypesOfCarExchangeSystem>(entity =>
            {
                entity.Property(e => e.TypeName).IsRequired();
            });

            modelBuilder.Entity<TypesOfParking>(entity =>
            {
                entity.Property(e => e.TypeName).IsRequired();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PassportNumber)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.PassportSeries)
                    .IsRequired()
                    .HasMaxLength(4);

                entity.Property(e => e.Password).IsRequired();

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.HasOne(d => d.idRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.idRole)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
