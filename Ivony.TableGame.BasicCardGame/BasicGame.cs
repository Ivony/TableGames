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



    protected async override Task RunGame( CancellationToken token )
    {
      CardDealer = CreateCardDealer();


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




    public void PlayerQuitted( GamePlayer player )
    {

      lock ( SyncRoot )
      {
        if ( !PlayerCollection.Contains( player ) )                          //不存在这个玩家，则忽略。
          return;

        AnnounceSystemMessage( "玩家 {0} 退出了游戏", player.PlayerName );
        GameCancellationSource.Cancel();                                     //当有玩家退出时，强行终止游戏
      }
    }



    CardDealer IBasicGame.CardDealer
    {
      get { return CardDealer; }
    }


  }
}
