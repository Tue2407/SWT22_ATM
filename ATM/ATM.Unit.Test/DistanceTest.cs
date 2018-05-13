using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMClasses.Interfaces;
using ATMClasses.Render;
using NUnit.Framework;

namespace ATM.Unit.Test
{
    class DistanceTest
    {
        private ICalcDistance _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new CalcDistance();
        }

        // Test-case For fly der flyver i et 2D plan.

        [TestCase(0, 0, 0)]
        [TestCase(19000, 0, 19000)]
        [TestCase(-30000, 0, 30000)]
        [TestCase(0, 44333, 44333)]
        [TestCase(0, -44332, 44332)]
        [TestCase(10322, 23433, 13111)]
        [TestCase(-30000, 90000, 120000)]
        [TestCase(10322, 23433, 13111)]
        [TestCase(-35000, -90000, 55000)]
        [TestCase(0, 90000, 90000)]
        [TestCase(90000, 0, 90000)]
        [TestCase(90000, 90000, 0)]
        [TestCase(90000, -90000, 180000)]
        [TestCase(-90000, 90000, 180000)]

        public void CalcDistance2D_returnsCorrect_Value(int first, int second, int result)
        {
            Assert.AreEqual(result, _uut.CalculateDistance1D(first, second));
        }
    }
}