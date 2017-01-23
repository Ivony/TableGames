using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameCard : StandardCard
  {

    public abstract Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target );



    protected void AnnounceSpecialCardUsed( SimpleGamePlayer user )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.PlayerName );
    }


    public override Task Play( CardGamePlayer initiatePlayer, object target, CancellationToken token )
    {
      var user = (SimpleGamePlayer) initiatePlayer;
      var targetPlayer = (SimpleGamePlayer) target;
      return UseCard( user, targetPlayer );
    }

    /// <summary>
    /// 获取使用该卡牌所需要的行动点数
    /// </summary>
    public override int ActionPoint
    {
      get { return 1; }
    }
  }
}
