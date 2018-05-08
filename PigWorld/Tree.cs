using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;  // Allow Debug.Assert

namespace PigWorldNamespace {

    /// <summary>
    /// Trees have simple behaviour, much simpler than Pigs.  Trees don't move, for example.
    /// 
    /// Trees simply grow pig food. Every 10th time the Tree's DoSomething method is invoked, 
    /// the Tree will attempt to deposit an item of PigFood on to an adjacent cell, provided:
    /// 1.	There are no pigs on any cell adjacent to the tree.
    /// 2.	Trees do not grow over walls, so an item of pig food is not deposited 
    ///     in an adjacent cell separated from the tree by a wall.
    /// 3.	Pig food is not deposited on to an adjacent cell already containing pig food.
    /// 
    /// Actually, conditions (2) and (3) are already guaranteed by the constructor for PigFood.  
    /// So nothing needs to be done in this class to ensure conditions (2) and (3).  
    /// 
    /// Original author: Lizveth Robles 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public class Tree : Plant {

        private int doSomethingCallCount = 0;

        /// <summary>
        /// Constructs a new Tree.
        /// </summary>
        /// <param name="pigWorld"> the PigWorld in which to create the Tree. </param>
        public Tree(PigWorld pigWorld)
            : this(pigWorld, Position.any) {  // In this context, "this" is a call to the constructor below.
        }

        /// <summary>
        /// Constructs a new Tree at the specified position. If the desired position is
        /// already occupied by another Thing, then the Tree will be placed in a
        /// nearby Cell. If there are no nearby Cells that are free, then the Tree
        /// will not be added to the pigWorld.
        /// </summary>
        /// <param name="pigWorld"></param>
        /// <param name="position"></param>
        public Tree(PigWorld pigWorld, Position position) {
            AddToWorld(pigWorld, position);
        }

        /// <summary>
        /// This method is called every unit of time by the PigWorld to give the Tree a
        /// chance to do something.
        /// 
        /// Every 10th time this method is invoked, the Tree will attempt to deposit 
        /// an item of PigFood on to an adjacent cell, as long as
        /// there are no pigs on any cell adjacent to the tree.
        /// If there are any pigs next to the tree, then it just keeps waiting for another 10 times.
        /// </summary>
        protected override void DoSomething() {
            // This will detect any type of pig (both BoyPigs and GirlPigs).
            const int CALL_COUNT_LIMIT = 10;
            const int MAX_ALLOWABLE_DISTANCE = 1;

            Echo nearestPig = FindNearest(typeof(Pig));
            doSomethingCallCount += 1;
            if (doSomethingCallCount == CALL_COUNT_LIMIT) {
                doSomethingCallCount = 0;   // Reset Call Count
                if (nearestPig == null || nearestPig.distance > MAX_ALLOWABLE_DISTANCE) {
                    DropFood();
                }
            }
        }

        /// <summary>
        /// Constructs a new piece of PigFood. 
        /// The PigFood will be placed near to the Tree's position.
        /// </summary>
        public void DropFood() {
            new PigFood(this.PigWorld, Cell.Position);
        }
    }
}
