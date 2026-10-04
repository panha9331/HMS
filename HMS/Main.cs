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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            LoadControl(new DashboardControl());
            lblPageTitle.Text = "Dashboard Overview";
        }

        private void LoadControl(UserControl control)
        {
            pnlContent.Controls.Clear();

            control.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(control);
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        
        private void btnDashboard_Click(object sender, EventArgs e)
        {
            LoadControl(new DashboardControl());
            lblPageTitle.Text = "Dashboard Overview";
        }
        private void btnRoom_Click(object sender, EventArgs e)
        {
            LoadControl(new RoomControl());
            lblPageTitle.Text = "Room Management";
        }
        private void btnCustomer_Click(object sender, EventArgs e)
        {
            LoadControl(new CustomerControl());
            lblPageTitle.Text = "Customer Management";
        }
        private void btnBooking_Click(object sender, EventArgs e)
        {
            LoadControl(new BookingControl());
            lblPageTitle.Text = "Booking Management";
        }
    }
}
