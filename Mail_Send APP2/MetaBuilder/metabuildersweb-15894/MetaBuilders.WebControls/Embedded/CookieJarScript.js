<!--
//------------------------------------------------------------------------------
// TopDragon CookieJar(tm)
//------------------------------------------------------------------------------
// A Javascript CookieJar object with associated methods for using cookies.
//
// Copyright 2001 by TopDragon Software (www.bydisn.com or tracy@bydisn.com).
//
// This code isn't shareware, it's absolutely free. But if you really want to 
// (or if your conscience bugs you) feel free to send cash, gifts, liquor,
// stocks, or just drop me a line and let me know that you're actually using it.
// That will give me incentive to do more free stuff like this.
//
// Be sure to visit the TopDragon software page at www.bydisn.com/software!
// There's lots of other free stuff (and some stuff that that isn't free too).
//------------------------------------------------------------------------------
// You may remove the following instructions/comments to decrease the size of 
// this file, but PLEASE leave the above copyright info. I worked hard on this.
//------------------------------------------------------------------------------


//------------------------------------------------------------------------------
// This cookie jar code has been modified from its original implementation.
// It now includes a dedicated Cookie object for each cookie, instead of a
// simple string value. This is to better support the use of keys in each cookie.
// Keys are common in server-side cookie handling.
//      Andy Smith ( AndyCodeMonkey@hotmail.com )
//------------------------------------------------------------------------------

