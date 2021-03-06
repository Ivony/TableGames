﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 诅咒卡
  /// </summary>
  public class CurseCard : ElementAttachmentCard, IOtherPlayerTarget
  {
    public override string Description
    {
      get { return "和五行牌配合使用，对某个玩家进行诅咒"; }
    }

    public override string Name
    {
      get { return "诅咒"; }
    }

    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {

      if ( Element == null )
        throw new InvalidOperationException();

      if ( target.Curse( user, new CurseEffect( Element ) ) == false )
        user.PlayerHost.WriteWarningMessage( $"你完成了诅咒的所有工作，但是，它似乎没有奏效。" );

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
