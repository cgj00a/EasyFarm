/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI>
Copyright (C) Mykezero

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
///////////////////////////////////////////////////////////////////*/

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace EasyFarm.ViewModels
{
    public class ProcessSelectionViewModel : BindableBase
    {
        /// <summary>
        ///     The name of the Processes to search for.
        /// </summary>
        private const string ProcessName = "pol";

        public string Title => "Please Select Your Character";

        public ProcessSelectionViewModel()
        {
            Processes = new ObservableCollection<Process>();
            RefreshCommand = new DelegateCommand(OnRefresh);
            OnRefresh();
        }

        public DelegateCommand RefreshCommand { get; set; }

        public Process SelectedProcess { get; set; }

        public ObservableCollection<Process> Processes { get; set; }

        private void OnRefresh()
        {
            Processes.Clear();

            Processes.AddRange(Process.GetProcessesByName(ProcessName)
                .Where(x => string.IsNullOrWhiteSpace((x.MainWindowTitle)))
                .ToList());

            if(Processes.Count > 0) return;

            Processes.AddRange(Process.GetProcesses()
                .Where(x => !string.IsNullOrWhiteSpace(x.MainWindowTitle))
                .ToList());
        }
    }
}