using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 定义玩家控制台的抽象
  /// </summary>
  public abstract class PlayerConsoleBase
  {




    protected PlayerConsoleBase( IPlayerHost playerHost )
    {
      PlayerHost = playerHost;
    }




    protected IPlayerHost PlayerHost { get; private set; }


    /// <summary>
    /// 向玩家客户端写入一条消息
    /// </summary>
    /// <param name="message">消息对象</param>
    public abstract void WriteMessage( GameMessage message );



    /// <summary>
    /// 从玩家客户端读取一条消息
    /// </summary>
    /// <param name="prompt">提示信息</param>
    /// <param name="token">取消标识</param>
    /// <returns>返回一个 Task 用于等待玩家响应</returns>
    public abstract Task<string> ReadLine( string prompt, CancellationToken token );


    /// <summary>
    /// 从玩家客户端读取一条消息
    /// </summary>
    /// <param name="prompt">提示信息</param>
    /// <param name="defaultValue">若玩家超时没有响应，所需要使用的默认值</param>
    /// <param name="token">取消标识</param>
    /// <returns>返回一个 Task 用于等待玩家响应</returns>
    public Task<string> ReadLine( string prompt, string defaultValue, CancellationToken token )
    {
      return ReadLine( prompt, defaultValue, DefaultTimeout, token );
    }


    /// <summary>
    /// 从玩家客户端读取一条消息
    /// </summary>
    /// <param name="prompt">提示信息</param>
    /// <param name="defaultValue">若玩家超时没有响应，所需要使用的默认值</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="token">取消标识</param>
    /// <returns>返回一个 Task 用于等待玩家响应</returns>
    public async Task<string> ReadLine( string prompt, string defaultValue, TimeSpan timeout, CancellationToken token )
    {

      var timeoutToken = new CancellationTokenSource( timeout ).Token;
      try
      {
        return await ReadLine( prompt, CancellationTokenSource.CreateLinkedTokenSource( timeoutToken, token ).Token );
      }
      catch ( OperationCanceledException )
      {
        if ( token.IsCancellationRequested )
          throw;

        return defaultValue;
      }

    }



    /// <summary>
    /// 让客户端在多个选项中选择一个
    /// </summary>
    /// <param name="prompt">提示信息</param>
    /// <param name="options">选项列表</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待用户选择，并返回选择结果</returns>
    public virtual Task<IOption> Choose( string prompt, IOption[] options, CancellationToken token )
    {

      if ( PlayerHost.Support( "Choose" ) )
        return ChooseImplement( prompt, options, token );

      else
        return ChooseCompatibilityImplement( prompt, options, token );
    }

    protected virtual async Task<IOption> ChooseCompatibilityImplement( string prompt, IOption[] options, CancellationToken token )
    {
      PlayerHost.WriteMessage( prompt );


      var promptText = string.Join( ", ", options.Select( ( item, index ) => string.Format( "{0}.{1}", index + 1, item.Name ) ) );
      promptText += " ";


      while ( true )
      {

        int optionIndex;

        var helpMode = false;
        var message = await ReadLine( promptText, token );


        if ( message.StartsWith( "?" ) )
        {
          message = message.Substring( 1 );
          helpMode = true;
        }


        if ( int.TryParse( message, out optionIndex ) )
        {
          if ( optionIndex > 0 && optionIndex <= options.Length )
          {
            var item = options[optionIndex - 1];

            if ( helpMode )
            {
              PlayerHost.WriteMessage( "{0}：{1}", item.Name, item.Description );
              continue;
            }

            return item;
          }
        }

        PlayerHost.WriteWarningMessage( "您输入的格式不正确，应该输入 {0} - {1} 之间的数字以选择对应序号的选项，输入 ?+数字 则可以查看对应选项的说明", 1, options.Length );
      }

    }




    /// <summary>
    /// 派生类实现此方法以实现 Choose 功能
    /// </summary>
    /// <param name="prompt">提示信息</param>
    /// <param name="options">选项列表</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待用户选择，并返回选择结果</returns>
    protected abstract Task<IOption> ChooseImplement( string prompt, IOption[] options, CancellationToken token );


    /// <summary>
    /// 让客户端在多个选项中选择一个
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="prompt">提示信息</param>
    /// <param name="options">选项列表</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待用户选择，并返回选择结果</returns>
    public async Task<T> Choose<T>( string prompt, T[] options, CancellationToken token ) where T : class, IOption
    {
      return (T) await Choose( prompt, (IOption[]) options, token );
    }



    /// <summary>
    /// 让客户端在多个选项中选择一个
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="prompt">提示信息</param>
    /// <param name="options">选项列表</param>
    /// <param name="defaultOption">默认选项</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待用户选择，并返回选择结果</returns>
    public Task<T> Choose<T>( string prompt, T[] options, T defaultOption, CancellationToken token ) where T : class, IOption
    {
      return Choose( prompt, options, defaultOption, DefaultTimeout, token );
    }

    /// <summary>
    /// 让客户端在多个选项中选择一个
    /// </summary>
    /// <typeparam name="T">选项类型</typeparam>
    /// <param name="prompt">提示信息</param>
    /// <param name="options">选项列表</param>
    /// <param name="defaultOption">默认选项</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待用户选择，并返回选择结果</returns>
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


    /// <summary>
    /// 派生类重写此方法获取默认超时时间
    /// </summary>
    protected TimeSpan DefaultTimeout { get { return TimeSpan.FromMinutes( 1 ); } }




    /// <summary>
    /// 定义从 IPlayerHost 到 PlayerConsoleBase 的隐式类型转换
    /// </summary>
    public static implicit operator PlayerConsoleBase( PlayerHostBase playerHost )
    {
      return playerHost.Console;
    }

    /// <summary>
    /// 定义从 GamePlayerBase 到 PlayerConsoleBase 的隐式类型转换
    /// </summary>
    public static implicit operator PlayerConsoleBase( GamePlayerBase player )
    {
      return player.PlayerHost.Console;
    }


  }
}
