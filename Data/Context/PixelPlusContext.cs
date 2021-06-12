using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Data.DbEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Context
{
    public class PixelPlusContext : DbContext
    {
        public PixelPlusContext(DbContextOptions<PixelPlusContext> options) : base(options) { }

        #region Users
        public DbSet<Users> Users { get; set; }
        #endregion

        #region Invoice
        public DbSet<Invoice> Invoices { get; set; }
        #endregion

        #region Subscribers
        DbSet<Subscribers> Subscribers { get; set; }
        #endregion

    }
}
