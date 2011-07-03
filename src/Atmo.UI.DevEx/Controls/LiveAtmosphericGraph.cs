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


using System;
using System.Collections.Generic;
using System.Linq;
using Atmo.Stats;
using DevExpress.XtraCharts;
using Atmo.Units;

namespace Atmo.UI.DevEx.Controls {
	public partial class LiveAtmosphericGraph : DevExpress.XtraEditors.XtraUserControl {
		
		public LiveAtmosphericGraph() {
			InitializeComponent();
		}

		public TemperatureUnit TemperatureUnit { get; set; }

		public SpeedUnit SpeedUnit { get; set; }

		public PressureUnit PressureUnit { get; set; }

		public void SetDataSource<T>(IEnumerable<T> items) where T:IReading {
			SetDataSource(items.ToList());
		}

		public void SetDataSource<T>(List<T> items) where T:IReading {
			List<Reading> readings;
			if(typeof(T) == typeof(Reading)) {
				readings = items as List<Reading>;
			}else {
				readings = new List<Reading>(items.Count);
				for(int i = 0; i < items.Count; i++) {
					readings.Add(new Reading(items[i]));
				}
			}
			SetDataSource(readings);
		}

		public void SetDataSource(List<Reading> items) {
			chartControl.DataSource = new List<Reading>(items);
			ForceYRanges(items);
			//chartControl.Update();
		}

		private void ForceYRanges(List<Reading> readings) {
			if (readings.Count == 0) {
				return;
			}
			var diagram = (chartControl.Diagram as XYDiagram);
			if (null == diagram) {
				return;
			}

			var minMaxCalculator = new ReadingMinMaxCalculator<Reading>();
			for(var i = 0; i < readings.Count; i++) {
				minMaxCalculator.Proccess(readings[i]);
			}

			var range = minMaxCalculator.Result;
			var minRange = new ReadingValues(
				3,
				3,
				3,
				3,
				0
			);

			if (range.Temperature.Size < minRange.Temperature && minRange.Temperature > 0) {
				diagram.AxisY.Range.Auto = false;
				var valRange = new Range(0, minRange.Temperature);
				valRange.Recenter(range.Temperature.Mid);
				diagram.AxisY.Range.SetMinMaxValues(valRange.Low, valRange.High);
			}
			else {
				diagram.AxisY.Range.Auto = true;
			}
			if (range.Pressure.Size < minRange.Pressure && minRange.Pressure > 0) {
				diagram.SecondaryAxesY[2].Range.Auto = false;
				var valRange = new Range(0, minRange.Pressure);
				valRange.Recenter(range.Pressure.Mid);
				diagram.SecondaryAxesY[2].Range.SetMinMaxValues(valRange.Low,valRange.High);
				//diagram.SecondaryAxesY[2].NumericOptions.Precision = (desiredPressureUnit == PressureUnit.Millibar ? 1 : 2);
			}
			else {
				diagram.SecondaryAxesY[2].Range.Auto = true;
			}
			if (range.Humidity.Size < minRange.Humidity && minRange.Humidity > 0) {
				diagram.SecondaryAxesY[0].Range.Auto = false;
				var valRange = new Range(0, minRange.Humidity);
				valRange.Recenter(range.Humidity.Mid);
				diagram.SecondaryAxesY[0].Range.SetMinMaxValues(valRange.Low, valRange.High);
				//diagram.SecondaryAxesY[0].NumericOptions.Precision = 1;
			}
			else {
				diagram.SecondaryAxesY[0].Range.Auto = true;
			}
			if (range.WindSpeed.Size < minRange.WindSpeed && minRange.WindSpeed > 0) {
				diagram.SecondaryAxesY[1].Range.Auto = false;
				var valRange = new Range(0, minRange.WindSpeed);
				valRange.Recenter(range.WindSpeed.Mid);
				diagram.SecondaryAxesY[1].Range.SetMinMaxValues(valRange.Low, valRange.High);
			}
			else {
				diagram.SecondaryAxesY[1].Range.Auto = true;
			}

		}


		public void FormatTimeAxis(TimeSpan timeSpan) {
			FormatTimeAxis((chartControl.Diagram as XYDiagram).AxisX, timeSpan);
		}

		private void FormatTimeAxis(AxisX axis, TimeSpan timeSpan) {

			axis.DateTimeScaleMode = DateTimeScaleMode.Manual;
			//axis.Range.Auto = true;

			if (timeSpan <= new TimeSpan(0, 15, 0)) {
				axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				axis.DateTimeOptions.FormatString = "HH:mm:ss";
				axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Second;
				axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Second;
			}
			else{
				axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				axis.DateTimeOptions.FormatString = "HH:mm";
				axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Minute;
				axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Minute;
			}


			/*else if (totalSpan <= new TimeSpan(1, 0, 0, 0)) {
				axis.DateTimeOptions.Format = DateTimeFormat.Custom;
				axis.DateTimeOptions.FormatString = "HH:mm";
				axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Hour;
				axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Hour;
			}
			else if (totalSpan <= new TimeSpan(180, 0, 0, 0)) {
				axis.DateTimeOptions.Format = DateTimeFormat.ShortDate;
				axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Day;
				axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
			}
			else {
				axis.DateTimeOptions.Format = DateTimeFormat.ShortDate;
				axis.DateTimeGridAlignment = DateTimeMeasurementUnit.Week;
				axis.DateTimeMeasureUnit = DateTimeMeasurementUnit.Day;
			}*/
		}


		public void SetLatest(Stats.ReadingAggregate latest) {
			if(null == latest) {
				return;
			}

			// Todo: get the desired unit
			int pressureDecimals = (PressureUnit == PressureUnit.Millibar ? 1 : 2);
			int humidityDecimals = 1;

			var diagram = chartControl.Diagram as XYDiagram;
			diagram.AxisY.ConstantLines[0].AxisValue = latest.Temperature;
			diagram.AxisY.ConstantLines[0].Title.Text = latest.Temperature.ToString("F1");
			diagram.SecondaryAxesY[2].NumericOptions.Precision = pressureDecimals;
			diagram.SecondaryAxesY[2].ConstantLines[0].AxisValue = latest.Pressure;
			diagram.SecondaryAxesY[2].ConstantLines[0].Title.Text = Math.Round(latest.Pressure, pressureDecimals).ToString();
			diagram.SecondaryAxesY[0].NumericOptions.Precision = humidityDecimals;
			diagram.SecondaryAxesY[0].ConstantLines[0].AxisValue = latest.Humidity;
			diagram.SecondaryAxesY[0].ConstantLines[0].Title.Text = String.Concat(Math.Round(latest.Humidity * 100.0, humidityDecimals).ToString(), '%');
			diagram.SecondaryAxesY[1].ConstantLines[0].AxisValue = latest.WindSpeed;
			diagram.SecondaryAxesY[1].ConstantLines[0].Title.Text = latest.WindSpeed.ToString("F2");
			diagram.SecondaryAxesY[3].ConstantLines[0].AxisValue = latest.WindDirection;
			diagram.SecondaryAxesY[3].ConstantLines[0].Title.Text = Math.Round(latest.WindDirection).ToString();

		}
	}
}
