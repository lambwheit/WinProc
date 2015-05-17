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
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using WinProc.core.ui;

namespace WinProc.core.process
{
    class ProcessManager
    {
        public static myProcessList GetProcesses
        {
            get
            {
                myProcessList _list = new myProcessList();

                try
                {
                    ManagementClass mClass = new ManagementClass("Win32_Process");
                    ManagementObjectCollection mObjCollection = mClass.GetInstances();

                    foreach (ManagementObject proc in mObjCollection)
                    {
                        try
                        {
                            myProcess myproc = new myProcess();
                            myproc.ID = proc["ProcessId"].ToString();
                            myproc.Name = proc["Name"].ToString();
                            if (proc["ExecutablePath"] != null) myproc.Filename = proc["ExecutablePath"].ToString();
                            myproc.Priority = proc["Priority"].ToString();
                            _list.Add(myproc);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    return _list;
                }
                catch (Exception ex)
                {
                    InformUser.showError(ex.Message);
                    return _list;
                }
            }
        }

        public static Boolean KillProcess(int id)
        {
            try
            {
                Process p = Process.GetProcessById(id);
                p.Kill();
                p.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                InformUser.showError(ex.Message);
                return false;
            }
        }

    }
}
