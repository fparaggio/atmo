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

namespace Atmo.Stats {
	public class ReadingRangeAggregate : ReadingValuesRange {

		public readonly TimeRange TimeStamp;
        [Obsolete("May need to remove this to increase data density")]
		private Reading _min;
        [Obsolete("May need to remove this to increase data density")]
		private Reading _max;

        public ReadingRangeAggregate(
            TimeRange timeStamp,
            ReadingValuesRange ranges
        ) : base(ranges)
        {
            TimeStamp = new TimeRange(timeStamp);
            _min = _max = null;
        }

		public ReadingRangeAggregate(
			TimeRange timeStamp,
			Range temperature,
			Range humidity,
			Range pressure,
			Range windSpeed,
			Range windDirection
		) : base(temperature, humidity, pressure, windSpeed, windDirection)
        {
			TimeStamp = new TimeRange(timeStamp);
			_min = _max = null;
		}

		public new Reading Min { get {
			return _min ?? (_min = new Reading(
				TimeStamp.Low,
                base.Min
			));
		} }

		public new Reading Max { get {
			return _max ?? (_max = new Reading(
				TimeStamp.High,
				base.Max
			));
		} }

	}
}
