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

        public DbSet<EmployeeManagerAssignment> EmployeeManagerAssignments { get; set; }

        public DbSet<LeaveBalance> LeaveBalances { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<LeaveApproval> LeaveApprovals { get; set; }



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
            // Employee Manager Assignment
            // ==========================

            modelBuilder.Entity<EmployeeManagerAssignment>()

                .HasOne(a => a.Employee)

                .WithMany(e => e.ManagerAssignments)

                .HasForeignKey(a => a.EmployeeId)

                .OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<EmployeeManagerAssignment>()

                .HasOne(a => a.Manager)

                .WithMany(e => e.EmployeeAssignments)

                .HasForeignKey(a => a.ManagerId)

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

                .HasMany(l => l.Approvals)

                .WithOne(a => a.LeaveRequest)

                .HasForeignKey(a => a.LeaveRequestId)

                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<LeaveRequest>()

                .Property(l => l.LeaveType)

                .HasConversion<int>();



            // ==========================
            // Leave Approval
            // ==========================

            modelBuilder.Entity<LeaveApproval>()

                .HasOne(a => a.Manager)

                .WithMany()

                .HasForeignKey(a => a.ManagerId)

                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<LeaveApproval>()

                .Property(a => a.Status)

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

            modelBuilder.Entity<RolePermission>()

                .HasKey(rp => new
                {
                    rp.RoleId,
                    rp.PermissionId
                });




            modelBuilder.Entity<RolePermission>()

                .HasOne(rp => rp.Role)

                .WithMany()

                .HasForeignKey(rp => rp.RoleId)

                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<RolePermission>()

                .HasOne(rp => rp.Permission)

                .WithMany(p => p.RolePermissions)

                .HasForeignKey(rp => rp.PermissionId)

                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}