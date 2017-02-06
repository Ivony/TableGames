using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public class HealingCard : StandardCard, ISelfTarget
  {
    public HealingCard( int point = 2 )
    {
      HealingPoint = point;
    }
    public int HealingPoint { get; }
    public override string Description
    {
      get { return $"使用后可以增加 {HealingPoint} 点生命"; }
    }

    public override string Name
    {
      get { return "治疗"; }
    }

    public override Task UseCard( SimpleGamePlayer user, SimpleGamePlayer target, CancellationToken token )
    {
      AnnounceSpecialCardUsed( user );
      target.HealthPoint += HealingPoint;
      target.PlayerHost.WriteMessage( $"您的生命值恢复了 {HealingPoint} 点，目前生命值 {target.HealthPoint} 。" );
      return Task.CompletedTask;
    }
  }
}
