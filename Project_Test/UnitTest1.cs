using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Chat_Logger.Domain;
using Chat_Logger.Repository;
using Chat_Logger.Logging;
using System.Collections.Generic;

namespace Chat_Logger.Tests
{
    // -----------------------------
    // ChatMessage unit tests (MSTest)
    // -----------------------------
    [TestClass]
    public class ChatMessageTests
    {
        [TestMethod]
        public void Constructor_ShouldSetProperties()
        {
            var now = DateTime.UtcNow;
            var msg = new ChatMessage("hello", now);

            Assert.AreEqual("hello", msg.Content);
            Assert.AreEqual(now, msg.Timestamp);
        }

        [TestMethod]
        public void Formatted_ShouldNotBeNullOrWhitespace()
        {
            var msg = new ChatMessage("hi", DateTime.Now);
            Assert.IsFalse(string.IsNullOrWhiteSpace(msg.Formatted));
        }
    }

    // -----------------------------
    // InMemoryChatRepository unit tests (MSTest)
    // -----------------------------
    [TestClass]
    public class InMemoryChatRepositoryTests
    {
        [TestMethod]
        public void GetAll_ShouldBeEmpty_Initially()
        {
            var repo = new InMemoryChatRepository();
            Assert.AreEqual(0, repo.GetAll().Count());
        }

        [TestMethod]
        public void Add_ShouldPersistMessage()
        {
            var repo = new InMemoryChatRepository();
            var msg = new ChatMessage("test", DateTime.Now);
            repo.Add(msg);

            var all = repo.GetAll().ToList();
            Assert.AreEqual(1, all.Count);
            Assert.AreSame(msg, all[0]);
        }

        [TestMethod]
        public void Add_Multiple_ShouldPreserveOrder()
        {
            var repo = new InMemoryChatRepository();
            var first = new ChatMessage("first", DateTime.Now);
            var second = new ChatMessage("second", DateTime.Now.AddSeconds(1));

            repo.Add(first);
            repo.Add(second);

            var list = repo.GetAll().ToList();
            Assert.AreEqual(2, list.Count);
            Assert.AreSame(first, list[0]);
            Assert.AreSame(second, list[1]);
        }

        [TestMethod]
        public void Count_ShouldReflectNumberOfAdds()
        {
            var repo = new InMemoryChatRepository();
            repo.Add(new ChatMessage("one", DateTime.Now));
            repo.Add(new ChatMessage("two", DateTime.Now));
            repo.Add(new ChatMessage("three", DateTime.Now));

            Assert.AreEqual(3, repo.GetAll().Count());
        }

        [TestMethod]
        public void GetAll_ShouldContain_AddedMessage()
        {
            var repo = new InMemoryChatRepository();
            var message = new ChatMessage("abc", DateTime.Now);
            repo.Add(message);
            CollectionAssert.Contains(repo.GetAll().ToList(), message);
        }
    }

    // -----------------------------
    // ChainLogger unit tests (MSTest)
    // -----------------------------
    [TestClass]
    public class ChainLoggerTests
    {
        [TestMethod]
        public void Log_ShouldNotThrow()
        {
            var logger = new ChainLogger();
            Exception ex = null;
            try
            {
                logger.Log("teszt", LogLevel.Info);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNull(ex);
        }

        [TestMethod]
        public void Log_MultipleMessages_ShouldNotThrow()
        {
            var logger = new ChainLogger();
            Exception ex = null;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    logger.Log($"msg-{i}", LogLevel.Info);
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNull(ex);
        }

        [TestMethod]
        public void Log_ErrorLevel_ShouldNotThrow()
        {
            var logger = new ChainLogger();
            Exception ex = null;
            try
            {
                logger.Log("hiba", LogLevel.Error);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.IsNull(ex);
        }
    }
}
