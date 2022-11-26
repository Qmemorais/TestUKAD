﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestUrls.EntityFramework;

namespace TestUrls.EntityFramework.Migrations
{
    [DbContext(typeof(UrlContext))]
    partial class UrlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.SiteTestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("InfoEntities");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlWithResponse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsSitemap")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWeb")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TestEntityId")
                        .HasColumnType("int");

                    b.Property<int>("TimeOfResponse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TestEntityId");

                    b.ToTable("UrlWithResponseEntities");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlWithResponse", b =>
                {
                    b.HasOne("TestUrls.EntityFramework.Entities.SiteTestEntity", "TestEntity")
                        .WithMany("UrlWithResponseEntities")
                        .HasForeignKey("TestEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TestEntity");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.SiteTestEntity", b =>
                {
                    b.Navigation("UrlWithResponseEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
