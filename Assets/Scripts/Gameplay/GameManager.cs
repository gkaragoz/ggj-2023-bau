﻿using System;
using Samples.Basic.Scripts;

namespace Gameplay
{
    public class GameManager : Singleton<GameManager>
    {
        public static Action OnStart;
        public static Action OnComplete;
    }
}