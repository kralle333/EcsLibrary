using System.Collections.Generic;
using EcsLibrary.GameMaking;

namespace EcsLibrary.Components;

public abstract class GameComponentState : Component
{
    public abstract void OnCollide(string with);
}
