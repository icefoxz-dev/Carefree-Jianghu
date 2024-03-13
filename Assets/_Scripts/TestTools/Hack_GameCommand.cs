using System.Collections;
using System.Collections.Generic;
using _Game;
using _Game._Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestGameCommand : MonoBehaviour
{
    [Button, GUIColor("yellow"), HideInEditorMode] public void PrintPlayerInfo() => Game.World.DebugInfo(Game.World.Role);
    [Button, GUIColor("cyan"), HideInEditorMode] public void NextRound()
    {
        var story = Game.GetController<StoryController>();
        story.ConfirmRound();
        Game.World.DebugInfo(Game.World.Role);
    }
}
