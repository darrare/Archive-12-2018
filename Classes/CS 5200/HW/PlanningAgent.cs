using Assets.Scripts.EnumTypes;
using Assets.Scripts.GameElements;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
	// Planning Agent is the over-head planner that decided where
	// individual units go and what tasks they perform.  Low-level
	// AI is handled by other classes (like pathfinding).
	public class PlanningAgent : Agent
	{
		#region Private Methods

		// Handy short-cuts for pulling all of the relevant data that you
		// might use for each decision.  Feel free to add your own.
		public int enemyAgentNbr { get; set; }
		public int mainMineNbr { get; set; }
		public int mainBaseNbr { get; set; }
		public bool lastFighterWasSoldier { get; set; }


		public List<int> mines { get; set; }

		public List<int> myWorkers { get; set; }
		public List<int> mySoldiers { get; set; }
		public List<int> myArchers { get; set; }
		public List<int> myBases { get; set; }
		public List<int> myBarracks { get; set; }
		public List<int> myRefineries { get; set; }

		public List<int> enemyWorkers { get; set; }
		public List<int> enemySoldiers { get; set; }
		public List<int> enemyArchers { get; set; }
		public List<int> enemyBases { get; set; }
		public List<int> enemyBarracks { get; set; }
		public List<int> enemyRefineries { get; set; }

		public List<Vector3Int> buildPositions { get; set; }

		/// <summary>
		/// Finds all of the possible build locations for a specific UnitType.
		/// Currently, all structures are 3x3, so these positions can be reused
		/// for all structures (Base, Barracks, Refinery)
		/// Run this once at the beginning of the game and have a list of
		/// locations that you can use to reduce later computation.  When you
		/// need a location for a build-site, simply pull one off of this list,
		/// determine if it is still buildable, determine if you want to use it
		/// (perhaps it is too far away or too close or not close enough to a mine),
		/// and then simply remove it from the list and build on it!
		/// This method is called from the Awake() method to run only once at the
		/// beginning of the game.
		/// </summary>
		/// <param name="unitType">the type of unit you want to build</param>
		public void FindProspectiveBuildPositions(UnitType unitType)
		{
			// For the entire map
			for (int i = 0; i < GameManager.Instance.MapSize.x; ++i)
			{
				for (int j = 0; j < GameManager.Instance.MapSize.y; ++j)
				{
					// Construct a new point near gridPosition
					Vector3Int testGridPosition = new Vector3Int(i, j, 0);

					// Test if that position can be used to build the unit
					if (Utility.IsValidGridLocation(testGridPosition)
						&& GameManager.Instance.IsBoundedAreaBuildable(unitType, testGridPosition))
					{
						// If this position is buildable, add it to the list
						buildPositions.Add(testGridPosition);
					}
				}
			}
		}

		/// <summary>
		/// Assuming you run the FindProspectiveBuildPositions, this method takes that
		/// list and finds the closest build position to the gridPosition.  This can be
		/// used to find a position close to a mine, close to the enemy base, close to
		/// your barracks, close to your base, close to a troop, etc.
		/// </summary>
		/// <param name="gridPosition">position that you want to build near</param>
		/// <param name="unitType">type of unit you want to build</param>
		/// <returns></returns>
		public Vector3Int FindClosestBuildPosition(Vector3Int gridPosition, UnitType unitType)
		{
			// Variables to store the closest position as we find it
			float minDist = float.MaxValue;
			Vector3Int minBuildPosition = gridPosition;

			// For all the possible build postions that we already found
			foreach (Vector3Int buildPosition in buildPositions)
			{
				// if the distance to that build position is closer than any other seen so far
				if (Vector3.Distance(gridPosition, buildPosition) < minDist && GameManager.Instance.IsBoundedAreaBuildable(unitType, buildPosition))
				{
					// Store this build position as the closest seen so far
					minDist = Vector3.Distance(gridPosition, buildPosition);
					minBuildPosition = buildPosition;
				}
			}

			// Return the closest build position
			return minBuildPosition;
		}

		/// <summary>
		/// Find the closest unit to the gridPosition out of a list of units.
		/// Use this method to find the enemy soldier closest to your archer,
		/// or the closest base to a mine, or the closest mine to a base, etc.
		/// </summary>
		/// <param name="gridPosition">position of an agent or base</param>
		public int FindClosestUnit(Vector3Int gridPosition, List<int> unitNbrs)
		{
			// Variables to store the closest unit as we find it
			int closestUnitNbr = -1;
			float closestUnitDist = float.MaxValue;

			// Iterate through all of the units
			foreach (int unitNbr in unitNbrs)
			{
				Unit unit = GameManager.Instance.GetUnit(unitNbr);
				float unitDist = Vector3.Distance(unit.GridPosition, gridPosition);

				// If this object is closer than any seen so far, save it
				if (!(unitDist < closestUnitDist)) continue;
				closestUnitDist = unitDist;
				closestUnitNbr = unitNbr;
			}

			// Return the closest unit's number
			return closestUnitNbr;
		}


		#endregion

		#region Public Methods

		/// <summary>
		/// Called when the object is instantiated in the scene 
		/// </summary>
		public void Awake()
		{
			lastFighterWasSoldier = false;

			buildPositions = new List<Vector3Int>();

			FindProspectiveBuildPositions(UnitType.BASE);



			// Set the main mine and base to "non-existant"
			mainMineNbr = -1;
			mainBaseNbr = -1;

			// Initialize all of the unit lists
			mines = new List<int>();

			myWorkers = new List<int>();
			mySoldiers = new List<int>();
			myArchers = new List<int>();
			myBases = new List<int>();
			myBarracks = new List<int>();
			myRefineries = new List<int>();

			enemyWorkers = new List<int>();
			enemySoldiers = new List<int>();
			enemyArchers = new List<int>();
			enemyBases = new List<int>();
			enemyBarracks = new List<int>();
			enemyRefineries = new List<int>();
		}

		/// <summary>
		/// Updates the game state for the Agent - called once per frame for GameManager
		/// Pulls all of the agents from the game and identifies who they belong to
		/// </summary>
		public void UpdateGameState()
		{
			// Update the common resources
			mines = GameManager.Instance.GetUnitNbrsOfType(UnitType.MINE);

			// Update all of my unitNbrs
			myWorkers = GameManager.Instance.GetUnitNbrsOfType(UnitType.WORKER, AgentNbr);
			mySoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, AgentNbr);
			myArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, AgentNbr);
			myBarracks = GameManager.Instance.GetUnitNbrsOfType(UnitType.BARRACKS, AgentNbr);
			myBases = GameManager.Instance.GetUnitNbrsOfType(UnitType.BASE, AgentNbr);
			myRefineries = GameManager.Instance.GetUnitNbrsOfType(UnitType.REFINERY, AgentNbr);

			// Update the enemy agents & unitNbrs
			List<int> enemyAgentNbrs = GameManager.Instance.GetEnemyAgentNbrs(AgentNbr);
			if (enemyAgentNbrs.Any())
			{
				enemyAgentNbr = enemyAgentNbrs[0];
				enemyWorkers = GameManager.Instance.GetUnitNbrsOfType(UnitType.WORKER, enemyAgentNbr);
				enemySoldiers = GameManager.Instance.GetUnitNbrsOfType(UnitType.SOLDIER, enemyAgentNbr);
				enemyArchers = GameManager.Instance.GetUnitNbrsOfType(UnitType.ARCHER, enemyAgentNbr);
				enemyBarracks = GameManager.Instance.GetUnitNbrsOfType(UnitType.BARRACKS, enemyAgentNbr);
				enemyBases = GameManager.Instance.GetUnitNbrsOfType(UnitType.BASE, enemyAgentNbr);
				enemyRefineries = GameManager.Instance.GetUnitNbrsOfType(UnitType.REFINERY, enemyAgentNbr);
			}
		}

		//public float getSoldierValue()
		//{
		//	if(Gold < Constants.COST[UnitType.SOLDIER])
		//	{
		//		return 0f;
		//	}
		//	float value = mySoldiers.Count / (enemySoldiers.Count + 1);
		//	print("value: " + value);
		//	return Mathf.Clamp(value, 0.1f, 1f);
		//}

		//public float getWorkerValue()
		//{
		//	if (Gold < Constants.COST[UnitType.WORKER])
		//		return 0f;
		//	float value = (1 - myWorkers.Count) / (2 * enemyWorkers.Count + 1);
		//	print("worker value" + value.ToString());
		//	//return Mathf.Clamp(value, 0.1f, 1f);
		//	return 1;
		//}

		public float getBaseValue()
		{
			if (Gold < Constants.COST[UnitType.BASE])
				return 0.1f;
			float value = (1 - myBases.Count) / (enemyBases.Count + 1);
			print("Base value: " + value.ToString());
			return Mathf.Clamp(value, 0.1f, 1.0f);
		}

		public float getBarracksValue()
		{
			if (Gold < Constants.COST[UnitType.BARRACKS])
				return 0f;
			float value = myBarracks.Count / (2 * enemyBarracks.Count + 1);
			print("Barracks value: " + value);
			return Mathf.Clamp(value,0.1f, 1.0f);
		}

		public float getRefineryValue()
		{
			if (Gold < Constants.COST[UnitType.REFINERY])
				return 0f;
			float value = (1 - myRefineries.Count) / (2 * enemyRefineries.Count + 1);
			//print("Refinery value: " + value);
			//return Mathf.Clamp(value, 0.1f, 1.0f);
			return 0f;
		}

		public float getAttackValue()
		{
			
			if (mySoldiers.Count < 1 || myArchers.Count < 1)
				return 0f;
			float value = (mySoldiers.Count + myArchers.Count) / (enemyArchers.Count + enemySoldiers.Count);
			print("Attack Value: " + value);
			return Mathf.Clamp(value, 0.1f, 1f);
		}

		public float getTrainWorkerValue()
		{
			float value = (1 - myWorkers.Count) / (2 * enemyWorkers.Count + myWorkers.Count + 1);
			print("Train Worker Value: " + value);
			//return Mathf.Clamp(value, 0.1f, 1f);
			return 0.1f;
		}

		public float getTrainSoldierValue()
		{
			if (Gold < Constants.COST[UnitType.SOLDIER])
			{
				return 0f;
			}
			float value = (1 -mySoldiers.Count) / (enemySoldiers.Count + 1);
			print("Train soldier value: " + value);
			return Mathf.Clamp(value, 0.1f, 1f);
		}

		public float getTrainArcherValue()
		{
			float value = (1 - myArchers.Count) / (2 * enemyArchers.Count + 1);
			print("train archer value: " + value);
			return Mathf.Clamp(value, 0.1f, 1f);
		}

		public float getGatherValue()
		{
			float value = 0f;
			if (Gold < 2000)
				value = 1f;
			else
				return value;

			print("gather value: " + value);

			return value;
		}

		public void getMostImportant()
		{
			List<float> myOptions = new List<float>();
			//myOptions.Add(getSoldierValue()); //0
			//myOptions.Add(getWorkerValue()); //1
			myOptions.Add(getBaseValue()); //0
			myOptions.Add(getBarracksValue()); //1
			myOptions.Add(getRefineryValue()); //2
			myOptions.Add(getAttackValue()); //3
			myOptions.Add(getTrainWorkerValue());//4
			myOptions.Add(getTrainSoldierValue());//5
			myOptions.Add(getTrainArcherValue());//6
			myOptions.Add(getGatherValue()); //7

			float maxOptValue = 1.0f;
			int optionIndex = 0;

			for(int i = 0; i < myOptions.Count; i++)
			{
				if(myOptions[i]  > maxOptValue)
				{
					maxOptValue = myOptions[i];
					optionIndex = i;
				}
			}

			print("option index: " + optionIndex);
			print("Attack value: (switch)" + getAttackValue());

			switch(optionIndex)
			{
				default:
					print("Build A Base.");
					foreach (int worker in myWorkers)
					{
						Unit unit = GameManager.Instance.GetUnit(worker);
						Vector3Int toBuild = FindClosestBuildPosition(unit.GridPosition, UnitType.BASE);
						if (toBuild != Vector3Int.zero)
						{
							Build(unit, toBuild, UnitType.BASE);
						}
					}
					break;
				case 0:
					print("Build A Base.");
					foreach (int worker in myWorkers)
					{
						Unit unit = GameManager.Instance.GetUnit(worker);
						Vector3Int toBuild = FindClosestBuildPosition(unit.GridPosition, UnitType.BASE);
						if (toBuild != Vector3Int.zero)
						{
							Build(unit, toBuild, UnitType.BASE);
						}
					}

					break;
				case 1:
					print("Build a barracks");
					foreach (int worker in myWorkers)
					{
						Unit unit = GameManager.Instance.GetUnit(worker);
						Vector3Int toBuild = FindClosestBuildPosition(unit.GridPosition, UnitType.BARRACKS);
						if (toBuild != Vector3Int.zero)
						{
							Build(unit, toBuild, UnitType.BARRACKS);
						}
					}


					break;
				case 2: //refinery
					print("build a refinery");
					foreach (int worker in myWorkers)
					{
						Unit unit = GameManager.Instance.GetUnit(worker);
						Vector3Int toBiuld = FindClosestBuildPosition(unit.GridPosition, UnitType.REFINERY);
						if (toBiuld != Vector3Int.zero)
						{
							Build(unit, toBiuld, UnitType.REFINERY);
						}
					}
					break;
				case 3: // refinery
					print("attack");
					foreach (int soldier in mySoldiers)
					{
						Unit soldierUnit = GameManager.Instance.GetUnit(soldier);
						if (enemySoldiers.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemySoldiers[UnityEngine.Random.Range(0, enemySoldiers.Count)]));
						}
						// If there are enemy archers, randomly select one and attack it
						else if (enemyArchers.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemyArchers[UnityEngine.Random.Range(0, enemyArchers.Count)]));
						}
						// If there are enemy workers, randomly select one and attack it
						else if (enemyWorkers.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemyWorkers[UnityEngine.Random.Range(0, enemyWorkers.Count)]));
						}
						// If there are enemy bases, randomly select one and attack it
						else if (enemyBases.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemyBases[UnityEngine.Random.Range(0, enemyBases.Count)]));
						}
						// If there are enemy barracks, randomly select one and attack it
						else if (enemyBarracks.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemyBarracks[UnityEngine.Random.Range(0, enemyBarracks.Count)]));
						}
						// If there are enemy refineries, randomly select one and attack it
						else if (enemyRefineries.Count > 0)
						{
							Attack(soldierUnit, GameManager.Instance.GetUnit(
								enemyRefineries[UnityEngine.Random.Range(0, enemyRefineries.Count)]));
						}
					}
					break;
				case 4: // train workers
					print("Train Workers");
					foreach (int numOfBases in myBases)
					{
						Unit myBase = GameManager.Instance.GetUnit(numOfBases);
						Train(myBase, UnitType.WORKER);
					}
					break;
				case 5: // train soldiers
					print("train soldiers");
					foreach (int numOfBarracks in myBarracks)
					{
						Unit baracks = GameManager.Instance.GetUnit(numOfBarracks);
						Train(baracks, UnitType.SOLDIER);
					}
					break;
				case 6: //train archers
					print("Train archers");
					foreach(int numOfBarracks in myBarracks)
					{
						Unit barracks = GameManager.Instance.GetUnit(numOfBarracks);
						Train(barracks, UnitType.ARCHER);
					}
					break;
				case 7://gather
					print("gather gold");
					foreach (int worker in myWorkers)
					{
						Unit unit = GameManager.Instance.GetUnit(worker);
						Unit mineUnit = GameManager.Instance.GetUnit(mainMineNbr);
						Unit baseUnit = GameManager.Instance.GetUnit(mainBaseNbr);
						if (mineUnit != null && baseUnit != null)
						{ Gather(unit, mineUnit, baseUnit); }
					}
					break;

			}

		}

		// Update the GameManager - called once per frame
		public void Update()
		{
			UpdateGameState();

			// If we have at least one base, assume the first one is our "main" base
			if (myBases.Count > 0)
			{
				mainBaseNbr = myBases[0];
			}
			else
			{
				mainBaseNbr = -1;
			}

			// If we have a base, find the closest mine to the base
			if (mines.Count > 0 && mainBaseNbr >= 0)
			{
				Unit baseUnit = GameManager.Instance.GetUnit(mainBaseNbr);
				mainMineNbr = FindClosestUnit(baseUnit.GridPosition, mines);
			}



			getMostImportant();

			// Process all of the units, prioritize building new structures over
			// training units in terms of spending gold

			//getSoldierValue();
			//getWorkerValue();
			//ProcessWorkers();

			//ProcessSoldiers();

			//ProcessArchers();

			//ProcessBarracks();
			
			//ProcessBases();
		}

		#endregion
	}

	

}

