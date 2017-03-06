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
      InitiatePlayer.Game.AnnounceMessage( "{0} 对 {1} 发起攻击， {1} 胸口一闷，哇的吐了一口鲜血。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
    }

    public void AnnounceAttackIneffective()
    {
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击，但 {1} 看起来毫发无损，随手整理了一下被风吹乱的秀发。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
    }

    public void AnnounceDoubleAttack()
    {
      Game.AnnounceMessage( "{0} 对 {1} 发起攻击， {1} 整个人像树叶一样的飞了出去。", InitiatePlayer.PlayerName, RecipientPlayer.PlayerName );
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



      var point = AttackPoint;

      if ( (bool?) DataBag.DoubleAttack == true )
      {
        point *= 2;
        AnnounceDoubleAttack();
      }

      else
        AnnounceNormalAttack();

      RecipientPlayer.HealthPoint -= point;
      RecipientPlayer.PlayerHost.WriteWarningMessage( "您受到攻击，生命值减少 {0} 点，目前生命值 {1}", point, RecipientPlayer.HealthPoint );
      return Task.CompletedTask;
    }

  }
}
