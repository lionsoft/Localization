using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable once CheckNamespace
namespace Lion.Localization.Tests.StringExtension
{
    [TestClass]
    public class StringExtensios_Tests
    {
        const string ExpectedScope = "~/StringExtension/StringExtensios_Tests";

        [TestMethod]
        public void ScopeByType()
        {
            var actual = StringExtensions.Scope(GetType());
            Assert.AreEqual(ExpectedScope, actual);
        }

        [TestMethod]
        public void ScopeByObj()
        {
            var actual = StringExtensions.Scope(this);
            Assert.AreEqual(ExpectedScope, actual);
        }

        [TestMethod]
        public void ScopeByCaller()
        {
            var actual = StringExtensions.Scope();
            Assert.AreEqual(ExpectedScope, actual);
        }

    }
}
