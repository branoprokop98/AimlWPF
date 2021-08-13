// <auto-generated />
using AimlWPF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AimlWPF.Migrations
{
    [DbContext(typeof(AimlContext))]
    [Migration("20210318122709_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("AimlWPF.Aiml", b =>
                {
                    b.Property<int>("AimlId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<int>("Mood")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Pattern")
                        .HasColumnType("TEXT");

                    b.Property<string>("Template")
                        .HasColumnType("TEXT");

                    b.HasKey("AimlId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Aimls");
                });

            modelBuilder.Entity("AimlWPF.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("AimlWPF.Aiml", b =>
                {
                    b.HasOne("AimlWPF.Category", "category")
                        .WithMany("Aimls")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("category");
                });

            modelBuilder.Entity("AimlWPF.Category", b =>
                {
                    b.Navigation("Aimls");
                });
#pragma warning restore 612, 618
        }
    }
}