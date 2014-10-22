using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供基本的卡牌游戏实现
  /// </summary>
  /// <typeparam name="TPlayer"></typeparam>
  public abstract class CardGame<TPlayer> : CardGameBase, IBasicGame
    where TPlayer : CardGamePlayer
  {

    public CardGame( IGameHost gameHost ) : base( gameHost ) { }


    /// <summary>
    /// 获取所有玩家
    /// </summary>
    public new TPlayer[] Players
    {
      get { return PlayerCollection.Cast<TPlayer>().ToArray(); }
    }





    protected override async Task Play( CardGamePlayerBase player, CancellationToken token )
    {
      await base.Play( player, token );

      foreach ( var dead in Players.Where( item => item.HealthPoint <= 0 ).ToArray() )
      {
        dead.Dead();
      }
    }




    /// <summary>
    /// 广播一个游戏事件
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    public virtual async Task OnGameEvent( IGameEvent gameEvent )
    {

      foreach ( var player in Players )
        await player.OnGameEvent( gameEvent );
    }






    private IEffectCollection _effects = new NotSupportEffectCollection();

    /// <summary>
    /// 获取全局效果列表
    /// </summary>
    protected virtual IEffectCollection Effects
    {
      get { return _effects; }
    }



    /// <summary>
    /// 当玩家退出游戏时，调用此方法
    /// </summary>
    /// <param name="player">退出游戏的玩家</param>
    public virtual void OnPlayerQuitted( GamePlayerBase player )
    {

      lock ( SyncRoot )
      {

        if ( GameState == GameState.NotInitialized )
          throw new InvalidOperationException();


        if ( !PlayerCollection.Contains( player ) )                          //如果不存在这个玩家，则忽略。
          return;


        AnnounceSystemMessage( "玩家 {0} 退出了游戏", player.PlayerName );
        PlayerCollection.Remove( player );


        if ( GameState == GameState.Running )
          GameCancellationSource.Cancel();                                   //如果游戏正在进行，则强行终止游戏。
      }
    }

  }
}
