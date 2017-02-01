using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class AttackEvent : GameEventBase, IGameBehaviorEvent
  {
    public AttackEvent( SimpleGamePlayer user, SimpleGamePlayer target, Element element, int point )
    {
      InitiatePlayer = user;
      RecipientPlayer = target;
      Element = element;
      AttackPoint = point;


      Game = user.Game;
    }


    protected SimpleGame Game { get; private set; }

    GamePlayerBase IGameBehaviorEvent.InitiatePlayer { get { return InitiatePlayer; } }


    GamePlayerBase IGameBehaviorEvent.RecipientPlayer { get { return RecipientPlayer; } }



    public SimpleGamePlayer InitiatePlayer { get; }


    public SimpleGamePlayer RecipientPlayer { get; }



    public Element Element { get; }

    public int AttackPoint { get; }



    public void AnnounceAttackIneffective()
    {
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但 {1} 看起来毫发无损。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
    }


    /// <summary>
    /// 指示事件是否已经被处理完毕
    /// </summary>
    public bool Handled
    {
      get;
      set;
    }

  }
}
