using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwelveCards.Core
{
  public abstract class Card
  {

    public abstract string Name { get; }


    public abstract string Description { get; }



    public abstract void Action( Game game, Player initiator, Cabin target )
    {
      game.Announce( "{0} 对 {1} 号舱位，使用了 {2} 。", initiator.CodeName, target.Index, this.Name );
      throw new NotImplementedException();
    }
  }
}
