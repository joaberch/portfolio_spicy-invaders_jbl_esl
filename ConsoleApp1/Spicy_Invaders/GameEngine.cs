﻿//ETML 
///Auteur : Joachim Berchel & Ethan Schafstall
///Date : 05.12.2024 
///Description : Logic for the game

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Spicy_Invaders
{
    /// <summary>
    /// GameLogic class the handles all game behaviours/calculations.
    /// </summary>
    public class GameEngine
    {
        /// <summary>
        /// Keys for moving player
        /// </summary>
        public List<ConsoleKey> ControlKeys { get; set; }
        /// <summary>
        /// enemies
        /// </summary>
        public List<Enemy> Enemies { get; }
        /// <summary>
        /// all projectiles
        /// </summary>
        public List<Projectile> Projectiles { get; }
        /// <summary>
        /// All drop item
        /// </summary>
        public List<Drop> Drops { get; set; }
        /// <summary>
        /// The player controlled ship
        /// </summary>
        public PlayerShip PlayerShip { get; }
        public GameEngine(List<ConsoleKey> consoleKeys)
        {
            this.Enemies = new List<Enemy>();
            this.Projectiles = new List<Projectile>();
            this.PlayerShip = new PlayerShip(GameSettings.PLAYER_START_POS.X, GameSettings.PLAYER_START_POS.Y);
            this.ControlKeys = consoleKeys;
            this.Drops = new List<Drop>();
        }
        public GameEngine()
        {
            this.Enemies = new List<Enemy>();
            this.Projectiles = new List<Projectile>();
            this.PlayerShip = new PlayerShip(GameSettings.PLAYER_START_POS.X, GameSettings.PLAYER_START_POS.Y);
            this.ControlKeys = new List<ConsoleKey>() { ConsoleKey.LeftArrow, ConsoleKey.RightArrow };
            this.Drops = new List<Drop>();
        }
        /// <summary>
        /// Method responsible for creating new enemy objects and adding them to the Enemies list. 
        /// </summary>
        /// <param name="isMelon">bool if is melon enemy is to be spawned</param>
        public void SpawnEnemy(bool isMelon, int wave = 0)
        {
            if (!isMelon)
            {
                Random random = new Random();

                EnemyType randomType = (EnemyType)random.Next(1, 4 + wave);

                int yPosition = 0;
                int yVelocity = 5;

                if ((int)randomType - 3 > 3)
                {
                    randomType = EnemyType.Grape;
                }
                else
                {
                    randomType = (EnemyType)random.Next(1, 3);
                }

                Enemy spawnedEnemy = new Enemy(GameSettings.ENEMY_START_POS.X, GameSettings.ENEMY_START_POS.Y + yPosition, randomType, Direction.Right);
                spawnedEnemy.Velocity = new Vector((int)GameSettings.ENEMYVELOCITY, yVelocity);
                Enemies.Add(spawnedEnemy);
            }
            else
            {
                Enemy spawnedMelon = new Enemy(GameSettings.ENEMY_START_POS.X, GameSettings.ENEMY_START_POS.Y, EnemyType.Melon, Direction.Right);
                spawnedMelon.Velocity = new Vector(2, 2);
                Enemies.Add(spawnedMelon);
            }
        }

        public void MoveDrop()
        {
            foreach (var item in Drops)
            {
                if (CheckDropperHasSamePositionAsPlayer(item))
                {
                    ChangePlayerShipWeapon();
                }
                item.Position.Y += item.Velocity.Y;
            }
        }

        public void ChangePlayerShipWeapon()
        {
            int nbrWeapon = Enum.GetNames(typeof(WeaponType)).Length;
            Random random = new Random();
            var weaponNumberSelected = 0;
            do
            {
                weaponNumberSelected = random.Next(nbrWeapon);
            } while (weaponNumberSelected == (int)PlayerShip.Weapon);
            PlayerShip.Weapon = (WeaponType)weaponNumberSelected;
        }

        public void ChangePlayerShipWeapon(WeaponType type)
        {
            PlayerShip.Weapon = type;
        }

        public bool CheckDropperHasSamePositionAsPlayer(Drop drop)
        {
            if (PlayerShip.Position.X >= drop.Position.X &&
                PlayerShip.Position.X <= drop.Position.X + PlayerShip.EntityWidth &&
                PlayerShip.Position.Y >= drop.Position.Y &&
                PlayerShip.Position.Y <= drop.Position.Y + PlayerShip.EntityWidth)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method responsible for moving enemy objects from Enemies list, and removing them from said list, based on enemy move direction and gameboard limits.
        /// </summary>
        /// <returns> Returns true if an enemy has reached bottom gameboard limit, otherwise returns false</returns>
        public void MoveEnemy()
        {
            if (Enemies == null) return;

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsAlive && Enemies[i].Position.X + Enemies[i].Velocity.X > GameSettings.GAMEBOARD_X_LIMIT && Enemies[i].TravelDirection == Direction.Right)
                {
                    Enemies[i].TravelDirection = Direction.Left;
                    Enemies[i].Position.Y = Enemies[i].Position.Y + Enemies[i].Velocity.Y;
                }
                if (Enemies[i].IsAlive && Enemies[i].Position.X - Enemies[i].Velocity.X <= GameSettings.GAMEBOARD_X_START && Enemies[i].TravelDirection == Direction.Left)
                {
                    Enemies[i].TravelDirection = Direction.Right;
                    Enemies[i].Position.Y = Enemies[i].Position.Y + Enemies[i].Velocity.Y;
                }
                if (Enemies[i].Position.Y + Enemies[i].Velocity.Y > GameSettings.GAMEBOARD_Y_LIMIT)
                {

                    PlayerShip.IsAlive = false;
                    return;
                }
            }

            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsAlive)
                {
                    Enemies[i].Move();

                }

            }
        }

        /// <summary>
        /// Method responsible for moving projectile objects from Projectiles list, and removing them from said list. Based on projectile move direction and gameboard limits
        /// </summary>
        public void MoveProjectile()
        {
            if (Projectiles == null) return;

            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i].Position.Y + Projectiles[i].Velocity.Y > GameSettings.GAMEBOARD_Y_LIMIT && Projectiles[i].TravelDirection == Direction.Down)
                {
                    Projectiles.RemoveAt(i);
                }
                else if (Projectiles[i].Position.Y - Projectiles[i].Velocity.Y <= 0 && Projectiles[i].TravelDirection == Direction.Up)
                {
                    Projectiles.RemoveAt(i);
                }
            }

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Move();
            }
        }

        /// <summary>
        /// Method responsible for checking projectile positions and comparing them with the enemy/player positions to see if there is a collision. 
        /// </summary>
        /// <returns>Returns true if the player was hit, otherwise false</returns>
        public void ProjectileCollisionDetection()
        {
            if (Projectiles == null) return;

            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i].TravelDirection == Direction.Up)
                {
                    for (int j = 0; j < Enemies.Count; j++)
                    {
                        bool yAxisCondition = false;

                        // Laser projectiles travel faster than others so a special condition is needed
                        if (Projectiles[i] is Laser)
                        {
                            // Special condition for Laser type
                            yAxisCondition = Projectiles[i].Position.Y <= Enemies[j].Position.Y + 3 ||
                                            (Projectiles[i].Position.Y - Projectiles[i].Velocity.Y < Enemies[j].Position.Y + 3 &&
                                             Projectiles[i].Position.Y >= Enemies[j].Position.Y);
                        }
                        else
                        {
                            // Regular condition for other projectiles
                            yAxisCondition = Projectiles[i].Position.Y <= Enemies[j].Position.Y + 3 &&
                                            Projectiles[i].Position.Y >= Enemies[j].Position.Y;
                        }
                        // If the projectile y and x position overlap with enemy x and y position > enemy is hit
                        if (yAxisCondition &&
                            Projectiles[i].Position.X >= Enemies[j].Position.X &&
                            Projectiles[i].Position.X <= Enemies[j].Position.X + Enemies[j].EntityWidth)
                        {
                            Enemies[j].Hit(Projectiles[i]);
                            Projectiles.RemoveAt(i);
                            break;
                        }
                    }
                }
                // if projectiles are moving down (meaning they were shot by enemies) than check x and y positions with player x and y positions
                else if (Projectiles[i].TravelDirection == Direction.Down)
                {
                    if (Projectiles[i].Position.Y < PlayerShip.Position.Y &&
                        Projectiles[i].Position.X >= PlayerShip.Position.X &&
                        Projectiles[i].Position.X <= PlayerShip.Position.X + PlayerShip.EntityWidth)
                    {
                        PlayerShip.Hit(Projectiles[i]);
                    }
                }

            }
            return;
        }

        /// <summary>
        /// Method responsible for checking projectile positions and comparing them with the gameboard limits, removing projectiles if they reached a certain boundery
        /// </summary>
        public void CheckProjectileBounderies()
        {
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (Projectiles[i].Position.Y + Projectiles[i].Velocity.Y > GameSettings.GAMEBOARD_Y_LIMIT && Projectiles[i].TravelDirection == Direction.Down)
                {
                    Projectiles.RemoveAt(i);
                }
                else if (Projectiles[i].Position.Y - Projectiles[i].Velocity.Y <= 0 && Projectiles[i].TravelDirection == Direction.Up)
                {
                    Projectiles.RemoveAt(i);
                }
            }
        }
        public void CheckDropBounderies()
        {
            for (int i = 0; i < Drops.Count; i++)
            {
                if (Drops[i].Position.Y + Drops[i].Velocity.Y >= PlayerShip.Position.Y && Drops[i].TravelDirection == Direction.Down)
                {
                    Drops.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Method responsible for making sure the player is within gameboard bounderies
        /// </summary>
        /// <param name="direction">The direction the player is trying to move in</param>
        /// <returns>Returns true player has reached a limit, otherwise false</returns>
        public bool CheckPlayerBounderies(Direction direction)
        {
            if (direction == Direction.Left && !(PlayerShip.Position.X > GameSettings.GAMEBOARD_X_START))
            {
                return true;
            }
            else if (direction == Direction.Right && !(PlayerShip.Position.X < GameSettings.GAMEBOARD_X_LIMIT))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  Method responsible translating key inputs into player actions.
        /// </summary>
        public void PlayerControls()
        {
            int maxProjectilesOnScreen = 0;
            ConsoleKey leftKey = ControlKeys[0];
            ConsoleKey rightKey = ControlKeys[1];

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                Direction direction = Direction.None;

                // using ifs since can't using the ConsoleKey list containing controls in a switch case
                if (pressedKey.Key == leftKey)
                {
                    direction = Direction.Left;
                }
                if (pressedKey.Key == rightKey)
                {
                    direction = Direction.Right;
                }
                if (pressedKey.Key == ConsoleKey.Spacebar)
                {

                    switch (PlayerShip.Weapon)
                    {
                        case WeaponType.Gun:
                            maxProjectilesOnScreen = 3;
                            break;
                        case WeaponType.LaserGun:
                            maxProjectilesOnScreen = 2;
                            break;
                        case WeaponType.MissileLauncher:
                            maxProjectilesOnScreen = 4;
                            break;
                    }
                    if (Projectiles.Count < maxProjectilesOnScreen)
                    {
                        Projectiles.Add(PlayerShip.Shoot());

                    }
                }

                if (!CheckPlayerBounderies(direction))
                {
                    PlayerShip.Move(direction);
                }
            }
        }
        /// <summary>
        /// resets hit animations for player and/or enemies
        /// </summary>
        public void ResetHitAnimations()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].IsHit = false;
            }
            PlayerShip.IsHit = false;
        }
        /// <summary>
        /// Removes dead enemies from the enemy list
        /// </summary>
        /// <returns></returns>
        public int RemoveDeadEnemy()
        {
            int points = 0;
            for (int i = 0; i < Enemies.Count; i++)
            {
                if (Enemies[i].IsAlive == false && Enemies[i].ExplosionLevel == 4)
                {
                    points += Enemies[i].Points;
                    if ( Enemies[i].Type == EnemyType.Melon)
                    {
                        Drop drop = Enemies[i].DropLoot();
                        AddDrop(drop);
                    }
                    Enemies.RemoveAt(i);
                }
            }
            return points;
        }
        public void AddDrop(Drop drop)
        {
            Drops.Add(drop);
        }
        /// <summary>
        /// updates explosion levels based on the current explosion level.
        /// </summary>
        public void UpdateExplosionLevel()
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                switch (Enemies[i].ExplosionLevel)
                {
                    case 1:
                        Enemies[i].ExplosionLevel = 2;
                        break;
                    case 2:
                        Enemies[i].ExplosionLevel = 3;
                        break;
                    case 3:
                        Enemies[i].ExplosionLevel = 4;
                        break;
                }
            }
        }
    }
}
