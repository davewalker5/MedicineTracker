using MedicineTracker.BusinessLogic.CommandLine;
using MedicineTracker.Entities.CommandLine;
using MedicineTracker.Entities.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace MedicineTracker.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CommandLineParserTest
    {
        private CommandLineParser _parser;

        [TestInitialize]
        public void TestInitialise()
        {
            _parser = new CommandLineParser();
            _parser.Add(CommandLineOptionType.Add, true, "--add", "-a", "Add tablets to the stock level for a given medication", 2, 2);
            _parser.Add(CommandLineOptionType.Set, true, "--set", "-s", "Set the stock level for a given medication", 2, 2);
        }

        [TestMethod]
        public void ValidUsingNamesTest()
        {
            string[] args = ["--add", "1", "28"];
            _parser!.Parse(args);

            var values = _parser?.GetValues(CommandLineOptionType.Add);
            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("28", values[1]);
        }

        [TestMethod]
        public void ValidUsingShortNamesTest()
        {
            string[] args = ["-a", "1", "28"];
            _parser!.Parse(args);

            var values = _parser?.GetValues(CommandLineOptionType.Add);
            Assert.IsNotNull(values);
            Assert.AreEqual(2, values.Count);
            Assert.AreEqual("1", values[0]);
            Assert.AreEqual("28", values[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(TooFewValuesException))]
        public void TooFewArgumentsFailsTest()
        {
            string[] args = ["-a", "1"];
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(TooManyValuesException))]
        public void TooManyArgumentsFailsTest()
        {
            string[] args = ["-a", "1", "28", "Extra Argument"];
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognisedCommandLineOptionException))]
        public void UnrecognisedOptionNameFailsTest()
        {
            string[] args = ["--oops", "1", "28"];
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(UnrecognisedCommandLineOptionException))]
        public void UnrecognisedOptionShortNameFailsTest()
        {
            string[] args = ["-o", "1", "28"];
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(MalformedCommandLineException))]
        public void MalformedCommandLineFailsTest()
        {
            string[] args = ["1", "--add", "28"];
            _parser!.Parse(args);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionTypeFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Add, true, "--other-add", "-oa", "Duplicate option type", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionNameFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Unknown, true, "--add", "-oa", "Duplicate option name", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateOptionException))]
        public void DuplicateOptionShortNameFailsTest()
        {
            _parser!.Add(CommandLineOptionType.Unknown, true, "--other-add", "-a", "Duplicate option shortname", 2, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(MultipleOperationsException))]
        public void MultipleOperationsFailsTest()
        {
            string[] args = ["--add", "1", "28", "--set", "1", "56"];
            _parser!.Parse(args);
        }
    }
}
