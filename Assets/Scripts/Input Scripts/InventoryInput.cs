﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorun Gork/InputActions/Inventory")]
public class InventoryInput : InputActions {

    public override void RespondToInput(GameController controller, string[] separatedInputWords)
    {
        //controller.interactableItems.DisplayInventoryByCommand();

        controller.itemHandler.DisplayInventoryByCommand();
    }

}