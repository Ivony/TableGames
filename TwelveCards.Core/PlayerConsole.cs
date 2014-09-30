using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{
  public abstract class PlayerConsoleBase
  {

    public abstract void WriteMessage( string message );
    public abstract void WriteWarning( string message );
    public abstract void WriteSystemInfo( string message );



  }
}
