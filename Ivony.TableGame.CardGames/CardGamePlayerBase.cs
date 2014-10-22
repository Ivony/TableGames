using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 定义卡牌游戏玩家的基本抽象
  /// </summary>
  public abstract class CardGamePlayerBase : GamePlayerBase
  {
    protected CardGamePlayerBase( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      SyncRoot = new object();
    }


    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    protected object SyncRoot { get; private set; }


    private ICardCollection _cardCollection = new CardCollection();
    /// <summary>
    /// 获取玩家卡牌容器
    /// </summary>
    protected virtual ICardCollection CardCollection
    {
      get { return _cardCollection; }
    }


    /// <summary>
    /// 玩家当前所持有的卡牌
    /// </summary>
    public virtual Card[] Cards
    {
      get { return CardCollection.ToArray(); }
    }



    public virtual async Task Play( CancellationToken token )
    {

      Game.AnnounceMessage( "轮到 {0} 出牌", PlayerName );

      await OnBeforePlayCard( token );

      await PlayCard( token );

      await OnAfterPlayCard( token );

    }



    protected virtual Task OnBeforePlayCard( CancellationToken token )
    {
      return Task.Run( () => { } );
    }


    protected abstract Task PlayCard( CancellationToken token );


    protected virtual Task OnAfterPlayCard( CancellationToken token )
    {
      return Task.Run( () => { } );
    }






  }
}
