function MetaBuilders_MultiViewBar_Init() {
	if ( !MetaBuilders_MultiViewBar_UplevelBrowser() ) { return; }
	for( var i=0; i < Page_MetaBuilders_MultiViewBars.length; i++ ) {
		var barInfo = Page_MetaBuilders_MultiViewBars[i];
		barInfo.ItemCount = parseInt( barInfo.ItemCount );
		barInfo.InitialSelectedIndex = parseInt( barInfo.InitialSelectedIndex );
		MetaBuilders_MultiViewBar_Load( barInfo );
	}
}
function MetaBuilders_MultiViewBar_UplevelBrowser() {
	if ( typeof( Page_MetaBuilders_MultiViewBars ) == "undefined" ) return false;
	if ( typeof( document.getElementById ) == "undefined" ) return false;
	return true;
}
function MetaBuilders_MultiViewBar_Load( barInfo ) {
	var bar = document.getElementById( barInfo.ID );
	if ( typeof( bar.Items ) != "undefined" ) {
		return;
	}
	bar.Items = new Array();
	bar.IndexOf = MetaBuilders_MultiViewBar_IndexOfItem;
	bar.CurrentItemTracker = MetaBuilders_MultiViewBar_GetHiddenElement( barInfo.HiddenID );
	if ( typeof( bar.rows ) != "undefined" ) {
		switch( barInfo.Placement ) {
			case "Surrounding":
				bar.ClearSelection = MetaBuilders_MultiViewBar_Surrounding_ClearSelection;
				bar.AddItem = MetaBuilders_MultiViewBar_Surrounding_AddItem;
				if ( barInfo.Layout == "Vertical" ) {
					for ( var i = 0; i < bar.rows.length; i = i + 2 ) {
						var itemHead = bar.rows[i];
						var itemBody = bar.rows[i+1];
						bar.AddItem( itemHead, itemBody );
					}
				} else {
					var row = bar.rows[0];
					for ( var i = 0; i < row.cells.length; i = i + 2 ) {
						var itemHead = row.cells[i];
						var itemBody = row.cells[i+1];
						bar.AddItem( itemHead, itemBody );
					}
				}
				break;
			case "Bottom":
				bar.ClearSelection = MetaBuilders_MultiViewBar_Bottom_ClearSelection;
				bar.AddItem = MetaBuilders_MultiViewBar_Bottom_AddItem;
				if ( barInfo.Layout == "Vertical" ) {
					if ( bar.rows.length == barInfo.ItemCount * 3 ) {
						for ( var i = 0; i < bar.rows.length / 3; i++ ) {
							var itemHead   = bar.rows[ i ];
							var itemBody   = bar.rows[ i + barInfo.ItemCount ];
							var itemButton = bar.rows[ i + barInfo.ItemCount * 2 ];
							bar.AddItem( itemHead, itemBody, itemButton );
						}
					}
				} else {
					var row = bar.rows[ 0 ];
					if ( row.cells.length == barInfo.ItemCount * 3 ) {
						for( var i = 0; i < row.cells.length / 3; i++ ) {
							var itemHead   = row.cells[ i ];
							var itemBody   = row.cells[ i + barInfo.ItemCount ];
							var itemButton = row.cells[ i + barInfo.ItemCount * 2 ];
							bar.AddItem( itemHead, itemBody, itemButton );
						}
					}
				}
				break;
			case "Top":
				bar.ClearSelection = MetaBuilders_MultiViewBar_Top_ClearSelection;
				bar.AddItem = MetaBuilders_MultiViewBar_Top_AddItem;
				if ( barInfo.Layout == "Vertical" ) {
					if ( bar.rows.length == barInfo.ItemCount * 2 ) {
						for ( var i = 0; i < bar.rows.length / 2; i++ ) {
							var itemButton   = bar.rows[ i ];
							var itemBody   = bar.rows[ i + barInfo.ItemCount ];
							bar.AddItem( itemBody, itemButton );
						}
					}
				} else {
					var row = bar.rows[ 0 ];
					if ( row.cells.length == barInfo.ItemCount * 2 ) {
						for ( var i = 0; i < row.cells.length / 2; i++ ) {
							var itemButton = row.cells[ i ];
							var itemBody = row.cells[ i + barInfo.ItemCount ];
							bar.AddItem( itemBody, itemButton );
						}
					}
				}
		}
		if ( bar.Items.length > 1 ) {
			var selectedIndex = parseInt( bar.CurrentItemTracker.value );
			var selectedItem = bar.Items[ selectedIndex ];
			var nonSelectedItem;
			if ( selectedIndex == 0 ) {
				nonSelectedItem = bar.Items[ selectedIndex + 1 ];
			} else {
				nonSelectedItem = bar.Items[ 0 ];
			}
			if ( selectedItem.StylingElement.style.cssText ) {
				bar.CurrentItemStyle = selectedItem.StylingElement.style.cssText;
				bar.CurrentItemCssClass = selectedItem.StylingElement.className;
				bar.ItemStyle = nonSelectedItem.StylingElement.style.cssText;
				bar.ItemCssClass = nonSelectedItem.StylingElement.className;
			} else {
				bar.CurrentItemStyle = "";
				bar.CurrentItemCssClass = "";
				bar.ItemStyle = "";
				bar.ItemCssClass = "";
			}
		}
	}
}
function MetaBuilders_MultiViewBar_Surrounding_AddItem( itemHead, itemBody ) {
	MetaBuilders_MultiViewBar_OverridePostbacks( itemHead );

	this.Items[ this.Items.length ] = itemHead;
	itemHead.Owner = this;
	if ( itemHead.tagName.toLowerCase() == "tr" ) {
		itemHead.StylingElement = itemHead.cells[ 0 ];
	} else {
		itemHead.StylingElement = itemHead;
	}
	itemHead.StylingElement.ApplyStyle = MetaBuilders_MultiViewBar_ApplyStyle;
	itemHead.BodyPanel = itemBody;
	itemHead.Select = MetaBuilders_MultiViewBar_Surrounding_Select;
	itemHead.onclick = itemHead.Select;
}
function MetaBuilders_MultiViewBar_Surrounding_Select() {
	this.Owner.ClearSelection();
	this.StylingElement.ApplyStyle( this.Owner.CurrentItemStyle );
	this.StylingElement.className = this.Owner.CurrentItemCssClass;
	this.BodyPanel.style.display = "";
	this.Owner.CurrentItemTracker.value = this.Owner.IndexOf( this );
}
function MetaBuilders_MultiViewBar_Surrounding_ClearSelection() {
	for( var i=0; i < this.Items.length; i++ ) {
		var child = this.Items[i];
		child.StylingElement.ApplyStyle( this.ItemStyle );
		child.StylingElement.className = this.ItemCssClass;
		child.BodyPanel.style.display = "none";
	}
	this.CurrentItemTracker.value = -1;
}
function MetaBuilders_MultiViewBar_Bottom_AddItem( itemHead, itemBody, itemButton ) {
	MetaBuilders_MultiViewBar_OverridePostbacks( itemButton );

	this.Items[ this.Items.length ] = itemButton;
	itemButton.Owner = this;
	itemButton.BodyPanel = itemBody;
	itemButton.HeadPanel = itemHead;
	if ( itemButton.tagName.toLowerCase() == "tr" ) {
		itemButton.StylingElement = itemButton.cells[ 0 ];
	} else {
		itemButton.StylingElement = itemButton;
	}
	itemButton.StylingElement.ApplyStyle = MetaBuilders_MultiViewBar_ApplyStyle;
	itemButton.Select = MetaBuilders_MultiViewBar_Bottom_Select;
	itemButton.onclick = itemButton.Select;
}
function MetaBuilders_MultiViewBar_Bottom_ClearSelection() {
	for( var i=0; i < this.Items.length; i++ ) {
		var child = this.Items[i];
		child.StylingElement.ApplyStyle( this.ItemStyle );
		child.StylingElement.className = this.ItemCssClass;
		child.BodyPanel.style.display = "none";
		child.HeadPanel.style.display = "none";
	}
	this.CurrentItemTracker.value = -1;
}
function MetaBuilders_MultiViewBar_Bottom_Select() {
	this.Owner.ClearSelection();
	this.StylingElement.ApplyStyle( this.Owner.CurrentItemStyle );
	this.StylingElement.className = this.Owner.CurrentItemCssClass;
	this.BodyPanel.style.display = "";
	this.HeadPanel.style.display = "";
	this.Owner.CurrentItemTracker.value = this.Owner.IndexOf( this );
}
function MetaBuilders_MultiViewBar_Top_AddItem( itemBody, itemButton ) {
	MetaBuilders_MultiViewBar_OverridePostbacks( itemButton );

	this.Items[ this.Items.length ] = itemButton;
	itemButton.Owner = this;
	itemButton.BodyPanel = itemBody;
	if ( itemButton.tagName.toLowerCase() == "tr" ) {
		itemButton.StylingElement = itemButton.cells[ 0 ];
	} else {
		itemButton.StylingElement = itemButton;
	}
	itemButton.StylingElement.ApplyStyle = MetaBuilders_MultiViewBar_ApplyStyle;
	itemButton.Select = MetaBuilders_MultiViewBar_Top_Select;
	itemButton.onclick = itemButton.Select;
}
function MetaBuilders_MultiViewBar_Top_ClearSelection() {
	for( var i=0; i < this.Items.length; i++ ) {
		var child = this.Items[i];
		child.StylingElement.ApplyStyle( this.ItemStyle );
		child.StylingElement.className = this.ItemCssClass;
		child.BodyPanel.style.display = "none";
	}
	this.CurrentItemTracker.value = -1;
}
function MetaBuilders_MultiViewBar_Top_Select() {
	this.Owner.ClearSelection();
	this.StylingElement.ApplyStyle( this.Owner.CurrentItemStyle );
	this.StylingElement.className = this.Owner.CurrentItemCssClass;
	this.BodyPanel.style.display = "";
	this.Owner.CurrentItemTracker.value = this.Owner.IndexOf( this );
}
function MetaBuilders_MultiViewBar_GetHiddenElement( name ) {
	for( var i = 0; i < document.forms.length; i++ ) {
		var theForm = document.forms[ i ];
		if ( theForm[ name ] != null ) {
			return theForm[ name ];
		}
	}
	return null;
}
function MetaBuilders_MultiViewBar_IndexOfItem( barItem ) {
	for( var i=0; i < this.Items.length; i++ ) {
		var item = this.Items[i];
		if ( item == barItem ) {
			return i;
		}
	}
	return -1;
}
function MetaBuilders_MultiViewBar_OverridePostbacks( container ) {
	container.onclick = function() { return false; };
	var postBackControls;
	postBackControls = container.getElementsByTagName( "TD" );
	if ( postBackControls != null ) {
		for( var i = 0; i < postBackControls.length; i++ ) {
			var postBackControl = postBackControls[ i ];
			postBackControl.onclick = function() { return false; };
		}
	}
	postBackControls = container.getElementsByTagName( "A" );
	if ( postBackControls != null ) {
		for( var i = 0; i < postBackControls.length; i++ ) {
			var postBackControl = postBackControls[ i ];
			postBackControl.onclick = function() { return false; };
			postBackControl.href = "javascript:return false;";
		}
	}
	postBackControls = container.getElementsByTagName( "INPUT" );
	if ( postBackControls != null ) {
		for( var i = 0; i < postBackControls.length; i++ ) {
			var postBackControl = postBackControls[ i ];
			postBackControl.onclick = function() { return false; };
		}
	}
	postBackControls = container.getElementsByTagName( "IMG" );
	if ( postBackControl != null ) {
		for( var i = 0; i < postBackControls.length; i++ ) {
			var postBackControl = postBackControls[ i ];
			postBackControl.onclick = function() { return false; };
		}
	}
}
function MetaBuilders_MultiViewBar_ApplyStyle( newStyle ) {
	var isOpera = navigator.userAgent.toLowerCase().indexOf("opera") != -1;
	if ( isOpera ) {
		MetaBuilders_MultiViewBar_ClearStyle( this );

		if ( newStyle.length == 0 ) { return; }
		var nameValues = newStyle.split(";");
		for ( var i = 0; i < nameValues.length; i++ ) {
			var nameValue = nameValues[ i ].split( ":" );
			var name = MetaBuilders_MultiViewBar_Trim( nameValue[ 0 ] );
			var value = MetaBuilders_MultiViewBar_Trim( nameValue[ 1 ] );
			if ( name.indexOf( "-" ) != -1 ) {
				var namePieces = name.split( "-" );
				if ( namePieces.length > 1 ) {
					name = MetaBuilders_MultiViewBar_Trim( namePieces[ 0 ] ).toLowerCase();
					for( var j = 1; j < namePieces.length; j++ ) {
						var namePiece = MetaBuilders_MultiViewBar_Trim( namePieces[ j ] ).toLowerCase();
						namePiece = namePiece.substring( 0, 1 ).toUpperCase() + namePiece.substring( 1, namePiece.length );
						name += namePiece;
					}
				}
			}
			
			this.style[ name ] = value;
		}
	} else {
		this.style.cssText = newStyle;
	}
}
function MetaBuilders_MultiViewBar_ClearStyle( target ) {
	var nameValues = target.style.cssText.split(";");
	for ( var i = 0; i < nameValues.length; i++ ) {
		var nameValue = nameValues[ i ].split( ":" );
		var name = MetaBuilders_MultiViewBar_Trim( nameValue[ 0 ] );
		var value = MetaBuilders_MultiViewBar_Trim( nameValue[ 1 ] );
		if ( name.indexOf( "-" ) != -1 ) {
			var namePieces = name.split( "-" );
			if ( namePieces.length > 1 ) {
				name = MetaBuilders_MultiViewBar_Trim( namePieces[ 0 ] ).toLowerCase();
				for( var j = 1; j < namePieces.length; j++ ) {
					var namePiece = MetaBuilders_MultiViewBar_Trim( namePieces[ j ] ).toLowerCase();
					namePiece = namePiece.substring( 0, 1 ).toUpperCase() + namePiece.substring( 1, namePiece.length );
					name += namePiece;
				}
			}
		}
		target.style[ name ] = "";
	}
}
function MetaBuilders_MultiViewBar_Trim( s ) {
	if ( s == null ) {
		return "";
	}
	while ((s.substring(0,1) == ' ') || (s.substring(0,1) == '\n') || (s.substring(0,1) == '\r')) {
		s = s.substring(1,s.length);
	}
	while ((s.substring(s.length-1,s.length) == ' ') || (s.substring(s.length-1,s.length) == '\n') || (s.substring(s.length-1,s.length) == '\r')) {
		s = s.substring(0,s.length-1);
	}
	return s;
}