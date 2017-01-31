using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ivony.TableGame;
using Ivony.TableGame.CardGames;
using Ivony.TableGame.SimpleGames.Rules;
using System.Threading;

namespace Ivony.TableGame.SimpleGames
{
  public class SimpleGamePlayer : StandardCardGamePlayer<SimpleGameCard>
  {


    /// <summary>
    /// 创建 SimpleGamePlayer 对象
    /// </summary>
    /// <param name="gameHost">游戏宿主</param>
    /// <param name="playerHost">玩家宿主</param>
    public SimpleGamePlayer( IGameHost gameHost, IPlayerHost playerHost )
      : base( gameHost, playerHost )
    {
      Game = (SimpleGame) gameHost.Game;
      HealthPoint = 25;

    }


    /// <summary>
    /// 获取当前参加的游戏对象
    /// </summary>
    public new SimpleGame Game
    {
      get;
      private set;
    }



    private SimpleGameCardCollection _cards = new SimpleGameCardCollection();
    /// <summary>
    /// 重写此属性自定义卡牌集
    /// </summary>
    protected override ICardCollection<SimpleGameCard> CardCollection { get { return _cards; } }




    protected override async Task PlayCard( CancellationToken token )
    {
      ActionPoint = 1;
      DealCards();


      await base.PlayCard( token );
    }





    /// <summary>
    /// 重写 GetGameInformation 方法获取该玩家可以看到的游戏信息
    /// </summary>
    /// <returns>游戏信息对象</returns>
    public override object GetGameInformation()
    {
      return new
      {
        Players = Game.Players.Select( item => item.PlayerName ),
        Cards = CardCollection,
      };
    }




    protected override async Task<object> CherryTarget( SimpleGameCard card, CancellationToken token )
    {


      var targets = GetTargets( card );

      if ( targets == null || targets.Any() == false )
        return null;

      if ( targets.Length == 1 )
        return targets[0];

      var result = await PlayerHost.Console.Choose( "请选择使用对象：", targets.Select( item => Option.Create( item ) ).ToArray(), null, token );
      if ( result == null )
      {
        PlayerHost.WriteWarningMessage( "操作超时" );
        result = targets.RandomItem();
      }

      return result;



    }

    private object[] GetTargets( SimpleGameCard card )
    {
      if ( card is IOtherPlayerTarget )
        return Game.Players.Where( item => item != this ).ToArray();

      else if ( card is IAnyPlayerTarget )
        return Game.Players;

      else if ( card is ISelfTarget )
        return new[] { this };

      else
        return null;

    }



    /// <summary>
    /// 给玩家发牌
    /// </summary>
    public void DealCards()
    {
      _cards.DealCards();
    }


    /// <summary>
    /// 移除所有效果
    /// </summary>
    internal void Purify()
    {
      foreach ( var effect in Effects )
      {

        if ( Effects.RemoveEffect( effect ) )
          PlayerHost.WriteWarningMessage( "您当前的 {0} 效果已经被解除", effect.Name );

      }
    }

    internal void ClearCards()
    {
      CardCollection.Clear();
      PlayerHost.WriteMessage( "您手上的卡牌已经清空，请等待下次发牌" );
    }



    private SimpleGamePlayerEffectCollection _effects = new SimpleGamePlayerEffectCollection();

    public override IEffectCollection Effects
    {
      get { return _effects; }
    }


    internal void SetEffect( SimpleGameEffect effect )
    {
      Effects.AddEffect( effect );
    }
  }
}
