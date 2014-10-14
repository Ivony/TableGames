using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public class BasicGame<TPlayer> : Game where TPlayer : BasicGamePlayer
  {

    public BasicGame( string name )
      : base( name )
    {
    }


    protected TPlayer[] Players
    {
      get { return PlayerCollection.Cast<TPlayer>().ToArray(); }
    }


    internal TPlayer GetPlayer( int targetIndex )
    {
      return Players[targetIndex];
    }


  }
}
