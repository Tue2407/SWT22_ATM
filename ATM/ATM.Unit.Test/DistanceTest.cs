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

        // Test-case For fly der flyver i et 1D plan.

        [TestCase(0, 0, 0)]
        [TestCase(10000, 0, 10000)]
        [TestCase(-11000, 0, 11000)]
        [TestCase(0, 12000, 12000)]
        [TestCase(0, -13000, 13000)]
        [TestCase(10000, 10000, 0)]
        [TestCase(-11000, 10000, 21000)]
        [TestCase(90000, -90000, 180000)]
        [TestCase(-90000, 90000, 180000)]
        [TestCase(0, 90000, 90000)]
        [TestCase(90000, 0, 90000)]
        [TestCase(90000, 90000, 0)]

        public void CalcDistance1D_returnsCorrect_Value(int a, int b, int value)
        {
            Assert.AreEqual(value, _uut.CalculateDistance1D(a, b));
        }

        [TestCase(0, 0, 0, 0, 0)]
        [TestCase(20000, 0, 0, 0, 20000)]
        [TestCase(0, 21000, 0, 0, 21000)]
        [TestCase(0, 0, 22000, 0, 22000)]
        [TestCase(0, 0, 0, 23000, 23000)]
        public void CalcDistance2D_returnsCorrect_Value(int x1, int y1, int x2, int y2, int value)
        {
            Assert.AreEqual(value, _uut.CalculateDistance2D(x1, y1, x2, y2));
        }
    }
}