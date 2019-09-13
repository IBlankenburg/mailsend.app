using System;
using System.Collections.Generic;
using System.Text;

namespace MetaBuilders.WebControls
{
	public partial class DynamicListBox
	{

		/// <summary>
		/// Represents a command stored on the clientside which alters the Items collection of the <see cref="DynamicListBox"/>.
		/// </summary>
		/// <remarks>
		/// You don't need to use this in your code.
		/// </remarks>
		private class ListCommand
		{

			// Don't call this
			// use the static Parse and Split methods
			private ListCommand()
			{
			}

			/// <summary>
			/// Parses a single command string and returns the ListCommand created
			/// </summary>
			/// <param name="command">The string command to parse.</param>
			/// <returns>The created ListCommand</returns>
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
			public static ListCommand Parse( String command )
			{
				ListCommand newCommand = new ListCommand();

				if ( command.StartsWith( "+", StringComparison.Ordinal ) )
				{
					newCommand.Operator = "+";
					String[] newItemText = command.Substring( 1 ).Split( textSeperator );
					newCommand.Value = newItemText[ 0 ];
					newCommand.Text = newItemText[ 1 ];
					if ( newItemText.Length == 3 )
					{
						String IndexText = newItemText[ 2 ];
						try
						{
							newCommand.Index = Int32.Parse( IndexText, System.Globalization.CultureInfo.InvariantCulture );
						}
						catch
						{
						}
					}
				}
				else if ( command.StartsWith( "-", StringComparison.Ordinal ) )
				{
					newCommand.Operator = "-";
					String removalIndexString = command.Substring( 1 );
					try
					{
						newCommand.Index = Int32.Parse( removalIndexString, System.Globalization.CultureInfo.InvariantCulture );
					}
					catch
					{
					}
				}

				return newCommand;
			}

			/// <summary>
			/// Splits a full command string set into an array of ListCommands.
			/// </summary>
			/// <remarks>You don't need to use this from your code.</remarks>
			/// <param name="postedCommands">The set of commands to split.</param>
			/// <returns>The created array of ListCommands.</returns>
			public static ListCommand[] Split( String postedCommands )
			{
				String[] commandsText = postedCommands.Split( commandSeperator );
				ListCommand[] commands = new ListCommand[ commandsText.Length ];
				for ( Int32 i = 0; i < commandsText.Length; i++ )
				{
					commands[ i ] = ListCommand.Parse( commandsText[ i ] );
				}
				return commands;
			}

			private const Char commandSeperator = '\x1F';
			private const Char textSeperator = '\x03';

			/// <summary>
			/// Gets or sets the operator of the command. + or -.
			/// </summary>
			public String Operator = "";
			/// <summary>
			/// Gets or sets the text of the added ListItem.
			/// </summary>
			public String Text = "";
			/// <summary>
			/// Gets or sets the value of the added ListItem.
			/// </summary>
			public String Value = "";
			/// <summary>
			/// Gets or sets the index of the ListItem to add or remove.
			/// </summary>
			public Int32 Index = -1;
		}

	}
}