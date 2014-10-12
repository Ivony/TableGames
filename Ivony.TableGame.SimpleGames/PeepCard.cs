using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class PeepCard : SimpleGameCard
  {



    public async override Task Execute( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      user.GameHost.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.CodeName );
      foreach ( var player in user.GameHost.Game.Players.Where( item => item != user ) )
        user.PlayerHost.WriteMessage( "{0} {1}", player.CodeName, string.Join( ", ", player.Cards.Select( item => item.Name ) ) );
    }

    public override string Name
    {
      get { throw new NotImplementedException(); }
    }

    public override string Description
    {
      get { throw new NotImplementedException(); }
    }
  }
}
