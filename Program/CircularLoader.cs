namespace WhatsappAutomation
{
    public partial class CircularLoader : UserControl
    {
        private System.Windows.Forms.Timer timer;
        private float angle;
        public CircularLoader()
        {
            //InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint|ControlStyles.UserPaint|ControlStyles.OptimizedDoubleBuffer,true);
            this.Size = new Size(50, 50);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            angle += 5;
            if (angle >=360)
            {
                angle = 0;
            }
            this.Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Blue,3);
            g.DrawArc(pen,0,0,this.Width-1,this.Height-1,angle,270);
        }
    }
}
