using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public class BasicGame : Game
  {

    public BasicGame( string name )
      : base( name )
    {
      GlobalEffectCollection = new List<GlobalEffect>();
    }



    public GlobalEffect[] GlobalEffects
    {
      get
      {

        lock ( SyncRoot )
        {
          EnsureEffectsAvailables();
          return GlobalEffectCollection.ToArray();
        }
      }
    }

    protected void EnsureEffectsAvailables()
    {
      lock ( SyncRoot )
      {
        GlobalEffectCollection = new List<GlobalEffect>( GlobalEffectCollection.Where( item => item.IsAvailable ) );
      }
    }

    protected IList<GlobalEffect> GlobalEffectCollection
    {
      get;
      private set;
    }




    protected BasicGamePlayer[] Players
    {
      get { return PlayerCollection.Cast<BasicGamePlayer>().ToArray(); }
    }


    internal BasicGamePlayer GetPlayer( int targetIndex )
    {
      return Players[targetIndex];
    }
  }
}
