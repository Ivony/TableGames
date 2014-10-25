using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public class GameAroundEvent : GameEventBase, IGamePlayerEvent
  {

    public GameAroundEvent( CardGamePlayer player )
    {
      Player = player;
    }

    public GamePlayerBase Player
    {
      get;
      private set;
    }


  }
}
