using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{
  public static class RandomExtensions
  {

    private static readonly Random random = new Random( DateTime.Now.Millisecond );

    public static T RandomItem<T>( this T[] array )
    {
      return array[random.Next( array.Length )];
    }

  }
}
