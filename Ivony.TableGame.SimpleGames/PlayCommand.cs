using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.SimpleGames
{
  public class PlayCommand
  {

    public PlayCommand( SimpleGamePlayer player, SimpleGameCard card, SimpleGamePlayer targetPlayer )
    {
      Player = player;
      Card = card;
      TargetPlayer = targetPlayer;
    }


    public SimpleGamePlayer Player { get; private set; }


    public SimpleGameCard Card { get; private set; }


    public SimpleGamePlayer TargetPlayer { get; private set; }


    public Task Execute()
    {
      return Card.Execute( Player, TargetPlayer );
    }
  }
}
