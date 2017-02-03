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
      var player = (SimpleGamePlayer) initiatePlayer;

      var target = await CherryTarget( player, token );
      await UseCard( player, (SimpleGamePlayer) target, token );
    }


    public abstract Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token );

  }
}
