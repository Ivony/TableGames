using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.BasicCardGames
{
  public class CardUsingEvent<TPlayer, TCard> : GameEventBase, IGameBehaviorEvent
    where TPlayer : BasicGamePlayer<TCard>
    where TCard : BasicCard
  {

    public CardUsingEvent( TPlayer initiatePLayer, TPlayer recipientPlayer, TCard card )
    {
      InitiatePlayer = initiatePLayer;
      RecipientPlayer = recipientPlayer;
      Card = card;
    }

    public GamePlayerBase InitiatePlayer { get; private set; }

    public GamePlayerBase RecipientPlayer { get; private set; }

    public TCard Card { get; private set; }
  }
}
