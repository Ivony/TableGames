using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  public sealed class Element
  {

    private Element( string name )
    {
      Name = name;
    }

    public string Name { get; }


    public static readonly Element 金 = new Element( "金" );
    public static readonly Element 木 = new Element( "木" );
    public static readonly Element 水 = new Element( "水" );
    public static readonly Element 火 = new Element( "火" );
    public static readonly Element 土 = new Element( "土" );


    public override string ToString()
    {
      return Name;
    }
  }
}
