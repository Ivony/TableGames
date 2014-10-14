using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class PeepCard : SimpleGameCard
  {



    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      AnnounceSpecialCardUsed( user );
      foreach ( var player in user.Game.Players.Where( item => item != user ) )
      {
        player.DealCards();
        user.PlayerHost.WriteMessage( "{0} HP:{1,-3}{2}{3} 卡牌：{4}", player.CodeName, player.HealthPoint, player.DefenceEffect, player.SpecialEffect, string.Join( ", ", player.Cards.Select( item => item.Name ) ) );
      }
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
