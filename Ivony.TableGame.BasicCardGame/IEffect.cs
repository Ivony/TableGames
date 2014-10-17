using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public interface IEffect
  {

    string Name { get; }

    string Description { get; }

  }
}
