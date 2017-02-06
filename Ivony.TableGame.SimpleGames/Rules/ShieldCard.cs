using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class ShieldCard : ElementAttachmentCard, ISelfTarget, IBasicCard
  {
    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

      if ( Element == Element.水 )
        target.Purify();

      var effect = new ShieldEffect( Element );
      target.SetEffect( effect );
      AnnounceSpecialCardUsed( user );

      target.PlayerHost.WriteMessage( $"您使用了{effect.Name}，{effect.Description}" );

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
