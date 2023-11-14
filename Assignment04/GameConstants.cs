﻿
namespace Assignment04
{
    public class GameConstants
    {
        //camera constants
        public const float CameraHeight = 25000.0f;
        public const float PlayfieldSizeX = 16000f;
        public const float PlayfieldSizeY = 12500f;
        //asteroid constants
        public const int NumAsteroids = 10;
        public const float AsteroidMinSpeed = 100.0f;
        public const float AsteroidMaxSpeed = 300.0f;
        public const float AsteroidSpeedAdjustment = 5.0f;
        public const float AsteroidBoundingSphereScale = 0.95f;  //95% size
        public const float ShipBoundingSphereScale = 0.5f;  //50% size
        public const int NumBullets = 30;
        public const float BulletSpeedAdjustment = 100.0f;
        public const float Playerspeed = 2500f;
        public const int KillBonus = 1;
    }
}