namespace Open3D.Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.open3DViewer1 = new Open3D.Controls.Open3DViewer();
            this.SuspendLayout();
            // 
            // open3DViewer1
            // 
            this.open3DViewer1.BackColor = System.Drawing.Color.Black;
            this.open3DViewer1.CameraMode = Open3D.Controls.CameraMode.Free;
            this.open3DViewer1.CameraRotationPoint = ((OpenTK.Vector3)(resources.GetObject("open3DViewer1.CameraRotationPoint")));
            this.open3DViewer1.ClearColor = System.Drawing.Color.Black;
            this.open3DViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.open3DViewer1.Location = new System.Drawing.Point(0, 0);
            this.open3DViewer1.Name = "open3DViewer1";
            this.open3DViewer1.Size = new System.Drawing.Size(984, 761);
            this.open3DViewer1.TabIndex = 0;
            this.open3DViewer1.VSync = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.open3DViewer1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Open3DViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Open3D.Controls.Open3DViewer open3DViewer1;
    }
}

