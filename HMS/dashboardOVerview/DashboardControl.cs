using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HMS
{
    public partial class DashboardControl : UserControl
    {
        public DashboardControl()
        {
            InitializeComponent();
        }

        private void DashboardControl_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Timer-based auto-refresh and dynamic card generation for responsive dashboard
        private System.Windows.Forms.Timer _refreshTimer;
        private Func<List<Tuple<string, string>>> _dataProvider;
        private List<Tuple<string, string>> _lastData = new List<Tuple<string, string>>();

        // richer item provider (supports icons, colors, sizes)
        private Func<List<DashboardItem>> _itemProvider;
        private List<DashboardItem> _lastItems = new List<DashboardItem>();

        // lightweight DTO for richer card content
        public class DashboardItem
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public Image Icon { get; set; }
            public Color? CardBackColor { get; set; }
            public Color? TitleColor { get; set; }
            public int? Width { get; set; }
            public int? Height { get; set; }
        }

        // performance & layout
        private readonly List<Panel> _cardPool = new List<Panel>();
        private int _desiredCardsPerRow = 4; // compact: 3-4 per row (default 4)
        private static readonly Font _titleFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        private static readonly Font _subtitleFont = new Font("Segoe UI", 8F, FontStyle.Regular);
        private const int _cardMargin = 8;


        // Call this to start polling for simple tuple data (backwards compatibility)
        // dataProvider should return a list of Tuple<Title, Subtitle> describing each card to display.
        public void StartAutoRefresh(Func<List<Tuple<string, string>>> dataProvider, int intervalMs = 5000)
        {
            if (dataProvider == null) throw new ArgumentNullException(nameof(dataProvider));

            _dataProvider = dataProvider;
            _itemProvider = null;

            StartTimer(intervalMs);
        }

        // Call this to start polling for richer DashboardItem data (supports icons/colors/sizes)
        public void StartAutoRefresh(Func<List<DashboardItem>> dataProvider, int intervalMs = 5000)
        {
            if (dataProvider == null) throw new ArgumentNullException(nameof(dataProvider));

            _itemProvider = dataProvider;
            _dataProvider = null;

            StartTimer(intervalMs);
        }

        private void StartTimer(int intervalMs)
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
            }

            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = intervalMs;
            _refreshTimer.Tick += (s, e) => RefreshFromProvider();
            _refreshTimer.Start();

            // ensure layout properties for responsiveness
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.AutoScroll = true;
            // ensure reasonable height so cards can wrap
            flowLayoutPanel1.Height = Math.Max(120, flowLayoutPanel1.Height);

            // enable double buffering to reduce flicker
            try
            {
                var pi = flowLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (pi != null) pi.SetValue(flowLayoutPanel1, true, null);
            }
            catch { }

            // listen for resize to adjust card widths
            flowLayoutPanel1.SizeChanged -= FlowLayoutPanel1_SizeChanged;
            flowLayoutPanel1.SizeChanged += FlowLayoutPanel1_SizeChanged;

            RefreshFromProvider();
        }

        public void StopAutoRefresh()
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Stop();
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        private void RefreshFromProvider()
        {
            // Run provider on threadpool to avoid blocking UI
            try
            {
                if (_itemProvider != null)
                {
                    Task.Run(() =>
                    {
                        List<DashboardItem> items = null;
                        try { items = _itemProvider.Invoke(); } catch { }
                        if (items == null) return;
                        if (!AreEqualItems(items, _lastItems))
                        {
                            _lastItems = new List<DashboardItem>(items);
                            if (!this.IsDisposed)
                                this.BeginInvoke((Action)(() => UpdateCards(items)));
                        }
                    });
                }
                else if (_dataProvider != null)
                {
                    Task.Run(() =>
                    {
                        List<Tuple<string, string>> data = null;
                        try { data = _dataProvider.Invoke(); } catch { }
                        if (data == null) return;
                        if (!AreEqual(data, _lastData))
                        {
                            _lastData = new List<Tuple<string, string>>(data);
                            if (!this.IsDisposed)
                                this.BeginInvoke((Action)(() => UpdateCards(data)));
                        }
                    });
                }
            }
            catch { }
        }

        private bool AreEqual(List<Tuple<string, string>> a, List<Tuple<string, string>> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i].Item1 != b[i].Item1) return false;
                if (a[i].Item2 != b[i].Item2) return false;
            }
            return true;
        }

        private bool AreEqualItems(List<DashboardItem> a, List<DashboardItem> b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                var ai = a[i];
                var bi = b[i];
                if (ai.Title != bi.Title) return false;
                if (ai.Subtitle != bi.Subtitle) return false;
                // icons and colors not strictly compared
            }
            return true;
        }

        private void UpdateCards(List<Tuple<string, string>> items)
        {
            // convert tuples to DashboardItem for reuse
            var list = items.Select(t => new DashboardItem { Title = t.Item1, Subtitle = t.Item2 }).ToList();
            UpdateCards(list);
        }

        private void UpdateCards(List<DashboardItem> items)
        {
            if (this.IsDisposed) return;

            // Ensure UI thread
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)(() => UpdateCards(items)));
                return;
            }

            flowLayoutPanel1.SuspendLayout();

            // ensure pool has enough panels
            for (int i = _cardPool.Count; i < items.Count; i++)
            {
                var p = CreateCardPanel();
                _cardPool.Add(p);
            }

            // update or reuse panels
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var panel = _cardPool[i];
                ApplyItemToPanel(panel, item);
            }

            // clear flowPanel and add the required panels in order
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                flowLayoutPanel1.Controls.Add(_cardPool[i]);
            }

            AdjustCardSizes();

            flowLayoutPanel1.ResumeLayout();
        }

        private Panel CreateCardPanel()
        {
            var p = new Panel
            {
                Margin = new Padding(_cardMargin),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox { Name = "_icon", Size = new Size(32, 32), SizeMode = PictureBoxSizeMode.Zoom, Location = new Point(8, 8), Visible = false };
            var lblTitle = new Label { Name = "_title", AutoSize = true, Font = _titleFont, Location = new Point(48, 8), ForeColor = Color.Black };
            var lblSub = new Label { Name = "_subtitle", AutoSize = true, Font = _subtitleFont, Location = new Point(48, 30), ForeColor = Color.Gray };

            p.Controls.Add(pic);
            p.Controls.Add(lblTitle);
            p.Controls.Add(lblSub);

            return p;
        }

        private void ApplyItemToPanel(Panel p, DashboardItem item)
        {
            if (p == null || item == null) return;

            p.BackColor = item.CardBackColor ?? Color.White;

            var pic = p.Controls.OfType<PictureBox>().FirstOrDefault(c => c.Name == "_icon");
            var lblTitle = p.Controls.OfType<Label>().FirstOrDefault(c => c.Name == "_title");
            var lblSub = p.Controls.OfType<Label>().FirstOrDefault(c => c.Name == "_subtitle");

            if (pic != null)
            {
                if (item.Icon != null)
                {
                    pic.Image = item.Icon;
                    pic.Visible = true;
                }
                else
                {
                    pic.Image = null;
                    pic.Visible = false;
                }
            }

            if (lblTitle != null)
            {
                lblTitle.Text = item.Title ?? string.Empty;
                lblTitle.ForeColor = item.TitleColor ?? Color.Black;
            }
            if (lblSub != null)
            {
                lblSub.Text = item.Subtitle ?? string.Empty;
            }

            // set requested size if provided
            p.Width = item.Width ?? p.Width;
            p.Height = item.Height ?? 80;
        }

        private void FlowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            AdjustCardSizes();
        }

        private void AdjustCardSizes()
        {
            if (flowLayoutPanel1.Controls.Count == 0) return;

            int available = Math.Max(1, flowLayoutPanel1.ClientSize.Width - flowLayoutPanel1.Padding.Left - flowLayoutPanel1.Padding.Right);
            int perRow = Math.Max(1, Math.Min(_desiredCardsPerRow, 4));

            int totalMargins = (perRow + 1) * _cardMargin * 2 / 2; // approximate margins
            int cardW = Math.Max(150, (available - (perRow + 1) * (_cardMargin * 2)) / perRow);

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                c.Width = cardW;
            }
        }

        // allow consumer to set compactness (3-4 cards per row)
        public void SetDesiredCardsPerRow(int desired)
        {
            _desiredCardsPerRow = Math.Max(1, Math.Min(6, desired));
            AdjustCardSizes();
        }

        private Panel CreateCard(string title, string subtitle)
        {
            return CreateCard(new DashboardItem { Title = title, Subtitle = subtitle });
        }

        private Panel CreateCard(DashboardItem item)
        {
            var w = item.Width ?? 240;
            var h = item.Height ?? 100;

            var p = new Panel
            {
                Width = w,
                Height = h,
                Margin = new Padding(8),
                BackColor = item.CardBackColor ?? Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var x = 10;
            if (item.Icon != null)
            {
                var pic = new PictureBox
                {
                    Image = item.Icon,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(8, 8),
                    Size = new Size(32, 32)
                };
                p.Controls.Add(pic);
                x += 40;
            }

            var lblTitle = new Label
            {
                Text = item.Title ?? string.Empty,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(x, 10),
                ForeColor = item.TitleColor ?? Color.Black
            };

            var lblSub = new Label
            {
                Text = item.Subtitle ?? string.Empty,
                AutoSize = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular),
                Location = new Point(x, 34),
                ForeColor = Color.Gray
            };

            p.Controls.Add(lblTitle);
            p.Controls.Add(lblSub);

            return p;
        }
    }
}
