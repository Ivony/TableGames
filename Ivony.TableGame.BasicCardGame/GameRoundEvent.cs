using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public class GameRoundEvent<TPlayer, TCard> : GameEventBase, IGamePlayerEvent
    where TPlayer : BasicGamePlayer<TCard>
    where TCard : BasicCard
  {

    public GameRoundEvent( TPlayer player, int rounds )
    {
      Player = player;
      Rounds = rounds;
    }

    public int Rounds { get; private set; }

    public GamePlayerBase Player { get; private set; }
  }
}
