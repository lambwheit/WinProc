/*
 * 
    WinProc - light Windows Processes Manager
    Copyright (C) 2015 uberalles - uber_alles999@yahoo.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinProc.core.process;

namespace WinProc.core.ui.forms
{
    public partial class mainForm : Form
    {
        private myProcessList _myProcList;

        public mainForm()
        {
            InitializeComponent();
            _myProcList = new myProcessList();
            _myProcList = ProcessManager.GetProcesses;
            counter.Text = _myProcList.Count + " processes.";
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            aboutForm about = new aboutForm();
            about.ShowDialog();
            about.Dispose();
            about = null;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            try
            {
                foreach (myProcess myproc in _myProcList)
                    procslist.Items.Add(new ListViewItem(new String[] { myproc.ID, myproc.Name, myproc.Priority, myproc.Filename }));
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
            }
        }

        private void killProcessMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (procslist.SelectedItems.Count == 1)
                    if (ProcessManager.KillProcess(int.Parse(procslist.SelectedItems[0].SubItems[0].Text)))
                    {
                        InformUser.showInfo("The process has been killed..");
                        RefreshProcs();
                    }
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
            }
        }

        private void RefreshProcs()
        {
            try
            {
                procslist.Items.Clear();
                _myProcList = ProcessManager.GetProcesses;
                counter.Text = _myProcList.Count + " processes.";
                foreach (myProcess myproc in _myProcList)
                    procslist.Items.Add(new ListViewItem(new String[] { myproc.ID, myproc.Name, myproc.Priority, myproc.Filename }));
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
            }
        }
        private void popupMenu_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (procslist.SelectedItems.Count == 1)
                    killProcessMenuItem.Visible = true;
                else
                    killProcessMenuItem.Visible = false;
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
            }
        }

        private void procslist_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    Point pt = popupMenu.PointToScreen(e.Location);
                    popupMenu.Show(pt);
                }
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
            }
        }

        private void refreshMenuItem_Click(object sender, EventArgs e)
        {
            RefreshProcs();
        }
    }
}
