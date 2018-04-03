using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace PasswordCheck
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow: Window
  {

    BindingList <PasswordCounter> Passwords;
    

    public MainWindow ()
    {
      InitializeComponent ();

      Passwords = new BindingList <PasswordCounter> ();
      CheckedPasswords.ItemsSource = Passwords;
    }


    private void AddPassword (PasswordCounter p)
    {
      Passwords.Add (p);
    }


    private void UpdatePassword (PasswordCounter p, string count)
    {
      p.Count = count;
      Passwords.ResetItem (Passwords.IndexOf (p));
    }


    private async Task <bool> CheckPassword ()
    {
      string password = PasswordInput.Text;
      PasswordInput.Text = String.Empty;

      PasswordCounter counter = new PasswordCounter ()
      {
        Password = password,
        Count = String.Empty
      };
      await CheckedPasswords.Dispatcher.BeginInvoke (new Action (() => AddPassword (counter)));

      string count = await PasswordChecker.PasswordCount (password);
      await CheckedPasswords.Dispatcher.BeginInvoke (
        new Action (() => UpdatePassword (counter, count)));
      return true;
    }


    private async void Button_Click (object sender, RoutedEventArgs e)
    {
      await CheckPassword ();
    }


    private async void PasswordInput_KeyDown (object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        await CheckPassword ();
    }


    private void RemovePassword_Click (object sender, RoutedEventArgs e)
    {
      Passwords.Remove (((Button) sender).Tag as PasswordCounter);
    }
  }
}
