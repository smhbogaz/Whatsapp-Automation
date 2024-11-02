namespace WhatsappAutomation;
#pragma warning disable IDE1006
#pragma warning disable CS8618
public partial class Main : MaterialForm
{
    private readonly List<Panel> toastPanels = [];
    private async Task ShowToastMessageAsync(ToastMessageType type, string title, string message)
    {
        Color color = TransparencyKey;
        Image image = Properties.Resources.ok;

        switch (type)
        {
            case ToastMessageType.Success:
                color = Color.FromArgb(57, 155, 53);
                image = Properties.Resources.ok;
                break;
            case ToastMessageType.Error:
                color = Color.FromArgb(227, 50, 45);
                image = Properties.Resources.cancel;
                break;
            case ToastMessageType.Info:
                color = Color.FromArgb(40, 60, 255);
                image = Properties.Resources.info;
                break;
            case ToastMessageType.Warning:
                color = Color.FromArgb(245, 171, 35);
                image = Properties.Resources.error;
                break;
        }


        // Yeni toast paneli oluþtur
        Panel newToastPanel = new()
        {
            Size = new Size(298, 59),
            Location = new Point((this.Width - 298) - 20, (this.Height - 59) - 20)
        };

        // Mevcut panelleri yukarý kaydýr
        foreach (var panel in toastPanels)
        {
            panel.Location = new Point(panel.Location.X, panel.Location.Y - newToastPanel.Height - 10);
        }

        // Yeni paneli listeye ekle ve form üzerine yerleþtir
        toastPanels.Add(newToastPanel);
        this.Controls.Add(newToastPanel);
        newToastPanel.BringToFront();
        // Panelin içindeki kontrolleri ayarla
        Panel panel_toast_color = new();
        newToastPanel.Controls.Add(panel_toast_color);
        panel_toast_color.Size = new Size(10, 59);
        panel_toast_color.BackColor = color;

        PictureBox panel_toast_image = new();
        newToastPanel.Controls.Add(panel_toast_image);
        panel_toast_image.Image = image;
        panel_toast_image.Location = new Point(16, 11);
        panel_toast_image.Size = new Size(25, 25);
        panel_toast_image.SizeMode = PictureBoxSizeMode.StretchImage;

        Label panel_toast_type = new();
        newToastPanel.Controls.Add(panel_toast_type);
        panel_toast_type.Width = 9999;
        panel_toast_type.Text = title;
        panel_toast_type.ForeColor = materialSkinManager.Theme == MaterialSkin.MaterialSkinManager.Themes.DARK ? Color.Black : Color.White;
        panel_toast_type.Font = new Font("Segoe UI Black", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 162);
        panel_toast_type.Location = new Point(47, 11);

        Label panel_toast_message = new();
        newToastPanel.Controls.Add(panel_toast_message);
        panel_toast_message.Width = 9999;
        panel_toast_message.MaximumSize = new Size(250, 250);
        panel_toast_message.AutoSize = true;
        panel_toast_message.Text = message;
        panel_toast_message.ForeColor = materialSkinManager.Theme == MaterialSkin.MaterialSkinManager.Themes.DARK ? Color.Black : Color.White;
        panel_toast_message.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
        panel_toast_message.Location = new Point(47, 37);

        newToastPanel.BackColor = materialSkinManager.Theme == MaterialSkin.MaterialSkinManager.Themes.LIGHT ? Color.Black : Color.White;




        // Göster ve hareket ettir
        newToastPanel.Visible = true;
        await Task.Delay(3000);

        while (newToastPanel.Location.X < this.Width)
        {
            newToastPanel.Location = new Point(newToastPanel.Location.X + 5, newToastPanel.Location.Y);
            await Task.Delay(10);
        }

        // Mesajlar kaybolduktan sonra paneli kaldýr ve listeyi güncelle
        newToastPanel.Visible = false;
        toastPanels.Remove(newToastPanel);
        this.Controls.Remove(newToastPanel);
    }

    public async Task CheckForUpdates()
    {
        double currentVersion = 5.3;
        this.Text += $" - {currentVersion}".Replace(",", ".");
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        using HttpClient httpClient = new(clientHandler);

        try
        {
            string response = await httpClient.GetStringAsync("HÝDDEN");
            string versionString = response.Trim().Replace(".", ",");
            if (double.TryParse(versionString, out double newVersion))
            {
                if (newVersion > currentVersion)
                {

                    if (MessageBox.Show("Güncelleme mevcut, indirmek istiyor musunuz?", "Güncelleme Mevcut", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo { FileName = "updater.exe", UseShellExecute = true, Arguments = "authorized" });
                        this.Close();
                    }
                }
            }
        }
        catch
        {

        }
    }

    readonly MaterialSkin.MaterialSkinManager materialSkinManager;
    public Main()
    {
        sw.Start();
        InitializeComponent();
        /*MATERIAL SKIN MANAGER*/
        materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
        materialSkinManager.EnforceBackcolorOnAllComponents = true;
        materialSkinManager.AddFormToManage(this);
        bool isLightTheme = Settings.Default.programTheme.Contains("LIGHT");
        materialSkinManager.Theme = isLightTheme ? MaterialSkin.MaterialSkinManager.Themes.LIGHT : MaterialSkin.MaterialSkinManager.Themes.DARK;
        //SETTINGS TAB
        settings_cb_theme.SelectedIndex = isLightTheme ? 0 : 1;
        //Indigo500,700,100
        materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
            GetPrimaryColor1(Settings.Default.programPirmaryColor1),
            GetPrimaryColor2(Settings.Default.programPirmaryColor2),
            GetPrimaryColor3(Settings.Default.programPirmaryColor3),
            GetAccentColor(Settings.Default.programAccentColor),
            MaterialSkin.TextShade.WHITE);
        //SETTÝNGS TAB
        settings_cb_AccentColor.SelectedIndex = (GetAccentColorIndex(Settings.Default.programAccentColor) != -1) ? GetAccentColorIndex(Settings.Default.programAccentColor) : 0;
        settings_cb_primary3.SelectedIndex = GetPrimaryColorIndex3(Settings.Default.programPirmaryColor3);
        settings_cb_primary2.SelectedIndex = GetPrimaryColorIndex2(Settings.Default.programPirmaryColor2);
        settings_cb_primary1.SelectedIndex = GetPrimaryColorIndex(Settings.Default.programPirmaryColor1);
        settings_cb_usedSchoolNumber.Checked = Settings.Default.usedSchoolNumber;
        settings_cb_chromeOptions.SelectedIndex = Settings.Default.chromeOptions;
        settings_cb_EnableTooltip.Checked = Settings.Default.enableTooltip;
        settings_cb_startNewChat.Checked = Settings.Default.startNewChat;
        settings_txt_ProcessDelay.Text = Settings.Default.processDelay + "";
        settings_txt_studentImageFolder.Text = Settings.Default.studentImageFolder;

