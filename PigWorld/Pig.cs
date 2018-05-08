using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;       // For Color
using System.Diagnostics;   // Allow Debug.Assert
using System.Media;         // For SoundPlayer

namespace PigWorldNamespace {

    /// <summary>
    /// A Pig is a type of Animal that eats PigFood, and mates with other Pigs. The
    /// behaviour of Pigs is implemented in the DoSomething method in this class.
    /// 
    /// There are two subclasses of Pig: GirlPig and BoyPig. GirlPigs have a unique
    /// method called TryToMakeBaby. While BoyPigs do not have this method, 
    /// BoyPigs invoke a GirlPig's TryToMakeBaby method, so both pigs must cooperate 
    /// to make a new baby. GirlPigs have the final say in making a baby and can 
    /// reject a request to make a baby.
    /// 
    /// Original author: Ryan Heise 
    /// Converted & modified by: Jim Reye
    /// </summary>
    public abstract class Pig : Animal {

        // The safe distance from a wolf. If a wolf is closer than this distance,
        // then the pig should run away.
        private const double SAFE_DISTANCE_FROM_WOLF = 2.5;

        // A reference to the mother of this pig, when the pig is created by the TryToMakeBaby method.
        // Otherwise, null.
        private Pig mother;

        // A reference to the father of this pig, when the pig is created by the TryToMakeBaby method.
        // Otherwise, null.
        private Pig father;  

        private Color color;  // The colour of this pig.
        public Color Color { get { return color; } }

        // The pieces of rope that have been dropped (if any). As new pieces
        // of rope are dropped, they are added to this list. I.e. the newest piece is
        // at the end of the List.
        // If this pig currently has no pieces of rope on the ground, then 
        // this List is empty -- i.e. its Count is zero -- but ropePieces is never null.
        public List<RopePiece> ropePieces = new List<RopePiece>();

        // When a pig would otherwise be looking for food, but there is no food in
        // the pigWorld at the moment, the pig will move in the wanderDirection. 
        private Direction wanderDirection = Direction.GetRandomDirection();

        static SoundPlayer gruntAudio = Util.LoadSound(@"Resources\pig_grunt.wav");
        static SoundPlayer shriekAudio = Util.LoadSound(@"Resources\pig_shriek.wav");

        /// <summary>
        /// Constructs a new Pig at, or near, the specified position. 
        /// If the desired position is already occupied by another LifeForm, 
        /// then the pig will be placed in a nearby Cell. 
        /// If no nearby Cells are free, then the pig will not be added to the pigWorld.
        /// 
        /// This constructor is protected since only Pig subclasses should be
        /// able to create a pig.
        /// </summary>
        /// <param name="pigWorld"> the pigWorld that this pig is entering </param>
        /// <param name="position"> the preferred position for the new pig </param>
        /// <param name="color"> the pig's colour (blue or pink) </param>
        /// <param name="mother"> the pig's mother (may be null)</param>
        /// <param name="father"> the pig's father (may be null)</param>
        protected Pig(PigWorld pigWorld, Position position, Color color, GirlPig mother, BoyPig father) {
            this.color = color;  // Must be set before AddToWorld is called.
            this.mother = mother;
            this.father = father;

            AddToWorld(pigWorld, position);  // Must be called before any sounds are played.
        }

        /// <summary>
        /// This method is called every unit of time by the PigWorld to give the Pig a
        /// chance to do something.
        /// </summary>
        protected override void DoSomething() {

            // Do not modify this method!

            if (IsTired()) {  // After producing offspring.
                Rest();
            } else if (IsWolfNearby()) {
                RunFromWolf();
            } else if (IsHungry) {
                LookForFoodUsingRope();
            } else if (IsInTheMoodForLove()) {
                LookForPig();
            } else {
                // Do nothing.

                // Do not modify, delete, or move the debugAnimalAction line below, 
                // or the Debug Info will not show correctly in the GUI, and that could be confusing.
                debugAnimalAction = "Do nothing";
            }
        }

