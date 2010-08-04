using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace AIcore
{
    /// <summary>
    /// Represent location of a object.
    /// A location contains position of object and orientation. 
    /// Position is a vector and orientation is planar rotation about
    /// y axis.
    /// </summary>
    struct Location
    {
        // the position in 3 space
        Vector3 _position;
        // the orientation, as a euler angle in radians around the
        // positive y axis (i.e. up) from the positive z axis.
        double _orientation;

        /// <summary>
        /// creates a new location with a 0 position and orientation. 
        /// </summary>
        public Location(float orin)
        {
            _orientation = orin; 
        }
        
     
        /// <summary>
        ///creates a location at the given position with no rotation. 
        /// </summary>
        /// <param name="position">vector define position</param>
        public Location(Vector3 position)
        {
            _position = position;
            _orientation = 0.0f;
        }

        /// <summary>
        /// creates a location with the given position and orientation 
        /// </summary>
        /// <param name="pos">position of location</param>
        /// <param name="orin">orientation</param>
        public Location(Vector3 pos, double orin)
        {
            _position = pos;
            _orientation = orin;
        }

        /// <summary>
        /// Creates a location with the position vector given as
        /// components and the given orientation.
        /// </summary>
        /// <param name="x">Position in x</param>
        /// <param name="y">Position in y</param>
        /// <param name="z">Position in z</param>
        /// <param name="orientation">orientation</param>
        public Location(double x, double y, double z, double orientation)
        {
            _position = new Vector3(x,y,z);
            _orientation = orientation;
        }

        //public static Location  operator = (Location other)
        //{
        //    _position = other._position;
        //    _orientation = other._orientation;
        //    return this;
        //}

        public static bool operator == (Location my, Location other) 
        {
            return my._position == other._position &&
                my._orientation == other._orientation;
        }

        public static bool operator != (Location my, Location other)
        {
            return my._position != other._position &&
                my._orientation != other._position;
        }

        /// <summary>
        /// Clear the orientation and position.
        /// </summary>
        public void clear()
        {
            _orientation = 0.0F;
            _position.X = 0.0F;
            _position.Y = 0.0F;
            _position.Z = 0.0F;
        }
        
    	public double Orientation
	    {
		    get { return _orientation;}
		    set { _orientation = value;}
	    }
   
        public Vector3 Position
        {
            get { return _position;}
            set { _position = value;}
        }
	}

    /// <summary>
    /// Kinematic class implements kinematic object,
    /// it includes static object which has constant velocity
    /// and Kinematic object which can accelerate.
    /// </summary>
    public class KinematicAI
    {
        /// <summary>
        /// This structure contains data for static
        /// objects
        /// </summary>
        public struct Static
        {
            public Vector2 position;
            public float orientation;
        }
        /// <summary>
        /// Kinematic object has velocity and possible angular
        /// velocity.
        /// </summary>
        public struct Kinematic
        {
            
            public Vector2 position;  // position of kinematic object
            public float orientation; // orientation of object angle to x
            public Vector2 velocity;  // velocity of the object
            public float rotation;    // gives angular velocity

            /// <summary>
            /// This makes simple update of object with steering between
            /// time interval
            /// </summary>
            /// <param name="steering">steering object contains acceleration data</param>
            /// <param name="time">time</param>
            public void update(SteeringOutput steering, float time)
            {
                // Update position and orientation 
                position += velocity * time + 0.5f * steering.linear * time * time;
                orientation += rotation * time + 0.5f * steering.angular * time * time;

                // and the velocity and rotation
                velocity += steering.linear * time;
                orientation += steering.angular * time;
            }
            /// <summary>
            /// This uses Newton-Euler update, which works fine
            /// for small time intervals.
            /// </summary>
            /// <param name="steering"></param>
            /// <param name="time"></param>
            public void updateEuler(SteeringOutput steering, float time)
            {
                // Update the position and orientation
                position += velocity * time;
                orientation += rotation * time;

                // and the velocity and rotation
                velocity += steering.linear * time;
                rotation += steering.angular * time;
            }
        }

    }

    public struct SteeringOutput
    {
        public Vector2 linear;    // correspond acceleration of object
        public float angular;     // correspond to angl. acceleration.

        public static bool operator ==(SteeringOutput left, SteeringOutput right)
        {
            return left.linear == right.linear && left.angular == left.angular;
        }

        public static bool operator !=(SteeringOutput left, SteeringOutput right)
        {
            return left.linear != right.linear || left.angular != left.angular;
        }
    }
    
    public class KinematicMovement
    {
        // holds data for character(can be unit or any car) and target
        Location character;
        Location target;

        // holds the maximum speed the character can travel
        float maxSpeed;

        public virtual SteeringOutput getSteering() {}
    }

    public class KinematicSeek : KinematicMovement
    {
        
        public SteeringOutput getSteering()
        {
            // create the structure for output
            SteeringOutput steering = new SteeringOutput();

            // get the direction to the target
            steering.linear = target.position - character.position;

            // the velocity is along this direction, at full speed
            steering.linear.Normalize();
            steering.linear *= maxSpeed;

            // face in the direction we want to move
            character.orientation = getNewOrientation(character.orientation,
                steering.linear);

            // output the steering
            steering.angular = 0;
            return steering;
        }
    }

    public class KinematicArrive : KinematicMovement
    {
        // holds satisfaction radius
        float radius;
        // holds the time to target constant
        float timeToTarget = 0.25f;
        // implements steering so that character arrives
        // at prescribed radius to the target.
        public SteeringOutput getSteering()
        {
            // create structure for output
            SteeringOutput steering = new SteeringOutput();

            // get the direction to target
            steering.linear = target.position - character.position;

            // check if we're within radius
            if (steering.linear.Length() < radius)
            {
                return null;
            }

            // we need to move to our target, we'd like to
            // get there in timeToTarget seconds
            steering.linear /= timeToTarget;

            // if this is too fast clip it to the max speed
            if (steering.linear.Length() > maxSpeed)
            {
                steering.linear.Normalize();
                steering.linear *= maxSpeed;
            }

            // face in the direction we want to move
            character.orientation = getNewOrientation(character.orientation,
                steering.linear);

            // output steering
            steering.angular = 0.f;
            return steering;
        }
    }

    public class KinematicWander : KinematicMovement
    {
        // Holds the maximum rotation speed we'd like, probably 
        // should be smaller than the maximum possible, to allow
        // a leisurely change in direction
        float maxRotation;

        public SteeringOutput getSteering()
        {
            // create the structure for output
            SteeringOutput steering = new SteeringOutput();

            // get velocity from the vector form of the orientation
            steering.linear = maxSpeed * character.orientation.asVector();

            // change our orientation randomly
            steering.angular = randomBinomial() * maxRotation;

            // output steering
            return steering;
        }

        /// <summary>
        /// It creates the sequence of random numbers
        /// between -1 and 1.
        /// </summary>
        /// <returns>Returns random number between -1 and 1</returns>
        double randomBinomial()
        {
            Random rand = new Random();
            return rand.NextDouble() - rand.NextDouble();
        }
    }
}
