using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public interface IPlayerHost
  {

    PlayerConsoleBase Console { get; }


    void JoinedGame( GamePlayer player );

    void LeavedGame();

    GamePlayer GetPlayer();

  }
}
