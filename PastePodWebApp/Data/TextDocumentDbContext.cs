using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PastePodWebApp.Data
{
    public class TextDocumentDbContext : DbContext
    {
        public TextDocumentDbContext(DbContextOptions<TextDocumentDbContext> options)
         : base(options)
        {
        }

        public DbSet<TextDocumentModel> TextDocuments { get; set; }
    }
}
