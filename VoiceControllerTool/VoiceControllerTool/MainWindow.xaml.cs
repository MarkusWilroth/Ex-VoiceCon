using System;
using System.IO;
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
using VCTUtility;

namespace VoiceControllerTool {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        List<VoiceData> voiceDataList;
        List<Image> imgComList;
        private string workingDir;
        private string sourcePath;

        private WrapPanel[] wrapPanels;

        

        public MainWindow() {
            InitializeComponent();
            workingDir = Environment.CurrentDirectory;
            sourcePath = System.IO.Path.Combine(Directory.GetParent(workingDir).Parent.FullName, "VCT-CommandBox.png");
            //sourcePath = System.IO.Path.Combine(Environment.CurrentDirectory, "VCT-CommandBox.png");
            Console.WriteLine("Path: " + sourcePath);

            SetWrapPanels(); //Gör så att wrapPanels får sina värden och att alla börjar som dolda

            voiceDataList = new List<VoiceData>();
            imgComList = new List<Image>();
            Image test = new Image();
            test.Width = 40;
            test.Height = 40;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(sourcePath, UriKind.Relative);
            bitmap.EndInit();
            test.Source = bitmap;

            Grid.Children.Add(test);
            Test();
            //Load();
        }

        private void SetWrapPanels() {
            wrapPanels = new WrapPanel[7];
            wrapPanels[0] = WPCom1;
            wrapPanels[1] = WPCom2;
            wrapPanels[2] = WPCom3;
            wrapPanels[3] = WPCom4;
            wrapPanels[4] = WPCom5;
            wrapPanels[5] = WPCom6;
            wrapPanels[6] = WPCom7;

            foreach (WrapPanel wrapPanel in wrapPanels) {
                wrapPanel.Visibility = Visibility.Hidden;
            }
        }

        private void Test() {
            //VoiceData voiceData = new VoiceData("TestTitle", "TestText");
            //voiceDataList.Add(voiceData);
            //Save();
            //VoskManager.Test();
        }

        public void AddVoiceData(VoiceData voiceData) {
            if (voiceData != null) {
                voiceDataList.Add(voiceData);
                Console.WriteLine("VoiceData (Count): " + voiceDataList.Count);
            }
            UpdateCommand();
        }

        private void UpdateCommand() {
            imgComList.Clear();
            for (int i = 0; i < voiceDataList.Count; i++) {
                wrapPanels[i].Visibility = Visibility.Visible;
                //Ska fyllas med rätt värden fixar det när du är klar med din grej
            }
        }

        public void WriteMessage(string message) { //Kommer i log fönster (typ "Save successful" ect)

        }

        private void Save() {
            string path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true) {
                path = sfd.FileName;
            }
            SaveSystem.Save(path, voiceDataList);
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

        private void BtnAdd_Click(object sender, RoutedEventArgs e) { //Öppnar add window
            AddWindow addWin = new AddWindow(this);
            addWin.Show();
        }
    }
}
