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




    public Task<string> ReadLine( string prompt )
    {
      return ReadLine( prompt, DefaultTimeout );
    }

    public abstract Task<string> ReadLine( string prompt, TimeSpan timeout );


    public Task<string> ReadLine( string prompt, string defaultValue )
    {
      return ReadLine( prompt, DefaultTimeout );
    }

    public async Task<string> ReadLine( string prompt, string defaultValue, TimeSpan timeout )
    {
      try
      {
        return await ReadLine( prompt, timeout );
      }
      catch ( TaskCanceledException )
      {
        return defaultValue;
      }

    }



    protected TimeSpan DefaultTimeout { get { return TimeSpan.FromMinutes( 1 ); } }
  }
}