        if (!Directory.Exists(DataFolderPath))
        {
            Directory.CreateDirectory(DataFolderPath);
        }
        if (!Directory.Exists(LogFolderPath))
        {
            Directory.CreateDirectory(LogFolderPath);
        }
        foreach (var item in Directory.GetFiles(DataFolderPath, "*.txt"))
        {
            settings_cb_txtFile.Items.Add(Path.GetFileName(item));
        }
        settings_cb_txtFile.Text = Settings.Default.txtFile;
        settings_btn_txtDelete.Visible = settings_btn_txtExport.Visible = settings_cb_txtFile.Items.Count > 0;
        /**/
        this.MinimumSize = this.MaximumSize = new Size(1050, 540);
        mgo_dgv.ForeColor = Color.Black;
    }

    public static string DocumnetsPath => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static string DataFolderPath => Path.Combine(DocumnetsPath, "WhatsAppAutomationData");
    public static string LogFolderPath => Path.Combine(DocumnetsPath, "WhatsAppAutomationData", "Log");
    public static string TxtPath => DataFolderPath + "\\" + Settings.Default.txtFile;


    #region FORM
    private void Main_Load(object sender, EventArgs e)
    {
        mgo_dgv.Columns.Add("v_ad", "Veli Adý");
        mgo_dgv.Columns.Add("v_no", "Veli No");
        mgo_dgv.Columns.Add("o_ad", "Öðrenci Adý");
        mgo_dgv.Columns.Add("o_no", "Öðrenci Numarasý");
        mgo_dgv.Columns.Add("o_sinif", "Öðrenci Sýnýfý");
        Main.log("Uygulama Baþladý: ");
        _ = CheckForUpdates();
    }
    int closedCount = 1;
    private void Main_FormClosed(object sender, FormClosedEventArgs e)
    {
        sw.Start();
        if (closedCount == 1) { Main.log("Uygulama Kapatýldý: "); closedCount++; }
        this.Dispose();
        new EventHandlerList().Dispose();
        Process[] a = Process.GetProcesses();
        foreach (var item in a)
        {
            if (item.ProcessName.Contains("chromedriver"))
            {
                item.Kill();
            }
        }
    }
    #endregion

    #region Rehbet Yöner Bilgi-Güncelleme
    private void tab_rehberYonet_Enter(object sender, EventArgs e)
    {
        ryb_veliAdi.Enabled = false;
        ryb_veliNumarasi.Enabled = false;
        ryb_ogrenciAdi.Enabled = false;
        ryb_ogrenciNo.Enabled = true;
        ryb_checkNumber.Enabled = true;
        ryb_sinif.Enabled = false;
        ryb_btnUpdate.Enabled = false;
        ryb_btnDelete.Enabled = false;
        ryb_btnClear.Enabled = false;
        //ryb_picture_student.Image= null;
    }
    int updateStudentNumber = 0;
    private void ryb_checkNumber_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }
            string[] allParent = File.ReadAllLines(path: TxtPath);
            string studentNumber = ryb_ogrenciNo.Text.Replace(" ", "").Trim();
            if (!allParent.Contains(SaveValue.OgrenciNumarasi + studentNumber)) { throw new NotFoundStudentNumber(); }

            if (Array.IndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber) != Array.LastIndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber))
            //numaraya sahip 2 kiþi varsa girer
            {
                for (int i = 0; i < allParent.Length; i++)
                {
                    if (allParent[i] == SaveValue.OgrenciNumarasi + studentNumber)
                    {
                        DialogResult result = MessageBox.Show($@"Güncellemek isteiðiniz bilgiler bunlarmý?
                            {allParent[i - 3]}                            
                            {allParent[i - 2]}
                            {allParent[i - 1]}
                            {allParent[i + 1]}
                        ", "Veli Seçin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            ryb_veliAdi.Text = allParent[i - 3].Replace(SaveValue.VeliAdi, "");
                            ryb_veliNumarasi.Hint = null;
                            ryb_veliNumarasi.Text = allParent[i - 2].Replace(SaveValue.VeliNo, "");
                            ryb_veliNumarasi.Hint = "Veli Telefon Numarasý";
                            ryb_ogrenciAdi.Text = allParent[i - 1].Replace(SaveValue.OgrenciAdi, "");
                            ryb_sinif.Text = allParent[i + 1].Replace(SaveValue.OgrenciSinif, "");

                            try
                            {
                                if (Directory.Exists(Settings.Default.studentImageFolder))
                                {
                                    foreach (string image in Directory.GetFiles(Settings.Default.studentImageFolder, "*", SearchOption.AllDirectories)
                                                        .Where(x => x.ToLower().EndsWith(".jpg") || x.ToLower().EndsWith(".png") || x.ToLower().EndsWith(".jpeg"))
                                                        .ToList()
                                                        )
                                    {
                                        if (Path.GetFileNameWithoutExtension(image).Contains(studentNumber))
                                        {
                                            ryb_picture_student.Image = Image.FromFile(image);
                                            ryb_picture_student.SizeMode = PictureBoxSizeMode.StretchImage;

                                            ryb_btnRotateImg.Enabled = true;
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }

                            updateStudentNumber = i;

                            ryb_veliAdi.Enabled = true;
                            ryb_veliNumarasi.Enabled = true;
                            ryb_ogrenciAdi.Enabled = true;
                            ryb_btnUpdate.Enabled = true;
                            ryb_sinif.Enabled = true;
                            ryb_btnDelete.Enabled = true;
                            ryb_btnClear.Enabled = true;
                            ryb_ogrenciNo.Enabled = false;
                            break;
                        }
                    }
                }
                return;
            }

            int index = Array.IndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber);
            ryb_veliAdi.Text = allParent[index - 3].Replace(SaveValue.VeliAdi, "");
            ryb_veliNumarasi.Hint = null;
            ryb_veliNumarasi.Text = allParent[index - 2].Replace(SaveValue.VeliNo, "");
            ryb_veliNumarasi.Hint = "Veli Telefon Numarasý";
            ryb_ogrenciAdi.Text = allParent[index - 1].Replace(SaveValue.OgrenciAdi, "");
            ryb_sinif.Text = allParent[index + 1].Replace(SaveValue.OgrenciSinif, "");
            updateStudentNumber = index;

            try
            {
                if (Directory.Exists(Settings.Default.studentImageFolder))
                {
                    foreach (string image in Directory.GetFiles(Settings.Default.studentImageFolder, "*", SearchOption.AllDirectories)
                                        .Where(x => x.ToLower().EndsWith(".jpg") || x.ToLower().EndsWith(".png") || x.ToLower().EndsWith(".jpeg"))
                                        .ToList()
                                        )
                    {
                        if (Path.GetFileNameWithoutExtension(image).Contains(studentNumber))
                        {
                            ryb_picture_student.Image = Image.FromFile(image);
                            ryb_picture_student.SizeMode = PictureBoxSizeMode.StretchImage;

                            ryb_btnRotateImg.Enabled = true;
                        }
                    }
                }
            }
            catch { }

            ryb_veliAdi.Enabled = true;
            ryb_veliNumarasi.Enabled = true;
            ryb_ogrenciAdi.Enabled = true;
            ryb_btnUpdate.Enabled = true;
            ryb_sinif.Enabled = true;
            ryb_btnDelete.Enabled = true;
            ryb_ogrenciNo.Enabled = false;
            ryb_btnClear.Enabled = true;
        }
        catch (NotFoundStudentNumber)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Öðrenci Numarasý Bulunamadý");
        }
        catch (Exception error)
        {
            MessageBox.Show("Hata: " + error.Message);
        }
    }
    private void ryb_btnUpdate_Click(object sender, EventArgs e)
    {
        updateLine(TxtPath, updateStudentNumber, SaveValue.OgrenciNumarasi + ryb_ogrenciNo.Text);
        updateLine(TxtPath, updateStudentNumber - 1, SaveValue.OgrenciAdi + ryb_ogrenciAdi.Text);
        updateLine(TxtPath, updateStudentNumber - 2, SaveValue.VeliNo + ryb_veliNumarasi.Text);
        updateLine(TxtPath, updateStudentNumber - 3, SaveValue.VeliAdi + ryb_veliAdi.Text);
        updateLine(TxtPath, updateStudentNumber + 1, SaveValue.OgrenciSinif + ryb_sinif.Text.Trim().ToLower());
        _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Öðrenci Güncelleme Baþarýlý");
    }
    private void ryb_btnDelete_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Silme Ýþlemini Onaylýyormusunuz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
            return;
        }

        deleteLine(TxtPath, updateStudentNumber);
        deleteLine(TxtPath, updateStudentNumber - 1);
        deleteLine(TxtPath, updateStudentNumber - 2);
        deleteLine(TxtPath, updateStudentNumber - 3);
        deleteLine(TxtPath, updateStudentNumber + 1);

        ryb_veliAdi.Enabled = false;
        ryb_veliNumarasi.Enabled = false;
        ryb_ogrenciAdi.Enabled = false;
        ryb_ogrenciNo.Enabled = true;
        ryb_checkNumber.Enabled = true;
        ryb_sinif.Enabled = false;
        ryb_btnUpdate.Enabled = false;
        ryb_btnDelete.Enabled = false;
        ryb_btnClear.Enabled = false;
        ryb_btnRotateImg.Enabled = false;
        ryb_picture_student.Image = null;

        ryb_veliAdi.Text = string.Empty;
        ryb_veliNumarasi.Text = string.Empty;
        ryb_ogrenciAdi.Text = string.Empty;
        ryb_ogrenciNo.Text = string.Empty;
        ryb_sinif.Text = string.Empty;

        _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Öðrenci Silme Ýþlemi Baþarýlý");
    }
    private void ryb_btnClear_Click(object sender, EventArgs e)
    {
        ryb_veliAdi.Enabled = false;
        ryb_veliNumarasi.Enabled = false;
        ryb_ogrenciAdi.Enabled = false;
        ryb_ogrenciNo.Enabled = true;
        ryb_checkNumber.Enabled = true;
        ryb_sinif.Enabled = false;
        ryb_btnUpdate.Enabled = false;
        ryb_btnDelete.Enabled = false;
        ryb_btnClear.Enabled = false;
        ryb_btnRotateImg.Enabled = false;
        ryb_picture_student.Image = null;
    }
    private void ryb_btnRotateImg_Click(object sender, EventArgs e)
    {
        Image img = ryb_picture_student.Image;
        if (img == null) { return; }
        img.RotateFlip(RotateFlipType.Rotate90FlipXY);
        ryb_picture_student.Image = img;
    }
    private void ryb_ogrenciNo_KeyUp(object sender, KeyEventArgs e)
    {

        try
        {
            string[] allParent = File.ReadAllLines(path: TxtPath);
            string studentNumber = ryb_ogrenciNo.Text.Replace(" ", "").Trim();
            if (!allParent.Contains(SaveValue.OgrenciNumarasi + studentNumber))
            {
                ryb_veliAdi.Text = null;
                ryb_veliNumarasi.Text = null;
                ryb_ogrenciAdi.Text = null;
                ryb_sinif.Text = null;
                ryb_picture_student.Image = null;
                return;
            }

            if (Array.IndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber) != Array.LastIndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber))
            {
                for (int i = 0; i < allParent.Length; i++)
                {
                    if (allParent[i] == SaveValue.OgrenciNumarasi + studentNumber)
                    {
                        ryb_veliAdi.Text = allParent[i - 3].Replace(SaveValue.VeliAdi, "");
                        ryb_veliNumarasi.Hint = null;
                        ryb_veliNumarasi.Text = allParent[i - 2].Replace(SaveValue.VeliNo, "");
                        ryb_veliNumarasi.Hint = "Veli Telefon Numarasý";
                        ryb_ogrenciAdi.Text = allParent[i - 1].Replace(SaveValue.OgrenciAdi, "");
                        ryb_sinif.Text = allParent[i + 1].Replace(SaveValue.OgrenciSinif, "");

                        try
                        {
                            if (Directory.Exists(Settings.Default.studentImageFolder))
                            {
                                foreach (string image in Directory.GetFiles(Settings.Default.studentImageFolder, "*", SearchOption.AllDirectories)
                                                    .Where(x => x.ToLower().EndsWith(".jpg") || x.ToLower().EndsWith(".png") || x.ToLower().EndsWith(".jpeg"))
                                                    .ToList()
                                                    )
                                {
                                    if (Path.GetFileNameWithoutExtension(image).Contains(studentNumber))
                                    {
                                        ryb_picture_student.Image = Image.FromFile(image);
                                        ryb_picture_student.SizeMode = PictureBoxSizeMode.StretchImage;
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        return;
                    }
                }
            }

            int index = Array.IndexOf(allParent, SaveValue.OgrenciNumarasi + studentNumber);
            ryb_veliAdi.Text = allParent[index - 3].Replace(SaveValue.VeliAdi, "");
            ryb_veliNumarasi.Hint = null;
            ryb_veliNumarasi.Text = allParent[index - 2].Replace(SaveValue.VeliNo, "");
            ryb_veliNumarasi.Hint = "Veli Telefon Numarasý";
            ryb_ogrenciAdi.Text = allParent[index - 1].Replace(SaveValue.OgrenciAdi, "");
            ryb_sinif.Text = allParent[index + 1].Replace(SaveValue.OgrenciSinif, "");
            updateStudentNumber = index;

            try
            {
                if (Directory.Exists(Settings.Default.studentImageFolder))
                {
                    foreach (string image in Directory.GetFiles(Settings.Default.studentImageFolder, "*", SearchOption.AllDirectories)
                                        .Where(x => x.ToLower().EndsWith(".jpg") || x.ToLower().EndsWith(".png") || x.ToLower().EndsWith(".jpeg"))
                                        .ToList()
                                        )
                    {
                        if (Path.GetFileNameWithoutExtension(image).Contains(studentNumber))
                        {
                            ryb_picture_student.Image = Image.FromFile(image);
                            ryb_picture_student.SizeMode = PictureBoxSizeMode.StretchImage;

                            ryb_btnRotateImg.Enabled = true;
                        }
                    }
                }
            }
            catch { }

        }
        catch { }

    }
    private static void deleteLine(string filePath, int lineNo)
    {
        string[] lines = File.ReadAllLines(filePath);
        var newLines = new string[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == lineNo)
            {
                newLines[i] = string.Empty;
            }
            else
            {
                newLines[i] = lines[i];
            }
        }
        File.WriteAllLines(filePath, newLines);
    }
    private static void updateLine(string filePath, int lineNo, string value)
    {
        string[] lines = File.ReadAllLines(filePath);
        var newLines = new string[lines.Length];
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == lineNo)
            {
                newLines[i] = value;
            }
            else
            {
                newLines[i] = lines[i];
            }
        }
        File.WriteAllLines(filePath, newLines);
    }
    #endregion

    #region Rehber Yönet Excelden-Veri-Aktarma
    private void rye_selectExcelFile_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "Bir excel dosyasý seçin.",
            Filter = "Excel Dosyalarý|*.xls;*.xlsx"
        };
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            rye_excelFilePath.Text = openFileDialog.FileName;
        }
    }
    private void rye_startProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }
            // Excel dosya yolu alýnýr
            string filePath = rye_excelFilePath.Text;
            if (filePath.Length < 1) { throw new ExcelFileNotSelected(); }

            // Gerekli tüm alanlarýn doldurulup doldurulmadýðý kontrol edilir

            if (
                !string.IsNullOrEmpty(rye_ogrenciNoKolon.Text) &&
                !string.IsNullOrEmpty(rye_ogrenciAdKolon.Text) &&
                !string.IsNullOrEmpty(rye_veliAdiKolon.Text) &&
                !string.IsNullOrEmpty(rye_veliNumaraKolon.Text) &&
                !string.IsNullOrEmpty(rye_sinifKolon.Text) &&
                !string.IsNullOrWhiteSpace(rye_ogrenciNoKolon.Text) &&
                !string.IsNullOrWhiteSpace(rye_ogrenciAdKolon.Text) &&
                !string.IsNullOrWhiteSpace(rye_veliAdiKolon.Text) &&
                !string.IsNullOrWhiteSpace(rye_veliNumaraKolon.Text) &&
                !string.IsNullOrWhiteSpace(rye_sinifKolon.Text)
            )
            { }
            else { throw new EnterAllValues(); }

            // Excel sütun isimleri ve deðerleri listelerde tutulur
            List<string> writeValue = [];
            List<string> columnsName =
            [
                rye_veliAdiKolon.Text,
                rye_veliNumaraKolon.Text,
                rye_ogrenciAdKolon.Text,
                rye_ogrenciNoKolon.Text,
                rye_sinifKolon.Text,
            ];
            int savedVeliCount = 0;

            // Excel dosyasý açýlýr ve ilk çalýþma sayfasý seçilir
            using var workbook = new XLWorkbook(filePath);

            var worksheet = workbook.Worksheets.First(); // Ýlk çalýþma sayfasýný seçiyoruz
            var rows = worksheet.RowsUsed();

            // Tüm satýrlar üzerinde dönülür (ilk satýr sütun isimleri olduðu için Skip(1) kullanýlýr)
            foreach (var row in rows.Skip(1))
            {
                // Sütun isimleri üzerinde dönülerek her bir hücre deðeri alýnýr
                foreach (var columnName in columnsName)
                {
                    // Sütun indeksi (index) belirlenir
                    int index = worksheet.ColumnsUsed(c => c.Cell(1).Value.ToString() == columnName).First().ColumnNumber();
                    // Belirtilen sütundaki hücre alýnýr
                    var cell = row.Cell(index);


                    // Hücre deðeri boþ deðilse listeye eklenir
                    string cellValue = cell.Value.ToString();
                    writeValue.Add(cellValue);


                    // Eðer listeye 5 deðer eklenmiþse, bu deðerler dosyaya yazýlýr
                    if (writeValue.Count == 5)
                    {
                        string[] fileRead = File.ReadAllLines(TxtPath);

                        // Öðrenci numarasý daha önce eklenmiþse hata mesajý gösterilir
                        if (fileRead.Contains(SaveValue.OgrenciNumarasi + writeValue[3]) && !Settings.Default.usedSchoolNumber)
                        {
                            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", $"Öðrenci Numarasý Kullanýlýyor - {writeValue[3]}");
                            writeValue.Clear();
                            continue;
                        }

                        // Dosyaya yazma iþlemi gerçekleþtirilir
                        StreamWriter fileWrite = new(TxtPath, true);
                        fileWrite.WriteLine(" ");
                        fileWrite.WriteLine(SaveValue.VeliAdi + writeValue[0]);
                        fileWrite.WriteLine(SaveValue.VeliNo + writeValue[1]);
                        fileWrite.WriteLine(SaveValue.OgrenciAdi + writeValue[2]);
                        fileWrite.WriteLine(SaveValue.OgrenciNumarasi + writeValue[3]);
                        fileWrite.WriteLine(SaveValue.OgrenciSinif + writeValue[4]);
                        fileWrite.WriteLine(" ");
                        fileWrite.Close();
                        writeValue.Clear();
                        savedVeliCount++;
                    }
                }
            }

            // Ýþlem tamamlandýðýnda bilgi mesajý gösterilir
            _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", $"{savedVeliCount} Veli Eklendi");


        }
        catch (EnterAllValues)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Tüm Deðerleri Eksiksiz Giriniz");
        }
        catch (ExcelFileNotSelected)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Excel Dosyasý Seçilmedi");
        }
        catch (System.IO.IOException)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Excel Dosyanýzý Kapatmalýsýnýz");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata: " + err.Message);
        }
    }
    #endregion

    #region Rehber Yönet Veli-Ekle
    private void ryv_ekle_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }
            if (
                    (ryv_veliNo.Text.Length == 10 || ryv_veliNo.Text.Length == 11) &&
                    !string.IsNullOrEmpty(ryv_ogrenciNo.Text) &&
                    !string.IsNullOrWhiteSpace(ryv_ogrenciNo.Text) &&
                    !string.IsNullOrEmpty(ryv_veliAdi.Text) &&
                    !string.IsNullOrWhiteSpace(ryv_veliAdi.Text) &&
                    !string.IsNullOrEmpty(ryv_ogrenciAd.Text) &&
                    !string.IsNullOrWhiteSpace(ryv_ogrenciAd.Text) &&
                    !string.IsNullOrEmpty(ryv_sinif.Text) &&
                    !string.IsNullOrWhiteSpace(ryv_sinif.Text)
                )
            {
                string[] fileRead = File.ReadAllLines(TxtPath);
                if (fileRead.Contains(SaveValue.OgrenciNumarasi + ryv_ogrenciNo.Text) && !Settings.Default.usedSchoolNumber) { throw new AlreadyUsingStudentNumber(); }
                #region Veli Ekleme
                {
                    StreamWriter fileWrite = new(TxtPath, true);
                    fileWrite.WriteLine(" ");
                    fileWrite.WriteLine(SaveValue.VeliAdi + ryv_veliAdi.Text.ToLower());
                    fileWrite.WriteLine(SaveValue.VeliNo + ryv_veliNo.Text);
                    fileWrite.WriteLine(SaveValue.OgrenciAdi + ryv_ogrenciAd.Text.ToLower());
                    fileWrite.WriteLine(SaveValue.OgrenciNumarasi + ryv_ogrenciNo.Text.Trim());
                    fileWrite.WriteLine(SaveValue.OgrenciSinif + ryv_sinif.Text.Trim().ToLower());
                    fileWrite.WriteLine(" ");
                    fileWrite.Close();
                }
                #endregion

                _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Veli Ekleme Baþarýlý");
            }
            else
            {
                _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Tüm Deðerleri Eksiksiz Giriniz");
            }

        }
        catch (AlreadyUsingStudentNumber)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Öðrenci Numarasý Kullanýlýyor");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata: " + err.Message);
        }
    }
    #endregion

    #region Devamsýzlýk Belgesi Gönder
    private void dbg_cb_withMessage_CheckedChanged(object sender, EventArgs e)
    {
        dbg_txtMessageContent.Enabled = dbg_cb_withMessage.Checked;
    }
    private void dbg_btnSelectPDFfolder_Click(object sender, EventArgs e)
    {
        using FolderBrowserDialog folderDialog = new();

        DialogResult result = folderDialog.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
        {
            string selectedFolder = folderDialog.SelectedPath;
            dbg_txtPDFpath.Text = selectedFolder;
        }

    }
    private void dbg_btnStartProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }

            if (dbg_cb_withMessage.Checked && (string.IsNullOrEmpty(dbg_txtMessageContent.Text) || dbg_txtMessageContent.Text.Replace(" ", "").Length < 1)) { throw new NotEnterMessageContent(); }

            if (MessageBox.Show("Ýþlem Baþlatýlýyor", "Onay", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                _ = ShowToastMessageAsync(ToastMessageType.Info, "Bilgilendirme", "Ýþlem Ýptal Edildi");
                return;
            }

            string[] pdfFiles;
            string[] allVeli;


            pdfFiles = Directory.GetFiles(dbg_txtPDFpath.Text, "*.pdf", SearchOption.AllDirectories);


            allVeli = File.ReadAllLines(TxtPath);
            if (pdfFiles.Length < 1) { throw new NoPdfFileInFolder(); }

            IWebDriver driver = Selenium.DriverInit();

            DialogResult dialogResult = MessageBox.Show("Açýlan Sitede QR Kodu okuttuktan sonra bu butona onay veriniz", "Onay", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult != DialogResult.OK)
            {
                driver.Quit();
                return;
            }
            List<string> sendPdfFiles = [];
            foreach (string pdfFile in pdfFiles)
            {

                if (Path.GetFileName(pdfFile).StartsWith("GÖNDERÝLDÝ_"))
                {
                    continue;
                }

                string? number = extractNumberAtPdf(Path.GetFileName(pdfFile));

                for (int i = 0; i < allVeli.Length; i++)
                {
                    if (allVeli[i] == SaveValue.OgrenciNumarasi + number)
                    {
                        Main.log("-----START-----", false);


                        string veliAdi = allVeli[i - 3].Replace(SaveValue.VeliAdi, "");
                        string veliNumarasi = allVeli[i - 2].Replace(SaveValue.VeliNo, "");
                        string ogrenciAdi = allVeli[i - 1].Replace(SaveValue.OgrenciAdi, "");
                        string ogrenciNumarasi = allVeli[i].Replace(SaveValue.OgrenciNumarasi, "");
                        string ogrenciSinifi = allVeli[i + 1].Replace(SaveValue.OgrenciSinif, "");
                        string messageContent = dbg_txtMessageContent.Text
                            .Replace("<veliadi>", veliAdi)
                            .Replace("<veliNumarasi>", veliNumarasi)
                            .Replace("<ogrenciAdi>", ogrenciAdi)
                            .Replace("<ogrenciNumarasi>", ogrenciNumarasi)
                            .Replace("<ogrenciSinifi>", ogrenciSinifi)
                            ;


                        Selenium.NewChat(driver);

                        Selenium.CancelSearching(driver);

                        Selenium.SearchInput(driver, sendKeys: allVeli[i - 2].Replace(SaveValue.VeliNo, "").Replace("-", "").Replace("(", "").Replace(")", ""));

                        string alertText = $"{allVeli[i - 2].Replace(SaveValue.VeliNo, "").Replace("-", "").Replace("(", "").Replace(")", "")} numaralý veli sizin rehberinizde kayýtlý deðil!";
                        bool state = Selenium.VeliClick(driver, false, alertText);
                        if (state)
                        {
                            dbg_lb_notsend.Items.Add(
                                $"" +
                                $"{allVeli[i]} " +
                                $"{allVeli[i - 3]} " +
                                $"{allVeli[i - 2]} " +
                                $"{allVeli[i - 1]} "
                            );
                            Main.log("------END----", false);
                            continue;
                        }

                        /*burda dosya gönderme iþlemleri*/
                        Selenium.ClickFileDiv(driver);//+butonuna týklar

                        Selenium.SendPdfPath(driver, path: pdfFile);

                        Selenium.SendPdf(driver, ref sendPdfFiles, pdfFile, messageContent);
                        /**/

                        /*if (dbg_cb_withMessage.Checked)
                        {
                            Selenium.WriteMessageContent(driver, dbg_txtMessageContent.Text);

                            Selenium.SendMessageClick(driver);
                        }*/
                        dbg_lb_send.Items.Add(
                                $"" +
                                $"{allVeli[i]} " +
                                $"{allVeli[i - 3]} " +
                                $"{allVeli[i - 2]} " +
                                $"{allVeli[i - 1]} "
                            );
                        Main.log("------END----", false);
                    }
                }

            }
            Thread.Sleep(2500);
            foreach (var item in sendPdfFiles)
            {
                string newFilePath = Path.Combine(Path.GetDirectoryName(item) ?? string.Empty, "GÖNDERÝLDÝ_" + Path.GetFileName(item));
                File.Move(item, newFilePath);
            }
            MessageBox.Show("Ýþlem Tamamlandý", "Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            driver.Quit();
        }
        catch (NotEnterMessageContent)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Mesaj Girmelisiniz");
        }
        catch (NoPdfFileInFolder)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Klasörde pdf dosyasý bulunmuyor");
        }
        catch (ArgumentException)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Klasör Seçmediniz");
        }
        catch (DriverAlreadyClosed)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Ýþlem penceresi kapatýldý devam edilemiyor");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata: " + err.Message);
        }
    }
    private void dbg_clear_sendlb_Click(object sender, EventArgs e)
    {
        dbg_lb_send.Items.Clear();
    }
    private void dbg_clear_notsendlb_Click(object sender, EventArgs e)
    {
        dbg_lb_notsend.Items.Clear();
    }
    private void dbg_clear_sendlb_copy_Click(object sender, EventArgs e)
    {
        if (dbg_lb_send.Items.Count < 1)
        {
            return;
        }
        string copy = "";
        for (int i = 0; i < dbg_lb_send.Items.Count; i++)
        {
            copy += dbg_lb_send.Items[i] + "\n";
        }
        Clipboard.SetText(copy);
        _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Panoya kopyalandý");
    }
    private void dbg_clear_notsendlb_copy_Click(object sender, EventArgs e)
    {
        if (dbg_lb_notsend.Items.Count < 1)
        {
            return;
        }
        string copy = "1";
        for (int i = 0; i < dbg_lb_notsend.Items.Count; i++)
        {
            copy += dbg_lb_notsend.Items[i] + "\n";
        }
        Clipboard.SetText(copy);
        _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Panoya kopyalandý");
    }
    private void dbg_txtMessageContent_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == System.Windows.Forms.Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    public static string? extractNumberAtPdf(string pdfName)
    {
        for (int i = 0; i < pdfName.Length; i++)
        {
            if (char.IsNumber(pdfName[i]))
            {
                if (
                    char.IsNumber(pdfName[i + 0]) &&
                    char.IsNumber(pdfName[i + 1]) &&
                    char.IsNumber(pdfName[i + 2]) &&
                    char.IsNumber(pdfName[i + 3])
                    )
                {
                    return $"{pdfName[i]}{pdfName[i + 1]}{pdfName[i + 2]}{pdfName[i + 3]}";
                }
                else if (
                    char.IsNumber(pdfName[i + 0]) &&
                    char.IsNumber(pdfName[i + 1]) &&
                    char.IsNumber(pdfName[i + 2])
                    )
                {
                    return $"{pdfName[i]}{pdfName[i + 1]}{pdfName[i + 2]}";
                }

            }
        }
        return null;
    }

    #endregion

    #region Mesaj Gönder Öðrenci-No-Ara
    private void mgo_startProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }

            if (string.IsNullOrEmpty(mgo_messageContent.Text) || mgo_messageContent.Text.Replace(" ", "").Length < 1) { throw new NotEnterMessage(); }

            if (MessageBox.Show("Ýþlem Baþlatýlýyor", "Onay", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
            {
                _ = ShowToastMessageAsync(ToastMessageType.Info, "Bilgilendirme", "Ýþlem Ýptal Edildi");
                return;
            }

            IWebDriver driver = Selenium.DriverInit();

            if (MessageBox.Show("Açýlan sitede QR kodu okuttuktan sonra bu butona onay verin", "Onay", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
            {
                driver.Quit();
                return;
            }

            foreach (DataGridViewRow row in mgo_dgv.SelectedRows)
            {
                Main.log("-----START-----", false);

                string? veliAdi = row.Cells[0].Value.ToString();
                string? veliNumarasi = row.Cells[1].Value.ToString()?.Replace("(", "").Replace(")", "").Replace("-", "");
                string? ogrenciAdi = row.Cells[2].Value.ToString();
                string? ogrenciNumarasi = row.Cells[3].Value.ToString();
                string? ogrenciSinifi = row.Cells[4].Value.ToString();
                string messageContent = mgo_messageContent.Text
                    .Replace("<veliadi>", veliAdi)
                    .Replace("<veliNumarasi>", veliNumarasi)
                    .Replace("<ogrenciAdi>", ogrenciAdi)
                    .Replace("<ogrenciNumarasi>", ogrenciNumarasi)
                    .Replace("<ogrenciSinifi>", ogrenciSinifi)
                    ;


                Selenium.NewChat(driver);

                Selenium.CancelSearching(driver);

                Selenium.SearchInput(driver, veliNumarasi ?? string.Empty);

                bool state = Selenium.VeliClick(driver, mgo_cb_alertNotFindNumber.Checked, $"{veliNumarasi} numaralý veli sizin rehberinizde kayýtlý deðil!");
                if (state) { Main.log("-----END-----", false); continue; }

                Selenium.WriteMessageContent(driver, messageContent);

                Selenium.SendMessageClick(driver);

                Main.log("-----END-----", false);
            }
            MessageBox.Show("Ýþlem Tamamlandý", "Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
            driver.Quit();
        }
        catch (NotEnterMessage)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Mesaj Girmediniz");
        }
        catch (DriverAlreadyClosed)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Ýþlem penceresi kapatýldý devam edilemiyor");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata: " + err.Message);
        }
    }
    private void mgo_btnFind_Click(object sender, EventArgs e)
    {
        if (existsDataPath()) { return; }
        mgo_dgv.Rows.Clear();
        string[] entity = File.ReadAllLines(TxtPath);
        int index = 0;
        foreach (string item in entity)
        {
            if (!string.IsNullOrEmpty(item))
            {
                if (item.Contains(SaveValue.OgrenciNumarasi + mgo_txtNo.Text))
                {
                    mgo_dgv.Rows.Add(
                        entity[Array.IndexOf(entity, item, index) - 3].Split(SaveValue.VeliAdi)[1],
                        entity[Array.IndexOf(entity, item, index) - 2].Split(SaveValue.VeliNo)[1],
                        entity[Array.IndexOf(entity, item, index) - 1].Split(SaveValue.OgrenciAdi)[1],
                        entity[Array.IndexOf(entity, item, index) + 0].Split(SaveValue.OgrenciNumarasi)[1],
                        entity[Array.IndexOf(entity, item, index) + 1].Split(SaveValue.OgrenciSinif)[1]
                        );
                }
            }
            index++;
        }
    }
    private void mgo_messageContent_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == System.Windows.Forms.Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }
    #endregion

    #region Mesaj Gönder Sýnýf-Seç
    private void mgs_selectAllClass_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < mgs_clb.Items.Count; i++)
        {
            mgs_clb.SetItemChecked(i, true);
        }
    }
    private void mgs_removeAllClass_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < mgs_clb.Items.Count; i++)
        {
            mgs_clb.SetItemChecked(i, false);
        }
    }
    private void mgs_startProcess_Click(object sender, EventArgs e)
    {
        try
        {
            if (existsDataPath()) { return; }
            if (string.IsNullOrEmpty(mgs_txtMessage.Text) || mgs_txtMessage.Text.Replace(" ", "").Length < 1) { throw new NotEnterMessage(); }

            List<string> selectedClass = [];
            List<string> selectedClassStudent = [];

            string[] allclass = File.ReadAllLines(TxtPath);

            for (int i = 0; i < mgs_clb.Items.Count; i++)
            {
                if (mgs_clb.GetItemChecked(i))
                {
                    selectedClass.Add(mgs_clb.Items[i].ToString() ?? string.Empty);
                }
            }
            if (selectedClass.Count < 1) { throw new NotSelectedClass(); }

            foreach (var item in selectedClass)
            {
                string sinif = item.Split("Sýnýf: ")[1].Split(" |")[0].Trim();
                for (int snf = 0; snf < allclass.Length; snf++)
                {
                    if (allclass[snf] == SaveValue.OgrenciSinif + sinif)
                    {
                        selectedClassStudent.Add($@"Veli Adý: {allclass[Array.IndexOf(allclass, (SaveValue.OgrenciSinif + sinif), snf) - 4].Split(SaveValue.VeliAdi)[1]} Veli Numarasý: {allclass[Array.IndexOf(allclass, (SaveValue.OgrenciSinif + sinif), snf) - 3].Split(SaveValue.VeliNo)[1]} Öðrenci Adý: {allclass[Array.IndexOf(allclass, (SaveValue.OgrenciSinif + sinif), snf) - 2].Split(SaveValue.OgrenciAdi)[1]} Öðrenci Numarasý: {allclass[Array.IndexOf(allclass, (SaveValue.OgrenciSinif + sinif), snf) - 1].Split(SaveValue.OgrenciNumarasi)[1]} Öðrenci Sýnýfý: {allclass[Array.IndexOf(allclass, (SaveValue.OgrenciSinif + sinif), snf)].Split(SaveValue.OgrenciSinif)[1]}");
                    }
                }
            }

            DialogResult result = MessageBox.Show($@"Ýþlem baþlatýlýyor.", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {

                IWebDriver driver = Selenium.DriverInit();

                DialogResult dialogResult = MessageBox.Show("Açýlan Sitede QR Kodu okuttuktan sonra bu butona onay veriniz", "Onay", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult != DialogResult.OK)
                {
                    driver.Quit();
                    return;
                }

                foreach (var student in selectedClassStudent)
                {
                    Main.log("-----START-----", false);



                    string veliAdi = student.Split("Veli Adý: ")[1].Split("Veli Numarasý: ")[0];
                    string veliNumarasi = student.Split("Veli Numarasý: ")[1].Split("Öðrenci Adý: ")[0];
                    string ogrenciAdi = student.Split("Öðrenci Adý: ")[1].Split("Öðrenci Numarasý: ")[0];
                    string ogrenciNumarasi = student.Split("Öðrenci Numarasý: ")[1].Split("Öðrenci Sýnýfý: ")[0];
                    string ogrenciSinifi = student.Split("Öðrenci Sýnýfý: ")[1];
                    string messageContent = mgs_txtMessage.Text
                        .Replace("<veliadi>", veliAdi)
                        .Replace("<veliNumarasi>", veliNumarasi)
                        .Replace("<ogrenciAdi>", ogrenciAdi)
                        .Replace("<ogrenciNumarasi>", ogrenciNumarasi)
                        .Replace("<ogrenciSinifi>", ogrenciSinifi)
                        ;

                    Selenium.NewChat(driver);

                    Selenium.CancelSearching(driver);

                    Selenium.SearchInput(driver, veliNumarasi);

                    string alertText = $"{veliNumarasi} numaralý veli sizin rehberinizde kayýtlý deðil!";
                    bool state = Selenium.VeliClick(driver, mgs_phoneNumberAlert.Checked, alertText);
                    if (state)
                    {
                        Main.log("-----END-----", false);
                        continue;
                    }

                    if (mgs_lbl_ImageLocation.Text != "RESÝM SEÇÝLMEDÝ")
                    {
                        Selenium.ClickFileDiv(driver);

                        Selenium.SendImagePath(driver, mgs_lbl_ImageLocation.Text);

                        Selenium.SendImage(driver, messageContent);
                    }
                    else
                    {
                        Selenium.WriteMessageContent(driver, messageContent);

                        Selenium.SendMessageClick(driver);
                    }

                    Main.log("----END------", false);
                }
                Thread.Sleep(2500);
                MessageBox.Show("Ýþlem Tamamlandý", "Baþarýlý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                driver.Quit();
            }
            else
            {
                _ = ShowToastMessageAsync(ToastMessageType.Info, "Bilgilendirme", "Ýþlem iptal edildi");
            }
        }
        catch (NotEnterMessage)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Mesaj girmediniz");
        }
        catch (NotSelectedClass)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Sýnýf Seçmediniz");
        }
        catch (DriverAlreadyClosed)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Ýþlem penceresi kapatýldý devam edilemiyor");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata " + err.Message);
        }
    }
    private void mgs_btn_addImage_Click(object sender, EventArgs e)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Resim Dosyalarý (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;",
            FilterIndex = 1,
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            mgs_lbl_ImageLocation.Text = openFileDialog.FileName;
        }
    }
    private void mgs_lbl_ImageLocation_Click(object sender, EventArgs e)
    {
        mgs_lbl_ImageLocation.Text = "RESÝM SEÇÝLMEDÝ";
    }
    private async void tabPage3_Enter(object sender, EventArgs e)
    {
        mgo_dgv.Rows.Clear();
        mgs_clb.Items.Clear();

        /*(M)esaj (G)önder (Ö)ðrenci No Ara*/
        if (existsDataPath()) { return; }
        string[] entity = await File.ReadAllLinesAsync(TxtPath);
        int index = 0;
        foreach (string item in entity)
        {
            if (!String.IsNullOrEmpty(item))
            {
                if (item.Contains(SaveValue.VeliAdi) && index < entity.Length)
                {

                    mgo_dgv.Rows.Add(
                         entity[Array.IndexOf(entity, item, index)].Split(SaveValue.VeliAdi)[1],
                         entity[Array.IndexOf(entity, item, index) + 1].Split(SaveValue.VeliNo)[1],
                         entity[Array.IndexOf(entity, item, index) + 2].Split(SaveValue.OgrenciAdi)[1],
                         entity[Array.IndexOf(entity, item, index) + 3].Split(SaveValue.OgrenciNumarasi)[1],
                         entity[Array.IndexOf(entity, item, index) + 4].Split(SaveValue.OgrenciSinif)[1]
                         );
                }
            }
            index++;
        }
        mgo_dgv.ForeColor = Color.Black;
        /**/



        /*(M)esaj (G)önder (S)ýnýf Seç*/
        string[] value = await File.ReadAllLinesAsync(TxtPath);
        Dictionary<string, int> siniflar = [];

        foreach (var sinif in value)
        {
            if (sinif.Contains(SaveValue.OgrenciSinif))
            {
                if (siniflar.ContainsKey(sinif.Replace(SaveValue.OgrenciSinif, "")))
                {
                    siniflar[sinif.Replace(SaveValue.OgrenciSinif, "")]++;
                }
                else
                {
                    siniflar.Add(sinif.Replace(SaveValue.OgrenciSinif, ""), 1);
                }
            }
        }

        foreach (var item in siniflar)
        {
            mgs_clb.Items.Add($"Sýnýf: {item.Key} | Kiþi: {item.Value}");
        }
        /**/
    }
    private void mgs_txtMessage_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == System.Windows.Forms.Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }
    private void mgs_lbl_ImageLocation_MouseHover(object sender, EventArgs e)
    {
        mgs_lbl_ImageLocation.Cursor = Cursors.Hand;
    }
    #endregion

    #region Ayarlar
    ToolTip toolTip;
    private void ToolTipInit()
    {
        toolTip = new();
        toolTip.SetToolTip(ryb_veliAdi, "Öðrenci velisinin adý");
        toolTip.SetToolTip(ryb_veliNumarasi, "Öðrenci velisinin telefon numarasý");
        toolTip.SetToolTip(ryb_ogrenciAdi, "Öðrencinin adý");
        toolTip.SetToolTip(ryb_ogrenciNo, "Öðrencinin okul numarasý.");
        toolTip.SetToolTip(ryb_sinif, "Öðrencinin sýnýfý.");
        toolTip.SetToolTip(ryb_btnUpdate, "Deðiþtirdiðiniz deðerleri kaydedin");
        toolTip.SetToolTip(ryb_checkNumber, "Güncellemek istediðiniz öðrencinin okul numarasýný girin");
        toolTip.SetToolTip(ryb_btnDelete, "Seçili öðrenciyi silin");
        /**/
        toolTip.SetToolTip(rye_selectExcelFile, "Ýþlem yapýlacak excel dosyasýný seçin");
        toolTip.SetToolTip(rye_sinifKolon, "Excel dosyanýzdaki sýnýflarýn bulunduðu kolon adýný girin.(Sýnýflar) þeklinde olabilir");
        toolTip.SetToolTip(rye_ogrenciNoKolon, "Excel dosyanýzdaki öðrenci numaralarýnýn bulunduðu kolon adýný girin.(Numaralar) þeklinde olabilir");
        toolTip.SetToolTip(rye_ogrenciAdKolon, "Excel dosyanýzdaki öðrencilerin adlarýnýn bulunduðu kolon adýný girin.(OgrAdlarý) þeklinde olabilir");
        toolTip.SetToolTip(rye_veliAdiKolon, "Excel dosyanýzdaki velilerin adlarýnýn bulunduðu kolon adýný girin.(VeliÝsimleri) þeklinde olabilir");
        toolTip.SetToolTip(rye_veliNumaraKolon, "Excel dosyanýzdaki velilerin telefon numaralarýnýn bulunduðu kolon adýný girin.(VeliTel) þeklinde olabilir");
        toolTip.SetToolTip(rye_startProcess, "Excel dosyanýzdaki verileri almak için baþlatýn");
        /**/
        toolTip.SetToolTip(ryv_veliAdi, "Eklenecek öðrencinin veli adý.");
        toolTip.SetToolTip(ryv_veliNo, "Eklenecek öðrenci velisinin telefon numarasý.");
        toolTip.SetToolTip(ryv_ogrenciAd, "Eklenecek öðrencinin adý.");
        toolTip.SetToolTip(ryv_ogrenciNo, "Eklenecek öðrencinin okul numarasý.");
        toolTip.SetToolTip(ryv_sinif, "Eklenecek öðrencinin sýnýfý.");
        toolTip.SetToolTip(ryv_ekle, "Eklediðiniz deðerleri kaydedin.");
        /**/
        toolTip.SetToolTip(dbg_btnSelectPDFfolder, "Devamsýzlýk belgelerinin bulunduðu klasörü seçin.");
        toolTip.SetToolTip(dbg_txtMessageContent, "Belge sonrasý gönderilecek mesajý girin.");
        toolTip.SetToolTip(dbg_cb_withMessage, "Belge sonrasý mesaj gönderme seçeneði");
        toolTip.SetToolTip(dbg_btnStartProcess, "Belge gönderimini baþlatýn!");
        /**/
        toolTip.SetToolTip(mgo_txtNo, "Öðrenci numarasý girin.");
        toolTip.SetToolTip(mgo_btnFind, "Öðrenci numarasý arayýn.");
        toolTip.SetToolTip(mgo_messageContent, "Öðrenciye gönderilecek mesajý seçin.");
        toolTip.SetToolTip(mgo_startProcess, "Öðrenciye mesajý gönderin!");
        /**/
        toolTip.SetToolTip(mgs_selectAllClass, "Tüm sýnýflarý seçin.");
        toolTip.SetToolTip(mgs_removeAllClass, "Tüm sýnýflarý kaldýrýn.");
        toolTip.SetToolTip(mgs_phoneNumberAlert, "Rehberinizde kayýtlý olmayan numaralar için uyarý mesajý seçeneði.");
        toolTip.SetToolTip(mgs_txtMessage, "Sýnýf öðrencilerine gönderilecek mesajý girin.");
        toolTip.SetToolTip(mgs_startProcess, "Sýnýf öðrencilerine mesaj gönderin.");
        /**/
        toolTip.AutomaticDelay = 50;
        toolTip.AutoPopDelay = 5000;
    }
    private void settings_cb_chromeOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (settings_cb_chromeOptions.SelectedIndex == 0)
        {
            Settings.Default.chromeOptions = 0;
            Settings.Default.Save();
            Settings.Default.Upgrade();
            Settings.Default.Reload();
        }
        else
        {
            Settings.Default.chromeOptions = 1;
            Settings.Default.Save();
            Settings.Default.Upgrade();
            Settings.Default.Reload();
        }
    }
    private void settings_cb_primary1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.programPirmaryColor1 = settings_cb_primary1.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
        materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
            GetPrimaryColor1(settings_cb_primary1.Text),
            GetPrimaryColor2(settings_cb_primary2.Text),
            GetPrimaryColor3(settings_cb_primary3.Text),
            GetAccentColor(Settings.Default.programAccentColor),
            MaterialSkin.TextShade.WHITE);
    }
    private void settings_cb_primary2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.programPirmaryColor2 = settings_cb_primary2.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
        materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
            GetPrimaryColor1(settings_cb_primary1.Text),
            GetPrimaryColor2(settings_cb_primary2.Text),
            GetPrimaryColor3(settings_cb_primary3.Text),
            GetAccentColor(Settings.Default.programAccentColor),
            MaterialSkin.TextShade.WHITE);
    }
    private void settings_cb_primary3_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.programPirmaryColor3 = settings_cb_primary3.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
        materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
            GetPrimaryColor1(settings_cb_primary1.Text),
            GetPrimaryColor2(settings_cb_primary2.Text),
            GetPrimaryColor3(settings_cb_primary3.Text),
            GetAccentColor(Settings.Default.programAccentColor),
            MaterialSkin.TextShade.WHITE);
    }
    private void settings_cb_txtFile_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.txtFile = settings_cb_txtFile.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
    }
    private void settings_cb_startNewChat_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.startNewChat = settings_cb_startNewChat.Checked;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
    }
    private void materialComboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.programTheme = settings_cb_theme.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
        if (Settings.Default.programTheme == "LIGHT")
        {
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.LIGHT;
        }
        else
        {
            materialSkinManager.Theme = MaterialSkin.MaterialSkinManager.Themes.DARK;
        }
    }
    private void settings_cb_AccentColor_SelectedIndexChanged(object sender, EventArgs e)
    {

        Settings.Default.programAccentColor = settings_cb_AccentColor.Text;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();


        materialSkinManager.ColorScheme = new MaterialSkin.ColorScheme(
            GetPrimaryColor1(settings_cb_primary1.Text),
            GetPrimaryColor2(settings_cb_primary2.Text),
            GetPrimaryColor3(settings_cb_primary3.Text),
            GetAccentColor(settings_cb_AccentColor.Text),
            MaterialSkin.TextShade.WHITE);
    }
    private void settings_btn_SelectStudentImageFolder_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderBrowserDialog = new();
        DialogResult result = folderBrowserDialog.ShowDialog();
        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
        {
            try
            {
                Settings.Default.studentImageFolder = folderBrowserDialog.SelectedPath;
                Settings.Default.Save();
                Settings.Default.Upgrade();
                Settings.Default.Reload();
                settings_txt_studentImageFolder.Text = folderBrowserDialog.SelectedPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
    private void settings_btn_saveProcessDelay_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToInt32(settings_txt_ProcessDelay.Text) < 1000)
            {
                _ = ShowToastMessageAsync(ToastMessageType.Info, "Bilgilendirme", "1000ms den küçük deðer giremezsiniz");
                _ = ShowToastMessageAsync(ToastMessageType.Info, "Bilgilendirme", "bu iþlem yavaþ bilgisayarlar için arttýrýlabilir.");
                return;
            }

            Settings.Default.processDelay = Convert.ToInt32(settings_txt_ProcessDelay.Text);
            Settings.Default.Save();
            Settings.Default.Upgrade();
            Settings.Default.Reload();
            _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Baþarýyla kaydedildi");
        }
        catch (FormatException)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Sayýsal Deðer Girmelisiniz");
        }
        catch (OverflowException)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Çok uzun deðer girdiniz");
        }
    }
    private void setting_btn_openTxtCreateMenu_Click(object sender, EventArgs e)
    {
        settings_btn_CreateTxtFile.Visible = !settings_btn_CreateTxtFile.Visible;
        settings_txt_CreatedTxtFileName.Visible = !settings_txt_CreatedTxtFileName.Visible;
    }
    private void settings_btn_CreateTxtFile_Click(object sender, EventArgs e)
    {
        try
        {
            if (settings_cb_txtFile.Items.Contains(settings_txt_CreatedTxtFileName.Text + ".txt")) { throw new ThisFileAvailable(); }
            if (string.IsNullOrEmpty(settings_txt_CreatedTxtFileName.Text)) { throw new NotEnterFileName(); }
            if (!Directory.Exists(DataFolderPath))
            {
                Directory.CreateDirectory(DataFolderPath);
            }

            string txtFilePath = Path.Combine(DataFolderPath, $"{settings_txt_CreatedTxtFileName.Text}.txt");
            File.Create(txtFilePath).Close();
            Settings.Default.txtFile = settings_txt_CreatedTxtFileName.Text + ".txt";


            settings_cb_txtFile.Items.Clear();
            foreach (var item in Directory.GetFiles(DataFolderPath, "*.txt"))
            {
                settings_cb_txtFile.Items.Add(Path.GetFileName(item));
            }
            settings_cb_txtFile.SelectedItem = Settings.Default.txtFile;
            settings_btn_txtDelete.Visible = settings_btn_txtExport.Visible = settings_cb_txtFile.Items.Count > 0;

            Thread.Sleep(200);

            _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Txt Dosyasý Oluþturuldu");
        }
        catch (NotEnterFileName)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Dosya Ýsmi Girmelisiniz");
        }
        catch (ThisFileAvailable)
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Bu isimde txt dosyasý mevcut");
        }
        catch (Exception err)
        {
            MessageBox.Show("Hata: " + err.Message);
        }
    }
    private void settings_btn_txtDelete_Click(object sender, EventArgs e)
    {
        if (File.Exists(TxtPath))
        {
            if (MessageBox.Show("Dosyayý Silmek Ýstediðinize Eminmisiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                File.Delete(TxtPath);

                _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Ýþlem Tamamlandý");

                settings_cb_txtFile.Items.Clear();
                foreach (var item in Directory.GetFiles(DataFolderPath, "*.txt"))
                {
                    settings_cb_txtFile.Items.Add(Path.GetFileName(item));
                }
                settings_cb_txtFile.SelectedItem = Settings.Default.txtFile;
                settings_btn_txtDelete.Visible = settings_btn_txtExport.Visible = settings_cb_txtFile.Items.Count > 0;
            }
        }
        else
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Dosya Mevcut Deðil");
        }
    }
    private void settings_btn_txtExport_Click(object sender, EventArgs e)
    {
        if (File.Exists(TxtPath))
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                try
                {
                    string hedefDosyaYolu = Path.Combine(folderBrowserDialog.SelectedPath, Path.GetFileName(TxtPath));
                    File.Copy(TxtPath, hedefDosyaYolu, true);

                    _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Dosya Baþarýyla Dýþa Aktarýldý");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
        else
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Dosya Mevcut Deðil");
        }
    }
    private void btn_linkedin_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://www.linkedin.com/in/semihb/",
            UseShellExecute = true,
        });
    }
    private void btn_web_Click(object sender, EventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "http://www.semihbogaz.com.tr/",
            UseShellExecute = true,
        });
    }
    private void settings_cb_usedSchoolNumber_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.usedSchoolNumber = settings_cb_usedSchoolNumber.Checked;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();
    }
    private void settings_cb_EnableTooltip_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.enableTooltip = settings_cb_EnableTooltip.Checked;
        Settings.Default.Save();
        Settings.Default.Upgrade();
        Settings.Default.Reload();

        if (Settings.Default.enableTooltip)
        {
            ToolTipInit();
        }
        else
        {
            toolTip.RemoveAll();
        }

    }
    private static MaterialSkin.Primary GetPrimaryColor3(string colorName)
    {
        return colorName switch
        {
            "Red50" => MaterialSkin.Primary.Red50,
            "Red100" => MaterialSkin.Primary.Red100,
            "Red200" => MaterialSkin.Primary.Red200,
            "Red300" => MaterialSkin.Primary.Red300,
            "Red400" => MaterialSkin.Primary.Red400,
            "Red500" => MaterialSkin.Primary.Red500,
            "Red600" => MaterialSkin.Primary.Red600,
            "Red700" => MaterialSkin.Primary.Red700,
            "Red800" => MaterialSkin.Primary.Red800,
            "Red900" => MaterialSkin.Primary.Red900,
            "Pink50" => MaterialSkin.Primary.Pink50,
            "Pink100" => MaterialSkin.Primary.Pink100,
            "Pink200" => MaterialSkin.Primary.Pink200,
            "Pink300" => MaterialSkin.Primary.Pink300,
            "Pink400" => MaterialSkin.Primary.Pink400,
            "Pink500" => MaterialSkin.Primary.Pink500,
            "Pink600" => MaterialSkin.Primary.Pink600,
            "Pink700" => MaterialSkin.Primary.Pink700,
            "Pink800" => MaterialSkin.Primary.Pink800,
            "Pink900" => MaterialSkin.Primary.Pink900,
            "Purple50" => MaterialSkin.Primary.Purple50,
            "Purple100" => MaterialSkin.Primary.Purple100,
            "Purple200" => MaterialSkin.Primary.Purple200,
            "Purple300" => MaterialSkin.Primary.Purple300,
            "Purple400" => MaterialSkin.Primary.Purple400,
            "Purple500" => MaterialSkin.Primary.Purple500,
            "Purple600" => MaterialSkin.Primary.Purple600,
            "Purple700" => MaterialSkin.Primary.Purple700,
            "Purple800" => MaterialSkin.Primary.Purple800,
            "Purple900" => MaterialSkin.Primary.Purple900,
            "DeepPurple50" => MaterialSkin.Primary.DeepPurple50,
            "DeepPurple100" => MaterialSkin.Primary.DeepPurple100,
            "DeepPurple200" => MaterialSkin.Primary.DeepPurple200,
            "DeepPurple300" => MaterialSkin.Primary.DeepPurple300,
            "DeepPurple400" => MaterialSkin.Primary.DeepPurple400,
            "DeepPurple500" => MaterialSkin.Primary.DeepPurple500,
            "DeepPurple600" => MaterialSkin.Primary.DeepPurple600,
            "DeepPurple700" => MaterialSkin.Primary.DeepPurple700,
            "DeepPurple800" => MaterialSkin.Primary.DeepPurple800,
            "DeepPurple900" => MaterialSkin.Primary.DeepPurple900,
            "Indigo50" => MaterialSkin.Primary.Indigo50,
            "Indigo100" => MaterialSkin.Primary.Indigo100,
            "Indigo200" => MaterialSkin.Primary.Indigo200,
            "Indigo300" => MaterialSkin.Primary.Indigo300,
            "Indigo400" => MaterialSkin.Primary.Indigo400,
            "Indigo500" => MaterialSkin.Primary.Indigo500,
            "Indigo600" => MaterialSkin.Primary.Indigo600,
            "Indigo700" => MaterialSkin.Primary.Indigo700,
            "Indigo800" => MaterialSkin.Primary.Indigo800,
            "Indigo900" => MaterialSkin.Primary.Indigo900,
            "Blue50" => MaterialSkin.Primary.Blue50,
            "Blue100" => MaterialSkin.Primary.Blue100,
            "Blue200" => MaterialSkin.Primary.Blue200,
            "Blue300" => MaterialSkin.Primary.Blue300,
            "Blue400" => MaterialSkin.Primary.Blue400,
            "Blue500" => MaterialSkin.Primary.Blue500,
            "Blue600" => MaterialSkin.Primary.Blue600,
            "Blue700" => MaterialSkin.Primary.Blue700,
            "Blue800" => MaterialSkin.Primary.Blue800,
            "Blue900" => MaterialSkin.Primary.Blue900,
            "LightBlue50" => MaterialSkin.Primary.LightBlue50,
            "LightBlue100" => MaterialSkin.Primary.LightBlue100,
            "LightBlue200" => MaterialSkin.Primary.LightBlue200,
            "LightBlue300" => MaterialSkin.Primary.LightBlue300,
            "LightBlue400" => MaterialSkin.Primary.LightBlue400,
            "LightBlue500" => MaterialSkin.Primary.LightBlue500,
            "LightBlue600" => MaterialSkin.Primary.LightBlue600,
            "LightBlue700" => MaterialSkin.Primary.LightBlue700,
            "LightBlue800" => MaterialSkin.Primary.LightBlue800,
            "LightBlue900" => MaterialSkin.Primary.LightBlue900,
            "Cyan50" => MaterialSkin.Primary.Cyan50,
            "Cyan100" => MaterialSkin.Primary.Cyan100,
            "Cyan200" => MaterialSkin.Primary.Cyan200,
            "Cyan300" => MaterialSkin.Primary.Cyan300,
            "Cyan400" => MaterialSkin.Primary.Cyan400,
            "Cyan500" => MaterialSkin.Primary.Cyan500,
            "Cyan600" => MaterialSkin.Primary.Cyan600,
            "Cyan700" => MaterialSkin.Primary.Cyan700,
            "Cyan800" => MaterialSkin.Primary.Cyan800,
            "Cyan900" => MaterialSkin.Primary.Cyan900,
            "Teal50" => MaterialSkin.Primary.Teal50,
            "Teal100" => MaterialSkin.Primary.Teal100,
            "Teal200" => MaterialSkin.Primary.Teal200,
            "Teal300" => MaterialSkin.Primary.Teal300,
            "Teal400" => MaterialSkin.Primary.Teal400,
            "Teal500" => MaterialSkin.Primary.Teal500,
            "Teal600" => MaterialSkin.Primary.Teal600,
            "Teal700" => MaterialSkin.Primary.Teal700,
            "Teal800" => MaterialSkin.Primary.Teal800,
            "Teal900" => MaterialSkin.Primary.Teal900,
            "Green50" => MaterialSkin.Primary.Green50,
            "Green100" => MaterialSkin.Primary.Green100,
            "Green200" => MaterialSkin.Primary.Green200,
            "Green300" => MaterialSkin.Primary.Green300,
            "Green400" => MaterialSkin.Primary.Green400,
            "Green500" => MaterialSkin.Primary.Green500,
            "Green600" => MaterialSkin.Primary.Green600,
            "Green700" => MaterialSkin.Primary.Green700,
            "Green800" => MaterialSkin.Primary.Green800,
            "Green900" => MaterialSkin.Primary.Green900,
            "LightGreen50" => MaterialSkin.Primary.LightGreen50,
            "LightGreen100" => MaterialSkin.Primary.LightGreen100,
            "LightGreen200" => MaterialSkin.Primary.LightGreen200,
            "LightGreen300" => MaterialSkin.Primary.LightGreen300,
            "LightGreen400" => MaterialSkin.Primary.LightGreen400,
            "LightGreen500" => MaterialSkin.Primary.LightGreen500,
            "LightGreen600" => MaterialSkin.Primary.LightGreen600,
            "LightGreen700" => MaterialSkin.Primary.LightGreen700,
            "LightGreen800" => MaterialSkin.Primary.LightGreen800,
            "LightGreen900" => MaterialSkin.Primary.LightGreen900,
            "Lime50" => MaterialSkin.Primary.Lime50,
            "Lime100" => MaterialSkin.Primary.Lime100,
            "Lime200" => MaterialSkin.Primary.Lime200,
            "Lime300" => MaterialSkin.Primary.Lime300,
            "Lime400" => MaterialSkin.Primary.Lime400,
            "Lime500" => MaterialSkin.Primary.Lime500,
            "Lime600" => MaterialSkin.Primary.Lime600,
            "Lime700" => MaterialSkin.Primary.Lime700,
            "Lime800" => MaterialSkin.Primary.Lime800,
            "Lime900" => MaterialSkin.Primary.Lime900,
            "Yellow50" => MaterialSkin.Primary.Yellow50,
            "Yellow100" => MaterialSkin.Primary.Yellow100,
            "Yellow200" => MaterialSkin.Primary.Yellow200,
            "Yellow300" => MaterialSkin.Primary.Yellow300,
            "Yellow400" => MaterialSkin.Primary.Yellow400,
            "Yellow500" => MaterialSkin.Primary.Yellow500,
            "Yellow600" => MaterialSkin.Primary.Yellow600,
            "Yellow700" => MaterialSkin.Primary.Yellow700,
            "Yellow800" => MaterialSkin.Primary.Yellow800,
            "Yellow900" => MaterialSkin.Primary.Yellow900,
            "Amber50" => MaterialSkin.Primary.Amber50,
            "Amber100" => MaterialSkin.Primary.Amber100,
            "Amber200" => MaterialSkin.Primary.Amber200,
            "Amber300" => MaterialSkin.Primary.Amber300,
            "Amber400" => MaterialSkin.Primary.Amber400,
            "Amber500" => MaterialSkin.Primary.Amber500,
            "Amber600" => MaterialSkin.Primary.Amber600,
            "Amber700" => MaterialSkin.Primary.Amber700,
            "Amber800" => MaterialSkin.Primary.Amber800,
            "Amber900" => MaterialSkin.Primary.Amber900,
            "Orange50" => MaterialSkin.Primary.Orange50,
            "Orange100" => MaterialSkin.Primary.Orange100,
            "Orange200" => MaterialSkin.Primary.Orange200,
            "Orange300" => MaterialSkin.Primary.Orange300,
            "Orange400" => MaterialSkin.Primary.Orange400,
            "Orange500" => MaterialSkin.Primary.Orange500,
            "Orange600" => MaterialSkin.Primary.Orange600,
            "Orange700" => MaterialSkin.Primary.Orange700,
            "Orange800" => MaterialSkin.Primary.Orange800,
            "Orange900" => MaterialSkin.Primary.Orange900,
            "DeepOrange50" => MaterialSkin.Primary.DeepOrange50,
            "DeepOrange100" => MaterialSkin.Primary.DeepOrange100,
            "DeepOrange200" => MaterialSkin.Primary.DeepOrange200,
            "DeepOrange300" => MaterialSkin.Primary.DeepOrange300,
            "DeepOrange400" => MaterialSkin.Primary.DeepOrange400,
            "DeepOrange500" => MaterialSkin.Primary.DeepOrange500,
            "DeepOrange600" => MaterialSkin.Primary.DeepOrange600,
            "DeepOrange700" => MaterialSkin.Primary.DeepOrange700,
            "DeepOrange800" => MaterialSkin.Primary.DeepOrange800,
            "DeepOrange900" => MaterialSkin.Primary.DeepOrange900,
            "Brown50" => MaterialSkin.Primary.Brown50,
            "Brown100" => MaterialSkin.Primary.Brown100,
            "Brown200" => MaterialSkin.Primary.Brown200,
            "Brown300" => MaterialSkin.Primary.Brown300,
            "Brown400" => MaterialSkin.Primary.Brown400,
            "Brown500" => MaterialSkin.Primary.Brown500,
            "Brown600" => MaterialSkin.Primary.Brown600,
            "Brown700" => MaterialSkin.Primary.Brown700,
            "Brown800" => MaterialSkin.Primary.Brown800,
            "Brown900" => MaterialSkin.Primary.Brown900,
            "Grey50" => MaterialSkin.Primary.Grey50,
            "Grey100" => MaterialSkin.Primary.Grey100,
            "Grey200" => MaterialSkin.Primary.Grey200,
            "Grey300" => MaterialSkin.Primary.Grey300,
            "Grey400" => MaterialSkin.Primary.Grey400,
            "Grey500" => MaterialSkin.Primary.Grey500,
            "Grey600" => MaterialSkin.Primary.Grey600,
            "Grey700" => MaterialSkin.Primary.Grey700,
            "Grey800" => MaterialSkin.Primary.Grey800,
            "Grey900" => MaterialSkin.Primary.Grey900,
            "BlueGrey50" => MaterialSkin.Primary.BlueGrey50,
            "BlueGrey100" => MaterialSkin.Primary.BlueGrey100,
            "BlueGrey200" => MaterialSkin.Primary.BlueGrey200,
            "BlueGrey300" => MaterialSkin.Primary.BlueGrey300,
            "BlueGrey400" => MaterialSkin.Primary.BlueGrey400,
            "BlueGrey500" => MaterialSkin.Primary.BlueGrey500,
            "BlueGrey600" => MaterialSkin.Primary.BlueGrey600,
            "BlueGrey700" => MaterialSkin.Primary.BlueGrey700,
            "BlueGrey800" => MaterialSkin.Primary.BlueGrey800,
            "BlueGrey900" => MaterialSkin.Primary.BlueGrey900,
            _ => MaterialSkin.Primary.Indigo100
        };
    }
    private static MaterialSkin.Primary GetPrimaryColor2(string colorName)
    {
        return colorName switch
        {
            "Red50" => MaterialSkin.Primary.Red50,
            "Red100" => MaterialSkin.Primary.Red100,
            "Red200" => MaterialSkin.Primary.Red200,
            "Red300" => MaterialSkin.Primary.Red300,
            "Red400" => MaterialSkin.Primary.Red400,
            "Red500" => MaterialSkin.Primary.Red500,
            "Red600" => MaterialSkin.Primary.Red600,
            "Red700" => MaterialSkin.Primary.Red700,
            "Red800" => MaterialSkin.Primary.Red800,
            "Red900" => MaterialSkin.Primary.Red900,
            "Pink50" => MaterialSkin.Primary.Pink50,
            "Pink100" => MaterialSkin.Primary.Pink100,
            "Pink200" => MaterialSkin.Primary.Pink200,
            "Pink300" => MaterialSkin.Primary.Pink300,
            "Pink400" => MaterialSkin.Primary.Pink400,
            "Pink500" => MaterialSkin.Primary.Pink500,
            "Pink600" => MaterialSkin.Primary.Pink600,
            "Pink700" => MaterialSkin.Primary.Pink700,
            "Pink800" => MaterialSkin.Primary.Pink800,
            "Pink900" => MaterialSkin.Primary.Pink900,
            "Purple50" => MaterialSkin.Primary.Purple50,
            "Purple100" => MaterialSkin.Primary.Purple100,
            "Purple200" => MaterialSkin.Primary.Purple200,
            "Purple300" => MaterialSkin.Primary.Purple300,
            "Purple400" => MaterialSkin.Primary.Purple400,
            "Purple500" => MaterialSkin.Primary.Purple500,
            "Purple600" => MaterialSkin.Primary.Purple600,
            "Purple700" => MaterialSkin.Primary.Purple700,
            "Purple800" => MaterialSkin.Primary.Purple800,
            "Purple900" => MaterialSkin.Primary.Purple900,
            "DeepPurple50" => MaterialSkin.Primary.DeepPurple50,
            "DeepPurple100" => MaterialSkin.Primary.DeepPurple100,
            "DeepPurple200" => MaterialSkin.Primary.DeepPurple200,
            "DeepPurple300" => MaterialSkin.Primary.DeepPurple300,
            "DeepPurple400" => MaterialSkin.Primary.DeepPurple400,
            "DeepPurple500" => MaterialSkin.Primary.DeepPurple500,
            "DeepPurple600" => MaterialSkin.Primary.DeepPurple600,
            "DeepPurple700" => MaterialSkin.Primary.DeepPurple700,
            "DeepPurple800" => MaterialSkin.Primary.DeepPurple800,
            "DeepPurple900" => MaterialSkin.Primary.DeepPurple900,
            "Indigo50" => MaterialSkin.Primary.Indigo50,
            "Indigo100" => MaterialSkin.Primary.Indigo100,
            "Indigo200" => MaterialSkin.Primary.Indigo200,
            "Indigo300" => MaterialSkin.Primary.Indigo300,
            "Indigo400" => MaterialSkin.Primary.Indigo400,
            "Indigo500" => MaterialSkin.Primary.Indigo500,
            "Indigo600" => MaterialSkin.Primary.Indigo600,
            "Indigo700" => MaterialSkin.Primary.Indigo700,
            "Indigo800" => MaterialSkin.Primary.Indigo800,
            "Indigo900" => MaterialSkin.Primary.Indigo900,
            "Blue50" => MaterialSkin.Primary.Blue50,
            "Blue100" => MaterialSkin.Primary.Blue100,
            "Blue200" => MaterialSkin.Primary.Blue200,
            "Blue300" => MaterialSkin.Primary.Blue300,
            "Blue400" => MaterialSkin.Primary.Blue400,
            "Blue500" => MaterialSkin.Primary.Blue500,
            "Blue600" => MaterialSkin.Primary.Blue600,
            "Blue700" => MaterialSkin.Primary.Blue700,
            "Blue800" => MaterialSkin.Primary.Blue800,
            "Blue900" => MaterialSkin.Primary.Blue900,
            "LightBlue50" => MaterialSkin.Primary.LightBlue50,
            "LightBlue100" => MaterialSkin.Primary.LightBlue100,
            "LightBlue200" => MaterialSkin.Primary.LightBlue200,
            "LightBlue300" => MaterialSkin.Primary.LightBlue300,
            "LightBlue400" => MaterialSkin.Primary.LightBlue400,
            "LightBlue500" => MaterialSkin.Primary.LightBlue500,
            "LightBlue600" => MaterialSkin.Primary.LightBlue600,
            "LightBlue700" => MaterialSkin.Primary.LightBlue700,
            "LightBlue800" => MaterialSkin.Primary.LightBlue800,
            "LightBlue900" => MaterialSkin.Primary.LightBlue900,
            "Cyan50" => MaterialSkin.Primary.Cyan50,
            "Cyan100" => MaterialSkin.Primary.Cyan100,
            "Cyan200" => MaterialSkin.Primary.Cyan200,
            "Cyan300" => MaterialSkin.Primary.Cyan300,
            "Cyan400" => MaterialSkin.Primary.Cyan400,
            "Cyan500" => MaterialSkin.Primary.Cyan500,
            "Cyan600" => MaterialSkin.Primary.Cyan600,
            "Cyan700" => MaterialSkin.Primary.Cyan700,
            "Cyan800" => MaterialSkin.Primary.Cyan800,
            "Cyan900" => MaterialSkin.Primary.Cyan900,
            "Teal50" => MaterialSkin.Primary.Teal50,
            "Teal100" => MaterialSkin.Primary.Teal100,
            "Teal200" => MaterialSkin.Primary.Teal200,
            "Teal300" => MaterialSkin.Primary.Teal300,
            "Teal400" => MaterialSkin.Primary.Teal400,
            "Teal500" => MaterialSkin.Primary.Teal500,
            "Teal600" => MaterialSkin.Primary.Teal600,
            "Teal700" => MaterialSkin.Primary.Teal700,
            "Teal800" => MaterialSkin.Primary.Teal800,
            "Teal900" => MaterialSkin.Primary.Teal900,
            "Green50" => MaterialSkin.Primary.Green50,
            "Green100" => MaterialSkin.Primary.Green100,
            "Green200" => MaterialSkin.Primary.Green200,
            "Green300" => MaterialSkin.Primary.Green300,
            "Green400" => MaterialSkin.Primary.Green400,
            "Green500" => MaterialSkin.Primary.Green500,
            "Green600" => MaterialSkin.Primary.Green600,
            "Green700" => MaterialSkin.Primary.Green700,
            "Green800" => MaterialSkin.Primary.Green800,
            "Green900" => MaterialSkin.Primary.Green900,
            "LightGreen50" => MaterialSkin.Primary.LightGreen50,
            "LightGreen100" => MaterialSkin.Primary.LightGreen100,
            "LightGreen200" => MaterialSkin.Primary.LightGreen200,
            "LightGreen300" => MaterialSkin.Primary.LightGreen300,
            "LightGreen400" => MaterialSkin.Primary.LightGreen400,
            "LightGreen500" => MaterialSkin.Primary.LightGreen500,
            "LightGreen600" => MaterialSkin.Primary.LightGreen600,
            "LightGreen700" => MaterialSkin.Primary.LightGreen700,
            "LightGreen800" => MaterialSkin.Primary.LightGreen800,
            "LightGreen900" => MaterialSkin.Primary.LightGreen900,
            "Lime50" => MaterialSkin.Primary.Lime50,
            "Lime100" => MaterialSkin.Primary.Lime100,
            "Lime200" => MaterialSkin.Primary.Lime200,
            "Lime300" => MaterialSkin.Primary.Lime300,
            "Lime400" => MaterialSkin.Primary.Lime400,
            "Lime500" => MaterialSkin.Primary.Lime500,
            "Lime600" => MaterialSkin.Primary.Lime600,
            "Lime700" => MaterialSkin.Primary.Lime700,
            "Lime800" => MaterialSkin.Primary.Lime800,
            "Lime900" => MaterialSkin.Primary.Lime900,
            "Yellow50" => MaterialSkin.Primary.Yellow50,
            "Yellow100" => MaterialSkin.Primary.Yellow100,
            "Yellow200" => MaterialSkin.Primary.Yellow200,
            "Yellow300" => MaterialSkin.Primary.Yellow300,
            "Yellow400" => MaterialSkin.Primary.Yellow400,
            "Yellow500" => MaterialSkin.Primary.Yellow500,
            "Yellow600" => MaterialSkin.Primary.Yellow600,
            "Yellow700" => MaterialSkin.Primary.Yellow700,
            "Yellow800" => MaterialSkin.Primary.Yellow800,
            "Yellow900" => MaterialSkin.Primary.Yellow900,
            "Amber50" => MaterialSkin.Primary.Amber50,
            "Amber100" => MaterialSkin.Primary.Amber100,
            "Amber200" => MaterialSkin.Primary.Amber200,
            "Amber300" => MaterialSkin.Primary.Amber300,
            "Amber400" => MaterialSkin.Primary.Amber400,
            "Amber500" => MaterialSkin.Primary.Amber500,
            "Amber600" => MaterialSkin.Primary.Amber600,
            "Amber700" => MaterialSkin.Primary.Amber700,
            "Amber800" => MaterialSkin.Primary.Amber800,
            "Amber900" => MaterialSkin.Primary.Amber900,
            "Orange50" => MaterialSkin.Primary.Orange50,
            "Orange100" => MaterialSkin.Primary.Orange100,
            "Orange200" => MaterialSkin.Primary.Orange200,
            "Orange300" => MaterialSkin.Primary.Orange300,
            "Orange400" => MaterialSkin.Primary.Orange400,
            "Orange500" => MaterialSkin.Primary.Orange500,
            "Orange600" => MaterialSkin.Primary.Orange600,
            "Orange700" => MaterialSkin.Primary.Orange700,
            "Orange800" => MaterialSkin.Primary.Orange800,
            "Orange900" => MaterialSkin.Primary.Orange900,
            "DeepOrange50" => MaterialSkin.Primary.DeepOrange50,
            "DeepOrange100" => MaterialSkin.Primary.DeepOrange100,
            "DeepOrange200" => MaterialSkin.Primary.DeepOrange200,
            "DeepOrange300" => MaterialSkin.Primary.DeepOrange300,
            "DeepOrange400" => MaterialSkin.Primary.DeepOrange400,
            "DeepOrange500" => MaterialSkin.Primary.DeepOrange500,
            "DeepOrange600" => MaterialSkin.Primary.DeepOrange600,
            "DeepOrange700" => MaterialSkin.Primary.DeepOrange700,
            "DeepOrange800" => MaterialSkin.Primary.DeepOrange800,
            "DeepOrange900" => MaterialSkin.Primary.DeepOrange900,
            "Brown50" => MaterialSkin.Primary.Brown50,
            "Brown100" => MaterialSkin.Primary.Brown100,
            "Brown200" => MaterialSkin.Primary.Brown200,
            "Brown300" => MaterialSkin.Primary.Brown300,
            "Brown400" => MaterialSkin.Primary.Brown400,
            "Brown500" => MaterialSkin.Primary.Brown500,
            "Brown600" => MaterialSkin.Primary.Brown600,
            "Brown700" => MaterialSkin.Primary.Brown700,
            "Brown800" => MaterialSkin.Primary.Brown800,
            "Brown900" => MaterialSkin.Primary.Brown900,
            "Grey50" => MaterialSkin.Primary.Grey50,
            "Grey100" => MaterialSkin.Primary.Grey100,
            "Grey200" => MaterialSkin.Primary.Grey200,
            "Grey300" => MaterialSkin.Primary.Grey300,
            "Grey400" => MaterialSkin.Primary.Grey400,
            "Grey500" => MaterialSkin.Primary.Grey500,
            "Grey600" => MaterialSkin.Primary.Grey600,
            "Grey700" => MaterialSkin.Primary.Grey700,
            "Grey800" => MaterialSkin.Primary.Grey800,
            "Grey900" => MaterialSkin.Primary.Grey900,
            "BlueGrey50" => MaterialSkin.Primary.BlueGrey50,
            "BlueGrey100" => MaterialSkin.Primary.BlueGrey100,
            "BlueGrey200" => MaterialSkin.Primary.BlueGrey200,
            "BlueGrey300" => MaterialSkin.Primary.BlueGrey300,
            "BlueGrey400" => MaterialSkin.Primary.BlueGrey400,
            "BlueGrey500" => MaterialSkin.Primary.BlueGrey500,
            "BlueGrey600" => MaterialSkin.Primary.BlueGrey600,
            "BlueGrey700" => MaterialSkin.Primary.BlueGrey700,
            "BlueGrey800" => MaterialSkin.Primary.BlueGrey800,
            "BlueGrey900" => MaterialSkin.Primary.BlueGrey900,
            _ => MaterialSkin.Primary.Indigo700,
        };
    }
    private static MaterialSkin.Primary GetPrimaryColor1(string colorName)
    {
        return colorName switch
        {
            "Red50" => MaterialSkin.Primary.Red50,
            "Red100" => MaterialSkin.Primary.Red100,
            "Red200" => MaterialSkin.Primary.Red200,
            "Red300" => MaterialSkin.Primary.Red300,
            "Red400" => MaterialSkin.Primary.Red400,
            "Red500" => MaterialSkin.Primary.Red500,
            "Red600" => MaterialSkin.Primary.Red600,
            "Red700" => MaterialSkin.Primary.Red700,
            "Red800" => MaterialSkin.Primary.Red800,
            "Red900" => MaterialSkin.Primary.Red900,
            "Pink50" => MaterialSkin.Primary.Pink50,
            "Pink100" => MaterialSkin.Primary.Pink100,
            "Pink200" => MaterialSkin.Primary.Pink200,
            "Pink300" => MaterialSkin.Primary.Pink300,
            "Pink400" => MaterialSkin.Primary.Pink400,
            "Pink500" => MaterialSkin.Primary.Pink500,
            "Pink600" => MaterialSkin.Primary.Pink600,
            "Pink700" => MaterialSkin.Primary.Pink700,
            "Pink800" => MaterialSkin.Primary.Pink800,
            "Pink900" => MaterialSkin.Primary.Pink900,
            "Purple50" => MaterialSkin.Primary.Purple50,
            "Purple100" => MaterialSkin.Primary.Purple100,
            "Purple200" => MaterialSkin.Primary.Purple200,
            "Purple300" => MaterialSkin.Primary.Purple300,
            "Purple400" => MaterialSkin.Primary.Purple400,
            "Purple500" => MaterialSkin.Primary.Purple500,
            "Purple600" => MaterialSkin.Primary.Purple600,
            "Purple700" => MaterialSkin.Primary.Purple700,
            "Purple800" => MaterialSkin.Primary.Purple800,
            "Purple900" => MaterialSkin.Primary.Purple900,
            "DeepPurple50" => MaterialSkin.Primary.DeepPurple50,
            "DeepPurple100" => MaterialSkin.Primary.DeepPurple100,
            "DeepPurple200" => MaterialSkin.Primary.DeepPurple200,
            "DeepPurple300" => MaterialSkin.Primary.DeepPurple300,
            "DeepPurple400" => MaterialSkin.Primary.DeepPurple400,
            "DeepPurple500" => MaterialSkin.Primary.DeepPurple500,
            "DeepPurple600" => MaterialSkin.Primary.DeepPurple600,
            "DeepPurple700" => MaterialSkin.Primary.DeepPurple700,
            "DeepPurple800" => MaterialSkin.Primary.DeepPurple800,
            "DeepPurple900" => MaterialSkin.Primary.DeepPurple900,
            "Indigo50" => MaterialSkin.Primary.Indigo50,
            "Indigo100" => MaterialSkin.Primary.Indigo100,
            "Indigo200" => MaterialSkin.Primary.Indigo200,
            "Indigo300" => MaterialSkin.Primary.Indigo300,
            "Indigo400" => MaterialSkin.Primary.Indigo400,
            "Indigo500" => MaterialSkin.Primary.Indigo500,
            "Indigo600" => MaterialSkin.Primary.Indigo600,
            "Indigo700" => MaterialSkin.Primary.Indigo700,
            "Indigo800" => MaterialSkin.Primary.Indigo800,
            "Indigo900" => MaterialSkin.Primary.Indigo900,
            "Blue50" => MaterialSkin.Primary.Blue50,
            "Blue100" => MaterialSkin.Primary.Blue100,
            "Blue200" => MaterialSkin.Primary.Blue200,
            "Blue300" => MaterialSkin.Primary.Blue300,
            "Blue400" => MaterialSkin.Primary.Blue400,
            "Blue500" => MaterialSkin.Primary.Blue500,
            "Blue600" => MaterialSkin.Primary.Blue600,
            "Blue700" => MaterialSkin.Primary.Blue700,
            "Blue800" => MaterialSkin.Primary.Blue800,
            "Blue900" => MaterialSkin.Primary.Blue900,
            "LightBlue50" => MaterialSkin.Primary.LightBlue50,
            "LightBlue100" => MaterialSkin.Primary.LightBlue100,
            "LightBlue200" => MaterialSkin.Primary.LightBlue200,
            "LightBlue300" => MaterialSkin.Primary.LightBlue300,
            "LightBlue400" => MaterialSkin.Primary.LightBlue400,
            "LightBlue500" => MaterialSkin.Primary.LightBlue500,
            "LightBlue600" => MaterialSkin.Primary.LightBlue600,
            "LightBlue700" => MaterialSkin.Primary.LightBlue700,
            "LightBlue800" => MaterialSkin.Primary.LightBlue800,
            "LightBlue900" => MaterialSkin.Primary.LightBlue900,
            "Cyan50" => MaterialSkin.Primary.Cyan50,
            "Cyan100" => MaterialSkin.Primary.Cyan100,
            "Cyan200" => MaterialSkin.Primary.Cyan200,
            "Cyan300" => MaterialSkin.Primary.Cyan300,
            "Cyan400" => MaterialSkin.Primary.Cyan400,
            "Cyan500" => MaterialSkin.Primary.Cyan500,
            "Cyan600" => MaterialSkin.Primary.Cyan600,
            "Cyan700" => MaterialSkin.Primary.Cyan700,
            "Cyan800" => MaterialSkin.Primary.Cyan800,
            "Cyan900" => MaterialSkin.Primary.Cyan900,
            "Teal50" => MaterialSkin.Primary.Teal50,
            "Teal100" => MaterialSkin.Primary.Teal100,
            "Teal200" => MaterialSkin.Primary.Teal200,
            "Teal300" => MaterialSkin.Primary.Teal300,
            "Teal400" => MaterialSkin.Primary.Teal400,
            "Teal500" => MaterialSkin.Primary.Teal500,
            "Teal600" => MaterialSkin.Primary.Teal600,
            "Teal700" => MaterialSkin.Primary.Teal700,
            "Teal800" => MaterialSkin.Primary.Teal800,
            "Teal900" => MaterialSkin.Primary.Teal900,
            "Green50" => MaterialSkin.Primary.Green50,
            "Green100" => MaterialSkin.Primary.Green100,
            "Green200" => MaterialSkin.Primary.Green200,
            "Green300" => MaterialSkin.Primary.Green300,
            "Green400" => MaterialSkin.Primary.Green400,
            "Green500" => MaterialSkin.Primary.Green500,
            "Green600" => MaterialSkin.Primary.Green600,
            "Green700" => MaterialSkin.Primary.Green700,
            "Green800" => MaterialSkin.Primary.Green800,
            "Green900" => MaterialSkin.Primary.Green900,
            "LightGreen50" => MaterialSkin.Primary.LightGreen50,
            "LightGreen100" => MaterialSkin.Primary.LightGreen100,
            "LightGreen200" => MaterialSkin.Primary.LightGreen200,
            "LightGreen300" => MaterialSkin.Primary.LightGreen300,
            "LightGreen400" => MaterialSkin.Primary.LightGreen400,
            "LightGreen500" => MaterialSkin.Primary.LightGreen500,
            "LightGreen600" => MaterialSkin.Primary.LightGreen600,
            "LightGreen700" => MaterialSkin.Primary.LightGreen700,
            "LightGreen800" => MaterialSkin.Primary.LightGreen800,
            "LightGreen900" => MaterialSkin.Primary.LightGreen900,
            "Lime50" => MaterialSkin.Primary.Lime50,
            "Lime100" => MaterialSkin.Primary.Lime100,
            "Lime200" => MaterialSkin.Primary.Lime200,
            "Lime300" => MaterialSkin.Primary.Lime300,
            "Lime400" => MaterialSkin.Primary.Lime400,
            "Lime500" => MaterialSkin.Primary.Lime500,
            "Lime600" => MaterialSkin.Primary.Lime600,
            "Lime700" => MaterialSkin.Primary.Lime700,
            "Lime800" => MaterialSkin.Primary.Lime800,
            "Lime900" => MaterialSkin.Primary.Lime900,
            "Yellow50" => MaterialSkin.Primary.Yellow50,
            "Yellow100" => MaterialSkin.Primary.Yellow100,
            "Yellow200" => MaterialSkin.Primary.Yellow200,
            "Yellow300" => MaterialSkin.Primary.Yellow300,
            "Yellow400" => MaterialSkin.Primary.Yellow400,
            "Yellow500" => MaterialSkin.Primary.Yellow500,
            "Yellow600" => MaterialSkin.Primary.Yellow600,
            "Yellow700" => MaterialSkin.Primary.Yellow700,
            "Yellow800" => MaterialSkin.Primary.Yellow800,
            "Yellow900" => MaterialSkin.Primary.Yellow900,
            "Amber50" => MaterialSkin.Primary.Amber50,
            "Amber100" => MaterialSkin.Primary.Amber100,
            "Amber200" => MaterialSkin.Primary.Amber200,
            "Amber300" => MaterialSkin.Primary.Amber300,
            "Amber400" => MaterialSkin.Primary.Amber400,
            "Amber500" => MaterialSkin.Primary.Amber500,
            "Amber600" => MaterialSkin.Primary.Amber600,
            "Amber700" => MaterialSkin.Primary.Amber700,
            "Amber800" => MaterialSkin.Primary.Amber800,
            "Amber900" => MaterialSkin.Primary.Amber900,
            "Orange50" => MaterialSkin.Primary.Orange50,
            "Orange100" => MaterialSkin.Primary.Orange100,
            "Orange200" => MaterialSkin.Primary.Orange200,
            "Orange300" => MaterialSkin.Primary.Orange300,
            "Orange400" => MaterialSkin.Primary.Orange400,
            "Orange500" => MaterialSkin.Primary.Orange500,
            "Orange600" => MaterialSkin.Primary.Orange600,
            "Orange700" => MaterialSkin.Primary.Orange700,
            "Orange800" => MaterialSkin.Primary.Orange800,
            "Orange900" => MaterialSkin.Primary.Orange900,
            "DeepOrange50" => MaterialSkin.Primary.DeepOrange50,
            "DeepOrange100" => MaterialSkin.Primary.DeepOrange100,
            "DeepOrange200" => MaterialSkin.Primary.DeepOrange200,
            "DeepOrange300" => MaterialSkin.Primary.DeepOrange300,
            "DeepOrange400" => MaterialSkin.Primary.DeepOrange400,
            "DeepOrange500" => MaterialSkin.Primary.DeepOrange500,
            "DeepOrange600" => MaterialSkin.Primary.DeepOrange600,
            "DeepOrange700" => MaterialSkin.Primary.DeepOrange700,
            "DeepOrange800" => MaterialSkin.Primary.DeepOrange800,
            "DeepOrange900" => MaterialSkin.Primary.DeepOrange900,
            "Brown50" => MaterialSkin.Primary.Brown50,
            "Brown100" => MaterialSkin.Primary.Brown100,
            "Brown200" => MaterialSkin.Primary.Brown200,
            "Brown300" => MaterialSkin.Primary.Brown300,
            "Brown400" => MaterialSkin.Primary.Brown400,
            "Brown500" => MaterialSkin.Primary.Brown500,
            "Brown600" => MaterialSkin.Primary.Brown600,
            "Brown700" => MaterialSkin.Primary.Brown700,
            "Brown800" => MaterialSkin.Primary.Brown800,
            "Brown900" => MaterialSkin.Primary.Brown900,
            "Grey50" => MaterialSkin.Primary.Grey50,
            "Grey100" => MaterialSkin.Primary.Grey100,
            "Grey200" => MaterialSkin.Primary.Grey200,
            "Grey300" => MaterialSkin.Primary.Grey300,
            "Grey400" => MaterialSkin.Primary.Grey400,
            "Grey500" => MaterialSkin.Primary.Grey500,
            "Grey600" => MaterialSkin.Primary.Grey600,
            "Grey700" => MaterialSkin.Primary.Grey700,
            "Grey800" => MaterialSkin.Primary.Grey800,
            "Grey900" => MaterialSkin.Primary.Grey900,
            "BlueGrey50" => MaterialSkin.Primary.BlueGrey50,
            "BlueGrey100" => MaterialSkin.Primary.BlueGrey100,
            "BlueGrey200" => MaterialSkin.Primary.BlueGrey200,
            "BlueGrey300" => MaterialSkin.Primary.BlueGrey300,
            "BlueGrey400" => MaterialSkin.Primary.BlueGrey400,
            "BlueGrey500" => MaterialSkin.Primary.BlueGrey500,
            "BlueGrey600" => MaterialSkin.Primary.BlueGrey600,
            "BlueGrey700" => MaterialSkin.Primary.BlueGrey700,
            "BlueGrey800" => MaterialSkin.Primary.BlueGrey800,
            "BlueGrey900" => MaterialSkin.Primary.BlueGrey900,
            _ => MaterialSkin.Primary.Indigo500,
        };
    }
    private static MaterialSkin.Accent GetAccentColor(string colorName)
    {
        return colorName switch
        {
            "Red100" => MaterialSkin.Accent.Red100,
            "Red200" => MaterialSkin.Accent.Red200,
            "Red400" => MaterialSkin.Accent.Red400,
            "Red700" => MaterialSkin.Accent.Red700,
            "Pink100" => MaterialSkin.Accent.Pink100,
            "Pink200" => MaterialSkin.Accent.Pink200,
            "Pink400" => MaterialSkin.Accent.Pink400,
            "Pink700" => MaterialSkin.Accent.Pink700,
            "Purple100" => MaterialSkin.Accent.Purple100,
            "Purple200" => MaterialSkin.Accent.Purple200,
            "Purple400" => MaterialSkin.Accent.Purple400,
            "Purple700" => MaterialSkin.Accent.Purple700,
            "DeepPurple100" => MaterialSkin.Accent.DeepPurple100,
            "DeepPurple200" => MaterialSkin.Accent.DeepPurple200,
            "DeepPurple400" => MaterialSkin.Accent.DeepPurple400,
            "DeepPurple700" => MaterialSkin.Accent.DeepPurple700,
            "Indigo100" => MaterialSkin.Accent.Indigo100,
            "Indigo200" => MaterialSkin.Accent.Indigo200,
            "Indigo400" => MaterialSkin.Accent.Indigo400,
            "Indigo700" => MaterialSkin.Accent.Indigo700,
            "Blue100" => MaterialSkin.Accent.Blue100,
            "Blue200" => MaterialSkin.Accent.Blue200,
            "Blue400" => MaterialSkin.Accent.Blue400,
            "Blue700" => MaterialSkin.Accent.Blue700,
            "LightBlue100" => MaterialSkin.Accent.LightBlue100,
            "LightBlue200" => MaterialSkin.Accent.LightBlue200,
            "LightBlue400" => MaterialSkin.Accent.LightBlue400,
            "LightBlue700" => MaterialSkin.Accent.LightBlue700,
            "Cyan100" => MaterialSkin.Accent.Cyan100,
            "Cyan200" => MaterialSkin.Accent.Cyan200,
            "Cyan400" => MaterialSkin.Accent.Cyan400,
            "Cyan700" => MaterialSkin.Accent.Cyan700,
            "Teal100" => MaterialSkin.Accent.Teal100,
            "Teal200" => MaterialSkin.Accent.Teal200,
            "Teal400" => MaterialSkin.Accent.Teal400,
            "Teal700" => MaterialSkin.Accent.Teal700,
            "Green100" => MaterialSkin.Accent.Green100,
            "Green200" => MaterialSkin.Accent.Green200,
            "Green400" => MaterialSkin.Accent.Green400,
            "Green700" => MaterialSkin.Accent.Green700,
            "LightGreen100" => MaterialSkin.Accent.LightGreen100,
            "LightGreen200" => MaterialSkin.Accent.LightGreen200,
            "LightGreen400" => MaterialSkin.Accent.LightGreen400,
            "LightGreen700" => MaterialSkin.Accent.LightGreen700,
            "Lime100" => MaterialSkin.Accent.Lime100,
            "Lime200" => MaterialSkin.Accent.Lime200,
            "Lime400" => MaterialSkin.Accent.Lime400,
            "Lime700" => MaterialSkin.Accent.Lime700,
            "Yellow100" => MaterialSkin.Accent.Yellow100,
            "Yellow200" => MaterialSkin.Accent.Yellow200,
            "Yellow400" => MaterialSkin.Accent.Yellow400,
            "Yellow700" => MaterialSkin.Accent.Yellow700,
            "Amber100" => MaterialSkin.Accent.Amber100,
            "Amber200" => MaterialSkin.Accent.Amber200,
            "Amber400" => MaterialSkin.Accent.Amber400,
            "Amber700" => MaterialSkin.Accent.Amber700,
            "Orange100" => MaterialSkin.Accent.Orange100,
            "Orange200" => MaterialSkin.Accent.Orange200,
            "Orange400" => MaterialSkin.Accent.Orange400,
            "Orange700" => MaterialSkin.Accent.Orange700,
            "DeepOrange100" => MaterialSkin.Accent.DeepOrange100,
            "DeepOrange200" => MaterialSkin.Accent.DeepOrange200,
            "DeepOrange400" => MaterialSkin.Accent.DeepOrange400,
            "DeepOrange700" => MaterialSkin.Accent.DeepOrange700,
            _ => MaterialSkin.Accent.Indigo400,
        };
    }
    private static int GetPrimaryColorIndex3(string colorName)
    {
        return colorName switch
        {
            "Red50" => 0,
            "Red100" => 1,
            "Red200" => 2,
            "Red300" => 3,
            "Red400" => 4,
            "Red500" => 5,
            "Red600" => 6,
            "Red700" => 7,
            "Red800" => 8,
            "Red900" => 9,
            "Pink50" => 10,
            "Pink100" => 11,
            "Pink200" => 12,
            "Pink300" => 13,
            "Pink400" => 14,
            "Pink500" => 15,
            "Pink600" => 16,
            "Pink700" => 17,
            "Pink800" => 18,
            "Pink900" => 19,
            "Purple50" => 20,
            "Purple100" => 21,
            "Purple200" => 22,
            "Purple300" => 23,
            "Purple400" => 24,
            "Purple500" => 25,
            "Purple600" => 26,
            "Purple700" => 27,
            "Purple800" => 28,
            "Purple900" => 29,
            "DeepPurple50" => 30,
            "DeepPurple100" => 31,
            "DeepPurple200" => 32,
            "DeepPurple300" => 33,
            "DeepPurple400" => 34,
            "DeepPurple500" => 35,
            "DeepPurple600" => 36,
            "DeepPurple700" => 37,
            "DeepPurple800" => 38,
            "DeepPurple900" => 39,
            "Indigo50" => 40,
            "Indigo100" => 41,
            "Indigo200" => 42,
            "Indigo300" => 43,
            "Indigo400" => 44,
            "Indigo500" => 45,
            "Indigo600" => 46,
            "Indigo700" => 47,
            "Indigo800" => 48,
            "Indigo900" => 49,
            "Blue50" => 50,
            "Blue100" => 51,
            "Blue200" => 52,
            "Blue300" => 53,
            "Blue400" => 54,
            "Blue500" => 55,
            "Blue600" => 56,
            "Blue700" => 57,
            "Blue800" => 58,
            "Blue900" => 59,
            "LightBlue50" => 60,
            "LightBlue100" => 61,
            "LightBlue200" => 62,
            "LightBlue300" => 63,
            "LightBlue400" => 64,
            "LightBlue500" => 65,
            "LightBlue600" => 66,
            "LightBlue700" => 67,
            "LightBlue800" => 68,
            "LightBlue900" => 69,
            "Cyan50" => 70,
            "Cyan100" => 71,
            "Cyan200" => 72,
            "Cyan300" => 73,
            "Cyan400" => 74,
            "Cyan500" => 75,
            "Cyan600" => 76,
            "Cyan700" => 77,
            "Cyan800" => 78,
            "Cyan900" => 79,
            "Teal50" => 80,
            "Teal100" => 81,
            "Teal200" => 82,
            "Teal300" => 83,
            "Teal400" => 84,
            "Teal500" => 85,
            "Teal600" => 86,
            "Teal700" => 87,
            "Teal800" => 88,
            "Teal900" => 89,
            "Green50" => 90,
            "Green100" => 91,
            "Green200" => 92,
            "Green300" => 93,
            "Green400" => 94,
            "Green500" => 95,
            "Green600" => 96,
            "Green700" => 97,
            "Green800" => 98,
            "Green900" => 99,
            "LightGreen50" => 100,
            "LightGreen100" => 101,
            "LightGreen200" => 102,
            "LightGreen300" => 103,
            "LightGreen400" => 104,
            "LightGreen500" => 105,
            "LightGreen600" => 106,
            "LightGreen700" => 107,
            "LightGreen800" => 108,
            "LightGreen900" => 109,
            "Lime50" => 110,
            "Lime100" => 111,
            "Lime200" => 112,
            "Lime300" => 113,
            "Lime400" => 114,
            "Lime500" => 115,
            "Lime600" => 116,
            "Lime700" => 117,
            "Lime800" => 118,
            "Lime900" => 119,
            "Yellow50" => 120,
            "Yellow100" => 121,
            "Yellow200" => 122,
            "Yellow300" => 123,
            "Yellow400" => 124,
            "Yellow500" => 125,
            "Yellow600" => 126,
            "Yellow700" => 127,
            "Yellow800" => 128,
            "Yellow900" => 129,
            "Amber50" => 130,
            "Amber100" => 131,
            "Amber200" => 132,
            "Amber300" => 133,
            "Amber400" => 134,
            "Amber500" => 135,
            "Amber600" => 136,
            "Amber700" => 137,
            "Amber800" => 138,
            "Amber900" => 139,
            "Orange50" => 140,
            "Orange100" => 141,
            "Orange200" => 142,
            "Orange300" => 143,
            "Orange400" => 144,
            "Orange500" => 145,
            "Orange600" => 146,
            "Orange700" => 147,
            "Orange800" => 148,
            "Orange900" => 149,
            "DeepOrange50" => 150,
            "DeepOrange100" => 151,
            "DeepOrange200" => 152,
            "DeepOrange300" => 153,
            "DeepOrange400" => 154,
            "DeepOrange500" => 155,
            "DeepOrange600" => 156,
            "DeepOrange700" => 157,
            "DeepOrange800" => 158,
            "DeepOrange900" => 159,
            "Brown50" => 160,
            "Brown100" => 161,
            "Brown200" => 162,
            "Brown300" => 163,
            "Brown400" => 164,
            "Brown500" => 165,
            "Brown600" => 166,
            "Brown700" => 167,
            "Brown800" => 168,
            "Brown900" => 169,
            "Grey50" => 170,
            "Grey100" => 171,
            "Grey200" => 172,
            "Grey300" => 173,
            "Grey400" => 174,
            "Grey500" => 175,
            "Grey600" => 176,
            "Grey700" => 177,
            "Grey800" => 178,
            "Grey900" => 179,
            "BlueGrey50" => 180,
            "BlueGrey100" => 181,
            "BlueGrey200" => 182,
            "BlueGrey300" => 183,
            "BlueGrey400" => 184,
            "BlueGrey500" => 185,
            "BlueGrey600" => 186,
            "BlueGrey700" => 187,
            "BlueGrey800" => 188,
            "BlueGrey900" => 189,
            _ => 41
        };
    }
    private static int GetPrimaryColorIndex2(string colorName)
    {
        return colorName switch
        {
            "Red50" => 0,
            "Red100" => 1,
            "Red200" => 2,
            "Red300" => 3,
            "Red400" => 4,
            "Red500" => 5,
            "Red600" => 6,
            "Red700" => 7,
            "Red800" => 8,
            "Red900" => 9,
            "Pink50" => 10,
            "Pink100" => 11,
            "Pink200" => 12,
            "Pink300" => 13,
            "Pink400" => 14,
            "Pink500" => 15,
            "Pink600" => 16,
            "Pink700" => 17,
            "Pink800" => 18,
            "Pink900" => 19,
            "Purple50" => 20,
            "Purple100" => 21,
            "Purple200" => 22,
            "Purple300" => 23,
            "Purple400" => 24,
            "Purple500" => 25,
            "Purple600" => 26,
            "Purple700" => 27,
            "Purple800" => 28,
            "Purple900" => 29,
            "DeepPurple50" => 30,
            "DeepPurple100" => 31,
            "DeepPurple200" => 32,
            "DeepPurple300" => 33,
            "DeepPurple400" => 34,
            "DeepPurple500" => 35,
            "DeepPurple600" => 36,
            "DeepPurple700" => 37,
            "DeepPurple800" => 38,
            "DeepPurple900" => 39,
            "Indigo50" => 40,
            "Indigo100" => 41,
            "Indigo200" => 42,
            "Indigo300" => 43,
            "Indigo400" => 44,
            "Indigo500" => 45,
            "Indigo600" => 46,
            "Indigo700" => 47,
            "Indigo800" => 48,
            "Indigo900" => 49,
            "Blue50" => 50,
            "Blue100" => 51,
            "Blue200" => 52,
            "Blue300" => 53,
            "Blue400" => 54,
            "Blue500" => 55,
            "Blue600" => 56,
            "Blue700" => 57,
            "Blue800" => 58,
            "Blue900" => 59,
            "LightBlue50" => 60,
            "LightBlue100" => 61,
            "LightBlue200" => 62,
            "LightBlue300" => 63,
            "LightBlue400" => 64,
            "LightBlue500" => 65,
            "LightBlue600" => 66,
            "LightBlue700" => 67,
            "LightBlue800" => 68,
            "LightBlue900" => 69,
            "Cyan50" => 70,
            "Cyan100" => 71,
            "Cyan200" => 72,
            "Cyan300" => 73,
            "Cyan400" => 74,
            "Cyan500" => 75,
            "Cyan600" => 76,
            "Cyan700" => 77,
            "Cyan800" => 78,
            "Cyan900" => 79,
            "Teal50" => 80,
            "Teal100" => 81,
            "Teal200" => 82,
            "Teal300" => 83,
            "Teal400" => 84,
            "Teal500" => 85,
            "Teal600" => 86,
            "Teal700" => 87,
            "Teal800" => 88,
            "Teal900" => 89,
            "Green50" => 90,
            "Green100" => 91,
            "Green200" => 92,
            "Green300" => 93,
            "Green400" => 94,
            "Green500" => 95,
            "Green600" => 96,
            "Green700" => 97,
            "Green800" => 98,
            "Green900" => 99,
            "LightGreen50" => 100,
            "LightGreen100" => 101,
            "LightGreen200" => 102,
            "LightGreen300" => 103,
            "LightGreen400" => 104,
            "LightGreen500" => 105,
            "LightGreen600" => 106,
            "LightGreen700" => 107,
            "LightGreen800" => 108,
            "LightGreen900" => 109,
            "Lime50" => 110,
            "Lime100" => 111,
            "Lime200" => 112,
            "Lime300" => 113,
            "Lime400" => 114,
            "Lime500" => 115,
            "Lime600" => 116,
            "Lime700" => 117,
            "Lime800" => 118,
            "Lime900" => 119,
            "Yellow50" => 120,
            "Yellow100" => 121,
            "Yellow200" => 122,
            "Yellow300" => 123,
            "Yellow400" => 124,
            "Yellow500" => 125,
            "Yellow600" => 126,
            "Yellow700" => 127,
            "Yellow800" => 128,
            "Yellow900" => 129,
            "Amber50" => 130,
            "Amber100" => 131,
            "Amber200" => 132,
            "Amber300" => 133,
            "Amber400" => 134,
            "Amber500" => 135,
            "Amber600" => 136,
            "Amber700" => 137,
            "Amber800" => 138,
            "Amber900" => 139,
            "Orange50" => 140,
            "Orange100" => 141,
            "Orange200" => 142,
            "Orange300" => 143,
            "Orange400" => 144,
            "Orange500" => 145,
            "Orange600" => 146,
            "Orange700" => 147,
            "Orange800" => 148,
            "Orange900" => 149,
            "DeepOrange50" => 150,
            "DeepOrange100" => 151,
            "DeepOrange200" => 152,
            "DeepOrange300" => 153,
            "DeepOrange400" => 154,
            "DeepOrange500" => 155,
            "DeepOrange600" => 156,
            "DeepOrange700" => 157,
            "DeepOrange800" => 158,
            "DeepOrange900" => 159,
            "Brown50" => 160,
            "Brown100" => 161,
            "Brown200" => 162,
            "Brown300" => 163,
            "Brown400" => 164,
            "Brown500" => 165,
            "Brown600" => 166,
            "Brown700" => 167,
            "Brown800" => 168,
            "Brown900" => 169,
            "Grey50" => 170,
            "Grey100" => 171,
            "Grey200" => 172,
            "Grey300" => 173,
            "Grey400" => 174,
            "Grey500" => 175,
            "Grey600" => 176,
            "Grey700" => 177,
            "Grey800" => 178,
            "Grey900" => 179,
            "BlueGrey50" => 180,
            "BlueGrey100" => 181,
            "BlueGrey200" => 182,
            "BlueGrey300" => 183,
            "BlueGrey400" => 184,
            "BlueGrey500" => 185,
            "BlueGrey600" => 186,
            "BlueGrey700" => 187,
            "BlueGrey800" => 188,
            "BlueGrey900" => 189,
            _ => 47
        };
    }
    private static int GetPrimaryColorIndex(string colorName)
    {
        return colorName switch
        {
            "Red50" => 0,
            "Red100" => 1,
            "Red200" => 2,
            "Red300" => 3,
            "Red400" => 4,
            "Red500" => 5,
            "Red600" => 6,
            "Red700" => 7,
            "Red800" => 8,
            "Red900" => 9,
            "Pink50" => 10,
            "Pink100" => 11,
            "Pink200" => 12,
            "Pink300" => 13,
            "Pink400" => 14,
            "Pink500" => 15,
            "Pink600" => 16,
            "Pink700" => 17,
            "Pink800" => 18,
            "Pink900" => 19,
            "Purple50" => 20,
            "Purple100" => 21,
            "Purple200" => 22,
            "Purple300" => 23,
            "Purple400" => 24,
            "Purple500" => 25,
            "Purple600" => 26,
            "Purple700" => 27,
            "Purple800" => 28,
            "Purple900" => 29,
            "DeepPurple50" => 30,
            "DeepPurple100" => 31,
            "DeepPurple200" => 32,
            "DeepPurple300" => 33,
            "DeepPurple400" => 34,
            "DeepPurple500" => 35,
            "DeepPurple600" => 36,
            "DeepPurple700" => 37,
            "DeepPurple800" => 38,
            "DeepPurple900" => 39,
            "Indigo50" => 40,
            "Indigo100" => 41,
            "Indigo200" => 42,
            "Indigo300" => 43,
            "Indigo400" => 44,
            "Indigo500" => 45,
            "Indigo600" => 46,
            "Indigo700" => 47,
            "Indigo800" => 48,
            "Indigo900" => 49,
            "Blue50" => 50,
            "Blue100" => 51,
            "Blue200" => 52,
            "Blue300" => 53,
            "Blue400" => 54,
            "Blue500" => 55,
            "Blue600" => 56,
            "Blue700" => 57,
            "Blue800" => 58,
            "Blue900" => 59,
            "LightBlue50" => 60,
            "LightBlue100" => 61,
            "LightBlue200" => 62,
            "LightBlue300" => 63,
            "LightBlue400" => 64,
            "LightBlue500" => 65,
            "LightBlue600" => 66,
            "LightBlue700" => 67,
            "LightBlue800" => 68,
            "LightBlue900" => 69,
            "Cyan50" => 70,
            "Cyan100" => 71,
            "Cyan200" => 72,
            "Cyan300" => 73,
            "Cyan400" => 74,
            "Cyan500" => 75,
            "Cyan600" => 76,
            "Cyan700" => 77,
            "Cyan800" => 78,
            "Cyan900" => 79,
            "Teal50" => 80,
            "Teal100" => 81,
            "Teal200" => 82,
            "Teal300" => 83,
            "Teal400" => 84,
            "Teal500" => 85,
            "Teal600" => 86,
            "Teal700" => 87,
            "Teal800" => 88,
            "Teal900" => 89,
            "Green50" => 90,
            "Green100" => 91,
            "Green200" => 92,
            "Green300" => 93,
            "Green400" => 94,
            "Green500" => 95,
            "Green600" => 96,
            "Green700" => 97,
            "Green800" => 98,
            "Green900" => 99,
            "LightGreen50" => 100,
            "LightGreen100" => 101,
            "LightGreen200" => 102,
            "LightGreen300" => 103,
            "LightGreen400" => 104,
            "LightGreen500" => 105,
            "LightGreen600" => 106,
            "LightGreen700" => 107,
            "LightGreen800" => 108,
            "LightGreen900" => 109,
            "Lime50" => 110,
            "Lime100" => 111,
            "Lime200" => 112,
            "Lime300" => 113,
            "Lime400" => 114,
            "Lime500" => 115,
            "Lime600" => 116,
            "Lime700" => 117,
            "Lime800" => 118,
            "Lime900" => 119,
            "Yellow50" => 120,
            "Yellow100" => 121,
            "Yellow200" => 122,
            "Yellow300" => 123,
            "Yellow400" => 124,
            "Yellow500" => 125,
            "Yellow600" => 126,
            "Yellow700" => 127,
            "Yellow800" => 128,
            "Yellow900" => 129,
            "Amber50" => 130,
            "Amber100" => 131,
            "Amber200" => 132,
            "Amber300" => 133,
            "Amber400" => 134,
            "Amber500" => 135,
            "Amber600" => 136,
            "Amber700" => 137,
            "Amber800" => 138,
            "Amber900" => 139,
            "Orange50" => 140,
            "Orange100" => 141,
            "Orange200" => 142,
            "Orange300" => 143,
            "Orange400" => 144,
            "Orange500" => 145,
            "Orange600" => 146,
            "Orange700" => 147,
            "Orange800" => 148,
            "Orange900" => 149,
            "DeepOrange50" => 150,
            "DeepOrange100" => 151,
            "DeepOrange200" => 152,
            "DeepOrange300" => 153,
            "DeepOrange400" => 154,
            "DeepOrange500" => 155,
            "DeepOrange600" => 156,
            "DeepOrange700" => 157,
            "DeepOrange800" => 158,
            "DeepOrange900" => 159,
            "Brown50" => 160,
            "Brown100" => 161,
            "Brown200" => 162,
            "Brown300" => 163,
            "Brown400" => 164,
            "Brown500" => 165,
            "Brown600" => 166,
            "Brown700" => 167,
            "Brown800" => 168,
            "Brown900" => 169,
            "Grey50" => 170,
            "Grey100" => 171,
            "Grey200" => 172,
            "Grey300" => 173,
            "Grey400" => 174,
            "Grey500" => 175,
            "Grey600" => 176,
            "Grey700" => 177,
            "Grey800" => 178,
            "Grey900" => 179,
            "BlueGrey50" => 180,
            "BlueGrey100" => 181,
            "BlueGrey200" => 182,
            "BlueGrey300" => 183,
            "BlueGrey400" => 184,
            "BlueGrey500" => 185,
            "BlueGrey600" => 186,
            "BlueGrey700" => 187,
            "BlueGrey800" => 188,
            "BlueGrey900" => 189,
            _ => 45
        };
    }
    private static int GetAccentColorIndex(string colorName)
    {
        return colorName switch
        {
            "Red100" => 0,
            "Red200" => 1,
            "Red400" => 2,
            "Red700" => 3,
            "Pink100" => 4,
            "Pink200" => 5,
            "Pink400" => 6,
            "Pink700" => 7,
            "Purple100" => 8,
            "Purple200" => 9,
            "Purple400" => 10,
            "Purple700" => 11,
            "DeepPurple100" => 12,
            "DeepPurple200" => 13,
            "DeepPurple400" => 14,
            "DeepPurple700" => 15,
            "Indigo100" => 16,
            "Indigo200" => 17,
            "Indigo400" => 18,
            "Indigo700" => 19,
            "Blue100" => 20,
            "Blue200" => 21,
            "Blue400" => 22,
            "Blue700" => 23,
            "LightBlue100" => 24,
            "LightBlue200" => 25,
            "LightBlue400" => 26,
            "LightBlue700" => 27,
            "Cyan100" => 28,
            "Cyan200" => 29,
            "Cyan400" => 30,
            "Cyan700" => 31,
            "Teal100" => 32,
            "Teal200" => 33,
            "Teal400" => 34,
            "Teal700" => 35,
            "Green100" => 36,
            "Green200" => 37,
            "Green400" => 38,
            "Green700" => 39,
            "LightGreen100" => 40,
            "LightGreen200" => 41,
            "LightGreen400" => 42,
            "LightGreen700" => 43,
            "Lime100" => 44,
            "Lime200" => 45,
            "Lime400" => 46,
            "Lime700" => 47,
            "Yellow100" => 48,
            "Yellow200" => 49,
            "Yellow400" => 50,
            "Yellow700" => 51,
            "Amber100" => 52,
            "Amber200" => 53,
            "Amber400" => 54,
            "Amber700" => 55,
            "Orange100" => 56,
            "Orange200" => 57,
            "Orange400" => 58,
            "Orange700" => 59,
            "DeepOrange100" => 60,
            "DeepOrange200" => 61,
            "DeepOrange400" => 62,
            "DeepOrange700" => 63,
            _ => -1
        };
    }
    #endregion

    #region Ýletiþim
    private async void contact_submit_Click(object sender, EventArgs e)
    {
        if (
            string.IsNullOrEmpty(contact_name.Text) ||
            string.IsNullOrEmpty(contact_surname.Text) ||
            string.IsNullOrEmpty(contact_whereUsedProgram.Text) ||
            string.IsNullOrEmpty(contact_describeProblem.Text) ||
            string.IsNullOrEmpty(contact_address.Text) ||
            string.IsNullOrWhiteSpace(contact_name.Text) ||
            string.IsNullOrWhiteSpace(contact_surname.Text) ||
            string.IsNullOrWhiteSpace(contact_whereUsedProgram.Text) ||
            string.IsNullOrWhiteSpace(contact_describeProblem.Text) ||
            string.IsNullOrWhiteSpace(contact_address.Text)
            )
        {
            _ = ShowToastMessageAsync(ToastMessageType.Warning, "Uyarý", "Tüm alanlarý doldurun");
            return;
        }


        setLoaderSize(materialCard6, ref circularLoader_contact, sizeReduction: 20, reductionY: 10);

        circularLoader_contact.Visible = true;

        string url = $"HÝDDEN";
        HttpClientHandler clientHandler = new()
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
        using HttpClient client = new(clientHandler);
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Destek Talebiniz Gönderilmedi");
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Destek Talebiniz Gönderildi");
            }
            else
            {
                _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Destek Talebiniz Gönderilmedi");
            }
        }
        catch (HttpRequestException err)
        {
            MessageBox.Show($"Hata Oluþtu: {err.Message}");
        }

        circularLoader_contact.Visible = false;
    }
    private void contact_input_Leave(object sender, EventArgs e)
    {
        Control? control = ((Control)sender);
        string text = control.Text;
        if (string.IsNullOrEmpty(text))
        {
            errorProvider_contact.SetError(control, "Bu alaný doldurmalýsýnýz");
            errorProvider_contact.BlinkRate = 0;
            errorProvider_contact.SetIconAlignment(control, ErrorIconAlignment.BottomRight);
        }
        else
        {
            errorProvider_contact.SetError(control, null);
        }
    }
    #endregion

    #region Log
    private void tab_log_Enter(object sender, EventArgs e)
    {
        setLoaderSize(materialListBox1, ref circularLoader_log, sizeReduction: 20, reductionY: 10);
        circularLoader_log.Visible = true;
        string logPath = Path.Combine(LogFolderPath, "log.txt");
        materialListBox1.Items.Clear();
        foreach (string _log in File.ReadAllLines(logPath).Reverse())
        {
            materialListBox1.Items.Add(new MaterialSkin.MaterialListBoxItem(_log, "X"));
        }
        circularLoader_log.Visible = false;
    }
    private void log_btn_export_Click(object sender, EventArgs e)
    {
        string path = LogFolderPath + "\\" + "log.txt";
        if (File.Exists(path))
        {
            FolderBrowserDialog folderBrowserDialog = new();
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                try
                {
                    string hedefDosyaYolu = Path.Combine(folderBrowserDialog.SelectedPath, Path.GetFileName(path));
                    File.Copy(path, hedefDosyaYolu, true);

                    _ = ShowToastMessageAsync(ToastMessageType.Success, "Baþarýlý", "Dosya Baþarýyla Dýþa Aktarýldý");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
        else
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Dosya Mevcut Deðil");
        }
    }
    public static Stopwatch sw = new();
    public static void log(string text, bool swEnabled = true)
    {
        string time = string.Empty;
        if (sw.IsRunning && swEnabled)
        {
            sw.Stop();
            time = $"({sw.ElapsedMilliseconds / 1000}.{sw.ElapsedMilliseconds % 1000}s) ";
            sw.Reset();
        }
        string logPath = Path.Combine(LogFolderPath, "log.txt");
        File.AppendAllText(logPath, time + text + " " + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + Environment.NewLine);
    }
    #endregion

    public bool existsDataPath()
    {
        if (!File.Exists(TxtPath))
        {
            _ = ShowToastMessageAsync(ToastMessageType.Error, "Hata", "Ayarlardan Txt Dosyasý Oluþturmanýz Gerek");
            return true;
        }
        return false;
    }
    public static void setLoaderSize(Control control, ref CircularLoader loader, int sizeReduction = 0, int reductionX = 0, int reductionY = 0)
    {
        int size = Math.Min(control.Size.Width, control.Size.Height) - sizeReduction;
        loader.Size = new Size(size, size);

        int x = control.Location.X + (control.Width - loader.Width) / 2;
        int y = control.Location.Y + (control.Height - loader.Height) / 2;
        loader.Location = new Point(x - reductionX, y - reductionY);
    }
}