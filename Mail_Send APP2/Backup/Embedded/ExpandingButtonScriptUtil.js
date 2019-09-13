function ExpandingButtons_Init() {
	if ( !ExpandingButtons_BrowserCapable() ) { return; }
	for( var i=0; i<MetaBuilders_WebControls_ExpandingButtons.length; i ++ ) {
		var info = MetaBuilders_WebControls_ExpandingButtons[i];
		ExpandingButtons_Load(info);
	}
}
function ExpandingButtons_Load(info) {
	var expanderControl = document.getElementById( info.ID );
	var targetControl = document.getElementById( info.TargetID );
	var trackerControl = document.getElementById( info.TrackerID );
	var expandedValue = info.ExValue;
	var contractedValue = info.CtValue;
	var valueChanger = info.Type;
	if ( expanderControl == null || targetControl == null || typeof( expanderControl.TargetControl ) != 'undefined' ) {
		return;
	}
	expanderControl.TargetControl = targetControl;
	expanderControl.TrackerControl = trackerControl;
	expanderControl.ToggleView = ExpandingButtons_Toggle;
	expanderControl.onclick = expanderControl.ToggleView;
	expanderControl.ExpandedValue = expandedValue;
	expanderControl.ContractedValue = contractedValue;
	switch( valueChanger ) {
		case "b":
			expanderControl.ToggleValue = ExpandingButtons_ToggleButtonText;
			break;
		case "l":
			expanderControl.ToggleValue = ExpandingButtons_ToggleLinkText;
			break;
		case "i":
			expanderControl.ToggleValue = ExpandingButtons_ToggleImageSrc;
			break;
		case "c":
			expanderControl.ToggleValue = ExpandingButtons_ToggleCheckBoxChecked;
			break;
		default:
			expanderControl.ToggleValue = function() { return true; };
	}
}
function ExpandingButtons_BrowserCapable() {
	if ( typeof( document.getElementById ) == "undefined" ) {
		return false;
	}
	return true;
}
function ExpandingButtons_Toggle() {
	var result = true;
	if ( this.TargetControl.style.display == "none" ) {
		this.TargetControl.style.display = "";
		if ( this.TrackerControl != null ) {
			this.TrackerControl.value = "True";
		}
		result = this.ToggleValue(this.ExpandedValue);
	} else {
		this.TargetControl.style.display = "none";
		if ( this.TrackerControl != null ) {
			this.TrackerControl.value = "False";
		}
		result = this.ToggleValue(this.ContractedValue);
	}
	if ( typeof( window.event ) != "undefined" && typeof( window.event.returnValue ) != "undefined" ) {
		window.event.returnValue = result;
	}
	return result;
}
function ExpandingButtons_ToggleButtonText(newValue) {
	this.value = newValue;
	return false;
}
function ExpandingButtons_ToggleLinkText(newValue) {
	this.innerHTML = newValue;
	return false;
}
function ExpandingButtons_ToggleImageSrc(newValue) {
	this.alt = newValue.Alt
	this.src = newValue.Src;
	this.title = newValue.Alt;
	return false;
}
function ExpandingButtons_ToggleCheckBoxChecked(newValue) {
	return ( ( newValue == "true" ) == this.checked );
}