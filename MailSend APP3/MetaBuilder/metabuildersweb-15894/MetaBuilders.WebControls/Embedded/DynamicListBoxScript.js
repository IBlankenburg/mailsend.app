function MetaBuilders_DynamicListBox_Init() {
	if ( !(document.getElementById) ) { return; }
	for( var i = 0; i < MetaBuilders_DynamicListBoxes.length; i++ ) {
		var info = MetaBuilders_DynamicListBoxes[i];
		MetaBuilders_DynamicListBox_Load( info );
	}
}
function MetaBuilders_DynamicListBox_Load( info ) {
	var list = document.getElementById(info.ID);
	if ( list == null || typeof( list.Tracker ) != 'undefined' ) {
		return;
	}
	list.Tracker = document.getElementById(info.TrackerID);
	list.Add = MetaBuilders_DynamicListBox_Add;
	list.InsertOption = MetaBuilders_DynamicListBox_InsertOption;
	list.Remove = MetaBuilders_DynamicListBox_Remove;
	list.MoveUp = MetaBuilders_DynamicListBox_MoveUp;
	list.MoveDown = MetaBuilders_DynamicListBox_MoveDown;
	list.Tracker.text = "";
}
function MetaBuilders_DynamicListBox_Add( value, text, index ) {
	var newOption = new Option();
	newOption.text = text;
	newOption.value = value;
	var insertIndex = parseInt( index );
	if ( !isNaN( insertIndex ) ) {
		this.Tracker.value += "+" + value + "\x03" + text + "\x03" + index + "\x1F";
		this.InsertOption(newOption,insertIndex);
	} else {
		this.Tracker.value += "+" + value + "\x03" + text + "\x1F";
		this.InsertOption(newOption);
	}
}
function MetaBuilders_DynamicListBox_Remove( index ) {
	if ( typeof( this.options.remove ) == "undefined" ) {
		this.options.remove = MetaBuilders_DynamicListBox_DownlevelRemove;
	}
	this.Tracker.value += "-" + index + "\x1F";
	this.options.remove(index);
}
function MetaBuilders_DynamicListBox_DownlevelRemove( index ) {
	this[index] = null;
}
function MetaBuilders_DynamicListBox_InsertOption( option, index ) {
	if ( typeof( index ) == "undefined" || index >= this.options.length ) {
		this.options[this.options.length] = option;
	} else {
		for( var i = this.options.length; i > index; i-- ) {
			var optionCopy = new Option();
			optionCopy.text = this.options[i-1].text;
			optionCopy.value = this.options[i-1].value;
			this.options[i] = optionCopy;
		}
		this.options[index] = option;
	}
}
function MetaBuilders_DynamicListBox_MoveUp() {
	var index = this.options.selectedIndex;
	if ( index == -1 || index == 0 ) {
		return;
	}
	var theItem = this.options[index];
	var oldText = theItem.text;
	var oldValue = theItem.value;
	this.Remove( index );
	this.Add( oldValue, oldText, index - 1 );
	this.options.selectedIndex = index - 1;
}
function MetaBuilders_DynamicListBox_MoveDown() {
	var index = this.options.selectedIndex;
	if ( index == -1 || index == this.options.length - 1 ) {
		return;
	}
	var theItem = this.options[index];
	var oldText = theItem.text;
	var oldValue = theItem.value;
	this.Remove( index );
	this.Add( oldValue, oldText, index + 1 );
	this.options.selectedIndex = index + 1;
}