function TDCookieJarObj() {
	this.cookies = new Array();
	this.secure = false;
	this.domain = "";
	this.path = "";
	this.theCookie = "";
	this.cookiesEnabled = testCookies();
	if ( !this.cookiesEnabled ) {
		this.toString = cookiesNotEnabled;
		this.refreshCookies = cookiesNotEnabled;
		this.setCookie = cookiesNotEnabled;
		this.getCookie = cookiesNotEnabled;
		this.deleteCookie = cookiesNotEnabled;
		this.setDomain = cookiesNotEnabled;
		this.setPath = cookiesNotEnabled;
		this.setSecure = cookiesNotEnabled;
		this.setUnsecure = cookiesNotEnabled;
	} else {
		this.toString = CookieJarToString;
		this.setCookie = setCookie;
		this.getCookie = getCookie;
		this.deleteCookie = deleteCookie;
		this.setDomain = setDomain;
		this.setPath = setPath;
		this.setSecure = setSecure;
		this.setUnsecure = setUnsecure;
		this.refreshCookies = refreshCookies;
		this.refreshCookies();
	}
	return this;

//------------------------------------------------------------------------------
// Internally Used Functions
//------------------------------------------------------------------------------

	function cookiesNotEnabled() {
		alert("Cookies are not enabled!");
	} // cookiesNotEnabled


	// changed on jul 8, 2002 by Andy to fill the cookies with Cookie objects instead of strings.
	function refreshCookies() {
		//delete this.cookies;
		this.cookies = new Array();

		this.theCookie = document.cookie;
		// If there is a cookie string, parse it and store the cookies
		if ( this.theCookie.length > 0 ) {
			this.theCookie += ';';
			var startpos = 0;
			var endpos = 0;
			do {
				endpos = this.theCookie.indexOf(';', startpos);
				var tmp = this.theCookie.substring(startpos, endpos);
				var eqpos = tmp.indexOf('=');
				var newCookie = new Cookie();
				if ( eqpos > -1 ) {
					var cname = tmp.substring(0,eqpos);
					newCookie.name = cname;
					newCookie.parse( unescape( tmp.substring(eqpos+1) ) );
					this.cookies[cname] = newCookie;
				} else {
					newCookie.name = tmp;
					this.cookies[tmp] = newCookie;
				}
				startpos = this.theCookie.charAt(endpos+1) == " " ? endpos + 2 : endpos + 1;
			} while ( startpos < this.theCookie.length );
		}
	} // refreshCookies

	function getExpirationDate(exp) {
		// See if it's a Date object
		if (exp.constructor == Date) {
			expdt = exp;
		// See if it's the string "NEVER" (never is 12/31/2099 in this case)
		} else if ( exp.toUpperCase() == "NEVER" ) {
			expdt = new Date(2099,11,31);
		// See if it's an incremental format: +nD (days), +nH (hours), +nM(minutes)
		} else if (exp.charAt(0) == '+') {
			var incr = parseInt(exp.substring(1,exp.length-1));
			var unit = exp.charAt(exp.length-1);
			if ( 'DdHhMm'.indexOf(unit) < 0 ) {
				alert("CookieJar.setCookie: Invalid expiration date increment unit");
				return "";
			}
			if ( isNaN(incr) ) {
				alert("CookieJar.setCookie: Non-numeric expiration date increment");
				return "";
			}
			switch (unit.toUpperCase()) {
				case "D" : incr *= 24;
				case "H" : incr *= 60;
				case "M" : incr *= 60;
				default  : incr *= 1000;
			}
			expdt = new Date();
			expdt.setTime(expdt.getTime()+incr);
		// See if it's a date string in a format accepted by the 
		// Date.parse method, such as "Dec. 25, 2001"
		} else {
			if ( isNaN(Date.parse(exp)) ) {
				alert("CookieJar.setCookie: Invalid expiration date");
				return "";
			}
			expdt = new Date(exp);
		}
		// Got a valid expiration date, format and return the expires string
		return ";expires="+expdt.toGMTString();
	} // getExpirationDate

	function testCookies() {
		// Sets and gets a cookie to see if cookies are enabled
		var exp = new Date();
		exp.setTime(exp.getTime()+(60*1000));
		var cstring = "test=1";
		document.cookie = cstring;
		if ( document.cookie ) {
			if ( document.cookie.indexOf('test=1') >= 0 ) {
				// Yup, they're enabled. Delete the test cookie
				exp.setFullYear(exp.getFullYear()-1);
				document.cookie = "test=;expires="+exp.toGMTString();
				return true;
			}
		}
		return false;
	} // testCookies


	// changed on jul 8, 2002 by Andy to use Cookie objects instead of strings
	function CookieJarToString() {
		this.refreshCookies();
		var str = "CookieJar: ";
		var ck;
		var cname;
		for ( cname in this.cookies ) {
			var ck = this.cookies[cname];
			str += ck.name +"="+ck.toString() +";";
		}
		return str;
	} // CookieJarToString

//------------------------------------------------------------------------------
// CookieJar Public Methods	
//------------------------------------------------------------------------------
	
	// changed on jul 8, 2002 by Andy to use Cookie objects instead of strings
	function setCookie(newCookie) {
		var cexp = "";
		if ( newCookie.expires ) {
			cexp = getExpirationDate(newCookie.expires);
		}
		var cdom = this.domain == "" ? "" : ";domain="+this.domain;
		var cpath = this.path == "" ? "" : ";path="+this.path;
		var csec = this.secure == false ? "" : ";SECURE";
		escapeCookieValue( newCookie )
		document.cookie = newCookie.name + "=" + escapeCookieValue( newCookie ) +cexp+cdom+cpath+csec;
		this.refreshCookies();
	} // setCookie
	
	function escapeCookieValue( ck ) {
		if ( !ck.hasKeys() ) {
			return escape( ck.getValue() );
		} else {
			var escapedValue = "";
			for( var i=0; i < ck.keys.length; i++ ) {
				if ( i != 0 ) {
					escapedValue += "&";
				}
				escapedValue += escape(ck.keys[i]) + "=" + escape(ck.values[ck.keys[i]]);
			}
			return escapedValue;
		}
	}

	function getCookie(cname) {
		this.refreshCookies();
		return this.cookies[cname];
	} // getCookie


	// changed on jul 8, 2002 by Andy to delete by Cookie object or name.
	function deleteCookie(deadCookie) {
		if ( typeof( deadCookie ) == "string" ) {
			deadCookie = this.getCookie( deadCookie );
		}
		var lastyear = new Date();
		lastyear.setFullYear(lastyear.getFullYear()-1);
		deadCookie.expires = lastyear;
		deadCookie.setValue("");
		this.setCookie(deadCookie);
		this.refreshCookies();
	} // deleteCookie

	function setDomain(domain) {
		this.domain = domain;
	} // setDomain

	function setPath(path) {
		this.path = path;
	} // setPath

	function setSecure() {
		if ( arguments.length == 0 ) {
			this.secure = true;
		} else {
			if ( typeof(arguments[0]) != "boolean" ) {
				alert("CookieJar.setSecure: Argument is not boolean");
			} else {
				this.secure = arguments[0];
			}
		}
	} // setSecure

	function setUnsecure() {
		if ( arguments.length == 0 ) {
			this.secure = false;
		} else {
			if ( typeof(arguments[0]) != "boolean" ) {
				alert("CookieJar.setUnsecure: Argument is not boolean");
			} else {
				this.secure = !arguments[0];
			}
		}
	} // setUnsecure

}



