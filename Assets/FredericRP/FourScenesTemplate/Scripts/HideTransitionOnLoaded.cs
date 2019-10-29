using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FredericRP.ProjectTemplate
{
  public class HideTransitionOnLoaded : MonoBehaviour
  {
    private void OnEnable()
    {
      // Hide transition when this object is enabled
      // TODO : for better handling, use a game manager with a state machine that hide the transition when the right state can be reached
      Transition.Transition.Hide();
    }
  }
}