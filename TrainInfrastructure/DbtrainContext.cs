using System;
using System.Collections.Generic;
using TrainDomain.Model;
using Microsoft.EntityFrameworkCore;

//namespace TrainDomain.Model;
namespace TrainInfrastructure;
public partial class DbtrainContext : DbContext
{
    public DbtrainContext()
    {
    }

    public DbtrainContext(DbContextOptions<DbtrainContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Carriage> Carriages { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Station> Stations { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=Probook455\\SQLEXPRESS; Database=DBTrain; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.ToTable("Cargo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("clientId");
            entity.Property(e => e.Contain)
                .HasColumnType("text")
                .HasColumnName("contain");
            entity.Property(e => e.Volume).HasColumnName("volume");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Client).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cargo_Client");

            entity.HasOne(d => d.Station).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cargo_Station");
        });

        modelBuilder.Entity<Carriage>(entity =>
        {
            entity.ToTable("Carriage");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaxVolume).HasColumnName("maxVolume");
            entity.Property(e => e.MaxWeight).HasColumnName("maxWeight");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.TrainId).HasColumnName("trainId");

            entity.HasOne(d => d.Cargo).WithMany(p => p.Carriages)
                .HasForeignKey(d => d.CargoId)
                .HasConstraintName("FK_Carriage_Cargo");

            entity.HasOne(d => d.Train).WithMany(p => p.Carriages)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carriage_Train");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("City");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("text")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Driver");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("text")
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.TrainId).HasColumnName("trainId");

            entity.HasOne(d => d.Train).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.TrainId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Driver_Train");
        });

        modelBuilder.Entity<Station>(entity =>
        {
            entity.ToTable("Station");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.City).WithMany(p => p.Stations)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Station_City");
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.ToTable("Train");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Model)
                .HasColumnType("text")
                .HasColumnName("model");
            entity.Property(e => e.Power).HasColumnName("power");

            entity.HasOne(d => d.Station).WithMany(p => p.Trains)
                .HasForeignKey(d => d.StationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Train_Station");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
