using System;
using Xunit;
using Chat_Logger.Domain;
using Chat_Logger.Repository;
using Chat_Logger.Logging;
using System.Linq;

namespace Chat_Logger.Tests
{
    public class ChatMessageTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var msg = new ChatMessage("hello", now);

            // Assert
            Assert.Equal("hello", msg.Content);
            Assert.Equal(now, msg.Timestamp);
        }

        [Fact]
        public void Formatted_ShouldReturnExpectedString()
        {
            // Arrange
            var t = new DateTime(2025, 1, 1, 12, 34, 56);
            var msg = new ChatMessage("Üzenet", t);

            // Act
            var formatted = msg.Formatted;

            // Assert
            Assert.Contains("Üzenet", formatted);
            Assert.Contains("2025", formatted);
        }
    }

    public class InMemoryChatRepositoryTests
    {
        [Fact]
        public void GetAll_ShouldBeEmpty_Initially()
        {
            var repo = new InMemoryChatRepository();
            Assert.Empty(repo.GetAll());
        }

        [Fact]
        public void Add_ShouldPersistMessage()
        {
            var repo = new InMemoryChatRepository();
            var msg = new ChatMessage("test", DateTime.Now);
            repo.Add(msg);

            var all = repo.GetAll().ToList();
            Assert.Single(all);
            Assert.Equal(msg, all[0]);
        }

        [Fact]
        public void Add_Multiple_ShouldPreserveOrder()
        {
            var repo = new InMemoryChatRepository();
            var first = new ChatMessage("first", DateTime.Now);
            var second = new ChatMessage("second", DateTime.Now.AddSeconds(1));

            repo.Add(first);
            repo.Add(second);

            var list = repo.GetAll().ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal(first, list[0]);
            Assert.Equal(second, list[1]);
        }
    }

    public class ChainLoggerTests
    {
        [Fact]
        public void Log_ShouldNotThrow()
        {
            var logger = new ChainLogger();
            var ex = Record.Exception(() => logger.Log("teszt", LogLevel.Info));
            Assert.Null(ex);
        }
    }
}
