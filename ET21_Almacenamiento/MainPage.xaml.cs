using System;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace ET21_Almacenamiento
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        StorageFolder directorio = null;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btGuardarClave_Click(object sender, RoutedEventArgs e)
        {
            object clave;
            ApplicationDataContainer clavesLocales = ApplicationData.Current.LocalSettings;
            if (!clavesLocales.Values.TryGetValue(tbClaveGuardar.Text, out clave))
            {
                clavesLocales.Values.Add(tbClaveGuardar.Text, tbValorGuardar.Text);
            }
            else
            {
                clavesLocales.Values[tbClaveGuardar.Text] = tbValorGuardar.Text;
            }

        }

        private async void btGuardarArchivo_Click(object sender, RoutedEventArgs e)
        {
            StorageFile fichero = null;
            if (directorio == null)
            {
                StorageFolder localDir = ApplicationData.Current.LocalFolder;
                fichero = await localDir.CreateFileAsync(tbArchivoGuardar.Text);
            }
            else
            {
                fichero = await directorio.CreateFileAsync(tbArchivoGuardar.Text);
            }
            await FileIO.WriteTextAsync(fichero, tbContenidoGuardar.Text);

        }

        private async void btSelDir_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");
            StorageFolder directorio = await picker.PickSingleFolderAsync();

            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(directorio);
        }

        private void btCargarClave_Click(object sender, RoutedEventArgs e)
        {
            if (tbClaveCargar.Text == "")
            {
                return;
            }

            object clave;
            ApplicationDataContainer clavesLocales = ApplicationData.Current.LocalSettings;
            if (!clavesLocales.Values.TryGetValue(tbClaveCargar.Text, out clave))
            {
                tbValorCargar.Text = "La clave que has escrito no existe";
            } else
            {
                tbValorCargar.Text = (string)clavesLocales.Values[tbClaveCargar.Text];
            }
        }

        private async void btCargarArchivo_Click(object sender, RoutedEventArgs e)
        {
            StorageFile fichero = null;

            if (directorio == null)
            {
                StorageFolder localDir = ApplicationData.Current.LocalFolder;
                fichero = await localDir.GetFileAsync(tbArchivoCargar.Text);
            }
            else
            {
                fichero = await directorio.GetFileAsync(tbArchivoCargar.Text);
            }
            tbContenidoCargar.Text = await FileIO.ReadTextAsync(fichero);
        }
    }
}
