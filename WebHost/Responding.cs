using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{

  /// <summary>
  /// 定义玩家响应行为
  /// </summary>
  public interface IResponding
  {
    /// <summary>
    /// 提示文字
    /// </summary>
    string PromptText { get; }

    /// <summary>
    /// 当玩家响应时调用此方法
    /// </summary>
    /// <param name="message"></param>
    void OnResponse( string message );


  }


  /// <summary>
  /// 辅助实现 IResponding
  /// </summary>
  /// <typeparam name="T"></typeparam>
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
          if ( OnResponseCore( message ) )
            PlayerHost.Responding = null;
        }
      }

    }

    protected abstract bool OnResponseCore( string message );


    protected void OnCancelled()
    {

      if ( TaskCompletionSource.TrySetCanceled() )
      {
        lock ( PlayerHost )
        {
          if ( PlayerHost.Responding == this )
            PlayerHost.Responding = null;
        }
      }
    }

  }


  internal class TextMessageResponding : Responding<string>
  {

    public TextMessageResponding( PlayerHost playerHost, string promptText, CancellationToken token ) : base( playerHost, promptText, token ) { }


    protected override bool OnResponseCore( string message )
    {
      TaskCompletionSource.TrySetResult( message );
      return true;
    }
  }


  internal class OptionsResponding : Responding<Option>
  {

    public OptionsResponding( PlayerHost playerHost, string promptText, Option[] options, CancellationToken token )
      : base( playerHost, promptText, token )
    {
      Options = options;
    }

    public Option[] Options { get; private set; }


    protected override bool OnResponseCore( string message )
    {

      Option option;
      if ( !TryGetOption( message, out option ) )
      {
        PlayerHost.WriteWarningMessage( "您输入的格式不正确，应该输入 {0} - {1} 之间的数字", 1, Options.Length );
        return false;
      }

      TaskCompletionSource.TrySetResult( option );
      return true;
    }

    private bool TryGetOption( string text, out Option option )
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


  public class MultipleOptionsResponding : Responding<Option[]>
  {
    public MultipleOptionsResponding( PlayerHost playerHost, string promptText, Option[] options, CancellationToken token )
      : base( playerHost, promptText, token )
    {

    }

    protected override bool OnResponseCore( string message )
    {
      throw new NotImplementedException();
    }
  }


}