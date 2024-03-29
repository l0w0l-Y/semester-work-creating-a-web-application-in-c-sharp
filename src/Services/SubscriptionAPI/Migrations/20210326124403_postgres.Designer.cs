﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SubscriptionAPI;

namespace SubscriptionAPI.Migrations
{
    [DbContext(typeof(SubscriptionContext))]
    [Migration("20210326124403_postgres")]
    partial class postgres
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SubscriptionAPI.Entities.PaidSubscription", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsAutorenewal")
                        .HasColumnType("boolean");

                    b.Property<int>("SubscribedToId")
                        .HasColumnType("integer");

                    b.Property<int?>("TariffId")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("TariffId");

                    b.ToTable("PaidSubscriptions");
                });

            modelBuilder.Entity("SubscriptionAPI.Entities.Tariff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PricePerMounth")
                        .HasColumnType("integer");

                    b.Property<int?>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Tariffs");
                });

            modelBuilder.Entity("SubscriptionAPI.Entities.TypeOfSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Tye")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TypeOfSubscriptions");
                });

            modelBuilder.Entity("SubscriptionAPI.Entities.PaidSubscription", b =>
                {
                    b.HasOne("SubscriptionAPI.Entities.Tariff", "Tariff")
                        .WithMany()
                        .HasForeignKey("TariffId");

                    b.Navigation("Tariff");
                });

            modelBuilder.Entity("SubscriptionAPI.Entities.Tariff", b =>
                {
                    b.HasOne("SubscriptionAPI.Entities.TypeOfSubscription", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}
