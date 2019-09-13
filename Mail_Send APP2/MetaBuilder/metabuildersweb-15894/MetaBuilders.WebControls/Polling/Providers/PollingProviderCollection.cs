using System;
using System.Configuration.Provider;

namespace MetaBuilders.WebControls
{

	/// <summary>
	/// A Collection which stores <see cref="PollingProvider"/> instances.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface" )]
	public sealed class PollingProviderCollection : ProviderCollection
	{

		/// <summary>
		/// Gets the <see cref="PollingProvider"/> with the given name.
		/// </summary>
		public new PollingProvider this[string name]
		{
			get
			{
				return (PollingProvider)base[name];
			}
		}

		/// <summary>
		/// Adds the given <see cref="PollingProvider"/> to the collection.
		/// </summary>
		/// <remarks>An <see cref="ArgumentException"/> will be thrown if the given provider is not a <see cref="PollingProvider"/>.</remarks>
		public override void Add( ProviderBase provider )
		{
			if ( provider == null )
			{
				throw new ArgumentNullException( "provider" );
			}

			if ( !( provider is PollingProvider ) )
			{
				throw new ArgumentException( "Invalid provider type", "provider" );
			}

			base.Add( provider );
		}
	}
}
