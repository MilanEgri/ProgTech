using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_Logger.Domain;

namespace Chat_Logger.Repository
{
    public interface IChatMessageRepository
    {
        /// <summary>
        /// Adattár műveletek interfésze (REPOSITORY PATTERN)
        /// </summary>
        void Add(ChatMessage message);
        List<ChatMessage> GetAll();
    }
}
