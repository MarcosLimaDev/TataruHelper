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
using System.Windows.Shapes;
using Translation;
using Translation.Utils;

namespace FFXIVTataruHelper.AppWindows
{
    /// <summary>
    /// Interaction logic for AISettingsWindow.xaml
    /// </summary>
    public partial class AISettingsWindow : Window
    {
        public AISettingsWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Ensure settings are loaded or use defaults
            ApiKeyBox.Password = GlobalTranslationSettings.OpenAI_ApiKey;
            EndpointBox.Text = GlobalTranslationSettings.OpenAI_Endpoint;
            ModelBox.Text = GlobalTranslationSettings.OpenAI_Model;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalTranslationSettings.OpenAI_ApiKey = ApiKeyBox.Password;
            GlobalTranslationSettings.OpenAI_Endpoint = EndpointBox.Text;
            GlobalTranslationSettings.OpenAI_Model = ModelBox.Text;

            // Save to JSON
            Helper.SaveStaticToJson(typeof(GlobalTranslationSettings), "TranslationSysSettings.json");

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
