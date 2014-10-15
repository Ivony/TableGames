using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public partial class PlayerHost
  {

    /// <summary>
    /// 获取是否正在等待玩家响应
    /// </summary>
    public bool WaitForResponse
    {
      get { return _responding != null; }
    }



    /// <summary>
    /// 获取输入提示信息，当等待用户响应时，显示该提示信息给用户。
    /// </summary>
    public string PromptText
    {
      get
      {
        lock ( SyncRoot )
        {
          if ( _responding == null )
            return null;

          else
            return _responding.PromptText;
        }
      }
    }



    private Responding _responding;



    internal void Response( string message )
    {

      lock ( SyncRoot )
      {
        if ( _responding == null )
        {
          this.WriteSystemMessage( "未在响应窗口时间或已经超时，无法再接收消息" );
          return;
        }


        _responding.OnResponse( message );

      }

    }




    private class Responding
    {

      private TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();



      public static Responding CreateResponding( PlayerHost host, string prompt, CancellationToken token )
      {
        lock ( host.SyncRoot )
        {
          if ( host._responding != null )
            throw new InvalidOperationException();

          return host._responding = new Responding( host, prompt, token );
        }

      }

      protected Responding( PlayerHost host, string prompt, CancellationToken token )
      {
        Host = host;
        PromptText = prompt;


        token.Register( Canceled );
      }

      protected virtual void Canceled()
      {
        taskSource.TrySetCanceled();
        lock ( Host.SyncRoot )
        {
          if ( Host._responding == this )
            Host._responding = null;
        }
      }


      protected PlayerHost Host
      {
        get;
        private set;
      }


      public Task<string> RespondingTask { get { return taskSource.Task; } }


      public string PromptText { get; private set; }


      public virtual void OnResponse( string message )
      {
        lock ( Host.SyncRoot )
        {
          if ( Host._responding == this )
          {
            Host._responding = null;
            taskSource.TrySetResult( message );
          }

          else
            taskSource.TrySetCanceled();
        }
      }
    }


    private class ChooseResponding : Responding
    {


      private ChooseResponding( PlayerHost host, string prompt, IOption[] options, CancellationToken token )
        : base( host, prompt, token )
      {
        Options = options;
      }

      public static ChooseResponding CreateResponding( PlayerHost host, string prompt, IOption[] options, CancellationToken token )
      {
        lock ( host.SyncRoot )
        {
          if ( host._responding != null )
            throw new InvalidOperationException();

          var responding = new ChooseResponding( host, prompt, options, token );
          host._responding = responding;
          return responding;
        }

      }


      private TaskCompletionSource<IOption> taskSource = new TaskCompletionSource<IOption>();


      public new Task<IOption> RespondingTask { get { return taskSource.Task; } }


      public IOption[] Options { get; private set; }


      public override void OnResponse( string message )
      {

        IOption option;
        if ( !TryGetOption( message, out option ) )
        {
          Host.WriteWarningMessage( "您输入的格式不正确，应该输入 {0} - {1} 之间的数字", 1, Options.Length );
          return;
        }


        taskSource.TrySetResult( option );
        base.OnResponse( message );
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

      protected override void Canceled()
      {
        taskSource.TrySetCanceled();
        base.Canceled();
      }

    }


  }
}