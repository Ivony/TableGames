using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicGame<TPlayer, TCard> : GameBase, IBasicGame
    where TPlayer : BasicGamePlayer<TCard>
    where TCard : BasicCard
  {

    public BasicGame( IGameHost gameHost )
      : base( gameHost )
    {
    }


    /// <summary>
    /// 获取所有玩家
    /// </summary>
    public new TPlayer[] Players
    {
      get { return PlayerCollection.Cast<TPlayer>().ToArray(); }
    }


    internal TPlayer GetPlayer( int targetIndex )
    {
      return Players[targetIndex];
    }




    protected abstract CardDealer CreateCardDealer();


    protected CardDealer CardDealer { get; private set; }



    /// <summary>
    /// 提供 RunGame 方法的标准实现，按照回合依次调用 Player 的 Play 方法，直到某个玩家的 HP 低于 0 为止。
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected async override Task RunGame( CancellationToken token )
    {
      CardDealer = CreateCardDealer();


      await EnsureAlready( token );

      AnnounceSystemMessage( "游戏开始" );

      int turn = 1;

      while ( true )
      {

        AnnounceSystemMessage( "第 {0} 回合", turn++ );

        foreach ( TPlayer player in Players )
        {
          await player.Play( token );

          token.ThrowIfCancellationRequested();

          var dead = Players.FirstOrDefault( item => item.HealthPoint <= 0 );
          if ( dead != null )
          {
            AnnounceSystemMessage( "玩家 {0} 已经阵亡，游戏结束", dead.PlayerName );
            return;
          }
        }
      }
    }

    protected async virtual Task EnsureAlready( CancellationToken token )
    {
      await Task.WhenAll( Players.Select( player => EnsureAlready( player, token ) ) );
    }

    private async Task EnsureAlready( TPlayer player, CancellationToken token )
    {
      string message;
      do
      {
        message = await player.PlayerHost.Console.ReadLine( "游戏即将开始，在游戏进行中请不要关闭客户端或浏览器。如果您已经准备好开始游戏，请输入 Ready，若您要退出游戏，请输入 Quit", "quit", token );


        if ( string.Equals( message, "Ready", StringComparison.OrdinalIgnoreCase ) )
          AnnounceSystemMessage( "{0} 已经准备好", player.PlayerName );

        else if ( string.Equals( message, "Quit", StringComparison.OrdinalIgnoreCase ) )
          player.PlayerHost.TryQuitGame();

        else
          continue;


      } while ( false );
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



    CardDealer IBasicGame.CardDealer
    {
      get { return CardDealer; }
    }


  }
}
