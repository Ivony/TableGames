using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public class PlayerConsole : PlayerConsoleBase
  {

    private List<GameMessage> _messages = new List<GameMessage>();

    public object SyncRoot { get; private set; }


    public new PlayerHost PlayerHost { get; private set; }

    public PlayerConsole( PlayerHost playerHost )
      : base( playerHost )
    {
      PlayerHost = playerHost;
      SyncRoot = new object();
    }


    protected override void WriteMessageImplement( GameMessage message )
    {
      _messages.Add( message );
    }

    public override async Task<string> ReadLine( string prompt, CancellationToken token )
    {
      return await new TextMessageResponding( PlayerHost, prompt, token ).RespondingTask.ConfigureAwait( false );
    }



    protected override async Task<Option> ChooseImplement( string prompt, Option[] options, CancellationToken token )
    {
      return await new OptionsResponding( PlayerHost, prompt, options, token ).RespondingTask.ConfigureAwait( false );
    }



    private int index = 0;

    /// <summary>
    /// 设置最后一次收取的消息位置
    /// </summary>
    /// <param name="messageIndex"></param>
    internal void SetMessageIndex( int messageIndex )
    {
      index = messageIndex;
    }


    /// <summary>
    /// 最后一次收取的消息位置
    /// </summary>
    internal int LastMesageIndex
    {
      get;
      private set;
    }

    /// <summary>
    /// 获取所有未收取的消息
    /// </summary>
    /// <returns></returns>
    public GameMessage[] GetMessages()
    {
      lock ( SyncRoot )
      {
        LastMesageIndex = _messages.Count;
        if ( index > LastMesageIndex )
          return new GameMessage[0];

        return _messages.GetRange( index, LastMesageIndex - index ).ToArray();
      }
    }


  }
}