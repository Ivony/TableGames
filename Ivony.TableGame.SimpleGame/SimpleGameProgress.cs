using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  class SimpleGameProgress : GameProgress
  {
    public SimpleGameProgress( SimpleGame game )
      : base( game )
    {
    }

    public override Task<bool> NextStep()
    {
      throw new NotImplementedException();
    }
  }
}
