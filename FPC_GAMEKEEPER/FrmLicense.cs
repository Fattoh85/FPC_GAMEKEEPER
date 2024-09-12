//using FPC.Model.Crypto;
using FPC.Model.Crypto;
using Org.BouncyCastle.Crypto;
using System;
using System.IO;
using System.Windows.Forms;

namespace FPC
{
    public partial class FrmLicense : Form
    {
        public static log4net.ILog log = FPC.Model.Logger.Logger.Get(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string filePath = "Files/sign.txt";

        public FrmLicense()
        {
            InitializeComponent();
        }

        public static string ReadFileContent(string filePath)
        {
            try
            {
                return File.Exists(filePath) ? File.ReadAllText(filePath) : null;
            }
            catch (Exception ex)
            {
                log.Error($"Error reading file: {ex.Message}");
                return null;
            }
        }

        public static void SaveFileContent(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                log.Debug("File saved successfully.");
            }
            catch (Exception ex)
            {
                log.Error($"Error saving file: {ex.Message}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileContent = ReadFileContent(filePath);
           
            string strToSign_MacAndHdd = SystemInfoLib.GetMacAndHdd(); 
            string strToSign_MotherboardSn = SystemInfoLib.GetMotherboardSn();

            string sign = fileContent != null ? fileContent : rtbKey.Text;

            if (VerifySignature(strToSign_MacAndHdd, sign) || VerifySignature(strToSign_MotherboardSn, sign))
            {
                if (string.IsNullOrEmpty(fileContent))
                {
                    SaveFileContent(filePath, rtbKey.Text);
                }

                ShowMain();
            }
            else
            {
                MessageBox.Show("Не верная подпись");
            }
        }

        private bool VerifySignature(string strToSign, string signatureString)
        {
            if (signatureString != null)
            {
                byte[] signatureBytes = Convert.FromBase64String(signatureString);
                AsymmetricKeyParameter publicKey = PemKeyManager.LoadKeyPair(null, null);

                return StationIdentVerifier.VerifyStationIdent(strToSign, signatureBytes, publicKey);
            }
            else
            { 
                return false;
            }
            
        }

        public void ShowMain()
        {
            FrmMain frmMain = new FrmMain();
            frmMain.ShowDialog();
            this.Close();
        }

        private void FrmLicense_Load(object sender, EventArgs e)
        {
            string strToSign_MacAndHdd = SystemInfoLib.GetMacAndHdd();
            string strToSign_MotherboardSn = SystemInfoLib.GetMotherboardSn();

            txtCopy.Text = strToSign_MotherboardSn;
            
            string fileContent = ReadFileContent(filePath);

            if (fileContent != null)
            {
                if (VerifySignature(strToSign_MacAndHdd, fileContent) || VerifySignature(strToSign_MotherboardSn, fileContent))
                {
                    ShowMain();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtCopy.Text);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            rtbKey.Text = Clipboard.GetText();
        }
    }
}
