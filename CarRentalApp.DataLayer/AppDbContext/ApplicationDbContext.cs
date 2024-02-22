using CarRentalApp.DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace CarRentalApp.DataLayer.AppDbContext
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //  Car table
            builder.Entity<Car>().HasKey(c=>c.Id);
            
            

            //  TotalCar table
            //  relationship: Car ---  1:M  --- TotalCar
            builder.Entity<TotalCar>().HasKey(tc => tc.RegistrationNumber);

            builder.Entity<TotalCar>().Property(tc => tc.RegistrationNumber)
                .ValueGeneratedNever();

            builder.Entity<TotalCar>().HasOne(c => c.Car)
                .WithMany(x => x.TotalCars)
                .HasForeignKey(x => x.CarId);





            //  Reservation table
            //  relationship: Car ---  1:M  --- Reservation
            //  relationship: CarTotal ---  1:M  --- Reservation
            //  relationship: Customer ---  1:M  --- Reservation
            //  relationship: Payment ---  1:1  --- Reservation
           
            builder.Entity<Reservation>().HasKey(ci => ci.ReservationId);
            

            builder.Entity<Reservation>().HasOne(c=>c.Car)
                .WithMany(c=>c.Reservations)
                .HasForeignKey(r => r.CarId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>().HasOne(c => c.TotalCar)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.RegistrationNumber).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>().HasOne(cs => cs.Customer)
                .WithMany(cs => cs.Reservations)
                .HasForeignKey(r => r.CustomerId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>().HasOne(p=>p.Payment)
                .WithOne(r => r.Reservation)
                .HasForeignKey("Payment", "PaymentId").OnDelete(DeleteBehavior.NoAction);

            //  Customer table
            builder.Entity<Customer>().HasKey(cs => cs.CustId);

            //Payment table
            builder.Entity<Payment>().HasKey(p => p.PaymentId);

        }
    }
}
