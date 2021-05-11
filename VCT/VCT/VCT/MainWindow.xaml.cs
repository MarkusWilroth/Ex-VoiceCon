using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using VCTUtility;

namespace VCT {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<VoiceData> voiceDataList;
        private int cmdSlots = 7; //Hur många slots det finns 
        private int scrollValue = 0; //Ska användas för att scrolla

        private WrapPanel[] wrapPanelArr; //Används för att inaktivera/aktivera commandBox (Text, title, tag, train, delete)
        private Label[] lbTitleArr, lbTextArr, lbTagArr; //Används för att ändra värderna i UI
        private string path;

        public MainWindow() {
            InitializeComponent();

            SetWrapPanels(); //Gör så att wrapPanels får sina värden (Så att man enkelt kan ändra deras värden i en for-loop) och att alla börjar som dolda

            NewProject();
            //Load();
        }

        private void SetWrapPanels() { //Sätter alla labels i arrays så att de enkelt kan ändras i en for-loop senare (Om du kan göra detta snyggare gör det gänra)
            wrapPanelArr = new WrapPanel[cmdSlots];
            wrapPanelArr[0] = WPCom1;
            wrapPanelArr[1] = WPCom2;
            wrapPanelArr[2] = WPCom3;
            wrapPanelArr[3] = WPCom4;
            wrapPanelArr[4] = WPCom5;
            wrapPanelArr[5] = WPCom6;
            wrapPanelArr[6] = WPCom7;

            lbTitleArr = new Label[cmdSlots];
            lbTitleArr[0] = lbTitle1;
            lbTitleArr[1] = lbTitle2;
            lbTitleArr[2] = lbTitle3;
            lbTitleArr[3] = lbTitle4;
            lbTitleArr[4] = lbTitle5;
            lbTitleArr[5] = lbTitle6;
            lbTitleArr[6] = lbTitle7;

            lbTextArr = new Label[cmdSlots];
            lbTextArr[0] = lbText1;
            lbTextArr[1] = lbText2;
            lbTextArr[2] = lbText3;
            lbTextArr[3] = lbText4;
            lbTextArr[4] = lbText5;
            lbTextArr[5] = lbText6;
            lbTextArr[6] = lbText7;

            lbTagArr = new Label[cmdSlots];
            lbTagArr[0] = lbTag1;
            lbTagArr[1] = lbTag2;
            lbTagArr[2] = lbTag3;
            lbTagArr[3] = lbTag4;
            lbTagArr[4] = lbTag5;
            lbTagArr[5] = lbTag6;
            lbTagArr[6] = lbTag7;

            foreach (WrapPanel wrapPanel in wrapPanelArr) { //Gör så att CommandBoxes börjar som hidden
                wrapPanel.Visibility = Visibility.Hidden;
            }
        }

        private void NewProject() {
            voiceDataList = new List<VoiceData>();
            UpdateCommand();
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
            int count = 0;
            for (int i = scrollValue; i < voiceDataList.Count; i++) {
                wrapPanelArr[i].Visibility = Visibility.Visible;
                lbTitleArr[i].Content = voiceDataList[i].voiceTitle;
                lbTextArr[i].Content = voiceDataList[i].voiceText;
                lbTagArr[i].Content = voiceDataList[i].voiceTag;
                count = i + 1;

                if (count >= cmdSlots) { //Finns inga flera slots
                    break;
                }
                //Ska fyllas med rätt värden fixar det när du är klar med din grej
            }

            if (count < cmdSlots) {
                for (int i = count; i < cmdSlots; i++) { //Ser till att de slots som inte används blir hidden
                    wrapPanelArr[i].Visibility = Visibility.Hidden;
                }
            }
        }

        public void WriteMessage(string message) { //Kommer i log fönster (typ "Save successful" ect)

        }

        private void SaveAs() {
            path = "";
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true) {
                path = sfd.FileName;
                if (path != null) {
                    Save();
                }
            }
        }

        private void Save() {
            SaveSystem.Save(path, voiceDataList);
        }

        

        private void Open() {
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

        #region Menu
        private void NewProject_Click(object sender, RoutedEventArgs e) {

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e) {
            Open();
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            if (path != null) {
                Save();
            } else {
                SaveAs();
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e) {
            SaveAs();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        #endregion

        public VoiceData[] GetKeywords() {
            return voiceDataList.ToArray();
        }
    }
}
