using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Domain
{

    [Table("Logs")] // Explicit táblanév megadása
    public class LogEntry
    {
        
        
            public int Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string Level { get; set; }
            public string Message { get; set; }
        
    }
}