        /// <summary>
        /// Look for the nearest pig of the opposite gender.
        /// This is an "abstract" method which means that the actual code
        /// is implemented in the two derived classes, BoyPig and GirlPig.
        /// </summary>
        protected abstract void LookForPig();

        /// <summary>
        /// Delete any ropePieces currently being trailed by (i.e. owned by) this pig.
        /// </summary>
        public void ClearRope() {
            foreach (RopePiece ropePiece in ropePieces) {
               ropePiece.Delete();
            }
            ropePieces.Clear();
        }

        /// <summary>
        /// When a pig is deleted, its rope must be deleted as well.
        /// </summary>
        public override void Delete() {
            base.Delete();
            ClearRope();
        }

        /// <summary>
        /// First the pig looks to see if there is any PigFood in its current Cell.
        /// If there is, the pig picks it up, eats it, and does nothing more. I.e. the pig stays where it is.
        /// Otherwise, the pig looks for the nearest PigFood, using the FindNearest method. 
        /// If there isn't any food anywhere in the pigWorld, the pig does nothing.
        /// If there is food, then the pig (tries to) move one square in the direction 
        /// of that food. If the move fails (e.g. because there's a wall in the way, 
        /// or any other reason), then the pig simply remains where it is.
        /// </summary>
        public void LookForFood() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "LookForFood";

