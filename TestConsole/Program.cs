using Ivony.TableGame.Pokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
  class Program
  {
    static void Main( string[] args )
    {

      Console.OutputEncoding = Encoding.UTF8;

      var cards = PokerCard.CreateCardSet( true );

      foreach ( var item in cards )
      {
        Console.Write( $"{item.Name,-8}" );
      }

      Console.ReadKey();

    }
  }
}
