using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
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



    protected void AnnounceSpecialCardUsed( SimpleGamePlayer user )
    {
      user.Game.AnnounceMessage( "{0} 使用了一张特殊卡牌", user.PlayerName );
    }



    protected virtual async Task<SimpleGamePlayer> CherryTarget( SimpleGamePlayer player, CancellationToken token )
    {
      var targets = GetTargets( player );

      if ( targets == null || targets.Any() == false )
        return null;

      if ( targets.Length == 1 )
        return targets[0];

      var result = await player.PlayerHost.Console.Choose( "请选择使用对象：", targets.Select( item => Option.Create( item ) ).ToArray(), null, token );
      if ( result == null )
        throw new TimeoutException();

      return result;
    }


    protected SimpleGamePlayer[] GetTargets( SimpleGamePlayer player )
    {
      if ( this is IOtherPlayerTarget )
        return player.Game.Players.Where( item => item != player ).ToArray();

      else if ( this is IAnyPlayerTarget )
        return player.Game.Players;

      else if ( this is ISelfTarget )
        return new[] { player };

      else
        return null;

    }

    /// <summary>
    /// 获取使用该卡牌所需要的行动点数
    /// </summary>
    public override int ActionPoint
    {
      get { return 1; }
    }


    /// <summary>
    /// 获取卡牌当前是否可用
    /// </summary>
    /// <param name="player">使用卡牌的玩家</param>
    /// <returns>是否可用</returns>
    public virtual bool Availables( SimpleGamePlayer player )
    {
      return player.ActionPoint >= ActionPoint;
    }
  }
}
