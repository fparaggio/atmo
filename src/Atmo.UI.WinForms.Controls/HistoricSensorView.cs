﻿// ================================================================================
//
// Atmo 2
// Copyright (C) 2011  BARANI DESIGN
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// 
// Contact: Jan Barani mailto:jan@baranidesign.com
//
// ================================================================================

using System.Drawing;
using System.Windows.Forms;

namespace Atmo.UI.WinForms.Controls {
	public partial class HistoricSensorView : UserControl {

		private bool _selected;

		public HistoricSensorView() {
			_selected = false;

			InitializeComponent();

			SetBackgroundColor();

		}

		public bool IsSelected {
			get { return _selected; }
			set { _selected = value; SetBackgroundColor(); }
		}

		public ISensorInfo SensorInfo { get; set; }

		private void SetBackgroundColor() {
			SetBackgroundColor(IsSelected);
		}

		private void SetBackgroundColor(bool selected) {
			BackColor = (selected) ? SystemColors.Highlight : Color.Transparent;
			ForeColor = (selected) ? SystemColors.ControlText : SystemColors.InactiveCaptionText;
		}

		public void Update(ISensorInfo sensorInfo) {
			SetValues(sensorInfo);
		}

		public void SetValues(ISensorInfo sensorInfo) {
			labelSensorName.Text = sensorInfo == null ? "Sensor" : sensorInfo.Name;
		}

		private void labelSensorName_Click(object sender, System.EventArgs e) {
			IsSelected = !IsSelected;
		}

	}
}
