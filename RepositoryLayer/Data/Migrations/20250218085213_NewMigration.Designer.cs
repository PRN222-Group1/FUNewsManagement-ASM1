﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryLayer.Data;

#nullable disable

namespace RepositoryLayer.Data.Migrations
{
    [DbContext(typeof(FuNewsManagementContext))]
    [Migration("20250218085213_NewMigration")]
    partial class NewMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RepositoryLayer.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryDescription")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("ParentCategoryID");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NewsArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("NewsArticleID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int")
                        .HasColumnName("CreatedByID");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Headline")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("NewsContent")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("NewsSource")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<bool?>("NewsStatus")
                        .HasColumnType("bit");

                    b.Property<string>("NewsTitle")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<int?>("UpdatedById")
                        .HasColumnType("int")
                        .HasColumnName("UpdatedByID");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatedById");

                    b.ToTable("NewsArticle", (string)null);
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NewsTag", b =>
                {
                    b.Property<int>("NewsArticleId")
                        .HasColumnType("int")
                        .HasColumnName("NewsArticleID");

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("TagID");

                    b.HasKey("NewsArticleId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NewsTag", (string)null);
                });

            modelBuilder.Entity("RepositoryLayer.Entities.SystemAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AccountID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountEmail")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("AccountName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AccountPassword")
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int?>("AccountRole")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SystemAccount", (string)null);
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TagID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Note")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("TagName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK_HashTag");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Category", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.Category", "ParentCategory")
                        .WithMany("InverseParentCategory")
                        .HasForeignKey("ParentCategoryId")
                        .HasConstraintName("FK_Category_Category");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NewsArticle", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.Category", "Category")
                        .WithMany("NewsArticles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_NewsArticle_Category");

                    b.HasOne("RepositoryLayer.Entities.SystemAccount", "CreatedBy")
                        .WithMany("NewsArticles")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_NewsArticle_SystemAccount");

                    b.Navigation("Category");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.NewsTag", b =>
                {
                    b.HasOne("RepositoryLayer.Entities.NewsArticle", "NewsArticle")
                        .WithMany()
                        .HasForeignKey("NewsArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_NewsTag_NewsArticle");

                    b.HasOne("RepositoryLayer.Entities.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_NewsTag_Tag");

                    b.Navigation("NewsArticle");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.Category", b =>
                {
                    b.Navigation("InverseParentCategory");

                    b.Navigation("NewsArticles");
                });

            modelBuilder.Entity("RepositoryLayer.Entities.SystemAccount", b =>
                {
                    b.Navigation("NewsArticles");
                });
#pragma warning restore 612, 618
        }
    }
}
