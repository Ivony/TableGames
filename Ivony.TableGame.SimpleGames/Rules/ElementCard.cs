using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Ivony.TableGame.SimpleGames.Rules
{


  /// <summary>
  /// 定义元素牌
  /// </summary>
  public class ElementCard : SimpleGameCard
  {

    public ElementCard( Element element )
    {

      Element = element;

    }

    public override string Name
    {
      get { return Element.Name; }
    }

    public override string Description
    {
      get { return Element.Name + "元素"; }
    }

    public Element Element { get; }


    public override async Task Play( CardGamePlayer initiatePlayer, CancellationToken token )
    {

      var player = (SimpleGamePlayer) initiatePlayer;

      var options = player.Cards.Cast<SimpleGameCard>()
        .Select( item => Option.Create( item, item.Name, item.Description, item is ElementAttachmentCard == false ) )
        .ToArray();

      var card = (ElementAttachmentCard) await initiatePlayer.PlayerHost.Console.Choose( "请选择要使用的卡牌：", options, null, token );
      if ( card == null )
        throw new TimeoutException();

      card.WithElement( Element );
      try
      {
        await player.PlayCard( card, token );
      }
      catch ( TimeoutException )
      {
        card.WithElement( null );
        throw;
      }
    }


  }
}
