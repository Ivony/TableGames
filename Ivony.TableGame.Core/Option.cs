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
    public Option( string name, string description, bool disabled = false )
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


    /// <summary>
    /// 创建包含指定值对象实例的选项
    /// </summary>
    /// <typeparam name="T">值对象类型</typeparam>
    /// <param name="value">值对象</param>
    /// <param name="option">选项数据</param>
    /// <returns>Option 对象</returns>
    public static Option<T> Create<T>( T value, Option option ) where T : class
    {
      return new Option<T>( value, option );
    }


    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="value">选项值</param>
    /// <param name="name">选项名称</param>
    /// <param name="description">选项描述</param>
    /// <param name="disabled">是否禁用</param>
    public static Option<T> Create<T>( T value, string name, string description, bool disabled = false ) where T : class
    {
      return Create( value, new Option( name, description, disabled ) );
    }


    /// <summary>
    /// 创建包含指定值对象实例的选项
    /// </summary>
    /// <typeparam name="T">值对象类型</typeparam>
    /// <param name="value">值对象</param>
    /// <returns>Option 对象</returns>
    public static Option<T> Create<T>( T value ) where T : class
    {

      var option = OptionProvider.CreateOption( value );
      if ( option == null )
        return null;

      return Create( value, option );
    }
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
    /// 选项实例
    /// </summary>
    public Option OptionItem { get; }



    /// <summary>
    /// 该选项所代表的值对象
    /// </summary>
    public T OptionValue { get; private set; }
  }


}
