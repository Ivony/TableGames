using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public abstract class PlayerConsoleBase
  {

    public abstract void WriteMessage( GameMessage message );


    public abstract Task<string> ReadLine( string prompt );

  }
}
