using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class AttackEvent : GameEventBase, IGameBehaviorEvent, IGameNeedHandledEvent
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



    /// <summary>
    /// 发起攻击的玩家对象
    /// </summary>
    public SimpleGamePlayer InitiatePlayer { get; }


    /// <summary>
    /// 攻击的目标玩家对象
    /// </summary>
    public SimpleGamePlayer RecipientPlayer { get; }



    public Element Element { get; }

    public int AttackPoint { get; }


    public void AnnounceNormalAttack()
    {
      InitiatePlayer.Game.AnnounceMessage( "{0} 对 {1} 发起{2}攻击。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName, Element == null ? "" : "某种五行属性的" );
    }

    public void AnnounceAttackIneffective()
    {
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但 {1} 看起来毫发无损。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
    }

    public void AnnounceDoubleAttack()
    {
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击并造成了严重的伤害", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
    }


    /// <summary>
    /// 指示事件是否已经被处理完毕
    /// </summary>
    public bool Handled
    {
      get;
      set;
    }


    public Task HandleEvent()
    {
      if ( Handled )
        return Task.CompletedTask;

      RecipientPlayer.HealthPoint -= AttackPoint;
      AnnounceNormalAttack();
      RecipientPlayer.PlayerHost.WriteWarningMessage( "您受到攻击，HP 减少 {0} 点，目前 HP {1}", AttackPoint, RecipientPlayer.HealthPoint );
      return Task.CompletedTask;
    }

  }
}
