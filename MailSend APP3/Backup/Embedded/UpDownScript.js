function UpDown_Increment(textBoxId,increment) {
	var textBoxControl = UpDown_GetTextbox( textBoxId );
	if( textBoxControl != null ) textBoxControl.value = parseInt(textBoxControl.value) + increment;
}
function UpDown_Decrement(textBoxId,increment) {
	var textBoxControl = UpDown_GetTextbox( textBoxId );
	if( textBoxControl != null ) textBoxControl.value = parseInt(textBoxControl.value) - increment;
}
function UpDown_GetTextbox(textBoxId) {
	for( var i=0; i<document.forms.length; i++ ) {
		var theForm = document.forms[i];
		if( typeof(theForm[textBoxId]) != "undefined" ) {
			var textBoxControl = theForm[textBoxId];
			if ( isNaN( parseInt( textBoxControl.value ) ) ) textBoxControl.value = 0;
			return textBoxControl;
		}
	}
	return null;
}