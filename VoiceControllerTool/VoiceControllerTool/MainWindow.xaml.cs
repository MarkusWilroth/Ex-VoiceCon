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
using Microsoft.Win32;

namespace VoiceControllerTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        List<VoiceData> voiceDataList;
        public MainWindow() {
            InitializeComponent();
            voiceDataList = new List<VoiceData>();
            //Test();
            //Load();
        }

        private void Test() {
            VoiceData voiceData = new VoiceData("TestTitle", "TestText");
            voiceDataList.Add(voiceData);
            Save();
        }

        private void Save() {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true) {
                path = sfd.FileName;
            }
            SaveSystem.SaveVoice(voiceDataList, path);
        }

        private void Load() {
            string path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true) {
                path = ofd.FileName;
            }
            SaveData saveData = SaveSystem.Load(path);
            if (saveData != null) {
                foreach (VoiceData voiceData in saveData.voiceDataList) {
                    Console.WriteLine("Title: " + voiceData.voiceTitle);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) {

        }
    }
}
