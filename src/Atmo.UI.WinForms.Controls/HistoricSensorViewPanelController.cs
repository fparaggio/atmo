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

using System.Windows.Forms;
using System;

namespace Atmo.UI.WinForms.Controls {
	public class HistoricSensorViewPanelController : ViewPanelController<HistoricSensorView, ISensorInfo> {

		public HistoricSensorViewPanelController(Control container) : base(container) { }

		public event Action<ISensorInfo> OnDeleteRequested;
		public event Action<ISensorInfo> OnRenameRequested;
		public event Action OnSelectionChanged;

		protected override HistoricSensorView CreateNewView() {
			var view = new HistoricSensorView() {
				IsSelected = DefaultSelected,
			};
			view.OnDeleteRequest += TriggerOnDeleteRequested;
			view.OnRenameRequest += TriggerOnRenameRequested;
			view.OnSelectionChanged += TriggerOnSelectionChanged;
			return view;
		}

		private void TriggerOnDeleteRequested(ISensorInfo sensorInfo) {
			if(null != OnDeleteRequested) {
				OnDeleteRequested(sensorInfo);
			}
		}

		private void TriggerOnRenameRequested(ISensorInfo sensorInfo) {
			if (null != OnRenameRequested) {
				OnRenameRequested(sensorInfo);
			}
		}

		private void TriggerOnSelectionChanged() {
			if(null != OnSelectionChanged) {
				OnSelectionChanged();
			}
		}

		protected override void UpdateView(HistoricSensorView view, ISensorInfo model) {
			if (view.Dock != DockStyle) {
				view.Dock = DockStyle;
			}
			view.Update(model);
		}

		public bool IsSensorSelected(ISensorInfo sensor) {
			foreach(var view in Views) {
				if(null == view.SensorInfo) {
					continue;
				}
				if(view.SensorInfo.Name == sensor.Name) {
					return view.IsSelected;
				}
			}
			return false;
		}
	}
}
