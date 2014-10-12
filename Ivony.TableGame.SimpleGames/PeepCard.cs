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
      get { return "窥视"; }
    }

    public override string Description
    {
      get { return "查看其它玩家手上所有的牌"; }
    }
  }
}
