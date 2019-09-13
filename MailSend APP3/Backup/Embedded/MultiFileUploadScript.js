function MetaBuilders_MultiFileUpload_Init() {
	if ( typeof( document.getElementById ) == "undefined" ) {
		return;
	}
	for( var i = 0; i < MetaBuilders_MultiFileUploads.length; i++ ) {
		var info = MetaBuilders_MultiFileUploads[i];
		MetaBuilders_MultiFileUpload_Load(info);
	}
}
function MetaBuilders_MultiFileUpload_Load(info) {
	var uploader = new Object();
	uploader.Selector = document.getElementById( info.SelectorID );
	uploader.Remover = document.getElementById( info.RemoverID );
	if ( uploader.Selector == null || uploader.Remover == null ) {
		return;
	}
	if ( typeof( uploader.Selector.Uploader ) != "undefined" ) {
		return;
	}
	
	uploader.Selector.Uploader = uploader;
	uploader.Remover.Uploader = uploader;
	
	uploader.Remover.RemoveUpload = MetaBuilders_MultiFileUpload_RemoveUpload;
	
	uploader.Uploaders = new Array();
	for( var j = 0; j < info.Uploaders.length; j++ ) {
		uploader.Uploaders[j] = document.getElementById( info.Uploaders[j] );
		uploader.Uploaders[j].Uploader = uploader;
		uploader.Uploaders[j].AddToSelection = MetaBuilders_MultiFileUpload_AddToSelection;
		MetaBuilders_MultiFileUpload_AddHandler( uploader.Uploaders[j], "change", "AddToSelection" );
	}
	
	MetaBuilders_MultiFileUpload_AddHandler( uploader.Remover, "click", "RemoveUpload" );
}
function MetaBuilders_MultiFileUpload_RemoveUpload(e) {
	var selector = this.Uploader.Selector;
	if ( selector.options.selectedIndex < 1 ) {
		return;
	}
	var selectedValue = selector.options[ selector.options.selectedIndex ].value;
	selector.options[ selector.options.selectedIndex ] = null;
	var uploadToRemove = null;
	for( var i = 0; i < this.Uploader.Uploaders.length; i++ ) {
		if ( selectedValue == this.Uploader.Uploaders[i].id ) {
			uploadToRemove = this.Uploader.Uploaders[i];
			break;
		}
	}
	if ( uploadToRemove != null ) {
		var parent = uploadToRemove.parentNode;
		parent.removeChild( uploadToRemove );

		var uploadToAdd = document.createElement( "input" );
		uploadToAdd.type = "file";
		uploadToAdd.id = uploadToRemove.id;
		uploadToAdd.name = uploadToRemove.name;
		uploadToAdd.style.display = "none";
		parent.appendChild( uploadToAdd );
	}
	MetaBuilders_MultiFileUpload_ReinitializeUploaders( this.Uploader );
}
function MetaBuilders_MultiFileUpload_ReinitializeUploaders( uploader ) {
	for( var i = 0; i < uploader.Uploaders.length; i++ ) {
		var oldUploader = uploader.Uploaders[i];
		var newUploader = document.getElementById( oldUploader.id );
		if ( newUploader != oldUploader ) {
			newUploader.Uploader = uploader;
			newUploader.AddToSelection = MetaBuilders_MultiFileUpload_AddToSelection;
			MetaBuilders_MultiFileUpload_AddHandler( newUploader, "change", "AddToSelection" );
			uploader.Uploaders[i] = newUploader;
		}
	}
	for( var i = 0; i < uploader.Uploaders.length; i++ ) {
		uploader.Uploaders[i].style.display = "none";
	}
	for( var i = 0; i < uploader.Uploaders.length; i++ ) {
		var fileUpload = uploader.Uploaders[i];
		if ( fileUpload.value.length == 0 ) {
			fileUpload.style.display = "";
			break;
		}
	}
}
function MetaBuilders_MultiFileUpload_AddToSelection(e) {
	var uploader = this.Uploader;
	uploader.Selector.options[ uploader.Selector.options.length ] = new Option( this.value, this.id );
	this.style.display = "none";
	for( var i = 0; i < uploader.Uploaders.length; i++ ) {
		var fileUpload = uploader.Uploaders[i];
		if ( fileUpload.value.length == 0 ) {
			fileUpload.style.display = "";
			break;
		}
	}
}
function MetaBuilders_MultiFileUpload_AddHandler(target,eventName,handlerName) { 
  if ( target.addEventListener ) { 
    target.addEventListener(eventName, function(e){target[handlerName](e);}, false);
  } else if ( target.attachEvent ) { 
    target.attachEvent("on" + eventName, function(e){target[handlerName](e);});
  } else { 
    var originalHandler = target["on" + eventName]; 
    if ( originalHandler ) { 
      target["on" + eventName] = function(e){originalHandler(e);target[handlerName](e);}; 
    } else { 
      target["on" + eventName] = target[handlerName]; 
    } 
  } 
}