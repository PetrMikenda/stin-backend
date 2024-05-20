namespace VAPW_semstralka_ver2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            colorDialog1 = new ColorDialog();
            toWasher = new Button();
            programStart = new Button();
            chooseProgram = new ComboBox();
            car = new PictureBox();
            frontDoor = new Panel();
            panel1 = new Panel();
            rearDoor = new Panel();
            washingHead = new Panel();
            traficLight = new Panel();
            inFrontOfIndic = new Panel();
            inIndic = new Panel();
            label1 = new Label();
            label2 = new Label();
            washingIndic = new Panel();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)car).BeginInit();
            frontDoor.SuspendLayout();
            SuspendLayout();
            // 
            // toWasher
            // 
            toWasher.Location = new Point(12, 12);
            toWasher.Name = "toWasher";
            toWasher.Size = new Size(134, 29);
            toWasher.TabIndex = 0;
            toWasher.Text = "car to washer";
            toWasher.UseVisualStyleBackColor = true;
            toWasher.Click += toWasher_Click;
            // 
            // programStart
            // 
            programStart.Location = new Point(12, 47);
            programStart.Name = "programStart";
            programStart.Size = new Size(134, 29);
            programStart.TabIndex = 1;
            programStart.Text = "program start";
            programStart.UseVisualStyleBackColor = true;
            programStart.Click += programStart_Click;
            // 
            // chooseProgram
            // 
            chooseProgram.FormattingEnabled = true;
            chooseProgram.Location = new Point(12, 82);
            chooseProgram.Name = "chooseProgram";
            chooseProgram.Size = new Size(134, 28);
            chooseProgram.TabIndex = 2;
            chooseProgram.SelectedIndexChanged += chooseProgram_SelectedIndexChanged;
            // 
            // car
            // 
            car.Location = new Point(55, 291);
            car.Name = "car";
            car.Size = new Size(191, 88);
            car.TabIndex = 3;
            car.TabStop = false;
            // 
            // frontDoor
            // 
            frontDoor.BackColor = SystemColors.ActiveCaptionText;
            frontDoor.Controls.Add(panel1);
            frontDoor.Location = new Point(282, 258);
            frontDoor.Name = "frontDoor";
            frontDoor.Size = new Size(36, 121);
            frontDoor.TabIndex = 4;
            // 
            // panel1
            // 
            panel1.Location = new Point(29, 95);
            panel1.Name = "panel1";
            panel1.Size = new Size(174, 10);
            panel1.TabIndex = 6;
            // 
            // rearDoor
            // 
            rearDoor.BackColor = SystemColors.ActiveCaptionText;
            rearDoor.Location = new Point(566, 258);
            rearDoor.Name = "rearDoor";
            rearDoor.Size = new Size(36, 121);
            rearDoor.TabIndex = 5;
            // 
            // washingHead
            // 
            washingHead.BackColor = Color.Aqua;
            washingHead.Location = new Point(324, 356);
            washingHead.Name = "washingHead";
            washingHead.Size = new Size(234, 10);
            washingHead.TabIndex = 6;
            // 
            // traficLight
            // 
            traficLight.BackColor = Color.Green;
            traficLight.Location = new Point(196, 225);
            traficLight.Name = "traficLight";
            traficLight.Size = new Size(33, 35);
            traficLight.TabIndex = 7;
            // 
            // inFrontOfIndic
            // 
            inFrontOfIndic.Location = new Point(188, 12);
            inFrontOfIndic.Name = "inFrontOfIndic";
            inFrontOfIndic.Size = new Size(25, 25);
            inFrontOfIndic.TabIndex = 8;
            // 
            // inIndic
            // 
            inIndic.Location = new Point(188, 53);
            inIndic.Name = "inIndic";
            inIndic.Size = new Size(25, 25);
            inIndic.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(219, 16);
            label1.Name = "label1";
            label1.Size = new Size(151, 20);
            label1.TabIndex = 10;
            label1.Text = "Car in front of washer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(221, 56);
            label2.Name = "label2";
            label2.Size = new Size(97, 20);
            label2.TabIndex = 11;
            label2.Text = "Car in washer";
            // 
            // washingIndic
            // 
            washingIndic.Location = new Point(188, 94);
            washingIndic.Name = "washingIndic";
            washingIndic.Size = new Size(25, 25);
            washingIndic.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(221, 97);
            label3.Name = "label3";
            label3.Size = new Size(69, 20);
            label3.TabIndex = 12;
            label3.Text = "Washing!";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(washingIndic);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(inIndic);
            Controls.Add(inFrontOfIndic);
            Controls.Add(traficLight);
            Controls.Add(washingHead);
            Controls.Add(rearDoor);
            Controls.Add(frontDoor);
            Controls.Add(car);
            Controls.Add(chooseProgram);
            Controls.Add(programStart);
            Controls.Add(toWasher);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)car).EndInit();
            frontDoor.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ColorDialog colorDialog1;
        private Button toWasher;
        private Button programStart;
        private ComboBox chooseProgram;
        private PictureBox car;
        private Panel frontDoor;
        private Panel rearDoor;
        private Panel panel1;
        private Panel washingHead;
        private Panel traficLight;
        private Panel inFrontOfIndic;
        private Panel inIndic;
        private Label label1;
        private Label label2;
        private Panel washingIndic;
        private Label label3;
    }
}
