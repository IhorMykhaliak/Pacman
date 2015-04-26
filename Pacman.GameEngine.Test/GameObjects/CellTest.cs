using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pacman.GameEngine.Test.GameObjects
{
    [TestClass]
    public class CellTest
    {
        #region Position

        [TestMethod]
        public void TestGetX()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(15.0f, 25.0f, Content.Empty, 10.0f);

            Assert.AreEqual(cell1.GetX() + 1, 0);
            Assert.AreEqual(cell2.GetX() + 1, 1);
        }

        [TestMethod]
        public void TestGetY()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(15.0f, 25.0f, Content.Empty, 10.0f);

            Assert.AreEqual(cell1.GetY() + 1, 0);
            Assert.AreEqual(cell2.GetY() + 1, 2);
        }

        #endregion

        #region Check conditions

        [TestMethod]
        public void TestIsWall()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.Wall, 20.0f);
            Cell cell2 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);

            Assert.IsTrue(cell1.IsWall());
            Assert.IsFalse(cell2.IsWall());
        }

        [TestMethod]
        public void TestIsCoin()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.Coin, 20.0f);
            Cell cell2 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);

            Assert.IsTrue(cell1.IsCoin());
            Assert.IsFalse(cell2.IsCoin());
        }

        [TestMethod]
        public void TestIsPowerUp()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.PowerUp, 20.0f);
            Cell cell2 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);

            Assert.IsTrue(cell1.IsPowerUp());
            Assert.IsFalse(cell2.IsPowerUp());
        }

        [TestMethod]
        public void TestIsEmpty()
        {
            Cell cell1 = new Cell(15.0f, 15.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(15.0f, 15.0f, Content.Wall, 20.0f);

            Assert.IsTrue(cell1.IsEmpty());
            Assert.IsFalse(cell2.IsEmpty());
        }

        [TestMethod]
        public void TestIsNextTo1()
        {
            Cell cell1 = new Cell(0.0f, 0.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(20.0f, 0.0f, Content.Wall, 20.0f);

            Assert.IsTrue(cell1.IsNextTo(cell2));
        }
        
        [TestMethod]
        public void TestIsNextTo2()
        {
            Cell cell1 = new Cell(0.0f, 0.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(20.0f, 20.0f, Content.Wall, 20.0f);

            Assert.IsFalse(cell1.IsNextTo(cell2));
        }
        
        [TestMethod]
        public void TestIsNextTo3()
        {
            Cell cell1 = new Cell(0.0f, 0.0f, Content.Empty, 20.0f);
            Cell cell2 = new Cell(50.0f, 25.0f, Content.Wall, 20.0f);

            Assert.IsFalse(cell1.IsNextTo(cell2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIsNextToNullCell()
        {
            Cell cell1 = new Cell(0.0f, 0.0f, Content.Empty, 20.0f);

            cell1.IsNextTo(null);
        }

        #endregion
    }
}
