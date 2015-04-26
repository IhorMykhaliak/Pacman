using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test
{
    [TestClass]
    public class BoundingSquareTest
    {
        [TestMethod]
        public void TestEqualsOperator()
        {
            BoundingSquare square1 = new BoundingSquare(0.0f, 0.0f, 20.0f);
            BoundingSquare square2 = new BoundingSquare(15.0f, 15.0f, 25.0f);
            BoundingSquare square3 = new BoundingSquare(0.0f, 0.0f, 25.0f);
            BoundingSquare square4 = new BoundingSquare(0.0f, 0.0f, 20.0f);

            Assert.IsFalse(square1 == square2);
            Assert.IsFalse(square1 == square3);
            Assert.IsTrue(square1 == square4);
        }

        [TestMethod]
        public void TestIntersectsWith()
        {
            BoundingSquare square1 = new BoundingSquare(0.0f, 0.0f, 20.0f);
            BoundingSquare square2 = new BoundingSquare(15.0f, 15.0f, 25.0f);
            BoundingSquare square3 = new BoundingSquare(25.0f, 30.0f, 25.0f);

            Assert.IsTrue(square1.IntersectsWith(square2));
            Assert.IsFalse(square1.IntersectsWith(square3));
        }

        [TestMethod]
        public void TestEquals()
        {
            BoundingSquare square1 = new BoundingSquare(0.0f, 0.0f, 20.0f);
            BoundingSquare square2 = new BoundingSquare(15.0f, 15.0f, 25.0f);
            BoundingSquare square3 = new BoundingSquare(0.0f, 0.0f, 25.0f);
            BoundingSquare square4 = new BoundingSquare(0.0f, 0.0f, 20.0f);

            Assert.IsFalse(square1.Equals(square2));
            Assert.IsFalse(square1.Equals(square3));
            Assert.IsTrue(square1.Equals(square4));
        }

        [TestMethod]
        public void TestEqualsWithNull()
        {
            BoundingSquare square1 = new BoundingSquare(0.0f, 0.0f, 20.0f);

            Assert.IsFalse(square1.Equals(null));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            BoundingSquare square1 = new BoundingSquare(0.0f, 0.0f, 20.0f);

            Assert.AreEqual(square1.GetHashCode(), (int)square1.X ^ (int)square1.Y);
        }
    }
}
