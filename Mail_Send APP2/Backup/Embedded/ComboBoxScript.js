
function MetaBuilders_ComboBox_Init() {
	for( var i = 0; i < MetaBuilders_ComboBoxes.length; i++ ) {
		var info = MetaBuilders_ComboBoxes[ i ];
		MetaBuilders_ComboBox_Load( info );
	}
}
function MetaBuilders_ComboBox_Load( info ) {
	var Combo = document.getElementById( info.ID );
	var Container = document.getElementById( info.ContainerID );
	var Entry = document.getElementById( info.EntryID );
	var List = document.getElementById( info.ListID );
	var Button = document.getElementById( info.ButtonID );
	
	if ( typeof( Container.List ) != 'undefined' ) {
		return;
	}
	
	Combo.style.display = "";
	
	Container.Entry = Entry;
	Container.List = List;
	Container.Button = Button;
	Entry.Container = Container;
	List.Container = Container;
	Button.Container = Container;

	Container.style.padding = "0px";
	
	Entry.style.margin = "0px";
	Entry.style.padding = "0px";
	Entry.style.width = ( List.offsetWidth ) + "px";
	Entry.style.borderWidth = "0px";
	Entry.TypeDownTimeout = -1;
	
	List.style.display = "none";
	List.style.position = "absolute";
	List.style.left = "0px";
	List.style.zIndex = 10000;
	List.size = ( info.ListSize > List.options.length ) ? List.options.length : info.ListSize;
	List.multiple = false;
	List.IsShowing = false;
	
	Button.style.display = "";
	Button.style.padding = "0px";
	Button.style.margin = "0px";
	if ( typeof ( Entry.dataSrc ) != "undefined" ) { // to overcome misaligniment in IE
		Button.style.position = "relative";
		Button.style.top = "auto";
		Button.style.bottom = "2px";
	}
	
	List.Show = MetaBuilders_ComboBox_List_Show;
	List.Hide = MetaBuilders_ComboBox_List_Hide;
	List.EnableBlur = MetaBuilders_ComboBox_List_EnableBlur;
	List.DisableBlur = MetaBuilders_ComboBox_List_DisableBlur;
	List.Select = MetaBuilders_ComboBox_List_Select;
	List.ClearSelection = MetaBuilders_ComboBox_List_ClearSelection;
	List.KeyAccess = MetaBuilders_ComboBox_List_KeyAccess;
	List.FireTextChange = MetaBuilders_ComboBox_List_FireTextChange;
	List.onchange = null;
	List.onclick = function(e){ this.Select(e); this.ClearSelection(); this.FireTextChange(); };
	List.onkeyup = function(e) { this.KeyAccess(e); };
	List.EnableBlur(null);
	

	Entry.TypeDown = MetaBuilders_ComboBox_Entry_TypeDown;
	Entry.KeyAccess = MetaBuilders_ComboBox_Entry_KeyAccess;
	Entry.onkeyup = function(e) { this.KeyAccess(e); this.TypeDown(e); };

	Button.ToggleList = MetaBuilders_ComboBox_Button_Toggle;
	Button.onclick = Button.ToggleList;
	Button.onselectstart = function(e){ return false; };
	Button.onmouseover = function(e) { this.Container.List.DisableBlur(e); };
	Button.onmouseout = function(e) { this.Container.List.EnableBlur(e); };

}
function MetaBuilders_ComboBox_Button_Toggle() {
	if ( this.Container.List.IsShowing == true ) {
		this.Container.List.Hide();
	} else {
		this.Container.List.Show();
	}
}
function MetaBuilders_ComboBox_Entry_TypeDown(e) {
	if ( this.TypeDownTimeout != -1 ) {
		window.clearTimeout( this.TypeDownTimeout );
		this.TypeDownTimeout = -1;
	}
	e = MetaBuilders_ComboBox_InitEvent( e );
	var items = this.Container.List.options;
	if( this.value == "" ) return;
	var ctrlKeys = Array( 8, 46, 37, 38, 39, 40, 33, 34, 35, 36, 45, 16, 20 );
	for( var i = 0; i < ctrlKeys.length; i++ ) {
		if( e.keyCode == ctrlKeys[i] ) return;
	}
	var isMatch = false;
	for( var i = 0; i < items.length; i++ ) {
		var item = items[i];
		if( item.text.toLowerCase().indexOf( this.value.toLowerCase() ) == 0 ) {
			this.Container.List.selectedIndex = i;
			isMatch = true;
			if ( typeof( this.Container.Entry.createTextRange ) != "undefined" ) {
				this.TypeDownTimeout = window.setTimeout( "MetaBuilders_ComboBox_Entry_TypeDown_Delayed( '" + this.Container.id + "');", 500 );
			}
			break;
		}
	}
	if ( !isMatch ) {
		this.Container.List.selectedIndex = -1;
	}
}
function MetaBuilders_ComboBox_Entry_TypeDown_Delayed(id) {
	var container = document.getElementById( id );
	container.List.Select();
}
function MetaBuilders_ComboBox_Entry_KeyAccess(e) {
	e = MetaBuilders_ComboBox_InitEvent( e );
	if( e.altKey && (e.keyCode == 38 || e.keyCode == 40) ) {
		this.Container.List.Show();
	}
}
function MetaBuilders_ComboBox_List_ClearSelection() {
	var entry = this.Container.Entry;
	if ( typeof( entry.createTextRange ) == "undefined" ) return;
	var rNew = entry.createTextRange();
	rNew.moveStart('character', entry.value.length) ;
	rNew.select();
}
function MetaBuilders_ComboBox_List_KeyAccess(e) {
	e = MetaBuilders_ComboBox_InitEvent( e );
	if( e.keyCode == 13 || e.keyCode == 32 ) {
		this.Select();
		return;
	}
	if( e.keyCode == 27 ) {
		this.Hide();
		this.Container.Entry.focus();
		return;
	}
}
function MetaBuilders_ComboBox_List_FireTextChange() {
	var textOnChange = this.Container.Entry.onchange;
	if ( textOnChange != null && typeof(textOnChange) == "function" ) {
		textOnChange();
	}
}
function MetaBuilders_ComboBox_List_EnableBlur() {
	this.onblur = this.Hide;
}
function MetaBuilders_ComboBox_List_DisableBlur() {
	this.onblur = null;
}
function MetaBuilders_ComboBox_List_Show() {
	if ( !this.IsShowing && !this.disabled ) {
		if ( typeof ( this.Container.Entry.dataSrc ) != "undefined" ) { // to overcome misaligniment in IE
			this.style.top = ( this.Container.offsetHeight + 10 ) + "px";
		} else {
			this.style.top = ( this.Container.offsetHeight ) + "px";
		}
		this.style.width = this.Container.offsetWidth + "px";
		this.style.display = "";
		this.focus();
		this.IsShowing = true;
	}
}
function MetaBuilders_ComboBox_List_Hide() {
	if ( this.IsShowing ) {
		this.style.display = "none";
		this.IsShowing = false;
	}
}
function MetaBuilders_ComboBox_List_Select() {
	if( this.options.length > 0 ) {
		var text = this.Container.Entry;
		var oldValue = text.value;
		var newValue = this.options[ this.selectedIndex ].text;
		text.value = newValue;
		if ( typeof( text.createTextRange ) != "undefined" ) {
			if (newValue != oldValue) {
				var rNew = text.createTextRange();
				rNew.moveStart('character', oldValue.length) ;
				rNew.select();
			}
		}
	}
	this.Hide();
	this.Container.Entry.focus();
}
function MetaBuilders_ComboBox_InitEvent( e ) {
	if( typeof( e ) == "undefined" && typeof( window.event ) != "undefined" ) e = window.event;
	if( e == null ) e = new Object();
	return e;
}
function MetaBuilders_ComboBox_SimpleAttach(selectElement,textElement) {
	textElement.value = selectElement.options[ selectElement.options.selectedIndex ].text;
	var textOnChange = textElement.onchange;
	if ( textOnChange != null && typeof( textOnChange ) == "function" ) {
		textOnChange();
	}
}
