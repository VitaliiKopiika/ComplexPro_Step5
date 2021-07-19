using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace Wpf_Binding
{

// https://msdn.microsoft.com/en-us/library/system.windows.input.inputbinding.commandparameter

    /*Remarks
The CommandParameter property is used to pass specific information to the command when it is executed. The type of the data is defined by the command. Many commands do not expect command parameters; for these commands, any command parameters passed will be ignored.

If the command that an input binding is associated with is a RoutedCommand, the CommandParameter of the input binding is passed to the RoutedCommand handlers through the ExecutedRoutedEventArgs and the CanExecuteRoutedEventArgs event data when the command is processed.

The data type and purpose of the command parameter are defined differently for each command and can be null. You can bind the Command, CommandParameter, and CommandTarget properties to an ICommand that is defined on an object. This enables you to define a custom command and associate it with user input. For more information, see the second example in InputBinding.

The InputBinding class does not support XAML usage because it does not expose a public default constructor (it has a default constructor, but it is protected). However, derived classes can expose a public constructor and therefore, can set properties that are inherited from InputBinding with XAML usage. Two existing InputBinding derived classes that can be instantiated in XAML and can set properties in XAML are KeyBinding and MouseBinding.
*/

    //XAML Attribute Usage
<inputBindingDerivedClass CommandParameter="commandParameterString"/>
XAML Property Element Usage
<inputBindingDerivedClass>
  <inputBindingDerivedClass.CommandParameter>
    <commandParameterObject/>
  </inputBindingDerivedClass.CommandParameter>
</inputBindingDerivedClass>

//XAML Values
//inputBindingDerivedClass
//A derived class of InputBinding that supports object element syntax, such as KeyBinding or MouseBinding. See Remarks.

//commandParameterString
//A string that is processed by a particular command. Strings are the common type used for command parameters because they can be easily set in XAML. For the expected string format and its purpose, see the documentation for the particular command that the input binding is associated with. Many commands do not expect parameters.

//commandParameterObject
//An object that is processed by a particular command. All existing WPF commands use strings. Therefore, this property element syntax is only relevant for custom command scenarios. In order to support this syntax, the commandParameterObject object must also support object element syntax (must have a public default constructor).

//    https://stackoverflow.com/questions/2634449/is-there-a-way-to-pass-a-parameter-to-a-command-through-a-keybinding-done-in-cod


InputBindings.Add(new KeyBinding(ChangeToRepositoryCommand, new KeyGesture(Key.F1)) { CommandParameter = 0 });

//Your solution is much more readable this way: 
InputBindings.Add(new KeyBinding(ChangeToRepositoryCommand, new KeyGesture(Key.F1)) { CommandParameter = 0 });
}

*/