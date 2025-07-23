
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;

public interface GenericAttack
{
    public abstract void PerformAttack();

    public abstract void SetMove(Move.Type moveType, Move.Effect effect);
}