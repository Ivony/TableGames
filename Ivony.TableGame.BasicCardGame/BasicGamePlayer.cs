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


    protected virtual Task OnBeforePlay( CancellationToken token )
    {
      return null;
    }


    protected virtual Task PlayCard( TCard card, CancellationToken token )
    {
      return null;
    }


    protected virtual Task OnAfterPlay( CancellationToken token )
    {
      return null;
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

    /// <summary>
    /// 派生类实现此方法对卡牌进行整理
    /// </summary>
    protected virtual void ArrangeCards() { }


    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    protected object SyncRoot { get; private set; }


    /// <summary>
    /// 获取或设置生命点数
    /// </summary>
    public int HealthPoint { get; set; }


    IEffectCollection _effects = new EffectCollection();

    protected virtual IEffectCollection Effects
    {
      get { return _effects; }
    }




    internal new IBasicGame InternalGame { get { return (IBasicGame) GameHost.Game; } }

    public override void QuitGame()
    {
      InternalGame.OnPlayerQuitted( this );
      base.QuitGame();
    }




    public override Task OnGameEvent( IGameEvent gameEvent )
    {
      var behaviorEvent = gameEvent as IGameBehaviorEvent;
      if ( behaviorEvent != null )
        return OnGameEvent( behaviorEvent );


      var playerEvent = gameEvent as IGamePlayerEvent;
      if ( playerEvent != null )
        return OnGameEvent( playerEvent );


      return null;
    }


    protected virtual async Task OnGameEvent( IGamePlayerEvent playerEvent )
    {
      if ( playerEvent.Player.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGamePlayerEffect>() )
          await item.OnGamePlayerEvent( playerEvent );
      }
    }

    protected virtual async Task OnGameEvent( IGameBehaviorEvent behaviorEvent )
    {

      if ( behaviorEvent.InitiatePlayer.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGameBehaviorEffect>() )
          await item.OnBehaviorInitiator( behaviorEvent );
      }

      if ( behaviorEvent.RecipientPlayer.Equals( this ) )
      {
        foreach ( var item in Effects.OfType<IGameBehaviorEffect>() )
          await item.OnBehaviorRecipient( behaviorEvent );
      }
    }



  }
}
