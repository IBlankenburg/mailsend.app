function MetaBuilders_SelectorField_Init() {
    if ( typeof( document.getElementById ) == "undefined" ) return;
    window.MetaBuilders_SelectorField_Fields = new Object();
    for( var i=0; i < MetaBuilders_SelectorField_CheckAllBoxes.length; i++ ) {
		var info = MetaBuilders_SelectorField_CheckAllBoxes[i];
		var checkAllBox = document.getElementById( info.ID );
		checkAllBox.participants = new Array(); 
		MetaBuilders_SelectorField_Fields[ info.Field ] = checkAllBox;
		
		for( var j=0; j < MetaBuilders_SelectorField_CheckBoxes.length; j++ ) {
			var selectorInfo = MetaBuilders_SelectorField_CheckBoxes[j];
			if ( selectorInfo.Field == info.Field ) {
				checkAllBox.participants[ checkAllBox.participants.length ] = document.getElementById( selectorInfo.ID );
			}
		}
	}
}
function MetaBuilders_SelectorField_SelectAll( parentCheckBox ) {
    if ( parentCheckBox == null || typeof( parentCheckBox.participants ) == "undefined" ) {
        return;
    }
    var participants = parentCheckBox.participants;
    for ( var i=0; i < participants.length; i++ ) {
        var participant = participants[i];
        if ( participant != null ) {
            participant.checked = parentCheckBox.checked;
        }
    }
}
function MetaBuilders_SelectorField_CheckChildren( field ) {
	if ( typeof( window.MetaBuilders_SelectorField_Fields ) == "undefined" ) return;
    var parent = window.MetaBuilders_SelectorField_Fields[ field ];
    if ( parent == null || typeof( parent.participants ) == "undefined" ) return;
    var participants = parent.participants;
    for ( var i=0; i < participants.length; i++ ) {
        var participant = participants[i];
        if ( participant != null && !participant.checked ) {
				parent.checked = false;
				return;
        }
    }
    parent.checked = true;
}
