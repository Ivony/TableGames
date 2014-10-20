using Ivony.TableGame.BasicCardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class AttackEvent : GameEventBase, IGameBehaviorEvent
  {
    public AttackEvent( SimpleGamePlayer user, SimpleGamePlayer target, int point )
    {
      InitiatePlayer = user;
      RecipientPlayer = target;
      AttackPoint = point;
    }


    public GamePlayerBase InitiatePlayer { get; private set; }


    public GamePlayerBase RecipientPlayer { get; private set; }


    public int AttackPoint { get; private set; }

  }
}
