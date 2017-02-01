using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public abstract class ElementAttachmentCard : BasicCard
  {

    public Element Element { get; private set; }


    public void WithElement( Element element )
    {
      Element = element;
    }
  }
}
