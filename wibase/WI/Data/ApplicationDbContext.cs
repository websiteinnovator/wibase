using System;
using System.Collections.Generic;
using System.Text;
using WI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WI.Models.Web;

namespace WI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        ///******************** Class Begins *********************************/
        //public DbSet<ClassModel> ClassModels { get; set; }
        //public DbSet<ClassStudentsModel> ClassStudentsModels { get; set; }
        //public DbSet<LectureModel> LectureModels { get; set; }
        //public DbSet<LectureHomework> LectureHomeworks { get; set; }
        ///******************** Class Ends *********************************/

    }
}
