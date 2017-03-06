using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class PeepCard : StandardCard
  {



    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      foreach ( var player in user.Game.Players.Where( item => item != user ) )
      {
        player.DealCards();
        user.PlayerHost.WriteMessage( "{0} HP:{1,-3}{2} 卡牌：{3}", player.PlayerName, player.HealthPoint, player.Effects, string.Join( ", ", player.Cards.Select( item => item.Name ) ) );
      }

      return Task.CompletedTask;
    }

    public override string Name
    {
      get { return "窥视"; }
    }

    public override string Description
    {
      get { return "查看其它玩家手上所有的牌"; }
    }


    /// <summary>
    /// 窥视不需要任何行动点数
    /// </summary>
    public override int ActionPoint
    {
      get { return 0; }
    }
  }
}
