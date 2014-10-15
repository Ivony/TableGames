using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public interface IResponding
  {
    string PromptText { get; }

    void OnResponse( string message );


  }

  public abstract class Responding<T> : IResponding
  {

    protected Responding( PlayerHost playerHost, string promptText, CancellationToken token )
    {

      lock ( playerHost.SyncRoot )
      {
        if ( playerHost.Responding != null )
          throw new InvalidOperationException();


        PlayerHost = playerHost;
        PromptText = promptText;
        TaskCompletionSource = new TaskCompletionSource<T>();

        token.Register( OnCancelled );
        playerHost.Responding = this;
      }
    }




    protected TaskCompletionSource<T> TaskCompletionSource { get; private set; }


    public Task<T> RespondingTask { get { return TaskCompletionSource.Task; } }

    protected PlayerHost PlayerHost { get; private set; }

    public string PromptText { get; private set; }


    public void OnResponse( string message )
    {
      lock ( PlayerHost.SyncRoot )
      {
        if ( PlayerHost.Responding == this )
        {
          OnResponseCore( message );
          PlayerHost.Responding = null;
        }
      }

    }

    protected abstract void OnResponseCore( string message );


    protected void OnCancelled()
    {
      TaskCompletionSource.TrySetCanceled();

      lock ( PlayerHost )
      {
        if ( PlayerHost.Responding == this )
          PlayerHost.Responding = null;
      }
    }

  }


  internal class TextMessageResponding : Responding<string>
  {

    public TextMessageResponding( PlayerHost playerHost, string promptText, CancellationToken token ) : base( playerHost, promptText, token ) { }


    protected override void OnResponseCore( string message )
    {
      TaskCompletionSource.TrySetResult( message );
    }
  }


  internal class OptionsResponding : Responding<IOption>
  {

    public OptionsResponding( PlayerHost playerHost, string promptText, IOption[] options, CancellationToken token )
      : base( playerHost, promptText, token )
    {
      Options = options;
    }

    public IOption[] Options { get; private set; }


    protected override void OnResponseCore( string message )
    {

      IOption option;
      if ( !TryGetOption( message, out option ) )
      {
        PlayerHost.WriteWarningMessage( "您输入的格式不正确，应该输入 {0} - {1} 之间的数字", 1, Options.Length );
        return;
      }

      TaskCompletionSource.TrySetResult( option );
    }

    private bool TryGetOption( string text, out IOption option )
    {
      option = null;
      int index;
      if ( !int.TryParse( text, out index ) )
        return false;

      if ( index < 1 || index > Options.Length )
        return false;

      option = Options[index - 1];
      return true;
    }

  }


}