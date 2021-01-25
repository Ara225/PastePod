using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PastePodWebApp.Models;

namespace PastePodWebApp.Data
{
    public static class DataAccess
    {
        public static Task<string> SaveDocument(TextDocumentViewModel modal, TextDocumentDbContext context)
        {
            string fileName = Guid.NewGuid().ToString();
            System.IO.File.WriteAllText(fileName, modal.TextContent);
            TextDocumentModel document = new TextDocumentModel
            {
                TextContentShort = modal.TextContent.Count() >= 200 ? modal.TextContent.Substring(0, 200) : modal.TextContent,
                CreatedOn = DateTime.Now,
                ExpiresOn = DateTime.Now.AddDays(30.0),
                FileName = fileName,
                OwnerId = null
            };
            context.Add(document);
            context.SaveChanges();
            return Task.FromResult(fileName);
        }

        public static Task<string> GetDocument(string fileName)
        {
            return Task.FromResult(System.IO.File.ReadAllText(fileName));
        }
    }
}
