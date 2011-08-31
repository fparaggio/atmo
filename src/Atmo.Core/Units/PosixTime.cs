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

namespace Atmo.Units {

	/// <summary>
	/// A 32bit signed posix formated time.
	/// </summary>
	public struct PosixTime {

		public static bool operator > (PosixTime a, PosixTime b) {
			return a.Value > b.Value;
		}

		public static bool operator < (PosixTime a, PosixTime b) {
			return a.Value < b.Value;
		}

		public static bool operator ==(PosixTime a, PosixTime b) {
			return a.Value == b.Value;
		}

		public static bool operator !=(PosixTime a, PosixTime b) {
			return a.Value != b.Value;
		}

		public static implicit operator int(PosixTime t) {
			return t.Value;
		}

		public static implicit operator PosixTime(int i) {
			return new PosixTime(i);
		}

		public readonly int Value;

		public PosixTime(int value) {
			Value = value;
		}

		public PosixTime(DateTime dateTime) {
			Value = UnitUtility.ConvertToPosixTime(dateTime);
		}



	}
}
