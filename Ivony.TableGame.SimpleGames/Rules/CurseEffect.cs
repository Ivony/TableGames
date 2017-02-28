using System;

namespace Ivony.TableGame.SimpleGames.Rules
{

  /// <summary>
  /// 诅咒效果
  /// </summary>
  internal class CurseEffect : SimpleGameEffect
  {
    /// <summary>
    /// 元素
    /// </summary>
    public Element Element { get; }

    public CurseEffect( Element element )
    {
      Element = element;
    }

    public override string Description
    {
      get
      {
        if ( Element == Element.金 )
          return "金之诅咒，被诅咒者五回合内无法摸到盾牌";
        else if ( Element == Element.木 )
          return "木之诅咒，被诅咒者五回合内无法摸到治疗牌，也无法通过任何途径获得 HP 增加。";
        else if ( Element == Element.水 )
          return "水之诅咒，被诅咒者五回合内每回合扣一点血";
        else if ( Element == Element.火 )
          return "火之诅咒，被诅咒着五回合内无法摸到攻击，也不能使用攻击。";
        else if ( Element == Element.土 )
          return "土之诅咒，被诅咒者五回合内有30%的概率有且仅有一次出牌无效。";

        throw new InvalidOperationException();
      }
    }

    public override string Name
    {
      get { return Element.Name + "之诅咒"; }
    }
  }
}