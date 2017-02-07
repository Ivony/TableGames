using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class GameHostCollection : KeyedCollection<string, GameRoom>
  {
    protected override string GetKeyForItem( GameRoom item )
    {
      return item.RoomName;
    }
  }
}