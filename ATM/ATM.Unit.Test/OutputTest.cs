using ATMClasses.Interfaces;
using ATMClasses.Output;
using NUnit.Framework;
using NSubstitute;

namespace ATM.Unit.Test
{
    [TestFixture]
    public class OutputTest
    {
        
        private IPrints _print;
        private IOutput _uut;

        [SetUp]
        public void Setup()
        {
            _print = Substitute.For<IPrints>();
            _uut = new Output();
        }

        [Test]
        public void Called_Output()
        {
            
        }
    }

}