            bool pigFoodInCurrentCell = Cell.Exists(typeof(PigFood));
            if (pigFoodInCurrentCell) {
                PigFood food = (PigFood)Cell.PickUp(typeof(PigFood));
                Eat(food);
            } else {
                Echo nearestPigFood = FindNearest(typeof(PigFood));
                if (nearestPigFood != null) {
                    Cell targetCell = Cell.GetAdjacentCell(nearestPigFood.direction);
                    if (targetCell != null) {
                        LifeForm adjacentLifeForm = targetCell.LifeFormOccupant;
                        if (adjacentLifeForm == null) {
                            Move(targetCell);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rope-based look-for-food method
        /// </summary>
        private void LookForFoodUsingRope() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "LookForFood";
            bool pigFoodInCurrentCell = Cell.Exists(typeof(PigFood));
            //If there is food in the current cell, eat it!
            if (pigFoodInCurrentCell) {
                PigFood food = (PigFood)Cell.PickUp(typeof(PigFood));
                Eat(food);
                ClearRope();
            } else {
                Echo nearestPigFood = FindNearest(typeof(PigFood));
                if (nearestPigFood == null) { 
                    ClearRope(); 
                    WanderAround();
                } else if (nearestPigFood != null) {
                    Cell targetCell = Cell.GetAdjacentCell(nearestPigFood.direction);
                    if (CanMove(nearestPigFood.direction) && targetCell.Exists(typeof(PigFood))) {
                            DropRope();
                            Move(targetCell);
                    } else if (CanMove(nearestPigFood.direction) && GetMyRopePiece(targetCell) == null) {
                            DropRope();
                            Move(targetCell);
                    } else if (!CanMove(nearestPigFood.direction) || GetMyRopePiece(targetCell) != null) {
                            ExamineSurroundingCells();
                        }
                    }
                }
            }

        /// <summary>
        /// If a pig can not move directly towards it's food, this method examines all surrounding cells
        /// and determines which direction is the best to move in. In the following order:
        /// 1. Move towards an empty cell in the closest direction to the closest food.
        /// 2. If no cells are empty move towards the cell with the oldest rope on it.
        /// 3. Unable to move to any nearby cell. Do nothing.
        /// pre: Pig is unable to move towards the food, or there is rope in the adjacent cell.
        /// post: Pig will move to a valid location, or not at all. Depending on the above circumstances.
        /// </summary>
        private void ExamineSurroundingCells() {

            const double DEFAULT_LOWEST_DIRECTION_DIFFERENCE = 360.00;
            Echo nearestPigFood = FindNearest(typeof(PigFood));
            double lowestDirectionDifference = 360.00;             //default value 360 is a placeholder
            Direction closestDirectionToFood = Direction.NORTH; //default value (Direction.NORTH) is a placeholder
            int oldestNearbyRope = -1;                          //default value -1 is a placeholder
            Direction oldestRopeDirection = Direction.NORTH;    //default value (Direction.NORTH) is a placeholder

            for (int i = 0; i < Direction.NUMBER_POSSIBLE; i++) {
                Direction direction = Direction.GetAdjacentCellDirection(i);
                Cell adjacentCell = Cell.GetAdjacentCell(direction);

                if (adjacentCell != null && CanMove(direction)) {
                    RopePiece ropeInCell = GetMyRopePiece(adjacentCell);
                    if (ropeInCell == null) {
                        double adjacentCellDirection = direction.Degrees;
                        double foodDirection = nearestPigFood.direction.Degrees;
                        double differenceInDegrees = Math.Abs(foodDirection - adjacentCellDirection);

                        // The two lines below solve the '359 degrees is right next to 0 degrees' issue using basic maths.
                        if (differenceInDegrees > 180) {             // 180 being 180 degrees (DO NOT change)
                            differenceInDegrees = 360 - differenceInDegrees;  // 360 being 360 degrees (DO NOT change)
                        }
                        if (lowestDirectionDifference > differenceInDegrees) {
                            lowestDirectionDifference = differenceInDegrees;
                            closestDirectionToFood = direction;
                        }
                    } else if (ropeInCell != null) {
                        Pig ropeOwner = ropeInCell.OwnerPig;
                        int ropeAge = ropeInCell.GetDistanceFromOwner();
                        if (oldestNearbyRope < ropeAge) {
                            oldestNearbyRope = ropeAge;
                            oldestRopeDirection = direction;
                        }
                    }
                }
            }
            if (lowestDirectionDifference != DEFAULT_LOWEST_DIRECTION_DIFFERENCE) { //If it has changed from default value, a valid direction has been found.
                DropRope();
                Move(closestDirectionToFood);
            } else {
                DropRope();
                Move(oldestRopeDirection);
            }
        }

        /// <summary>
        /// When a pig would otherwise be looking for food, but there is no food in
        /// the pigWorld at the moment, the pig will move in the wanderDirection. 
        /// If a pig is wandering and it bumps into an obstacle, it will randomly
        /// assign a new wander direction and then continue wandering.
        /// </summary>
        protected void WanderAround() {
            int count = 0;
            while (!CanMove(wanderDirection) && count < 20) {
                wanderDirection = Direction.GetRandomDirection();
                count += 1;
            }

            Move(wanderDirection);
        }

        /// <summary>
        /// Move in ANY random direction that is possible, i.e. not obstructed by a wall, etc.
        /// Because the pig is panicking, it might move towards a threat, such as a Wolf.
        /// </summary>
        protected void Panic() {
            int count = 0;
            Direction direction;

            do {
                direction = Direction.GetRandomDirection();
                count += 1;
            } while (!CanMove(direction) && count < 20);

            Move(direction);
        }

        /// <summary>
        /// This method makes the pig drop rope onto the current cell.
        /// 
        /// Note that, when a rope is being used, the pig must 
        /// call DropRope BEFORE the pig moves from its current cell, 
        /// because ropes are used to keep a history of where a pig has been,
        /// which excludes its current location (unless it is revisiting a cell).
        /// </summary>
        void DropRope() {
            // Create the rope on the current cell
            Position position = Cell.Position;
            RopePiece newRopePiece = new RopePiece(PigWorld, this, position);

            // Add it to the previous rope pieces.
            ropePieces.Add(newRopePiece);
        }

        /// <summary>
        /// Accesses a piece of rope on the specified cell owned by this pig.
        /// If there is more than one piece of rope on that cell owned by this pig,
        /// then the most recently placed will be returned.
        /// If no ropes owned by this pig are on that cell, null is returned.
        /// </summary>
        /// <returns> the requested rope from that Cell. </returns>
        public RopePiece GetMyRopePiece(Cell cell) {
            List<RopePiece> ropes = cell.InspectAll<RopePiece>();
            RopePiece bestRopeSoFar = null;

            foreach (RopePiece ropePiece in ropes) {

                if (ropePiece.OwnerPig == this) {
                    if (bestRopeSoFar == null) {
                        bestRopeSoFar = ropePiece;
                    }
                    else {
                        if (ropePiece.GetDistanceFromOwner() < bestRopeSoFar.GetDistanceFromOwner())
                            bestRopeSoFar = ropePiece;
                    }
                }
            }
            return bestRopeSoFar;
        }

        /// <summary>
        /// Determines if there is a wolf nearby.
        /// </summary>
        /// <returns> true if there is a wolf nearby, or false otherwise. </returns>
        public bool IsWolfNearby() {
            Echo echo = FindNearest(typeof(Wolf));  
        
            return (echo != null && echo.distance < SAFE_DISTANCE_FROM_WOLF);
        }
    
        /// <summary>
        /// This method will make the pig try to run in the opposite direction of the nearest wolf.
        /// </summary>
        public void RunFromWolf() {
            // Do not modify, delete, or move the debugAnimalAction line below, 
            // or the Debug Info will not show correctly in the GUI, and that could be confusing.
            debugAnimalAction = "RunFromWolf";

            Echo echo = FindNearest(typeof(Wolf));  

            if (echo != null) {
                Direction oppositeDirection = echo.direction.GetOppositeDirection();
                bool pigMoved = Move(oppositeDirection);
                if (!pigMoved) {
                    Panic();
                }
            }
        }

        /// <summary>
        /// Tests to see if this object is indeed a child of possibleParent.
        /// </summary>
        /// <param name="possibleParent"> The Pig who's past is in question, or a null
        /// reference </param>
        /// <returns> true if this pig is a child of possibleParent </returns>
        public bool IsParent(Pig possibleParent) {
            if (this.mother == possibleParent) 
                return true;
            if (this.father == possibleParent) 
                return true;
            return false;
        }
    
        /// <summary>
        /// Tests if this and the other pig are full brothers/sisters.  That is,
        /// tests that both pigs have the same two parents.
        /// </summary>
        /// <param name="possibleSibling"> A Pig ... or perhaps a null reference </param>
        /// <returns> true if this pig and the other pig have the same two parents.  </returns>
        protected bool IsSibling(Pig pig) {
            if ( (pig.mother == null) || (pig.father == null) )
                return false;
            if ( (this.mother == pig.mother) && (this.father == pig.father) )
                return true;
            return false;
        }

        /// <summary>
        /// Plays a grunting sound.
        /// 
        /// Note that if your program calls the Grunt or Shriek methods too rapidly,
        /// you'll hear a stuttering sound instead of the proper sounds. 
        /// This is because each call to these methods terminates any sound that is already being played. 
        /// I.e. calling too rapidly means that only the first part of each sound will be played, 
        /// resulting in a sound that is like stuttering.  Unfortunately, there is no easy way to 
        /// overcome this.
        /// </summary>
        public void Grunt() {
            if (PigWorld.EnableRealAudio) {
                gruntAudio.Play();
            }
        }

        /// <summary>
        /// Plays a shrieking sound.
        /// 
        /// See comments for the Grunt method with regards to stuttering sounds 
        /// if this method is called too rapidly.
        /// </summary>
        public void Shriek() {
            if (PigWorld.EnableRealAudio) {
                shriekAudio.Play();
            }
        }
    }
}
