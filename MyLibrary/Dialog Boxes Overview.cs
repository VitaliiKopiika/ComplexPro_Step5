
//Dialog Boxes Overview

//https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/dialog-boxes-overview


// *********************************************************************
//                      Message Boxes
// *********************************************************************


//To create a message box, you use the MessageBox class. MessageBox lets you configure the message 
//box text, title, icon, and buttons, using code like the following.

// Configure the message box to be displayed

string messageBoxText = "Do you want to save changes?";
string caption = "Word Processor";
MessageBoxButton button = MessageBoxButton.YesNoCancel;
MessageBoxImage icon = MessageBoxImage.Warning;

// To show a message box, you call the staticShow method, as demonstrated in the following code.

// Display message box

MessageBox.Show(messageBoxText, caption, button, icon);

//When code that shows a message box needs to detect and process the user's decision (which button 
//was pressed), the code can inspect the message box result, as shown in the following code.
// Display message box

MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

// Process message box results

switch (result)
{
    case MessageBoxResult.Yes:
        // User pressed Yes button
        // ...
        break;
    case MessageBoxResult.No:
        // User pressed No button
        // ...
        break;
    case MessageBoxResult.Cancel:
        // User pressed Cancel button
        // ...
        break;
}

//For more information on using message boxes, see MessageBox, MessageBox Sample, and Dialog Box Sample.



// *********************************************************************
//                      Open File Dialog
// *********************************************************************

//   Вариант 0
//The common open file dialog box is implemented as the OpenFileDialog class and is located in 
//the Microsoft.Win32 namespace. The following code shows how to create, configure, and show one, 
//and how to process the result.

// Configure open file dialog box
Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
dlg.FileName = "Document"; // Default file name
dlg.DefaultExt = ".txt"; // Default file extension
dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

// Show open file dialog box
Nullable<bool> result = dlg.ShowDialog();

// Process open file dialog box results
if (result == true)
{
    // Open document
    string filename = dlg.FileName;
}

//   Вариант 1  https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/dialog-boxes-overview
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "All files (*.*)|*.*|PNG Photos (*.png)|*.png";  //"Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
            }
            

//   Вариант 2   http://www.cyberforum.ru/wpf-silverlight/thread526936.html
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|PNG Photos (*.png)|*.png";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == null)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            // Insert code to read the stream here.

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

//For more information on the open file dialog box, see OpenFileDialog.



// *********************************************************************
//                      Save File Dialog Box
// *********************************************************************

//The save file dialog box, shown in the following figure, is used by file saving functionality to 
//retrieve the name of a file to save.
//The common save file dialog box is implemented as the SaveFileDialog class, and is located in 
//the Microsoft.Win32 namespace. The following code shows how to create, configure, and show one, 
//and how to process the result.
// Configure save file dialog box

Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
dlg.FileName = "Document"; // Default file name
dlg.DefaultExt = ".text"; // Default file extension
dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

// Show save file dialog box
Nullable<bool> result = dlg.ShowDialog();

// Process save file dialog box results
if (result == true)
{
    // Save document
    string filename = dlg.FileName;
}

For more information on the save file dialog box, see SaveFileDialog.


// *********************************************************************
//                      Print Dialog Box
// *********************************************************************
//The print dialog box, shown in the following figure, is used by printing functionality to 
//choose and configure the printer that a user would like to print data to.
//The common print dialog box is implemented as the PrintDialog class, and is located in 
//the System.Windows.Controls namespace. The following code shows how to create, configure, and show one.

// Configure printer dialog box
System.Windows.Controls.PrintDialog dlg = new System.Windows.Controls.PrintDialog();
dlg.PageRangeSelection = PageRangeSelection.AllPages;
dlg.UserPageRangeEnabled = true;

// Show save file dialog box
Nullable<bool> result = dlg.ShowDialog();

// Process save file dialog box results
if (result == true)
{
    // Print document
}

//For more information on the print dialog box, see PrintDialog. For detailed discussion of printing 
//in WPF, see Printing Overview.



// *********************************************************************
//                      ValidationRule
// *********************************************************************

using System.Globalization;
using System.Windows.Controls;

namespace SDKSample
{
    public class MarginValidationRule : ValidationRule
    {
        double minMargin;
        double maxMargin;

        public double MinMargin
        {
            get { return this.minMargin; }
            set { this.minMargin = value; }
        }

        public double MaxMargin
        {
            get { return this.maxMargin; }
            set { this.maxMargin = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double margin;

            // Is a number?
            if (!double.TryParse((string)value, out margin))
            {
                return new ValidationResult(false, "Not a number.");
            }

            // Is in range?
            if ((margin < this.minMargin) || (margin > this.maxMargin))
            {
                string msg = string.Format("Margin must be between {0} and {1}.", this.minMargin, this.maxMargin);
                return new ValidationResult(false, msg);
            }

            // Number is valid
            return new ValidationResult(true, null);
        }
    }
}

<Window 
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
->  x:Class="SDKSample.MarginsDialogBox"
    xmlns:local="clr-namespace:SDKSample"
    ...
->    ShowInTaskbar="False"
->    WindowStartupLocation="CenterOwner" 
->    FocusManager.FocusedElement="{Binding ElementName=leftMarginTextBox}">

//---  C# Example
//  FocusManager.SetFocusedElement(INPUT_DATA_window, INPUT_DATA_textbox);    
//  FocusManager.SetFocusedElement(INPUT_DATA_window, button2);    
//---

  <Grid>

<Label Grid.Column="0" Grid.Row="0">Left Margin:</Label>
<TextBox Name="leftMarginTextBox" Grid.Column="1" Grid.Row="0">
  <TextBox.Text>
    <Binding Path="Left" UpdateSourceTrigger="PropertyChanged">
      <Binding.ValidationRules>
        <local:MarginValidationRule MinMargin="0" MaxMargin="10" />
      </Binding.ValidationRules>
    </Binding>
  </TextBox.Text>
</TextBox>

</Grid>

//Once the validation rule is associated, WPF will automatically apply it when data is entered 
//into the bound control. When a control contains invalid data, WPF will display a red border 
//around the invalid control, as shown in the following figure.
</Window>

//                Overall ValidationControl on OK-button click.
//WPF does not restrict a user to the invalid control until they have entered valid data. 
//This is good behavior for a dialog box; a user should be able to freely navigate the controls 
//in a dialog box whether or not data is valid. However, this means a user can enter invalid data 
//and press the OK button. For this reason, your code also needs to validate all controls in a dialog 
//box when the OK button is pressed by handling the Click event.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SDKSample
{
    public partial class MarginsDialogBox : Window
    {
void okButton_Click(object sender, RoutedEventArgs e)
{
    // Don't accept the dialog box if there is invalid data
    if (!IsValid(this)) return;
}

        // Validate all dependency objects in a window
        bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {   
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
        }
    }
}