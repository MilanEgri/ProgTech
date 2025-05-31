using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Domain
{
    /// <summary>
    /// Chat üzenetet reprezentáló osztály
    /// </summary>
    /// 
    [Table("Messages")] // Explicit táblanév megadása
    public class ChatMessage
    {
        //// Üzenet tartalma (SOLID - Single Responsibility)
        //public string Content { get; }

        public ChatMessage()
        {
                
        }


        public ChatMessage(string content, DateTime timestamp)
        {
            Content = content;
            Timestamp = timestamp;
        }

        //public int Id { get; set; }
        //public string Content { get; set; }
        //public DateTime Timestamp { get; set; }
        //public int Length { get; set; } // Migrációval hozzáadott mező

        //public string Formatted => $"[{Timestamp:HH:mm:ss}] {Content}";



        public int Id { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }

            [NotMapped] // Ez az adatbázisban nem lesz mező
            public string Formatted => $"[{Timestamp:HH:mm:ss}] {Content}";
        
    }
}