/// <summary>represents one cookie in the cookies collection of the CookieJar.</summary>
function Cookie() {

// public
	this.name = "";
	this.getValue = Cookie_getValue;
	this.setValue = Cookie_setValue;
	this.expires = null;
	
	this.hasKeys = Cookie_getHasKeys;
	this.keys = new Array();
	this.values = new Array();
	
	this.parse = Cookie_parse;
	this.toString = Cookie_toString;

// internal
	this.nonKeyValue = "";
	this.keyValue = Cookie_keyValue;
	
	return this;

	/// <summary>returns a string representation of the value of the Cookie.</summary>
	function Cookie_toString() {
		if ( this.hasKeys() ) {
			return this.keyValue();
		} else {
			return this.nonKeyValue;
		}
	}
	
	/// <summary>returns the value of the Coookie, with the optional key name.</summary>
	/// <remarks>
	/// called as getValue(), returns the full value string of the Cookie.
	/// called as getValue( myKey ), return the value of the given key in the Cookie.
	/// </remarks>
	function Cookie_getValue() {
		if ( this.nonKeyValue == "" ) {
			return "";
		}
		
		var requestedKey = "";
		
		if ( arguments.length == 1 ) {
			requestedKey = arguments[0];
		}
		
		if ( requestedKey != "" ) {
			return this.values[ requestedKey ];
		} else {
			return this.nonKeyValue;
		}
	}
	
	/// <summary>sets the value of the Cookie, with the option key name.</summary>
	/// <remarks>
	/// called as setValue(myValue), sets the full value of the Cookie to that value.
	/// called as setValue(myKey, myValue), sets the key to that value.
	/// </remarks>
	function Cookie_setValue() {
		var newKey = "";
		var newValue = "";
		
		if ( arguments.length == 1 ) {
			newValue = arguments[0];
		} else if ( arguments.length >= 2 ) {
			newKey = arguments[0];
			newValue = arguments[1];
		}
		
		if ( newKey == "" ) {
			// no key was given so erase the key/value pairs, and set the full value
			delete this.keys;
			this.keys = new Array();
			delete this.values;
			this.values = new Array();
			
			this.nonKeyValue = newValue;
		} else {
			// ensure the key exists and set the value for the key.
			var found = false;
			for( var i=0; i < this.keys.length; i++ ) {
				if ( this.keys[i] == newKey ) {
					found = true;
					break;
				}
			}
			if ( !found ) {
				this.keys[this.keys.length] = newKey;
			}
			
			this.values[ newKey ] = newValue;
			this.nonKeyValue = this.keyValue();
		}
	}

	/// <summary>Indicates whether the Cookie contains keys</summary>
	function Cookie_getHasKeys() {
		return this.keys.length > 0;
	}

	/// <summary>Parses a value string into either the normal value, or into seperate key/value pairs</summary>
	function Cookie_parse( cookieString ) {
		this.nonKeyValue = cookieString;
		
		var keyvalues = this.nonKeyValue.split( "&" );
		var parseKeys = ( keyvalues.length > 1 );
		if ( parseKeys ) {
			for( var i = 0; i < keyvalues.length; i++ ) {
				var keypair = keyvalues[i];
				if ( keypair.indexOf("=") < 0 ) {
					parseKeys = false;
					break;
				}
			}
		}
		if ( parseKeys ) {
			for ( var i = 0; i < keyvalues.length; i++ ) {
				var newKey = keyvalues[i].substring(0, keyvalues[i].indexOf("=") );
				var newValue = keyvalues[i].substring( keyvalues[i].indexOf("=") + 1, keyvalues[i].length );
				this.keys[ i ] = newKey;
				this.values[ newKey ] = newValue;
			}
		}
	}
	
	
	/// <summary>returns a string representation of the values collection.</summary>
	/// <remarks>
	/// this is not intended to be used by your code.
	/// it is for internal use by the toString() method.
	/// </remarks>
	function Cookie_keyValue() {
		var finalValue = "";
		for ( var i = 0; i < this.keys.length; i++ ) {
			if ( i != 0 ) {
				finalValue += "&";
			}
			finalValue += this.keys[i] + "=" + this.values[this.keys[i]];
		}
		return finalValue;
	}

}

var CookieJar = new TDCookieJarObj();
