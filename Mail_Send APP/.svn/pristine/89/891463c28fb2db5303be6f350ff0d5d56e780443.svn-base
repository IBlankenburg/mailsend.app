function MetaBuilders_NamingContainer_Init() {
	MetaBuilders_NamingContainer_NamedElements = null;
	for( var i = 0; i < MetaBuilders_NamingContainers.length; i++ ) {
		var nm = MetaBuilders_NamingContainers[ i ];
		MetaBuilders_NamingContainer_Load( nm.ID, nm.Name  );
	}
}
function MetaBuilders_NamingContainer_Load( namingContainerID, namingContainerName ) {
	var container = new Object();
	container.__NamingContainerID = namingContainerID;
	container.__NamingContainerName = namingContainerName;
	var containerChildren = MetaBuilders_NamingContainer_FindChildren( namingContainerID, namingContainerName );
	for( var i = 0; i < containerChildren.length; i++ ) {
		var child = containerChildren[ i ];
		if ( !(child.NamingContainer) || child.NamingContainer.__NamingContainerID.length < namingContainerID.length ) {
			child.NamingContainer = container;
			var childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
			if ( childID != "" ) {
				var childSimpleID = ( namingContainerID.length > 0 ) ? childID.substring( namingContainerID.length + 1, childID.length ) : childID;
				if ( !(child.id) && child.type && child.type.toLowerCase() == "radio" ) {
					childSimpleID = child.value;
				}
				container[ childSimpleID ] = child;
			}
		}
	}
}
function MetaBuilders_NamingContainer_FindChildren( namingContainerID, namingContainerName ) {
	var children = new Array();
	var potentials = null;
	
	if ( document.getElementById && document.getElementById( namingContainerID ) != null && document.getElementsByTagName ) {
		var container = document.getElementById( namingContainerID );
		potentials = container.getElementsByTagName( "*" );
	}
	if ( potentials == null || potentials.length == 0 ) {
		potentials = MetaBuilders_NamingContainer_FindNamedElements();
	}
	for( var i = 0; i < potentials.length; i++ ) {
		var child = potentials[ i ];
		var childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
		if ( childID != "" ) {
			if ( namingContainerID.length == 0 || childID.substring( 0, namingContainerID.length ) == namingContainerID || childID.substring( 0, namingContainerName.length ) == namingContainerName ) {
				children[ children.length ] = child;
			}
		}
	}
	return children;
}
function MetaBuilders_NamingContainer_FindNamedElements() {
	if ( MetaBuilders_NamingContainer_NamedElements == null ) {
		MetaBuilders_NamingContainer_NamedElements = new Array();
		var allElements = new Array();
		var child;
		var childID;
		
		if ( document.getElementsByTagName ) {
			allElements = document.getElementsByTagName( "*" );
		} else if ( document.all ) {
			allElements = document.all;
		}
		
		if ( allElements.length != 0 ) {
			for ( var i = 0; i < allElements.length; i++ ) {
				child = allElements[ i ];
				childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
				if ( childID != "" ) {
					MetaBuilders_NamingContainer_NamedElements[ MetaBuilders_NamingContainer_NamedElements.length ] = child;
				}
			}
		} else {
			for( var a = 0; a < document.forms.length; a++ ) {
				var theForm = document.forms[ a ];
				for( var b = 0; b < theForm.elements.length; b++ ) {
					child = theForm.elements[ b ];
					childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
					if ( childID != "" ) {
						MetaBuilders_NamingContainer_NamedElements[ MetaBuilders_NamingContainer_NamedElements.length ] = child;
					}
				}
			}
			for( var c = 0; c < document.images.length; c++ ) {
				child = document.images[ c ];
				childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
				if ( childID != "" ) {
					MetaBuilders_NamingContainer_NamedElements[ MetaBuilders_NamingContainer_NamedElements.length ] = child;
				}
			}
		}
		for( var j = 0; j < MetaBuilders_NamingContainer_NamedElements.length; j++ ) {
			child = MetaBuilders_NamingContainer_NamedElements[ j ];
			childID = ( child.id ) ? child.id : ( child.name ) ? child.name : "";
		}
	}
	return MetaBuilders_NamingContainer_NamedElements;
}
var MetaBuilders_NamingContainer_NamedElements = null;
