
function ResizeMonitor_Init() {
	if( ResizeMonitor_BrowserCapable() ) {
		document.ResizeMonitor_LastWidth = ResizeMonitor_CurrentWidth();
		document.ResizeMonitor_LastHeight = ResizeMonitor_CurrentHeight();
		ResizeMonitor_AttachResizeHandler();
		document.ResizeMonitor_LastTimer = 0;
		ResizeMonitor_SaveDimensions();
	}
}
function ResizeMonitor_BrowserCapable() {
	if( typeof( window.setTimeout ) == "undefined" ) { return false; }
	if( typeof( window.clearTimeout ) == "undefined" ) { return false; }
	if ( ResizeMonitor_CurrentWidth() == 0 || ResizeMonitor_CurrentHeight() == 0) {return false; }
	if ( !ResizeMonitor_FindForm() ) { return false; }
	return true;
}
function ResizeMonitor_AttachResizeHandler() {
	if ( window.opera ) { window.setInterval("ResizeMonitor_CheckSize();", document.ResizeMonitor_TimeoutLength / 2 ); }
	else { window.onresize=ResizeMonitor_CheckSize; }
}
function ResizeMonitor_CurrentWidth() {
	if ( typeof(window.innerWidth) != "undefined" ) { return window.innerWidth; }
	if ( typeof(document.body) != "undefined" && typeof(document.body.parentElement) != "undefined" && typeof(document.body.parentElement.clientWidth) != "undefined") { return document.body.parentElement.clientWidth; }
	return 0;
}
function ResizeMonitor_CurrentHeight() {
	if ( typeof(window.innerHeight) != "undefined" ) { return window.innerHeight; }
	if ( typeof(document.body) != "undefined" && typeof(document.body.parentElement) != "undefined" && typeof(document.body.parentElement.clientHeight) != "undefined") { return document.body.parentElement.clientHeight; }
	return 0;
}
function ResizeMonitor_CheckSize() {
	if ( (document.ResizeMonitor_LastHeight != ResizeMonitor_CurrentHeight() ) || (document.ResizeMonitor_LastWidth != ResizeMonitor_CurrentWidth() ) ) {
		document.ResizeMonitor_LastWidth = ResizeMonitor_CurrentWidth();
		document.ResizeMonitor_LastHeight = ResizeMonitor_CurrentHeight();
		if ( document.ResizeMonitor_LastTimer != 0 ) { window.clearTimeout( document.ResizeMonitor_LastTimer ); }
		document.ResizeMonitor_LastTimer = window.setTimeout("ResizeMonitor_Postback();", document.ResizeMonitor_TimeoutLength);
	}
}
function ResizeMonitor_SaveDimensions() {
	document.ResizeMonitor_Form[document.ResizeMonitor_WidthHolder].value = ResizeMonitor_CurrentWidth();
	document.ResizeMonitor_Form[document.ResizeMonitor_HeightHolder].value = ResizeMonitor_CurrentHeight();
}
function ResizeMonitor_FindForm() {
	for( var i = 0; i < document.forms.length; i++ ) {
		var myForm = document.forms[i];
		if ( typeof( myForm[document.ResizeMonitor_WidthHolder] ) != "undefined" ) {
			document.ResizeMonitor_Form = myForm;
			return true;
		}
	}
	return false;
}