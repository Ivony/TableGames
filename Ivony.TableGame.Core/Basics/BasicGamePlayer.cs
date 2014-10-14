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

      SyncRoot = new object();

    }


    public abstract Task Play();



    protected object SyncRoot { get; private set; }



    public int HealthPoint { get; set; }




  }
}
