using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PastePodWebApp.Models;

namespace PastePodWebApp.Data
{
    public static class DataAccess
    {
        public static async Task<string> SaveDocument(TextDocumentViewModel modal, IdentityUser user, TextDocumentDbContext context)
        {
            string fileName = modal.FileName != null ? modal.FileName : Guid.NewGuid().ToString();
            System.Diagnostics.Debug.WriteLine(modal.TextContent);
            System.IO.File.WriteAllText(fileName, modal.TextContent, System.Text.Encoding.UTF8);
            TextDocumentModel document;
            if (modal.FileName != null)
            {
                document = await DataAccess.GetDocumentDbRecord(context, fileName);
                document.TextContentShort = modal.TextContent.Count() >= 200 ? modal.TextContent.Substring(0, 200) : modal.TextContent;
                document.FileName = fileName;
                context.Update(document);
            }
            else
            {
                document = new TextDocumentModel
                {
                    TextContentShort = modal.TextContent.Count() >= 200 ? modal.TextContent.Substring(0, 200) : modal.TextContent,
                    FileName = fileName,
                    OwnerId = user == null ? null : user.Id,
                    CreatedOn = DateTime.Now,
                    ExpiresOn = DateTime.Now.AddDays(30.0)
                };
                context.Add(document);
            }
            context.SaveChanges();
            return fileName;
        }

        public static Task<string> GetDocumentContents(string fileName)
        {
            return Task.FromResult(System.IO.File.ReadAllText(fileName, System.Text.Encoding.UTF8));
        }

        public static Task DeleteDocument(TextDocumentDbContext context, string fileName)
        {
            System.IO.File.Delete(fileName);
            context.Remove(context.TextDocuments.Where((item) => item.FileName == fileName).ToList()[0]);
            context.SaveChanges();
            return Task.FromResult(0);
        }

        public static Task<List<TextDocumentModel>> GetDocumentsByUserId(TextDocumentDbContext context, string userId)
        {
            List<TextDocumentModel> documents = context.TextDocuments.Where((item) => item.OwnerId == userId).ToList();
            return Task.FromResult(documents);
        }

        public static Task<TextDocumentModel> GetDocumentDbRecord(TextDocumentDbContext context, string fileName)
        {
            TextDocumentModel document = context.TextDocuments.Where((item) => item.FileName == fileName).ToList()[0];
            return Task.FromResult(document);
        }
    }
}
