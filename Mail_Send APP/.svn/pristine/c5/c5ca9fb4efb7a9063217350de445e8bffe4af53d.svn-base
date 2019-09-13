function MetaBuilders_ListLink_Init() {
	window.ListLinkManager["empty"] = new Array("","None");
	var parents = new Array();
	for( var i = 0; i < ListLinks.length; i++ ) {
		var info = ListLinks[i];
		var parentControl = MetaBuilders_ListLink_FindList( info.ParentID );
		var childControl = MetaBuilders_ListLink_FindList( info.ChildID );
		var childLockFirstItem = ( info.LockFirst != "False" );
		
		var linkName = "Link " + info.ParentID + " " + info.ChildID;
		
		if ( parentControl != null && childControl != null ) {
			if ( typeof( parentControl[ linkName ] ) == 'undefined' ) {
				childControl.LockFirstItem = childLockFirstItem;
				MetaBuilders_ListLink_Load( parentControl, childControl );
				parentControl[ linkName ] = true;
			}
			parents[parents.length] = parentControl;
		}
	}
	for( var j = 0; j < parents.length; j++ ) {
		parents[j].RepopulateChildren();
	}
}
function MetaBuilders_ListLink_Load( parent, child ) {
	if ( typeof ( parent.ChildLists ) == "undefined" ) {
		parent.ChildLists = new Array();
	}
	parent.ChildLists[parent.ChildLists.length] = child;
	parent.RepopulateChildren = MetaBuilders_ListLink_RepopulateChildren;
	parent.GetParentValueKey = MetaBuilders_ListLink_GetParentValueKey;
	parent.onchange = function(e) { this.RepopulateChildren(); };
}
function MetaBuilders_ListLink_FindList( name ) {
	for( var i = 0; i < document.forms.length; i++ ) {
		var theForm = document.forms[i];
		var theList = theForm[name];
		if ( theList != null ) {
			return theList;
		}
	}
	return null;
}
function MetaBuilders_ListLink_RepopulateChildren() {
	for ( var i = 0; i < this.ChildLists.length; i++ ) {
		MetaBuilders_ListLink_ClearList(this.ChildLists[i]);
		MetaBuilders_ListLink_FillList(this.ChildLists[i], this.GetParentValueKey(this.ChildLists[i]));
	}
}
function MetaBuilders_ListLink_ClearList(list){
	if ( typeof(list.ChildLists) != "undefined" ) {
		for( var j = 0; j < list.ChildLists.length; j++ ) {
			MetaBuilders_ListLink_ClearList( list.ChildLists[j] );
		}
	}
	if ( list.selectedIndex >= 0 ) {
		list.originalSelectedValue = list.options[list.selectedIndex].value;
	} else {
		list.originalSelectedValue = "";
	}
	var lastIndex = list.options.length - 1;
	var firstIndex = 0;
	if ( list.LockFirstItem ) {
		firstIndex = 1;
	}
	for (var i = lastIndex; i >= firstIndex; i--){
		list.options[i] = null;
	}
	if ( list.LockFirstItem ) {
		list.selectedIndex = 0;
	} else {
		list.selectedIndex = -1;
	}
}
function MetaBuilders_ListLink_GetParentValueKey(childList) {
	if ( this.selectedIndex != -1 ) {
		return this.name + "," + childList.name + "=" + this.options[this.selectedIndex].value;
	} else {
		return "";
	}
}
function MetaBuilders_ListLink_FillList(list, parentValue){
	if (parentValue != "" && window.ListLinkManager[parentValue]){
		var newChildItems = window.ListLinkManager[parentValue];
		if ( newChildItems.length > 0 ) {
			for (var i = 0; i < newChildItems.length; i = i + 2){
				list.options[list.options.length] = new Option(newChildItems[i + 1], newChildItems[i]);
				if ( list.options[list.options.length-1].value == list.originalSelectedValue ) {
					list.options[list.options.length-1].selected = true;
				}
			}
		}
	}
	if ( list.options.length == 0) {
		var emptyItem = window.ListLinkManager["empty"];
		list.options[0] = new Option(emptyItem[1], emptyItem[0]);
	}
	if ( typeof( list.ChildLists ) != "undefined" ) {
		for ( var j = 0; j < list.ChildLists.length; j++ ) {
			MetaBuilders_ListLink_FillList(list.ChildLists[j], list.GetParentValueKey(list.ChildLists[j]));
		}
	}
}