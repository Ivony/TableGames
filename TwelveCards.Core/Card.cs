using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwelveCards
{
  public abstract class Card : IEffectProvider
  {

    public abstract string Name { get; }


    public abstract string Description { get; }




    public abstract EffectBase GetEffect( Player player, Cabin target );
  }
}