using Chat_Logger.Data;
using Chat_Logger.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_Logger.Repository
{
    public class DatabaseChatRepository : IChatMessageRepository
    {
        public void Add(ChatMessage message)
        {
            SQLiteHelper.AddMessage(message);
        }

        public List<ChatMessage> GetAll()
        {
            return SQLiteHelper.GetAllMessages();
        }
    }
}
