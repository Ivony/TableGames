using Ivony.TableGame;
using Ivony.TableGame.SimpleGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Game( typeof( GameFactory ) )]

namespace Ivony.TableGame.SimpleGames
{
  public class GameFactory : IGameFactory
  {
    string IGameFactory.GameName => "SimpleGame";

    string IGameFactory.GameDescription => "SimpleGame is a simple demo for card game.";


    GameBase IGameFactory.CreateGame()
    {
      return new SimpleGame();
    }
  }

}