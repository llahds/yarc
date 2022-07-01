using Api.Common;

namespace Api.Tests
{
    [TestClass]
    public class StringSearchTests
    {
        [TestMethod]
        public void Text_With_Tokens_Next_To_Word_Boundary_Returns_True()
        {
            var text = "This title contains banned text.";
            Assert.IsTrue(text.Contains(new[] { "banned text" }));
        }

        [TestMethod]
        public void Text_With_Partial_Match_Returns_False()
        {
            var text = "This title contains banned text.";
            Assert.IsFalse(text.Contains(new[] { "ban" }));
        }

        [TestMethod]
        public void Text_With_Tokens_At_Start_Returns_True()
        {
            var text = "Banned text this title contains.";
            Assert.IsTrue(text.Contains(new[] { "banned text" }));
        }

        [TestMethod]
        public void Text_With_Tokens_At_End_Returns_True()
        {
            var text = "This title contains banned text";
            Assert.IsTrue(text.Contains(new[] { "banned text" }));
        }

        [TestMethod]
        public void Text_Without_Tokens_Returns_False()
        {
            var text = "This title contains banned text.";
            Assert.IsFalse(text.Contains(new[] { "unbanned text" }));
        }

        [TestMethod]
        public void Text_With_Numeric_Tokens_Returns_True()
        {
            var text = "Banned 123 text this title contains.";
            Assert.IsTrue(text.Contains(new[] { "banned text", "123" }));
        }

        [TestMethod]
        public void Text_With_Invalid_Numeric_Tokens_Returns_False()
        {
            var text = "Banned 1234 text this title contains.";
            Assert.IsFalse(text.Contains(new[] { "banned text", "123" }));
        }
    }
}