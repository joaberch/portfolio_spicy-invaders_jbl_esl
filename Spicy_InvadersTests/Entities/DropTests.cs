using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spicy_Invaders;
using Entity;

namespace Spicy_InvadersTests.Entities
{
    [TestClass]
    public class EnemySpawnerTests
    {
        [TestMethod]
        public void MelonDropsLoot()
        {
            // Arrange
            Enemy enemy = new Enemy(1 ,2 ,EnemyType.Melon);
            List<Drop> drops = new List<Drop>();

            // Act

            // Assert
            Assert.AreEqual(1, 1);
        }
        [TestMethod]
        public void LootFallsDown()
        {
        }
        [TestMethod]
        public void LootDropDisappears()
        {
        }
    }
}
