using System;
using System.Web;
using System.Reflection;
using System.Web.Configuration;
using System.Configuration;

namespace MetaBuilders.WebControls {

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses" )]
	internal class DialogHandlerFactory : System.Web.IHttpHandlerFactory {
		
		#region IHttpHandlerFactory Members

		public void ReleaseHandler(IHttpHandler handler) {
		}

		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated) {
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}
			switch( context.Request.QueryString[HandlerNameKey] ) {
				case "DialogInputBoxPage":
					return new DialogInputBoxPage();
				case "DialogMessageBoxPage":
					return new DialogMessageBoxPage();
			}
			return null;
		}

		#endregion

		/// <summary>
		/// Ensures that the IHttpHandlerFactory required for built in dialogs to function, is registered for this application.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		internal static void EnsureHandlerFactory() {

			HttpContext context = HttpContext.Current;
			if ( context == null )
			{
				return;
			}

			String cacheKey = "IHttpHandlerFactory Installed " + HandlerName;
			Object handlerMarker = context.Cache.Get( cacheKey );
			if ( handlerMarker != null )
			{
				return;
			}

			try
			{
				Configuration c = WebConfigurationManager.OpenWebConfiguration( HttpContext.Current.Request.ApplicationPath );
				WriteHandlerToConfiguration( c );

				context.Cache.Insert( cacheKey, new Object() );
			}
			catch ( Exception ex )
			{
				// gulp, probably Medium Trust
				// we'll just pretend we did it right and not check again
				context.Cache.Insert( cacheKey, new Object() );
				System.Diagnostics.Debug.Write( ex.ToString() );
			}
		}

		/// <exclude />
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		internal static void WriteHandlerToConfiguration( Configuration config ) {
			HttpHandlersSection handlers = config.GetSection( "system.web/httpHandlers" ) as HttpHandlersSection;

			foreach ( HttpHandlerAction h in handlers.Handlers )
			{
				if ( h.Path == HandlerName )
				{
					return;
				}
			}

			HttpHandlerAction myHandler = new HttpHandlerAction( HandlerName, HandlerType, "*", false );
			handlers.Handlers.Add( myHandler );

			try
			{
				config.Save();
			}
			catch ( Exception ex )
			{
				// Gulp
				System.Diagnostics.Debug.Write( ex.ToString() );
			}
		}

		private static String HandlerType {
			get {
				return "MetaBuilders.WebControls.DialogHandlerFactory,MetaBuilders.WebControls";
			}
		}

		internal static String HandlerName {
			get {
				return "MetaBuilders_DialogWindow.axd";
			}
		}

		internal static String HandlerNameKey {
			get {
				return "MetaBuilders_Dialog";
			}
		}
	}
}
