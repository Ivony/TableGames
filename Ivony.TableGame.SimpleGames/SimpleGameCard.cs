using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameCard : Card
  {

    public abstract Task Execute( SimpleGamePlayer user, SimpleGamePlayer target );
  }
}
