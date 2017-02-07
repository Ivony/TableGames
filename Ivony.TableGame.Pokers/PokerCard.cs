using Ivony.TableGame.CardGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Pokers
{


  public enum PokerKind
  {
    /// <summary>王牌</summary>
    Joker,
    /// <summary>黑桃</summary>
    Spade,
    /// <summary>红心</summary>
    Heart,
    /// <summary>草花</summary>
    Club,
    /// <summary>方片</summary>
    Diamond,
  }

  public class PokerCard : Card
  {
    private PokerCard( PokerKind kind, int point )
    {
      Name = GetName( Kind = kind, Point = point );
    }

    private PokerCard( bool secondJoker )
    {
      Kind = PokerKind.Joker;
      Point = secondJoker ? -1 : 0;

      Name = GetName( Kind, Point );
    }


    private string GetName( PokerKind kind, int point )
    {

      string name;
      switch ( kind )
      {
        case PokerKind.Spade:
          name = "♠";
          break;
        case PokerKind.Heart:
          name = "♥";
          break;
        case PokerKind.Club:
          name = "♣";
          break;
        case PokerKind.Diamond:
          name = "♦";
          break;
        case PokerKind.Joker:

          if ( Point == 0 )
            return "JOKER";

          else
            return "joker";

        default:
          throw new InvalidOperationException();
      }




      if ( point == 11 )
        name += "J";

      else if ( point == 12 )
        name += "Q";

      else if ( point == 13 )
        name += "K";

      else if ( point == 1 )
        name += "A";

      else if ( point <= 10 && point > 1 )
        name += point;

      else
        throw new InvalidOperationException();



      return name;
    }


    public PokerKind Kind { get; }

    public int Point { get; }


    public override string Description
    {
      get { return Name; }
    }

    public override string Name { get; }



    /// <summary>
    /// 创建一整副扑克牌
    /// </summary>
    /// <param name="includeJokers">是否包含王牌</param>
    /// <returns>一副扑克牌</returns>
    public static PokerCard[] CreateCardSet( bool includeJokers = false )
    {

      var cards = new List<PokerCard>( includeJokers ? 54 : 52 );

      cards.AddRange( Enumerable.Range( 1, 13 ).Select( point => new PokerCard( PokerKind.Spade, point ) ) );
      cards.AddRange( Enumerable.Range( 1, 13 ).Select( point => new PokerCard( PokerKind.Heart, point ) ) );
      cards.AddRange( Enumerable.Range( 1, 13 ).Select( point => new PokerCard( PokerKind.Club, point ) ) );
      cards.AddRange( Enumerable.Range( 1, 13 ).Select( point => new PokerCard( PokerKind.Diamond, point ) ) );

      if ( includeJokers )
      {
        cards.Add( new PokerCard( false ) );
        cards.Add( new PokerCard( true ) );
      }

      return cards.ToArray();
    }
  }


}
