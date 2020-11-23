using System;
using System.Collections.Generic;
using System.Text;

class CommonUtil {
	public static int get7BitEncodingLength(int value) {
		int length = 1;
		while (value >= 128) {
			length++;
			value /= 128;
		}

		return length;
	}
}

