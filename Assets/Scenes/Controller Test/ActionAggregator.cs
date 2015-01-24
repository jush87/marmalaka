﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class ActionAggregator : MonoBehaviour
{
    public static ActionAggregator singleton;

    IList<PlayerAction> actionList;

    public void Awake ()
    {
        if (singleton == null) {
            singleton = this;
        } else {
            Destroy (gameObject);
        }

        actionList = new List<PlayerAction> ();
    }

    public static IList<PlayerAction> GetActionList ()
    {
        return singleton.actionList;
    }

    public static void InsertAction (PlayerAction action)
    {
        int currentIndex = singleton.actionList.Count;

        if (currentIndex == 0) {
            singleton.actionList.Add (action);
            return;
        }

        while (currentIndex > 0) {
            PlayerAction compAction = singleton.actionList [currentIndex - 1];

            if (compAction.timerData.turnNumber > action.timerData.turnNumber ||
                compAction.timerData.turnNumber == action.timerData.turnNumber &&
                compAction.timerData.timeInTurn > action.timerData.timeInTurn) {

                currentIndex --;

            } else {

                singleton.actionList.Insert (currentIndex, action);
                break;

            }
        }

    }

    public void TurnOver (int turnNo)
    {

        IList<PlayerAction> turnActionList = new List<PlayerAction> ();

        foreach (PlayerAction pAction in actionList) {
            if (pAction.timerData.turnNumber == turnNo) {
                turnActionList.Add (pAction);
            }
        }

        foreach (PlayerAction pAction in turnActionList) {
            actionList.Remove (pAction);
        }

        ActionExecuter.QueueActions (turnActionList);
    }

    [RPC]
    public void AddAction (NetworkPlayer netPlayer, int localPlayerId, int actionType, int timeSlotNo, float deltaInSlot)
    {

        if (!Network.isServer) {
            Debug.LogError ("Server function called on Client!!");
        }

        PlayerAction action = new PlayerAction (netPlayer, localPlayerId, actionType, timeSlotNo, deltaInSlot);

        Debug.Log ("RECEIVED BY SERVER: " + action.AppendActionString (new StringBuilder ()).ToString ());

        InsertAction (action);
    }

    public void AddActionServerLocal (PlayerAction action)
    {
        
        if (!Network.isServer) {
            Debug.LogError ("Server function called on Client!!");
        }
        
        Debug.Log ("RECEIVED BY SERVER: " + action.AppendActionString (new StringBuilder ()).ToString ());
        
        InsertAction (action);

    }
    
}
