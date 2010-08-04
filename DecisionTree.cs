using System;
using System.Collections.Generic;
using System.Text;

namespace AIcore
{
    /// <summary>
    /// DecisionTreeNode is base class for all classes for decision tree.
    /// </summary>
    class DecisionTreeNode
    {
        public virtual DecisionTreeNode makeDecision() {}
    }

    /// <summary>
    /// DecisionTreeAction is last tree node in decision tree. It
    /// simply returns itself as the result of the next decision.
    /// </summary>
    class DecisionTreeAction : DecisionTreeNode
    {
        public virtual DecisionTreeNode makeDecision()
        {
            return this;
        }
    }
    /// <summary>
    /// Choose the decision branch based on input boolean.
    /// </summary>
    class DecisionTree
    {
        public DecisionTreeNode trueBranch;
        public DecisionTreeNode falseBranch;

        public DecisionTreeNode makeDecision()
        {
            // Choose a branch based on the getBranch method
            if (getBranch())
            {
                // make sure its not null before recursing.
                if (trueBranch == null)
                    return null;
                else
                    return trueBranch.makeDecision();
            }
            else
            {
                if (falseBranch == null)
                    return null;
                else
                    return falseBranch.makeDecision();
            }
        }
    }

    class RandomDecision : DecisionTree
    {
        public bool lastDecision;
        public int lastDecisionFrame;

        // Creates a new random decision
        public RandomDecision()
        {
            lastDecisionFrame = 0;
            lastDecision = false;
        }

        // Works out which branch to follow.
        public virtual bool getBranch()
        {
            int thisFrame = TimingData;
            // If we didn't get here last time,
            // then things may chnage
            if (thisFrame > lastDecisionFrame + 1)
            {
                lastDecision = Random;
            }

            // in any case, store the frame number
            lastDecisionFrame = thisFrame;

            // and return the stored value
            return lastDecision;
        }
    }

    class RandomDecisionWithTimeOut : RandomDecision
    {
        public int firstDecisionFrame;
        int timeOutDuration;

        public RandomDecisionWithTimeOut()
        {
        }

        public virtual bool getBranch()
        {
            thisFrame = TimingData;
            // Check if the stored decision is either too old,
            // or if we timed out.
            if (thisFrame > lastDecisionFrame + 1 ||
                thisFrame > firstDecisionFrame + timeOutDuration)
            {
                // make a new decision
                lastDecision = Random();
                // and record that it was just made
                firstDecisionFrame = thisFrame;
            }
            // update the frame number
            lastDecisionFrame = thisFrame;

            // return the stored value
            return lastDecision;

        }
    }
}

