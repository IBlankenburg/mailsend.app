function MetaBuilders_CheckedListBox_Init() {
	if ( typeof( document.getElementById ) == "undefined" ) return;
	if (typeof(MetaBuilders_CheckedListBoxes) == "undefined") return;
	for ( var i = 0; i < MetaBuilders_CheckedListBoxes.length; i++) {
		var info = MetaBuilders_CheckedListBoxes[i];
		MetaBuilders_CheckedListBox_Load( info );
	}
}
function MetaBuilders_CheckedListBox_Load( info ) {
	var checkedListBox = document.getElementById( info.ID );
	var container = document.getElementById( info.ContainerID );
	if ( checkedListBox == null || container == null || typeof( checkedListBox.Container ) != 'undefined' ) {
		return;
	}
	checkedListBox.Container = container;
	if ( typeof(checkedListBox.style.borderColor) == "undefined" || checkedListBox.style.borderColor == "" ) {
		checkedListBox.style.borderColor = "ButtonFace";
	}
	checkedListBox.ResizeWidth = MetaBuilders_CheckedListBox_ResizeWidth;
	if ( checkedListBox.style.width == "" ) {
		checkedListBox.ResizeWidth();
	}
}
function MetaBuilders_CheckedListBox_ResizeWidth() {
	var ScrollbarPadding = 20;
	var ContainerWidth;
	if( typeof( document.defaultView ) != "undefined" ) { // The w3c standard
		ContainerWidth = document.defaultView.getComputedStyle( this.Container, "" ).getPropertyValue("width");
	} else if ( typeof( this.Container.offsetWidth ) != "undefined" ) { // ie
		ContainerWidth = this.Container.offsetWidth;
	} else {
		return;
	}
	this.style.width = ( parseInt( ContainerWidth ) + ScrollbarPadding ) + "px";
}