using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{
  /// <summary>
  /// 定义五行元素
  /// </summary>
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




    /// <summary>
    /// 判断元素与另一个元素是否相生
    /// </summary>
    /// <param name="element">要判断的另一个元素</param>
    /// <returns>是否相生</returns>
    public bool IsReinforce( Element element )
    {
      return IsReinforce( this, element );
    }

    /// <summary>
    /// 判断两个元素是否相生
    /// </summary>
    /// <param name="element1">元素1</param>
    /// <param name="element2">元素2</param>
    /// <returns>是否相生</returns>
    public static bool IsReinforce( Element element1, Element element2 )
    {
      return element1 == 金 && element2 == 水
        || element1 == 水 && element2 == 木
        || element1 == 木 && element2 == 火
        || element1 == 火 && element2 == 土
        || element1 == 土 && element2 == 金;
    }




    /// <summary>
    /// 判断元素与另一个元素是否相克
    /// </summary>
    /// <param name="element">要判断的另一个元素</param>
    /// <returns>是否相克</returns>
    public bool IsCounteract( Element element )
    {
      return IsCounteract( this, element );
    }

    /// <summary>
    /// 判断两个元素是否相克
    /// </summary>
    /// <param name="element1">元素1</param>
    /// <param name="element2">元素2</param>
    /// <returns>是否相克</returns>
    public static bool IsCounteract( Element element1, Element element2 )
    {
      return element1 == 火 && element2 == 金
        || element1 == 金 && element2 == 木
        || element1 == 木 && element2 == 土
        || element1 == 土 && element2 == 水
        || element1 == 水 && element2 == 火;

    }



    public override string ToString()
    {
      return Name;
    }

  }
}
