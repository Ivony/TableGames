using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义卡牌游戏的抽象
  /// </summary>
  public abstract class CardGame : GameBase
  {



    /// <summary>
    /// 创建 CardGame 对象
    /// </summary>
    /// <param name="gameHost">承载该游戏的游戏宿主</param>
    protected CardGame( IGameHost gameHost ) : base( gameHost ) { }



    /// <summary>
    /// 提供 RunGame 方法的标准实现，按照回合依次调用 Player 的 Play 方法，直到游戏被终止。
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected async override Task RunGame( CancellationToken token )
    {
      await EnsureAlready( token );

      AnnounceSystemMessage( "游戏开始" );

      Rounds = 1;

      while ( true )
      {
        await PlayAround( token );
      }
    }




    /// <summary>
    /// 当前回合数
    /// </summary>
    public int Rounds { get; private set; }


    /// <summary>
    /// 开始一个回合
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task，用于等待这个回合游戏结束</returns>
    protected virtual async Task PlayAround( CancellationToken token )
    {
      AnnounceSystemMessage( "第 {0} 回合", Rounds );
      foreach ( CardGamePlayer player in Players )
      {
        await PlayerPlay( player, token );
        token.ThrowIfCancellationRequested();
      }

      Rounds++;
    }



    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="player">轮到出牌的玩家</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task，用于等待这个玩家处理结束</returns>
    protected virtual async Task PlayerPlay( CardGamePlayer player, CancellationToken token )
    {
      await OnBeforePlay( player, token );
      await player.Play( token );
      await OnAfterPlay( player, token );
    }



    protected virtual Task OnBeforePlay( CardGamePlayer player, CancellationToken token )
    {
      return Task.CompletedTask;
    }


    protected virtual Task OnAfterPlay( CardGamePlayer player, CancellationToken token )
    {
      return Task.CompletedTask;
    }





    /// <summary>
    /// 确保所有的玩家都已经准备好了
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected async virtual Task EnsureAlready( CancellationToken token )
    {
      await Task.WhenAll( Players.Select( player => EnsureAlready( (CardGamePlayer) player, token ) ) );
    }


    /// <summary>
    /// 确保指定的玩家已经准备好
    /// </summary>
    /// <param name="player">要确认准备状态的玩家</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待玩家确认准备状态</returns>
    private async Task EnsureAlready( CardGamePlayer player, CancellationToken token )
    {
      player.PlayerHost.WriteSystemMessage( "游戏即将开始，在游戏进行中请不要关闭客户端或浏览器。" );
      var option = await player.PlayerHost.Console.Choose( null, new[] { new Option( "准备好了", "准备好可以进行游戏" ), new Option( "退出游戏", "退出这个游戏" ) }, token );

      if ( option.Name == "准备好了" )
        AnnounceSystemMessage( "{0} 已经准备好", player.PlayerName );

      else if ( option.Name == "退出游戏" )
        player.PlayerHost.TryQuitGame();

    }



    /// <summary>
    /// 强行终止游戏
    /// </summary>
    public void Abort()
    {
      GameCancellationSource.Cancel();
    }



    /// <summary>
    /// 当玩家退出游戏时，调用此方法
    /// </summary>
    /// <param name="player">退出游戏的玩家</param>
    public override void OnPlayerQuitted( GamePlayerBase player )
    {

      lock ( SyncRoot )
      {

        if ( GameState == GameState.NotInitialized )
          throw new InvalidOperationException();


        if ( !PlayerCollection.Contains( player ) )       //如果不存在这个玩家，则忽略。
          return;


        AnnounceSystemMessage( "玩家 {0} 退出了游戏", player.PlayerName );
        PlayerCollection.Remove( player );

        lock ( SyncRoot )
        {
          if ( !Players.Any() )                           //若已经没有玩家了，释放游戏资源
            ReleaseGame();

          if ( GameState == GameState.Running )           //如果游戏正在进行，则强行终止游戏。
            Abort();
        }
      }
    }



    /// <summary>
    /// 广播一个游戏事件
    /// </summary>
    /// <param name="gameEvent">要广播的游戏事件</param>
    /// <returns>用于等待事件广播完毕的 Task 对象</returns>
    public virtual async Task SendGameEvent( IGameEvent gameEvent )
    {

      var parallelEvent = gameEvent as IParallelGameEvent;


      if ( parallelEvent != null )
      {
        //并行广播游戏事件
        var tasks = Players.Cast<CardGamePlayer>().Select( item => item.OnGameEvent( parallelEvent ) ).ToArray();
        await Task.WhenAll( tasks );
      }
      else
      {
        foreach ( var player in Players.Cast<CardGamePlayer>() )
          await player.OnGameEvent( gameEvent );
      }

      var handledEvent = gameEvent as IGameNeedHandledEvent;
      if ( handledEvent != null )
      {
        await handledEvent.HandleEvent();
      }
    }



    /// <summary>
    /// 获取参与此游戏客户端必须支持的特性列表
    /// </summary>
    public override IEnumerable<string> GetRequiredFeatures()
    {
      return base.GetRequiredFeatures().Union( new[] { "OptionsResponding" } );
    }
  }
}
