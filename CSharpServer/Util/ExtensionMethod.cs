using System;
using System.Collections.Generic;
using System.Text;

static class ExtensionMethod {
	public static int get7BitEncodingLength(this int value) {
		int length = 1;
		while (value >= 128) {
			length++;
			value /= 128;
		}

		return length;
	}
}

