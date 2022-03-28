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

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.GeneralInfoEntity", b =>
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

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InfoEntityId")
                        .HasColumnType("int");

                    b.Property<bool>("IsSitemap")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWeb")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InfoEntityId");

                    b.ToTable("UrlEntities");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlResponseEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("InfoEntityId")
                        .HasColumnType("int");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TimeOfResponse")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("InfoEntityId");

                    b.ToTable("UrlResponseEntities");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlEntity", b =>
                {
                    b.HasOne("TestUrls.EntityFramework.Entities.GeneralInfoEntity", "InfoEntity")
                        .WithMany("UrlEntities")
                        .HasForeignKey("InfoEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InfoEntity");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.UrlResponseEntity", b =>
                {
                    b.HasOne("TestUrls.EntityFramework.Entities.GeneralInfoEntity", "InfoEntity")
                        .WithMany("UrlResponseEntities")
                        .HasForeignKey("InfoEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InfoEntity");
                });

            modelBuilder.Entity("TestUrls.EntityFramework.Entities.GeneralInfoEntity", b =>
                {
                    b.Navigation("UrlEntities");

                    b.Navigation("UrlResponseEntities");
                });
#pragma warning restore 612, 618
        }
    }
}
