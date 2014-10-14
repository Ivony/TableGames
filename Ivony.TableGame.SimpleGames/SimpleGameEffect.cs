using Ivony.TableGame.Basics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public abstract class SimpleGameEffect : EffectBase
  {


    public SimpleGamePlayer Player { get; private set; }


    public SimpleGame Game { get; private set; }

    internal void Applied( SimpleGamePlayer player )
    {
      Player = player;
      Game = Player.Game;
    }





    public abstract Task<bool> OnAttack( SimpleGamePlayer user, SimpleGamePlayer target, int point );

  }
}
