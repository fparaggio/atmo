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
	public class ReadingAggregate : ReadingValues, IReading {

		public TimeRange TimeRange;
		public int Count;

		public new static ReadingAggregate CreateInvalid() {
			return new ReadingAggregate(DateTime.MinValue, DateTime.MaxValue, ReadingValues.CreateInvalid(), 0);
		}

		public ReadingAggregate(DateTime beingStamp, DateTime endStamp, ReadingValues values, int count) : base(values) {
			TimeRange = new TimeRange(beingStamp, endStamp);
			Count = count;
		}

		public TimeSpan TimeSpan {
			get {
				return TimeRange.Span;
				//return EndStamp.AddTicks(1).Subtract(BeginStamp);
			}
		}

		public DateTime TimeStamp {
			// TODO: make this the midpoint???
			get { return TimeRange.Low; }
		}

		public DateTime BeginStamp {
			get { return TimeRange.Low; }
		}

		public DateTime EndStamp {
			get { return TimeRange.High; }
		}


	}
}
