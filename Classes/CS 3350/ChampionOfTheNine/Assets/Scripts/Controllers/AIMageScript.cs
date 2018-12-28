using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Script that controls an AI mage character
/// </summary>
public class AIMageScript : AIScript
{
    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();
        targetRange = Constants.MAGE_AI_RANGE;
    }

    /// <summary>
    /// Attacks the target
    /// </summary>
    protected override void Attack()
    {
        if (!character.GCDTimer.IsRunning)
        {
            // Sometimes does nothing
            int choice = Random.Range(0, Constants.MAGE_AI_SHOT_THRESHOLD);
            if (choice == 0)
            {
                character.SetArmAngle(Utilities.GetAngleDegrees(character.Arm.transform.position, target.transform.position) +
                    Random.Range(-Constants.MAGE_AI_ANGLE_RANGE, Constants.MAGE_AI_ANGLE_RANGE));
                character.FireMainAbility();
            }
            else
            {
                character.GCDTimer.Start();
            }
        }
    }
}
