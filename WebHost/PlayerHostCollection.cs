using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class PlayerHostCollection : KeyedCollection<Guid, PlayerHost>
  {
    protected override Guid GetKeyForItem( PlayerHost item )
    {
      return item.ID;
    }
  }
}