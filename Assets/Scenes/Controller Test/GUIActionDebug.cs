﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GUIActionDebug : MonoBehaviour
{

    private Rect plannedArea = new Rect (160, 20, 250, 400);
    private Rect playingArea = new Rect (430, 20, 250, 400);

    Vector2 plannedScrollPos = Vector2.zero;
    Vector2 playingScrollPos = Vector2.zero;

    
    public void OnGUI ()
    {

        IList<PlayerAction> actionList = ActionAggregator.GetActionList ();

        GUILayout.BeginArea (plannedArea);
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Planned Actions");
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();

        plannedScrollPos = GUILayout.BeginScrollView (plannedScrollPos);
        GUILayout.TextArea (BuildPlannedActionListString (actionList));
        GUILayout.EndScrollView ();

        GUILayout.EndVertical ();
        GUILayout.EndArea ();

        GUILayout.BeginArea (playingArea);
        GUILayout.BeginVertical ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.Label ("Playing Actions");
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();

        playingScrollPos = GUILayout.BeginScrollView (playingScrollPos);
       
        IList<PlayerAction> playingActionList = ActionExecuter.GetPlayingActionList ();

        GUILayout.TextArea (BuildPlannedActionListString (playingActionList));
        GUILayout.EndScrollView ();

        
        GUILayout.EndVertical ();
        GUILayout.EndArea ();

    }

    public string BuildPlannedActionListString (IList<PlayerAction> actionList)
    {

        StringBuilder actionString = new StringBuilder ();

        foreach (PlayerAction pAction in actionList) {
            pAction.AppendActionString (actionString, true);
        }

        return actionString.ToString ();

    }
}