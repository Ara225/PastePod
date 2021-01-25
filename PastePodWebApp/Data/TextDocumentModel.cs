using System;

namespace PastePodWebApp.Data
{
    public class TextDocumentModel
    {
        public int Id { get; set; }

        public string TextContentShort { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string FileName { get; set; }

        public string? OwnerId { get; set; }
    }
}
