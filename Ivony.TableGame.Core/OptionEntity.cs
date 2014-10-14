using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public sealed class OptionEntity : IOption
  {

    public OptionEntity( IOption option )
    {
      Name = option.Name;
      Description = option.Description;
    }


    public string Name { get; private set; }

    public string Description { get; private set; }
  }
}
