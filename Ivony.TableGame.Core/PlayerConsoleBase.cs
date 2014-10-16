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

    public Task<string> ReadLine( string prompt, string defaultValue, CancellationToken token )
    {
      return ReadLine( prompt, defaultValue, DefaultTimeout, token );
    }

    public async Task<string> ReadLine( string prompt, string defaultValue, TimeSpan timeout, CancellationToken token )
    {

      var timeoutToken = new CancellationTokenSource( timeout ).Token;
      try
      {
        return await ReadLine( prompt, CancellationTokenSource.CreateLinkedTokenSource( timeoutToken, token ).Token );
      }
      catch ( TaskCanceledException )
      {
        if ( token.IsCancellationRequested )
          throw;

        return defaultValue;
      }

    }



    public abstract Task<IOption> Choose( string prompt, IOption[] options, CancellationToken token );


    public async Task<T> Choose<T>( string prompt, T[] options, CancellationToken token ) where T : class, IOption
    {
      return (T) await Choose( prompt, (IOption[]) options, token );
    }



    public Task<T> Choose<T>( string prompt, T[] options, T defaultOption, CancellationToken token ) where T : class, IOption
    {
      return Choose( prompt, options, defaultOption, DefaultTimeout, token );
    }

    public async Task<T> Choose<T>( string prompt, T[] options, T defaultOption, TimeSpan timeout, CancellationToken token ) where T : class, IOption
    {

      var timeoutToken = new CancellationTokenSource( timeout ).Token;
      try
      {
        return await Choose( prompt, options, CancellationTokenSource.CreateLinkedTokenSource( timeoutToken, token ).Token );
      }
      catch ( TaskCanceledException )
      {
        if ( token.IsCancellationRequested )
          throw;

        return defaultOption;
      }

    }



    protected TimeSpan DefaultTimeout { get { return TimeSpan.FromMinutes( 1 ); } }
  }
}
