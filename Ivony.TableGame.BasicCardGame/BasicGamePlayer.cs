using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.Basics
{
  public abstract class BasicGamePlayer<TCard> : GamePlayer where TCard : BasicCard
  {


    protected BasicGamePlayer( string codeName, IGameHost gameHost, IPlayerHost playerHost )
      : base( codeName, gameHost, playerHost )
    {

      SyncRoot = new object();
    }




    public TCard[] Cards { get { return CardCollection.Cast<TCard>().ToArray(); } }





    public async virtual Task Play()
    {

      GameHost.Game.AnnounceMessage( "轮到 {0} 出牌", CodeName );

      await OnBeforePlay();
      await PlayCard( await CherryCard() );

      await OnAfterPlay();

    }

    private async Task<TCard> CherryCard()
    {
      do
      {
        string commandText = null;

        commandText = await PlayerHost.Console.ReadLine( "请出牌： ", null );

        if ( commandText == null )
        {
          GameHost.Game.AnnounceSystemMessage( "{0} 操作超时", CodeName );
          commandText = new[] { "1", "2", "3", "4", "5" }.RandomItem();
          PlayerHost.WriteWarningMessage( "操作超时，随机打出第 {0} 张牌", commandText );
        }


        int index;
        try
        {
          index = ParseCardIndex( commandText );
          return Cards[index - 1];
        }
        catch ( FormatException )
        {
          PlayerHost.WriteMessage( "输入的命令格式错误" );
        }

      } while ( true );
    }

    private int ParseCardIndex( string text )
    {

      int cardIndex;

      if ( !int.TryParse( text, out cardIndex ) || cardIndex > 5 || cardIndex < 1 )
        throw new FormatException();

      return cardIndex;
    }



    protected async virtual Task OnBeforePlay()
    {

    }


    protected async virtual Task PlayCard( TCard card )
    {
    }


    protected async virtual Task OnAfterPlay()
    {
    }



    /// <summary>
    /// 给玩家发牌
    /// </summary>
    protected void DealCards( int amount )
    {
      if ( amount <= 0 )
        return;

      lock ( SyncRoot )
      {
        CardCollection.AddRange( ((IBasicGame) GameHost.Game).CardDealer.DealCards( amount ) );

        ArrangeCards();
      }
    }

    protected virtual void ArrangeCards() { }


    protected object SyncRoot { get; private set; }



    public int HealthPoint { get; set; }




  }
}
