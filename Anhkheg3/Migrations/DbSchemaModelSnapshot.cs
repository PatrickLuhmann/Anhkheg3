using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Anhkheg3;

namespace Anhkheg3.Migrations
{
    [DbContext(typeof(DbSchema))]
    partial class DbSchemaModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("Anhkheg3.Purchase", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Cost");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Gallons");

                    b.Property<int>("Odometer");

                    b.Property<decimal>("Trip");

                    b.Property<int>("VehicleId");

                    b.HasKey("ID");

                    b.HasIndex("VehicleId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("Anhkheg3.Vehicle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Make");

                    b.Property<string>("Model");

                    b.Property<string>("Name");

                    b.Property<int>("StartingMileage");

                    b.Property<int>("Year");

                    b.HasKey("ID");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Anhkheg3.Purchase", b =>
                {
                    b.HasOne("Anhkheg3.Vehicle", "Vehicle")
                        .WithMany("Purchases")
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
