using LeaveManagement.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagement.API.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }



        public DbSet<Employee> Employees { get; set; }

        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }



        // ==========================
        // Permission Management
        // ==========================

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }



        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);



            // ==========================
            // Employee - ApplicationUser
            // ==========================

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)

                .WithOne(u => u.Employee)

                .HasForeignKey<Employee>(
                    e => e.UserId)

                .OnDelete(DeleteBehavior.Cascade);





            // ==========================
            // Employee Manager Relation
            // ==========================

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Manager)

                .WithMany(e => e.Employees)

                .HasForeignKey(e => e.ManagerId)

                .OnDelete(DeleteBehavior.Restrict);







            // ==========================
            // Leave Request
            // ==========================

            modelBuilder.Entity<LeaveRequest>()

                .HasOne(l => l.Employee)

                .WithMany()

                .HasForeignKey(l => l.EmployeeId)

                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<LeaveRequest>()

                .Property(l => l.LeaveType)

                .HasConversion<int>();







            // ==========================
            // Leave Balance
            // ==========================

            modelBuilder.Entity<LeaveBalance>()

                .HasOne(l => l.Employee)

                .WithMany()

                .HasForeignKey(l => l.EmployeeId)

                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<LeaveBalance>()

                .Property(l => l.LeaveType)

                .HasConversion<int>();







            // ==========================
            // Permission Configuration
            // ==========================


            // Composite Key

            modelBuilder.Entity<RolePermission>()

                .HasKey(rp => new
                {
                    rp.RoleId,
                    rp.PermissionId
                });






            // Role -> RolePermission

            modelBuilder.Entity<RolePermission>()

                .HasOne(rp => rp.Role)

                .WithMany()

                .HasForeignKey(rp => rp.RoleId)

                .OnDelete(DeleteBehavior.Cascade);







            // Permission -> RolePermission

            modelBuilder.Entity<RolePermission>()

                .HasOne(rp => rp.Permission)

                .WithMany(p => p.RolePermissions)

                .HasForeignKey(rp => rp.PermissionId)

                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}