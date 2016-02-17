using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CirclesOfHell
{
    public static class PhysicsVars
    {
        public const float JUMPING_INITIAL_VELOCITY = 500.0f; //The initial vertical velocity of the jump (calculating from the current Y position)
        public const float JUMPING_ACCELERATION_UP = 350.0f; //Acceleration for jumping up
        public const float JUMPING_ACCELERATION_DOWN = 300.0f; //Acceleration for moving back to the ground
        public const int HORIZONTAL_SPEED_GROUND = 200; //Normal walking speed when on the ground
        public const int HORIZONTAL_SPEED_AIR = 200; //The initial speed for jumping

        public const float ENEMY_JUMPING_INITIAL_VELOCITY = 500.0f; //The initial vertical velocity of the jump (calculating from the current Y position)
        public const float ENEMY_JUMPING_ACCELERATION_UP = 350.0f; //Acceleration for jumping up
        public const float ENEMY_JUMPING_ACCELERATION_DOWN = 300.0f; //Acceleration for moving back to the ground
        public const int ENEMY_HORIZONTAL_SPEED_GROUND = 200; //Normal walking speed when on the ground
        public const int ENEMY_HORIZONTAL_SPEED_AIR = 170; //The initial speed for jumping
    }
}
