using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class PurifyCard : SimpleGameCard
  {
    public async override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target )
    {
      var game = user.Game;
      lock ( game.SyncRoot )
      {
        foreach ( var player in game.Players )
          player.Purify();
      }
    }

    public override string Name
    {
      get { return "净化"; }
    }

    public override string Description
    {
      get { return "清除当前所有玩家所有状态"; }
    }
  }
}
