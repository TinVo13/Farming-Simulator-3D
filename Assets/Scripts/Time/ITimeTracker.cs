using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeTracker
{
   void ClockUpdate(GameTimestamp timestamp);
}
