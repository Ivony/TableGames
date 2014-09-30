using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards
{
  public interface IPlayerHost
  {

    PlayerConsoleBase Console { get; }

  }
}
