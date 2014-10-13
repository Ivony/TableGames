using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicGamePlayer : GamePlayer
  {


    protected BasicGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {

      Game = (BasicGame) gameHost.Game;
      SyncRoot = new object();

    }



    public PlayerEffect[] Effects { get { return EffectCollection.ToArray(); } }


    protected IList<PlayerEffect> EffectCollection
    {
      get;
      private set;
    }



    public Task Play();



    protected BasicCard[] Cards
    {
      get { return CardCollection.Cast<BasicCard>().ToArray(); }
    }




    protected object SyncRoot { get; private set; }


    protected BasicGame Game { get; private set; }




    public int HealthPoint { get; set; }



    protected Task UseCard( int cardIndex, int targetIndex )
    {
      return UseCard( cardIndex, Game.GetPlayer( targetIndex ) );
    }


    protected Task UseCard( int cardIndex, BasicGamePlayer targetPLayer = null )
    {
      lock ( SyncRoot )
      {
        return UseCard( Cards[cardIndex], targetPLayer );
      }
    }


    protected Task UseCard( BasicCard card, BasicGamePlayer targetPlayer = null )
    {

      if ( card == null )
        throw new ArgumentNullException( "card" );

      lock ( SyncRoot )
      {
        if ( !CardCollection.Contains( card ) )
          throw new ArgumentException( "卡牌不属于当前玩家", "card" );

        CardCollection.Remove( card );
      }

      return card.UseCard( this, targetPlayer );
    }

    public void ApplyEffect( PlayerEffect effect )
    {

      lock ( SyncRoot )
      {
        foreach ( var item in effect.GetMutexInternal( Effects ) )
          EffectCollection.Remove( item );


        effect.Applied( this );
      }
    }

    protected virtual void EnsureEffectsAvailables()
    {
      lock ( SyncRoot )
      {
        EffectCollection = new List<PlayerEffect>( EffectCollection.Where( item => item.IsAvailable ) );
      }
    }

  }
}
