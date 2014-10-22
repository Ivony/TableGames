using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public abstract class CardGamePlayer<TCard> : GamePlayerBase, IGameEventListener where TCard : Card
  {


    protected CardGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      SyncRoot = new object();
    }



    private ICardCollection _cardCollection = new CardCollection();
    /// <summary>
    /// 玩家所持有的卡牌集合
    /// </summary>
    protected virtual ICardCollection CardCollection
    {
      get { return _cardCollection; }
    }


    /// <summary>
    /// 玩家所持有的卡牌
    /// </summary>
    public TCard[] Cards { get { return CardCollection.Cast<TCard>().ToArray(); } }



    IEffectCollection _effects = new NotSupportEffectCollection();
    /// <summary>
    /// 获取玩家目前所有效果的集合
    /// </summary>
    public virtual IEffectCollection Effects
    {
      get { return _effects; }
    }





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
      return Task.Run( () => { } );
    }


    protected virtual Task PlayCard( TCard card, CancellationToken token )
    {
      return Task.Run( () => { } );
    }


    protected virtual Task OnAfterPlay( CancellationToken token )
    {
      return Task.Run( () => { } );
    }



    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    protected object SyncRoot { get; private set; }


    /// <summary>
    /// 获取或设置生命点数
    /// </summary>
    public int HealthPoint { get; set; }



    internal IBasicGame InternalGame { get { return (IBasicGame) GameHost.Game; } }

    public override void QuitGame()
    {
      InternalGame.OnPlayerQuitted( this );
      base.QuitGame();
    }




    public Task OnGameEvent( IGameEvent gameEvent )
    {
      var behaviorEvent = gameEvent as IGameBehaviorEvent;
      if ( behaviorEvent != null )
        return OnGameEvent( behaviorEvent );


      var playerEvent = gameEvent as IGamePlayerEvent;
      if ( playerEvent != null )
        return OnGameEvent( playerEvent );


      return Task.Run( () => { } );
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
