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
            drops.Add(enemy.Hit(bullet));

            // Assert
            Assert.AreEqual(drops.Count(), 1);

        }

        [TestMethod]
        public void LootFallsDown()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            Game game = new Game(new Language.English(), (WeaponType)2, ConsoleColor.Gray, new List<ConsoleKey> { });

            int baseXPos = 3;
            int baseYPos = 3;
            Drop drop = new Drop(baseXPos, baseYPos, dropType: DropType.weaponUpgrade);

            Enemy enemy = new Enemy(10, 10, EnemyType.Melon);

            // TODO : this list doesn't exist yet. 
            //gameEngine.Drops.Add(drop);
            game.GameLogic = gameEngine;

            // Act 
            game.Run();

            // Assert
            Assert.AreNotEqual(drop.Position.X, baseXPos);
        }
        [TestMethod]
        public void LootDropDisappears()
        {
            // Arrange
            GameEngine gameEngine = new GameEngine();
            Game game = new Game(new Language.English(), (WeaponType)2, ConsoleColor.Gray, new List<ConsoleKey> { });

            int baseXPos = 10;
            int baseYPos = 10;
            Drop drop = new Drop(baseXPos, baseYPos, dropType: DropType.weaponUpgrade);

            Enemy enemy = new Enemy(3, 3, EnemyType.Melon);

            // TODO : this list doesn't exist yet. 
            //gameEngine.Drops.Add(drop);
            game.GameLogic = gameEngine;

            // Act 
            game.Run();


            // Assert
            // TODO
            //bool dropsListEmpty = game.GameLogic.Drops.Count() < 1;
            //Assert.IsTrue(dropsListEmpty);
        }
    }
}
