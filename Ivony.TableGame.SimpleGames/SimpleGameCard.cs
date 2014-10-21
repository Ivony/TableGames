using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameCard : Card
  {

    public abstract Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target );



    protected void AnnounceSpecialCardUsed( SimpleGamePlayer user )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.PlayerName );
    }

  }
}
