﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwelveCards.Core.Cards
{
  public class SearchlightCard : SpecialCard, IEffectProvider
  {
    public override string Name
    {
      get { return "探照灯"; }
    }

    public override string Description
    {
      get { return "显示某个舱位的情况"; }
    }



    ///
    public EffectBase GetEffect( Player player, Cabin target )
    {
      return new SearchlightEffect( player, target );
    }
  }
}
