
namespace BM64_LevelCreator
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.MapViewPanel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.LayerView_Zoom_PicBox = new System.Windows.Forms.PictureBox();
            this.MapView_DPad_PicBox = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SectionCoords_TextBox = new System.Windows.Forms.TextBox();
            this.LayerID_NumUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LayerView_DPad_PicBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LayerViewPanel = new System.Windows.Forms.Panel();
            this.LoadMapFile_Button = new System.Windows.Forms.Button();
            this.SectionViewPanel = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.TextureSelectPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.TileSelectPanel = new System.Windows.Forms.Panel();
            this.TileInfoPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ObjectID_TextBox = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.EnemyID_TextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Export2OBJ_Button = new System.Windows.Forms.Button();
            this.BuildROM_Button = new System.Windows.Forms.Button();
            this.MapNames_ComboBox = new System.Windows.Forms.ComboBox();
            this.RipFiles_Button = new System.Windows.Forms.Button();
            this.SaveMapFile_Button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LayerView_Zoom_PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapView_DPad_PicBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerID_NumUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerView_DPad_PicBox)).BeginInit();
            this.panel4.SuspendLayout();
            this.TextureSelectPanel.SuspendLayout();
            this.TileInfoPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MapViewPanel
            // 
            this.MapViewPanel.BackColor = System.Drawing.Color.Gray;
            this.MapViewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MapViewPanel.Location = new System.Drawing.Point(3, 28);
            this.MapViewPanel.Name = "MapViewPanel";
            this.MapViewPanel.Size = new System.Drawing.Size(261, 200);
            this.MapViewPanel.TabIndex = 0;
            this.MapViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.MapViewPanel_Paint);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.LayerView_Zoom_PicBox);
            this.panel2.Controls.Add(this.MapView_DPad_PicBox);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.SectionCoords_TextBox);
            this.panel2.Controls.Add(this.LayerID_NumUpDown);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.LayerView_DPad_PicBox);
            this.panel2.Controls.Add(this.MapViewPanel);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.LayerViewPanel);
            this.panel2.Location = new System.Drawing.Point(463, 56);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(357, 485);
            this.panel2.TabIndex = 1;
            // 
            // LayerView_Zoom_PicBox
            // 
            this.LayerView_Zoom_PicBox.Image = global::BM64_LevelCreator.Properties.Resources.Zoom;
            this.LayerView_Zoom_PicBox.Location = new System.Drawing.Point(279, 411);
            this.LayerView_Zoom_PicBox.Margin = new System.Windows.Forms.Padding(2);
            this.LayerView_Zoom_PicBox.Name = "LayerView_Zoom_PicBox";
            this.LayerView_Zoom_PicBox.Size = new System.Drawing.Size(50, 25);
            this.LayerView_Zoom_PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LayerView_Zoom_PicBox.TabIndex = 11;
            this.LayerView_Zoom_PicBox.TabStop = false;
            this.LayerView_Zoom_PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LayerView_Zoom_PicBox_MouseClick);
            // 
            // MapView_DPad_PicBox
            // 
            this.MapView_DPad_PicBox.Image = global::BM64_LevelCreator.Properties.Resources.DPad;
            this.MapView_DPad_PicBox.Location = new System.Drawing.Point(278, 89);
            this.MapView_DPad_PicBox.Margin = new System.Windows.Forms.Padding(2);
            this.MapView_DPad_PicBox.Name = "MapView_DPad_PicBox";
            this.MapView_DPad_PicBox.Size = new System.Drawing.Size(61, 61);
            this.MapView_DPad_PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MapView_DPad_PicBox.TabIndex = 7;
            this.MapView_DPad_PicBox.TabStop = false;
            this.MapView_DPad_PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapView_DPad_PicBox_MouseClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(276, 392);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "Zoom";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(275, 37);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Layer";
            // 
            // SectionCoords_TextBox
            // 
            this.SectionCoords_TextBox.BackColor = System.Drawing.Color.White;
            this.SectionCoords_TextBox.Enabled = false;
            this.SectionCoords_TextBox.Location = new System.Drawing.Point(278, 286);
            this.SectionCoords_TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SectionCoords_TextBox.Name = "SectionCoords_TextBox";
            this.SectionCoords_TextBox.Size = new System.Drawing.Size(62, 20);
            this.SectionCoords_TextBox.TabIndex = 9;
            this.SectionCoords_TextBox.Text = "( 0 | 0 )";
            // 
            // LayerID_NumUpDown
            // 
            this.LayerID_NumUpDown.Location = new System.Drawing.Point(278, 56);
            this.LayerID_NumUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.LayerID_NumUpDown.Name = "LayerID_NumUpDown";
            this.LayerID_NumUpDown.Size = new System.Drawing.Size(61, 20);
            this.LayerID_NumUpDown.TabIndex = 2;
            this.LayerID_NumUpDown.ValueChanged += new System.EventHandler(this.LayerID_NumUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(274, 267);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Section";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Full Map View";
            // 
            // LayerView_DPad_PicBox
            // 
            this.LayerView_DPad_PicBox.Image = global::BM64_LevelCreator.Properties.Resources.DPad;
            this.LayerView_DPad_PicBox.Location = new System.Drawing.Point(279, 316);
            this.LayerView_DPad_PicBox.Margin = new System.Windows.Forms.Padding(2);
            this.LayerView_DPad_PicBox.Name = "LayerView_DPad_PicBox";
            this.LayerView_DPad_PicBox.Size = new System.Drawing.Size(61, 61);
            this.LayerView_DPad_PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LayerView_DPad_PicBox.TabIndex = 8;
            this.LayerView_DPad_PicBox.TabStop = false;
            this.LayerView_DPad_PicBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LayerView_DPad_PicBox_MouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(9, 242);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Selected Layer";
            // 
            // LayerViewPanel
            // 
            this.LayerViewPanel.BackColor = System.Drawing.Color.Gray;
            this.LayerViewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LayerViewPanel.Location = new System.Drawing.Point(3, 261);
            this.LayerViewPanel.Name = "LayerViewPanel";
            this.LayerViewPanel.Size = new System.Drawing.Size(261, 200);
            this.LayerViewPanel.TabIndex = 0;
            this.LayerViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.LayerViewPanel_Paint);
            this.LayerViewPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LayerViewPanel_MouseClick);
            // 
            // LoadMapFile_Button
            // 
            this.LoadMapFile_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadMapFile_Button.Location = new System.Drawing.Point(382, 5);
            this.LoadMapFile_Button.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.LoadMapFile_Button.Name = "LoadMapFile_Button";
            this.LoadMapFile_Button.Size = new System.Drawing.Size(69, 24);
            this.LoadMapFile_Button.TabIndex = 4;
            this.LoadMapFile_Button.Text = "Load Map";
            this.LoadMapFile_Button.UseVisualStyleBackColor = true;
            this.LoadMapFile_Button.Click += new System.EventHandler(this.LoadMapFile_Button_Click);
            // 
            // SectionViewPanel
            // 
            this.SectionViewPanel.BackColor = System.Drawing.Color.Gray;
            this.SectionViewPanel.Location = new System.Drawing.Point(3, 28);
            this.SectionViewPanel.Name = "SectionViewPanel";
            this.SectionViewPanel.Size = new System.Drawing.Size(256, 256);
            this.SectionViewPanel.TabIndex = 2;
            this.SectionViewPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.SectionViewPanel_Paint);
            this.SectionViewPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SectionViewPanel_MouseClick);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.TextureSelectPanel);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.TileSelectPanel);
            this.panel4.Controls.Add(this.TileInfoPanel);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.SectionViewPanel);
            this.panel4.Location = new System.Drawing.Point(12, 56);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(430, 485);
            this.panel4.TabIndex = 3;
            // 
            // TextureSelectPanel
            // 
            this.TextureSelectPanel.BackColor = System.Drawing.Color.Gray;
            this.TextureSelectPanel.Controls.Add(this.button1);
            this.TextureSelectPanel.Location = new System.Drawing.Point(276, 318);
            this.TextureSelectPanel.Name = "TextureSelectPanel";
            this.TextureSelectPanel.Size = new System.Drawing.Size(64, 143);
            this.TextureSelectPanel.TabIndex = 4;
            this.TextureSelectPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TextureSelectPanel_Paint);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Location = new System.Drawing.Point(273, 297);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 17);
            this.label12.TabIndex = 5;
            this.label12.Text = "Texture";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label11.Location = new System.Drawing.Point(272, 9);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "Tile List";
            // 
            // TileSelectPanel
            // 
            this.TileSelectPanel.BackColor = System.Drawing.Color.Gray;
            this.TileSelectPanel.Location = new System.Drawing.Point(275, 28);
            this.TileSelectPanel.Name = "TileSelectPanel";
            this.TileSelectPanel.Size = new System.Drawing.Size(64, 256);
            this.TileSelectPanel.TabIndex = 3;
            this.TileSelectPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TileSelectPanel_Paint);
            this.TileSelectPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TileSelectPanel_MouseClick);
            // 
            // TileInfoPanel
            // 
            this.TileInfoPanel.BackColor = System.Drawing.Color.Gray;
            this.TileInfoPanel.Controls.Add(this.panel3);
            this.TileInfoPanel.Location = new System.Drawing.Point(4, 317);
            this.TileInfoPanel.Name = "TileInfoPanel";
            this.TileInfoPanel.Size = new System.Drawing.Size(255, 144);
            this.TileInfoPanel.TabIndex = 3;
            this.TileInfoPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TileInfoPanel_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.ObjectID_TextBox);
            this.panel3.Controls.Add(this.textBox2);
            this.panel3.Controls.Add(this.EnemyID_TextBox);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Location = new System.Drawing.Point(10, 64);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(182, 76);
            this.panel3.TabIndex = 3;
            // 
            // ObjectID_TextBox
            // 
            this.ObjectID_TextBox.Location = new System.Drawing.Point(82, 50);
            this.ObjectID_TextBox.Name = "ObjectID_TextBox";
            this.ObjectID_TextBox.Size = new System.Drawing.Size(39, 20);
            this.ObjectID_TextBox.TabIndex = 9;
            this.ObjectID_TextBox.TextChanged += new System.EventHandler(this.ObjectID_TextBox_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(82, 0);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(39, 20);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // EnemyID_TextBox
            // 
            this.EnemyID_TextBox.Location = new System.Drawing.Point(82, 25);
            this.EnemyID_TextBox.Name = "EnemyID_TextBox";
            this.EnemyID_TextBox.Size = new System.Drawing.Size(39, 20);
            this.EnemyID_TextBox.TabIndex = 8;
            this.EnemyID_TextBox.TextChanged += new System.EventHandler(this.EnemyID_TextBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label8.Location = new System.Drawing.Point(2, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 20);
            this.label8.TabIndex = 4;
            this.label8.Text = "???";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(2, 25);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 20);
            this.label9.TabIndex = 5;
            this.label9.Text = "Enemy-ID";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label10.Location = new System.Drawing.Point(2, 50);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 20);
            this.label10.TabIndex = 6;
            this.label10.Text = "Obj-ID";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(11, 297);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Current Tile Info";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Selected Section";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Export2OBJ_Button);
            this.panel1.Controls.Add(this.BuildROM_Button);
            this.panel1.Controls.Add(this.MapNames_ComboBox);
            this.panel1.Controls.Add(this.RipFiles_Button);
            this.panel1.Controls.Add(this.SaveMapFile_Button);
            this.panel1.Controls.Add(this.LoadMapFile_Button);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 39);
            this.panel1.TabIndex = 6;
            // 
            // Export2OBJ_Button
            // 
            this.Export2OBJ_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Export2OBJ_Button.Location = new System.Drawing.Point(541, 5);
            this.Export2OBJ_Button.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.Export2OBJ_Button.Name = "Export2OBJ_Button";
            this.Export2OBJ_Button.Size = new System.Drawing.Size(69, 24);
            this.Export2OBJ_Button.TabIndex = 9;
            this.Export2OBJ_Button.Text = "Export OBJ";
            this.Export2OBJ_Button.UseVisualStyleBackColor = true;
            this.Export2OBJ_Button.Click += new System.EventHandler(this.Export2OBJ_Button_Click);
            // 
            // BuildROM_Button
            // 
            this.BuildROM_Button.Location = new System.Drawing.Point(75, 6);
            this.BuildROM_Button.Name = "BuildROM_Button";
            this.BuildROM_Button.Size = new System.Drawing.Size(69, 23);
            this.BuildROM_Button.TabIndex = 8;
            this.BuildROM_Button.Text = "Build ROM";
            this.BuildROM_Button.UseVisualStyleBackColor = true;
            this.BuildROM_Button.Click += new System.EventHandler(this.BuildROM_Button_Click);
            // 
            // MapNames_ComboBox
            // 
            this.MapNames_ComboBox.FormattingEnabled = true;
            this.MapNames_ComboBox.Location = new System.Drawing.Point(180, 6);
            this.MapNames_ComboBox.Name = "MapNames_ComboBox";
            this.MapNames_ComboBox.Size = new System.Drawing.Size(190, 21);
            this.MapNames_ComboBox.TabIndex = 7;
            // 
            // RipFiles_Button
            // 
            this.RipFiles_Button.Location = new System.Drawing.Point(0, 6);
            this.RipFiles_Button.Name = "RipFiles_Button";
            this.RipFiles_Button.Size = new System.Drawing.Size(69, 23);
            this.RipFiles_Button.TabIndex = 6;
            this.RipFiles_Button.Text = "Rip Files";
            this.RipFiles_Button.UseVisualStyleBackColor = true;
            this.RipFiles_Button.Click += new System.EventHandler(this.RipFiles_Button_Click_1);
            // 
            // SaveMapFile_Button
            // 
            this.SaveMapFile_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveMapFile_Button.Location = new System.Drawing.Point(462, 5);
            this.SaveMapFile_Button.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.SaveMapFile_Button.Name = "SaveMapFile_Button";
            this.SaveMapFile_Button.Size = new System.Drawing.Size(69, 24);
            this.SaveMapFile_Button.TabIndex = 5;
            this.SaveMapFile_Button.Text = "Save Map";
            this.SaveMapFile_Button.UseVisualStyleBackColor = true;
            this.SaveMapFile_Button.Click += new System.EventHandler(this.SaveMapFile_Button_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Change";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.ClientSize = new System.Drawing.Size(831, 552);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Bomberman 64 - Level Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LayerView_Zoom_PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MapView_DPad_PicBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerID_NumUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerView_DPad_PicBox)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.TextureSelectPanel.ResumeLayout(false);
            this.TileInfoPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MapViewPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel SectionViewPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel LayerViewPanel;
        private System.Windows.Forms.Panel TileInfoPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown LayerID_NumUpDown;
        private System.Windows.Forms.Button LoadMapFile_Button;
        private System.Windows.Forms.PictureBox MapView_DPad_PicBox;
        private System.Windows.Forms.PictureBox LayerView_DPad_PicBox;
        private System.Windows.Forms.TextBox SectionCoords_TextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel TileSelectPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox LayerView_Zoom_PicBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ObjectID_TextBox;
        private System.Windows.Forms.TextBox EnemyID_TextBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button SaveMapFile_Button;
        private System.Windows.Forms.Button RipFiles_Button;
        private System.Windows.Forms.ComboBox MapNames_ComboBox;
        private System.Windows.Forms.Button BuildROM_Button;
        private System.Windows.Forms.Button Export2OBJ_Button;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel TextureSelectPanel;
        private System.Windows.Forms.Button button1;
    }
}

