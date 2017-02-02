using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public abstract class BasicCard : SimpleGameCard
  {
    public override async Task Play( CardGamePlayer initiatePlayer, CancellationToken token )
    {

      var target = await CherryTarget( (SimpleGamePlayer) initiatePlayer, token );
      if ( target == null )
        throw new TimeoutException();

      var user = (SimpleGamePlayer) initiatePlayer;
      var targetPlayer = (SimpleGamePlayer) target;
      await UseCard( user, targetPlayer );
    }


    public abstract Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target );

  }
}
