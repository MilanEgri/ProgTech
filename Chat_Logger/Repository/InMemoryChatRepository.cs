using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat_Logger.Domain;

namespace Chat_Logger.Repository
{
    /// <summary>
    /// Memóriában tároló implementáció (REPOSITORY PATTERN)
    /// </summary>
    public class InMemoryChatRepository : IChatMessageRepository
    {
        // Ideiglenes tároló a memóriában
        private readonly List<ChatMessage> _messages = new List<ChatMessage>();

        public void Add(ChatMessage message)
        {
            // Üzenet hozzáadása a listához
            _messages.Add(message);
        }

        public List<ChatMessage> GetAll()
        {
            // Üzenetek másolatának visszaadása (SOLID - Single Responsibility)
            return new List<ChatMessage>(_messages);
        }
    }
}
