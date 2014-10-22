using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{
  public abstract class StandardCard : Card
  {

    public abstract int ActionPoint { get; }

    public abstract Task Play( CardGamePlayerBase i, CardGamePlayerBase t );

    public abstract StandardCard Combine( params Card[] cards );

  }
}
