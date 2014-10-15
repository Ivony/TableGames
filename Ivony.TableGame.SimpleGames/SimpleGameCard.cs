using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameCard : BasicCard
  {

    public abstract Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target );



    protected void AnnounceSpecialCardUsed( SimpleGamePlayer user )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.PlayerName );
    }

  }
}
