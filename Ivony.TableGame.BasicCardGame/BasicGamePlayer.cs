using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicGamePlayer<TCard> : GamePlayer where TCard : BasicCard
  {


    protected BasicGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {

      SyncRoot = new object();
    }




    public new TCard[] Cards { get { return CardCollection.Cast<TCard>().ToArray(); } }





    public async virtual Task Play( CancellationToken token )
    {

      GameHost.Game.AnnounceMessage( "轮到 {0} 出牌", CodeName );

      await OnBeforePlay( token );
      await PlayCard( await CherryCard( token ), token );

      await OnAfterPlay( token );

    }

    private async Task<TCard> CherryCard( CancellationToken token )
    {

      var timeoutToken = new CancellationTokenSource( TimeSpan.FromMinutes( 1 ) ).Token;
      try
      {
        return (TCard) await PlayerHost.Console.Choose( "请出牌：", Cards,
          CancellationTokenSource.CreateLinkedTokenSource( token, timeoutToken ).Token );
      }
      catch ( TaskCanceledException )
      {
        if ( token.IsCancellationRequested )
          throw;

        lock ( SyncRoot )
        {
          var index = Random.Next( Cards.Length );
          PlayerHost.WriteWarningMessage( "操作超时，随机打出第 {0} 张牌", index );
          return Cards[index];
        }
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

    public override void Release()
    {
      InternalGame.ReleasePlayer( this );
      base.Release();
    }


  }
}
