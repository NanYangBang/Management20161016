String.format = function () {
	if (arguments.length == 0)
		return null;

	var str = arguments[0];
	for (var i = 1; i < arguments.length; i++) {
		var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
		str = str.replace(re, arguments[i]);
	}
	return str;
}

Array.prototype.indexOf = function (substr, start) {
	var ta, rt, d = '\0';
	if (start != null) { ta = this.slice(start); rt = start; } else { ta = this; rt = 0; }
	var str = d + ta.join(d) + d, t = str.indexOf(d + substr + d);
	if (t == -1) return -1; rt += str.slice(0, t).replace(/[^\0]/g, '').length;
	return rt;
}

String.prototype.ASCIIHash = function () {
	var text_to_insert = [];
	var str = this.split('');
	for (i = 0; i < str.length; i++) {
		text_to_insert.push(str[i].charCodeAt());
	}
	return text_to_insert.join('');
}

String.prototype.toJson = function () {
	return eval('(' + this + ')');
}

Date.formatDate = function (date) {
	return new Date(date.replace(/\s(\+|\-)/, ' UTC$1'));
}

String.prototype.ASCIILength = function () {
	var count = 0;
	for (var i = 0; i < this.length; i++) {
		if (this.charCodeAt(i) > 255) {
			count += 2;
		} else {
			count++;
		}
	}
	return count;
}

String.prototype.hashCode = function () {
	var hash = 0;
	if (this.length == 0) return hash;
	for (i = 0; i < this.length; i++) {
		char = this.charCodeAt(i);
		hash = ((hash << 5) - hash) + char;
		hash = hash & hash; // Convert to 32bit integer
	}
	return hash;
}

Number.prototype.toRound = function (places) {
	var decimal = this.toString();
	places = Math.pow(10, places);
	decimal = Math.round(decimal * places) / places;
	return decimal;
}

String.prototype.trim = function () {
	return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
};