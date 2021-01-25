using System;

namespace PastePodWebApp.Data
{
    public class TextDocumentModel
    {
        public int Id { get; set; }

        public string TextContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string URL { get; set; }
    }
}
