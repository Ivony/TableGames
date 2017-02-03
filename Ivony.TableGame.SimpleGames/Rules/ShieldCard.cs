using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ShieldCard : ElementAttachmentCard, ISelfTarget
  {
    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

      if ( Element == Element.水 )
        target.Purify();

      target.SetEffect( new ShieldEffect( Element ) );
      AnnounceSpecialCardUsed( user );

      string message;

      if ( Element == Element.金 )
        message = $"您使用了金属性盾牌，可以抵挡三次普通攻击。";

      else if ( Element == Element.木 )
        message = $"您使用了木属性盾牌，下一次普通攻击无效，且为您恢复生命。";

      else if ( Element == Element.水 )
        message = $"您使用了水属性盾牌，下一次普通攻击无效，且净化攻击者。";

      else if ( Element == Element.火 )
        message = $"您使用了火属性盾牌，下一次普通攻击无效，且给予攻击者伤害。";

      else if ( Element == Element.土 )
        message = $"您使用了土属性盾牌，下一次普通攻击无效，且攻击者将无法行动一个回合。";

      else if ( Element == null )
        message = $"您使用了盾牌，下一次普通攻击将对您无效。";

      else
        throw new InvalidOperationException( "未知的属性" );

      target.PlayerHost.WriteMessage( message );

      return Task.CompletedTask;
    }



    public override string Name
    {
      get { return "盾牌"; }
    }

    public override string Description
    {
      get { return "使用此卡牌后可以抵挡一次普通攻击，附着五行元素后将获得更强大的功能"; }
    }



  }
}
