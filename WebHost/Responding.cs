using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Ivony.TableGame.WebHost
{


  /// <summary>
  /// 定义客户端的响应
  /// </summary>
  internal interface IResponding
  {


    /// <summary>
    /// 获取响应类型
    /// </summary>
    string Type { get; }

    /// <summary>
    /// 获取需要告知客户端的响应信息
    /// </summary>
    /// <returns>响应信息</returns>
    JObject GetInfo();

    /// <summary>
    /// 提示文字，提示客户端应当产生何种响应格式
    /// </summary>
    string PromptText { get; }

    /// <summary>
    /// 当客户端响应信息时调用此方法
    /// </summary>
    /// <param name="message">响应信息</param>
    void OnResponse( string message );


    /// <summary>
    /// 响应标识
    /// </summary>
    Guid RespondingID { get; }


    /// <summary>
    /// 是否已经完成
    /// </summary>
    bool IsCompleted { get; }
  }


  /// <summary>
  /// 辅助实现 IResponding
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Responding<T> : IResponding
  {



    /// <summary>
    /// 获取响应类型
    /// </summary>
    public abstract string Type { get; }

    public virtual JObject GetInfo()
    {
      return JObject.FromObject( new
      {
        Url = "Responding/" + RespondingID,
        Type,
        PromptText,
      } );
    }


    /// <summary>
    /// 创建 Responding 对象
    /// </summary>
    /// <param name="playerHost">玩家宿主</param>
    /// <param name="promptText">响应提示文字</param>
    /// <param name="token">取消标识</param>
    protected Responding( PlayerHost playerHost, string promptText, CancellationToken token )
    {


      RespondingID = Guid.NewGuid();

      lock ( playerHost.SyncRoot )
      {
        if ( playerHost.Responding != null )
        {
          if ( playerHost.Responding.IsCompleted )
            playerHost.Responding = null;

          else
            throw new InvalidOperationException();
        }


        PlayerHost = playerHost;
        PromptText = promptText;
        TaskCompletionSource = new TaskCompletionSource<T>();

        token.Register( OnCancelled );
        playerHost.Responding = this;
      }
    }



    /// <summary>
    /// 获取响应唯一标识
    /// </summary>
    public Guid RespondingID { get; }



    /// <summary>
    /// 获取控制响应结果任务的 TaskCompletionSource 对象
    /// </summary>
    protected TaskCompletionSource<T> TaskCompletionSource { get; private set; }


    /// <summary>
    /// 获取等待响应的任务
    /// </summary>
    public Task<T> RespondingTask { get { return TaskCompletionSource.Task; } }


    /// <summary>
    /// 获取玩家宿主
    /// </summary>
    protected PlayerHost PlayerHost { get; private set; }

    /// <summary>
    /// 获取响应提示文字
    /// </summary>
    public string PromptText { get; private set; }


    /// <summary>
    /// 当玩家宿主获取到响应时
    /// </summary>
    /// <param name="message">响应消息</param>
    public void OnResponse( string message )
    {
      lock ( PlayerHost.SyncRoot )
      {
        if ( PlayerHost.Responding == this )
        {
          if ( OnResponseCore( message ) )
            PlayerHost.Responding = null;
        }
      }

    }

    /// <summary>
    /// 派生类实现此方法处理客户端响应
    /// </summary>
    /// <param name="message">响应消息</param>
    /// <returns>是否处理成功</returns>
    protected abstract bool OnResponseCore( string message );


    protected void OnCancelled()
    {

      lock ( PlayerHost.SyncRoot )
      {
        if ( TaskCompletionSource.TrySetCanceled() )
        {
          if ( PlayerHost.Responding == this )
            PlayerHost.Responding = null;
        }
      }
    }

    bool IResponding.IsCompleted
    {

      get
      {
        var task = RespondingTask;
        if ( task.IsCanceled || task.IsCompleted || task.IsFaulted )
          return true;

        else
          return false;
      }

    }
  }


  internal class TextMessageResponding : Responding<string>
  {


    public override string Type { get { return "Text"; } }


    public TextMessageResponding( PlayerHost playerHost, string promptText, CancellationToken token ) : base( playerHost, promptText, token ) { }


    protected override bool OnResponseCore( string message )
    {
      TaskCompletionSource.TrySetResult( message );
      return true;
    }
  }


  internal class OptionsResponding : Responding<Option>
  {

    public override string Type { get { return "Options"; } }


    public override JObject GetInfo()
    {
      var data = base.GetInfo();
      data["Options"] = JArray.FromObject( Options );

      return data;
    }

    public OptionsResponding( PlayerHost playerHost, string promptText, Option[] options, CancellationToken token )
      : base( playerHost, promptText, token )
    {
      Options = options;
    }

    public Option[] Options { get; private set; }


    protected override bool OnResponseCore( string message )
    {

      Option option;
      if ( !TryGetOption( message, out option ) )
        return false;

      TaskCompletionSource.TrySetResult( option );
      return true;
    }

    private bool TryGetOption( string text, out Option option )
    {
      option = null;
      int index;
      if ( !int.TryParse( text, out index ) )
        return false;

      if ( index < 0 || index >= Options.Length )
        return false;

      option = Options[index];
      return true;
    }
  }

}