using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public abstract class PlayerConsoleBase
  {

    public abstract void WriteMessage( GameMessage message );



    public abstract Task<string> ReadLine( string prompt, CancellationToken token );

    public Task<string> ReadLine( string prompt, TimeSpan timeout )
    {
      if ( timeout < TimeSpan.Zero )
        throw new ArgumentOutOfRangeException( "timeout" );

      return ReadLine( prompt, new CancellationTokenSource( timeout ).Token );
    }


    public Task<string> ReadLine( string prompt )
    {
      return ReadLine( prompt, DefaultTimeout );
    }


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
    public async Task<string> ReadLine( string prompt, string defaultValue, CancellationToken token )
    {
      try
      {
        return await ReadLine( prompt, token );
      }
      catch ( TaskCanceledException )
      {
        return defaultValue;
      }

    }



    public abstract Task<IOption> Choose( string prompt, IOption[] options, CancellationToken token );




    protected TimeSpan DefaultTimeout { get { return TimeSpan.FromMinutes( 1 ); } }
  }
}
