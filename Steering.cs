using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace AIcore
{
    class Seek : KinematicMovement
    {
        // Holds the maximum acceleration of the character
        float maxAcceleration;

        // Returns the desired steering output
        public SteeringOutput getSteering()
        {
            // create the structure to hold our output
            SteeringOutput steering = new SteeringOutput();

            // get the direction to the target
            steering.linear = target.position - character.position;
    
            // give full aceleration is along this direction
            steering.linear.Normalize();
            steering.linear *= maxAcceleration;

            // output the steering
            steering.angular = 0;
            return steering;
        }
    }

    class Arive : KinematicMovement
    {
        // Holds the maximum acceleration and speed at the target
        float maxAcceleration;
        float maxSpeed;

        // Holds the radius for arriving at the target
        float targetRadius;

        // Holds the radius for slowing the movement down
        float slowRadius;

        // Holds the time over which to achive target speed
        float timeToTarget = 0.1f;

        public override SteeringOutput getSteering()
        {
            // Create the structure to hold our output
            SteeringOutput steering = new SteeringOutput();

            // Get the direction to the target
            Vector2 direction = target.position - character.position;
            float distance = direction.Length();

            // Check if we are there return no steering
            if (distance < targetRadius)
                return null;

            // If we are outside the slowRadius,the go max speed
            if (distance > slowRadius)
            {
                targetSpeed = maxSpeed;
            }
            else
            {
                targetSpeed = maxSpeed * distance / slowRadius;
            }

            // The target velocity combines speed and direction
            Vector2 targetVelocity = direction;
            targetVelocity.Normalize();
            targetVelocity *= targetSpeed;

            // Acceleration tries to get to target velocity
            steering.linear = targetVelocity - character.velocity;
            steering.linear /= timeToTarget;

            // Check if the acceleration is too fast
            if (steering.linear.Length() > maxAcceleration)
            {
                steering.linear.Normalize();
                steering.linear *= maxAcceleration;
            }

            // Output the steering
            steering.angular = 0.f;
            return steering;
        }

    }

    class Align : KinematicMovement
    {
        // Holds the max angular acceleration and rotation
        // of the character
        float maxAngularAcceleration;
        float maxRotation;

        // Holds the radius for arriving at the target
        float targetRadius;

        // Holds the radius for slowing down
        float slowRadius;

        // Holds the time over which to achive target speed
        float timeToTarget = 0.1f;

        public override SteeringOutput getSteering()
        {
            base.getSteering();
            // Create the structure to hold the output
            SteeringOutput steering = new SteeringOutput();

            // Get the naive direction to target
            float rotation = target.orientation - character.orientation;

            // Map the result to the (-pi,pi) interval
            rotation = mapToRange(rotation);
            float rotationAngle = Math.Abs(rotationDirection);
 
            // Check if we are there, return no steering
            if (rotationAngle < targetRadius)
                return null;

            // If we are outside the slowRadius, then use
            // maximum rotation
            if (rotationAngle > slowRadius)
            {
                targetRotation = maxRotation;
            }
            else
            {
                targetRotation = maxRotation * rotationAngle / slowRadius;
            }

            // The final target rotation combines
            // speed (already in the variable) and direction
            targetRotation *= rotation / rotationAngle;
            
            // Acceleration tries to get to the target rotation
            steering.angular = targetRotation - character.rotation;
            steering.angular /= timeToTarget;

            // Check if the acceleration is too great
            float angularAcceleration = Math.Abs(steering.angular);
            if (angularAcceleration > maxAngularAcceleration)
            {
                steering.angular /= angularAcceleration;
                steering.angular *= maxAngularAcceleration;
            }

            // Output the steering
            steering.linear = new Vector2(0.f,0.f);
            return steering;
        }
    }

    /// <summary>
    /// This class tries to match the velocity of 
    /// the target object.
    /// </summary>
    class VelocityMatch : KinematicMovement
    {
    }
}
