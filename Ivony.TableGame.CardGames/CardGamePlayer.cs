﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义卡牌游戏玩家的基本抽象
  /// </summary>
  public abstract class CardGamePlayer : GamePlayerBase
  {
    protected CardGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      Game = (CardGame) gameHost.Game;
      SyncRoot = new object();
    }


    /// <summary>
    /// 获取当前游戏对象
    /// </summary>
    protected new CardGame Game { get; private set; }

    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    protected object SyncRoot { get; private set; }



    /// <summary>
    /// 玩家进行该回合的操作
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个用于等待玩家处理完成的 Task</returns>
    public virtual async Task Play( CancellationToken token )
    {

      Game.AnnounceMessage( "轮到 {0} 出牌", PlayerName );

      await OnGameEvent( new GameAroundEvent( this ) );

      await OnBeforePlayCard( token );

      try
      {
        await PlayCard( token );
      }
      catch ( TimeoutException )
      {
      }

      await OnAfterPlayCard( token );

    }



    protected virtual Task OnBeforePlayCard( CancellationToken token )
    {
      return Task.CompletedTask;
    }


    protected abstract Task PlayCard( CancellationToken token );


    protected virtual Task OnAfterPlayCard( CancellationToken token )
    {
      return Task.CompletedTask;
    }



    /// <summary>
    /// 重写此方法退出游戏
    /// </summary>
    public override void QuitGame()
    {
      Game.OnPlayerQuitted( this );
      base.QuitGame();
    }



    public virtual Task OnGameEvent( IGameEvent gameEvent )
    {
      return Task.CompletedTask;
    }





  }
}
