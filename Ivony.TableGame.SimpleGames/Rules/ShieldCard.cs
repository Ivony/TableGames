using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ShieldCard : ElementAttachmentCard, ISelfTarget
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {

      if ( Element == Element.水 )
        target.Purify();

      target.SetEffect( new CardEffect( Element ) );
      AnnounceSpecialCardUsed( user );

      string message;
      if ( Element == null )
        message = $"{user.PlayerName} 对您使用了盾牌，下一次普通攻击将对您无效。";

      else
      {
        if ( Element == Element.金 )
          message = $"{user.PlayerName} 对您使用了金属性盾牌，可以抵挡五次普通攻击。";

        if ( Element == Element.木 )
          message = $"{user.PlayerName} 对您使用了木属性盾牌，下一次普通攻击无效，且为您恢复生命。";

        if ( Element == Element.水 )
          message = $"{user.PlayerName} 对您使用了水属性盾牌，下一次普通攻击无效，且净化攻击者。";

        if ( Element == Element.火 )
          message = $"{user.PlayerName} 对您使用了火属性盾牌，下一次普通攻击无效，且给予攻击者伤害。";

        if ( Element == Element.土 )
          message = $"{user.PlayerName} 对您使用了水属性盾牌，下一次普通攻击无效，且攻击者将无法行动一个回合。";
      }
    }



    public override string Name
    {
      get { return "盾牌"; }
    }

    public override string Description
    {
      get { return "使用此卡牌后可以抵挡一次普通攻击，附着五行元素后将获得更强大的功能"; }
    }


    private class CardEffect : SimpleGameEffect, IDefenceEffect
    {


      public CardEffect( Element element = null )
      {
        Element = element;
        if ( Element == Element.金 )
          times = 5;

        else
          times = 1;
      }


      public SimpleGamePlayer Player { get; }

      public Element Element { get; }


      public override string Name
      {
        get { return "盾牌"; }
      }

      public override string Description
      {
        get { return "盾牌效果可以抵御一次攻击"; }
      }



      protected override async Task OnAttack( AttackEvent attackEvent )
      {
        var recipient = attackEvent.RecipientPlayer;
        var initiate = attackEvent.InitiatePlayer;
        attackEvent.Handled = true;

        if ( attackEvent.Element == null )
        {
          attackEvent.AnnounceAttackIneffective();
          attackEvent.Handled = true;



          times--;
          if ( Element == Element.金 )
          {
            if ( times > 0 )
              recipient.PlayerHost.WriteWarningMessage( $"金属性盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，还剩 {times} 次机会。" );

            else
              recipient.PlayerHost.WriteWarningMessage( $"金属性盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，防御效果已经失效" );
          }
          else if ( Element == Element.木 )
          {
            recipient.PlayerHost.WriteWarningMessage( $"木属性的盾牌使得对您的攻击转变为治疗，您获得了 {attackEvent.AttackPoint} 点生命值，目前生命值为 {recipient.HealthPoint} ，防御效果已经失效" );
            recipient.HealthPoint += attackEvent.AttackPoint;
          }
          else if ( Element == Element.水 )
          {
            recipient.PlayerHost.WriteWarningMessage( $"水属性的盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，并清除攻击方的一切效果，防御效果已经失效" );
            initiate.Purify();
          }
          else if ( Element == Element.火 )
          {
            recipient.PlayerHost.WriteWarningMessage( $"火属性的盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，并对攻击方造成同等伤害，防御效果已经失效" );
            await recipient.Game.OnGameEvent( new AttackEvent( recipient, initiate, null, attackEvent.AttackPoint ) );
          }
          else if ( Element == Element.土 )
          {
            recipient.PlayerHost.WriteWarningMessage( $"土属性的盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，并禁锢攻击方一回合，防御效果已经失效" );
          }

          else if ( Element == null )
            recipient.PlayerHost.WriteWarningMessage( $"您使用盾牌阻挡了 {attackEvent.AttackPoint} 点攻击，防御效果已经失效" );

        }
        else
        {
          if ( attackEvent.Element.IsCounteract( Element ) )
          {
            attackEvent.AnnounceDoubleAttack();
            recipient.HealthPoint -= attackEvent.AttackPoint * 2;

            recipient.PlayerHost.WriteWarningMessage( $"您受到{attackEvent.Element.Name}属性的攻击，与您的盾牌属性相克，造成了双倍伤害 {attackEvent.AttackPoint * 2} 点，目前 HP 还剩 {recipient.HealthPoint} 点。" );
          }

          else if ( attackEvent.Element.IsReinforce( Element ) )
          {
            attackEvent.AnnounceDoubleAttack();
            recipient.HealthPoint += attackEvent.AttackPoint;

            recipient.PlayerHost.WriteWarningMessage( $"您受到{attackEvent.Element.Name}属性的攻击，与您的盾牌属性相生，所受伤害变为治疗，增加 HP {attackEvent.AttackPoint} 点，目前 HP 还剩 {recipient.HealthPoint} 点。" );
          }
          else if ( attackEvent.Element == Element )
          {
            attackEvent.AnnounceDoubleAttack();
            recipient.HealthPoint -= attackEvent.AttackPoint;

            recipient.PlayerHost.WriteWarningMessage( $"您受到{attackEvent.Element.Name}属性的攻击，与您的盾牌属性相同，盾牌失效，造成了 {attackEvent.AttackPoint} 点伤害，目前 HP 还剩 {recipient.HealthPoint} 点。" );
          }
          else
          {
            attackEvent.AnnounceDoubleAttack();
            recipient.HealthPoint -= attackEvent.AttackPoint;

            recipient.PlayerHost.WriteWarningMessage( $"{Element.Name}属性的盾牌阻挡了 {attackEvent.AttackPoint} 点{attackEvent.Element}属性攻击，防御效果已经失效" );
          }


          times = 0;

        }

        if ( times == 0 )
          recipient.Effects.RemoveEffect( this );

      }


      private int times;


      public override string ToString()
      {
        return Element?.Name + "盾";
      }

    }

  }
}
