using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ivony.TableGame.CardGames;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class BlessCard : ElementAttachmentCard, ISelfTarget
  {
    public override string Name => "祝福";

    public override string Description => "与五行牌配合使用达成各种祝福效果";

    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

      if ( Element == null )
        throw new InvalidOperationException();

      target.SetEffect( new CurseEffect( Element ) );
      AnnounceSpecialCardUsed( user );

      return Task.CompletedTask;

    }

    public override bool Availables( SimpleGamePlayer player )
    {
      return false;
    }

    internal override bool AvailablesWithElement( SimpleGamePlayer player )
    {
      return base.Availables( player );
    }
  }
}
