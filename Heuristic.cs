using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.DirectX;
using System.Text;

namespace AIcore
{
    class Heuristic
    {
        public static float Estimate(Node start)
        {
            if (start != null)
            {
                Vector3 diff = goal.location.Position - start.location.Position;
                return diff.Length;
            }
            else
            {
                return 0.0f;
            }
        }
    }
}
