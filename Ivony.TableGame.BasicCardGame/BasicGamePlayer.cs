using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public abstract class BasicGamePlayer<TCard> : GamePlayerBase where TCard : BasicCard
  {


    protected BasicGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {

      SyncRoot = new object();
    }




    public new TCard[] Cards { get { return CardCollection.Cast<TCard>().ToArray(); } }





    public async virtual Task Play( CancellationToken token )
    {

      Game.AnnounceMessage( "轮到 {0} 出牌", PlayerName );

      await OnBeforePlay( token );
      var card = await CherryCard( token );

      
      await PlayCard( card, token );
      await OnAfterPlay( token );

    }

    private async Task<TCard> CherryCard( CancellationToken token )
    {

      var card = await PlayerHost.Console.Choose( "请出牌：", Cards, null, token );

      if ( card != null )
        return card;


      lock ( SyncRoot )
      {
        var index = Random.Next( Cards.Length );
        PlayerHost.WriteWarningMessage( "操作超时，随机打出第 {0} 张牌", index + 1 );
        return Cards[index];
      }

    }


    protected async virtual Task OnBeforePlay( CancellationToken token )
    {

    }


    protected async virtual Task PlayCard( TCard card, CancellationToken token )
    {
    }


    protected async virtual Task OnAfterPlay( CancellationToken token )
    {
    }



    /// <summary>
    /// 给玩家发牌
    /// </summary>
    protected void DealCards( int amount )
    {
      if ( amount <= 0 )
        return;

      lock ( SyncRoot )
      {
        CardCollection.AddRange( InternalGame.CardDealer.DealCards( amount ) );

        ArrangeCards();
      }
    }

    protected virtual void ArrangeCards() { }


    protected object SyncRoot { get; private set; }



    public int HealthPoint { get; set; }



    internal new IBasicGame InternalGame { get { return (IBasicGame) GameHost.Game; } }

    public override void QuitGame()
    {
      InternalGame.OnPlayerQuitted( this );
      base.QuitGame();
    }


  }
}
