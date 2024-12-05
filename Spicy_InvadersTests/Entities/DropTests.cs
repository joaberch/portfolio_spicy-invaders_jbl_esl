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
            enemy.HealthPoints = 10;
            List<Drop> drops = new List<Drop>();
            Bullet bullet = new Bullet();
            bullet.Damage = 10;


            // Act
            drops.Add(enemy.DropLoot());

            // Assert
            Assert.AreEqual(drops.Count(), 1);

        }

        [TestMethod]
        public void LootFallsDown()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            int baseYPos = 10;
            int baseXPos = 10;
            Drop drop = new Drop(baseXPos, baseYPos, DropType.weaponUpgrade);
            gameEngine.Drops.Add(drop);

            // Act 
            gameEngine.MoveDrop();

            // Assert
            Assert.AreNotEqual(gameEngine.Drops[0].Position.Y, baseYPos);
        }
        [TestMethod]
        public void LootDropDisappearsBelowPlayer()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            int playerXPos = gameEngine.PlayerShip.Position.X;
            int playerYPos = gameEngine.PlayerShip.Position.Y;
            Drop drop = new Drop(playerXPos, playerYPos, DropType.weaponUpgrade);
            gameEngine.Drops.Add(drop);

            // Act 
            gameEngine.MoveDrop();
            gameEngine.CheckDropBounderies();

            // Assert
            bool dropsListEmpty = gameEngine.Drops.Count() < 1;
            Assert.IsTrue(dropsListEmpty);
        }
        [TestMethod]
        public void LootDropDisappearsAbovePlayer()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            int playerXPos = gameEngine.PlayerShip.Position.X;
            int playerYPos = gameEngine.PlayerShip.Position.Y;
            Drop drop = new Drop(playerXPos-5, playerYPos-5, DropType.weaponUpgrade);
            gameEngine.Drops.Add(drop);

            // Act 
            gameEngine.CheckDropBounderies();

            // Assert
            bool dropsListEmpty = gameEngine.Drops.Count() < 1;
            Assert.IsFalse(dropsListEmpty);
        }
        [TestMethod]
        public void PlayerUpgradesWeapon()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            int XPos = gameEngine.PlayerShip.Position.X;
            int YPos = gameEngine.PlayerShip.Position.Y;
            Drop drop = new Drop(XPos, YPos, DropType.weaponUpgrade);
            gameEngine.Drops.Add(drop);
            var currentWeapon = gameEngine.PlayerShip.Weapon;

            // Act 
            gameEngine.MoveDrop();


            // Assert
            var newWeapon = gameEngine.PlayerShip.Weapon;
            Assert.AreNotEqual(currentWeapon, newWeapon);
        }
        [TestMethod]
        public void CheckPlayerDropCollision()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            int XPos = gameEngine.PlayerShip.Position.X;
            int YPos = gameEngine.PlayerShip.Position.Y;
            Drop drop = new Drop(XPos, YPos, DropType.weaponUpgrade);
            gameEngine.Drops.Add(drop);

            // Act & Assert
            Assert.IsTrue(gameEngine.CheckDropperHasSamePositionAsPlayer(drop));
        }
    }
}
