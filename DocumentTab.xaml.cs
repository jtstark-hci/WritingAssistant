using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Text;
using System.Diagnostics;
using Windows.Storage.Streams;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WritingAssistant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocumentTab : Page
    {
        FileListItem listItem;

        public DocumentTab()
        {
            this.InitializeComponent();
            docTitle.Text = "Untitled";
            listItem = new FileListItem();
            listItem.tab = this;
            listItem.ChangeFileName(docTitle.Text);
        }

        public DocumentTab(FileListItem li)
        {
            this.InitializeComponent();
            docTitle.Text = li.file.DisplayName;
            listItem = li;
            OpenFile(li.file);

        }

        public async void OpenFile(StorageFile file)
        {
            using (IRandomAccessStream randAccStream =
            await file.OpenAsync(FileAccessMode.Read))
            {
                editor.Document.LoadFromStream(TextSetOptions.FormatRtf, randAccStream);
            }
        }

        public void Close()
        {
            listItem.alreadyOpen = false;
            listItem.tab = null;
        }

        

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            editor.Document.GetText(TextGetOptions.UseCrlf, out _);
        }

        private void TitleCancelButton_Click(object sender, RoutedEventArgs e)
        {
            titleEditBox.Text = "";
            editTitleFlyout.Hide();
        }

        private void TitleConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            docTitle.Text = titleEditBox.Text;
            listItem.ChangeFileName(titleEditBox.Text);
            titleEditBox.Text = "";
            editTitleFlyout.Hide();
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Microsoft.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((SolidColorBrush)rectangle.Fill).Color;

            editor.Document.Selection.CharacterFormat.ForegroundColor = color;

            fontColorButton.Flyout.Hide();
            editor.Focus(FocusState.Keyboard);
        }
    }
}
