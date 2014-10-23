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
