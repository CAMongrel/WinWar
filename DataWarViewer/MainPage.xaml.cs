using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinWarCS.Data;
using WinWarCS.Data.Resources;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace DataWarViewer
{
   public class ListEntryHeader
   {
      public string IndexDesc { get; set; }
      public string NameDesc { get; set; }
      public string TypeDesc { get; set; }
   }

   public class ListEntry
   {
      public int Index { get; set; }
      public string Name { get; set; }
      public ContentFileType Type { get; set; }
      public BasicResource Resource { get; set; }
   }

   /// <summary>
   /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
   /// </summary>
   public sealed partial class MainPage : Page
   {
      public MainPage()
      {
         this.InitializeComponent();
      }

      private void openButton_PointerReleased(object sender, PointerRoutedEventArgs e)
      {
      }

      private void closeButton_PointerReleased(object sender, PointerRoutedEventArgs e)
      {
      }

      private async void openButton_Click(object sender, RoutedEventArgs e)
      {
         FileOpenPicker picker = new FileOpenPicker();
         picker.ViewMode = PickerViewMode.List;
         picker.FileTypeFilter.Add(".WAR");

         StorageFile file = await picker.PickSingleFileAsync();
         if (file != null)
         {
            var stream = await file.OpenAsync(FileAccessMode.Read);
            WarFile.LoadResourcesFromStream(stream.AsStreamForRead());
         }

         UpdateStatusText();
         PopulateListBox();

         listBox.Visibility = Visibility.Visible;
         closeButton.IsEnabled = true;
      }

      private void closeButton_Click(object sender, RoutedEventArgs e)
      {
         listBox.Visibility = Visibility.Collapsed;
         closeButton.IsEnabled = false;

         WarFile.Unload();

         UpdateStatusText();
      }

      private void UpdateStatusText()
      {
         if (WarFile.AreResoucesLoaded == false)
         {
            statusText.Text = "Not loaded";            
         }
         else
         {
            statusText.Text = "Loaded - " + WarFile.Count + " elements - Type: " + (WarFile.IsDemo ? "Demo" : "Retail");
         }
      }

      private void PopulateListBox()
      {
         if (WarFile.AreResoucesLoaded == false)
         {
            listBox.Items.Clear();
            return;
         }

         int resCnt = WarFile.Count;

         ObservableCollection<ListEntry> coll = new ObservableCollection<ListEntry>();
         for (int i = 0; i < resCnt; i++)
         {
            ListEntry entry = new ListEntry();
            entry.Resource = WarFile.GetResource(i);
            entry.Index = i;
            entry.Name = WarFile.KnowledgeBase[i].text;
            entry.Type = entry.Resource != null ? entry.Resource.Type : ContentFileType.FileUnknown;
            coll.Add(entry);
         }

         ListEntryHeader header = new ListEntryHeader();
         header.IndexDesc = "Index";
         header.NameDesc = "Name";
         header.TypeDesc = "Type";

         listBox.Header = header;
         listBox.ItemsSource = coll;
      }
   }
}
