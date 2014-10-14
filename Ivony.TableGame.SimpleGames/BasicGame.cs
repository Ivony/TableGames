using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicGame<TPlayer, TCard> : Game, IBasicGame
    where TPlayer : BasicGamePlayer<TCard>
    where TCard : BasicCard
  {

    public BasicGame( string name )
      : base( name )
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




    public async override Task Run()
    {
      CardDealer = CreateCardDealer();


      AnnounceSystemMessage( "游戏开始" );

      int turn = 1;

      while ( true )
      {

        AnnounceSystemMessage( "第 {0} 回合", turn++ );

        foreach ( TPlayer player in Players )
        {
          await player.Play();


          var dead = Players.FirstOrDefault( item => item.HealthPoint <= 0 );
          if ( dead != null )
          {
            AnnounceSystemMessage( "玩家 {0} 已经阵亡，游戏结束", dead.CodeName );
            return;
          }
        }
      }
    }




    CardDealer IBasicGame.CardDealer
    {
      get { return CardDealer; }
    }
  }
}
