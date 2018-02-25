﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputActions : ScriptableObject {

    public string keyWord;

    public abstract void RespondToInput(GameController controller, string[] separatedInputWords);
}