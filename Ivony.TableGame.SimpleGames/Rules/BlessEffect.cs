using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 定义祝福效果
  /// </summary>
  public class BlessEffect : SimpleGameEffect
  {

    /// <summary>
    /// 元素
    /// </summary>
    public Element Element { get; }


    public BlessEffect( Element element )
    {
      Element = element;
    }



    public override string Description
    {
      get
      {
        if ( Element == Element.金 )
          return "金之祝福，使用者五回合内遇到攻击伤害大于 1 时，只减损 1 点 HP，攻击伤害等于 1 时，有 50% 的概率不掉血";
        else if ( Element == Element.木 )
          return "木之祝福，使用者五回合内每回合加 1 HP";
        else if ( Element == Element.水 )
          return "水之祝福，使用者五回合内不被反噬伤害，不被诅咒，现有诅咒效果立即消除。";
        else if ( Element == Element.火 )
          return "火之祝福，使用者五回合内攻击伤害 + 1。";
        else if ( Element == Element.土 )
          return "土之祝福，使用者五回合内不受超时闪电伤害，使用盾牌时有 50% 的机会不消耗行动值。";

        throw new InvalidOperationException();
      }
    }

    public override string Name
    {
      get
      {
        return Element.Name + "之祝福";
      }
    }
  }
}
