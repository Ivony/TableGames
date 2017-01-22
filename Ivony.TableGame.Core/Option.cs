using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 定义一个选项的标准实现，仅包含所需要的内容
  /// </summary>
  [Serializable]
  public sealed class Option
  {


    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="name">选项名称</param>
    /// <param name="description">选项描述</param>
    /// <param name="disabled">是否禁用</param>
    public Option( string name, string description, bool disabled )
    {
      Name = name;
      Description = description;
      Disabled = disabled;
    }


    /// <summary>
    /// 选项名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 选项描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 选项是否不可选
    /// </summary>
    public bool Disabled { get; private set; }

  }



  /// <summary>
  /// 定义一个选项的标准实现，除了包含所需要的内容，还包含一个指定值对象的实例
  /// </summary>
  /// <typeparam name="T">值对象类型</typeparam>
  [Serializable]
  public sealed class Option<T> where T : class
  {



    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="value">选项值</param>
    /// <param name="optionItem">选项描述</param>
    public Option( T value, Option optionItem )
    {
      OptionItem = optionItem;
      OptionValue = value;
    }

    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="value">选项值</param>
    /// <param name="name">选项名称</param>
    /// <param name="description">选项描述</param>
    /// <param name="disabled">是否禁用</param>
    public Option( T value, string name, string description, bool disabled = false ) : this( value, new Option( name, description, disabled ) ) { }


    /// <summary>
    /// 选项实例
    /// </summary>
    public Option OptionItem { get; }



    /// <summary>
    /// 该选项所代表的值对象
    /// </summary>
    public T OptionValue { get; private set; }
  }


}
