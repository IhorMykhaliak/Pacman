namespace Pacman.DesktopUI
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.menu = new System.Windows.Forms.PictureBox();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.menu)).BeginInit();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.Transparent;
            this.menu.Image = global::Pacman.DesktopUI.Properties.Resources.menu;
            this.menu.InitialImage = null;
            this.menu.Location = new System.Drawing.Point(-2, -4);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(652, 664);
            this.menu.TabIndex = 0;
            this.menu.TabStop = false;
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.scoreLabel.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scoreLabel.ForeColor = System.Drawing.Color.Gold;
            this.scoreLabel.Location = new System.Drawing.Point(320, 614);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(92, 35);
            this.scoreLabel.TabIndex = 1;
            this.scoreLabel.Text = "Score:";
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.BackColor = System.Drawing.Color.Transparent;
            this.countLabel.Font = new System.Drawing.Font("Comic Sans MS", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.countLabel.ForeColor = System.Drawing.Color.Gold;
            this.countLabel.Location = new System.Drawing.Point(418, 614);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(30, 35);
            this.countLabel.TabIndex = 2;
            this.countLabel.Text = "0";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(648, 658);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.countLabel);
            this.Controls.Add(this.scoreLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GameForm";
            this.Text = "Pac-Man";
            this.Load += new System.EventHandler(this.GameLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameFormKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.menu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox menu;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label countLabel;

    }
}

